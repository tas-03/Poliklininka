using Microsoft.Extensions.Configuration;
using Npgsql;
using Poliklininka.Models.Admin;

namespace Poliklininka.Services.Admin;

public class AdminAdoService : IAdminAdoService
{
    private readonly string _connectionString;

    public AdminAdoService(string connectionString)
    {
        _connectionString = connectionString;
    }

    private NpgsqlConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    public async Task<List<AdminUserDto>> GetUsersAsync()
    {
        var users = new List<AdminUserDto>();

        const string sql = """
            SELECT
                u.id,
                u.login,
                u.password,
                u.full_name,
                u.role,
                u.discriminator,

                COALESCE(p.phone_number, '') AS phone_number,
                COALESCE(p.insurance_policy, '') AS insurance_policy,
                COALESCE(p.address, '') AS address,

                COALESCE(d.specialization, '') AS specialization,
                COALESCE(d.office, '') AS office
            FROM users u
            LEFT JOIN patients p ON p.id = u.id
            LEFT JOIN doctors d ON d.id = u.id
            ORDER BY u.id;
            """;

        await using var connection = CreateConnection();
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            users.Add(new AdminUserDto
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Login = reader.GetString(reader.GetOrdinal("login")),
                Password = reader.GetString(reader.GetOrdinal("password")),
                FullName = reader.GetString(reader.GetOrdinal("full_name")),
                Role = reader.GetString(reader.GetOrdinal("role")),
                Discriminator = reader.GetString(reader.GetOrdinal("discriminator")),

                PhoneNumber = reader.GetString(reader.GetOrdinal("phone_number")),
                InsurancePolicy = reader.GetString(reader.GetOrdinal("insurance_policy")),
                Address = reader.GetString(reader.GetOrdinal("address")),

                Specialization = reader.GetString(reader.GetOrdinal("specialization")),
                Office = reader.GetString(reader.GetOrdinal("office"))
            });
        }

        return users;
    }

    public async Task AddUserAsync(AdminUserDto user)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var discriminator = GetDiscriminatorByRole(user.Role);

            const string insertUserSql = """
                INSERT INTO users (login, password, full_name, role, discriminator)
                VALUES (@login, @password, @fullName, @role, @discriminator)
                RETURNING id;
                """;

            await using var userCommand = new NpgsqlCommand(insertUserSql, connection, transaction);
            userCommand.Parameters.AddWithValue("@login", user.Login);
            userCommand.Parameters.AddWithValue("@password", user.Password);
            userCommand.Parameters.AddWithValue("@fullName", user.FullName);
            userCommand.Parameters.AddWithValue("@role", user.Role);
            userCommand.Parameters.AddWithValue("@discriminator", discriminator);

            var newUserId = Convert.ToInt32(await userCommand.ExecuteScalarAsync());

            if (user.Role == "Patient")
            {
                const string insertPatientSql = """
                    INSERT INTO patients (id, phone_number, insurance_policy, address)
                    VALUES (@id, @phoneNumber, @insurancePolicy, @address);
                    """;

                await using var patientCommand = new NpgsqlCommand(insertPatientSql, connection, transaction);
                patientCommand.Parameters.AddWithValue("@id", newUserId);
                patientCommand.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
                patientCommand.Parameters.AddWithValue("@insurancePolicy", user.InsurancePolicy);
                patientCommand.Parameters.AddWithValue("@address", user.Address);

                await patientCommand.ExecuteNonQueryAsync();
            }

            if (user.Role == "Doctor")
            {
                const string insertDoctorSql = """
                    INSERT INTO doctors (id, specialization, office)
                    VALUES (@id, @specialization, @office);
                    """;

                await using var doctorCommand = new NpgsqlCommand(insertDoctorSql, connection, transaction);
                doctorCommand.Parameters.AddWithValue("@id", newUserId);
                doctorCommand.Parameters.AddWithValue("@specialization", user.Specialization);
                doctorCommand.Parameters.AddWithValue("@office", user.Office);

                await doctorCommand.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateUserAsync(AdminUserDto user)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var discriminator = GetDiscriminatorByRole(user.Role);

            const string updateUserSql = """
                UPDATE users
                SET login = @login,
                    password = @password,
                    full_name = @fullName,
                    role = @role,
                    discriminator = @discriminator
                WHERE id = @id;
                """;

            await using var userCommand = new NpgsqlCommand(updateUserSql, connection, transaction);
            userCommand.Parameters.AddWithValue("@id", user.Id);
            userCommand.Parameters.AddWithValue("@login", user.Login);
            userCommand.Parameters.AddWithValue("@password", user.Password);
            userCommand.Parameters.AddWithValue("@fullName", user.FullName);
            userCommand.Parameters.AddWithValue("@role", user.Role);
            userCommand.Parameters.AddWithValue("@discriminator", discriminator);

            await userCommand.ExecuteNonQueryAsync();

            if (user.Role == "Patient")
            {
                const string upsertPatientSql = """
                    INSERT INTO patients (id, phone_number, insurance_policy, address)
                    VALUES (@id, @phoneNumber, @insurancePolicy, @address)
                    ON CONFLICT (id)
                    DO UPDATE SET
                        phone_number = EXCLUDED.phone_number,
                        insurance_policy = EXCLUDED.insurance_policy,
                        address = EXCLUDED.address;
                    """;

                await using var patientCommand = new NpgsqlCommand(upsertPatientSql, connection, transaction);
                patientCommand.Parameters.AddWithValue("@id", user.Id);
                patientCommand.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
                patientCommand.Parameters.AddWithValue("@insurancePolicy", user.InsurancePolicy);
                patientCommand.Parameters.AddWithValue("@address", user.Address);

                await patientCommand.ExecuteNonQueryAsync();
            }

            if (user.Role == "Doctor")
            {
                const string upsertDoctorSql = """
                    INSERT INTO doctors (id, specialization, office)
                    VALUES (@id, @specialization, @office)
                    ON CONFLICT (id)
                    DO UPDATE SET
                        specialization = EXCLUDED.specialization,
                        office = EXCLUDED.office;
                    """;

                await using var doctorCommand = new NpgsqlCommand(upsertDoctorSql, connection, transaction);
                doctorCommand.Parameters.AddWithValue("@id", user.Id);
                doctorCommand.Parameters.AddWithValue("@specialization", user.Specialization);
                doctorCommand.Parameters.AddWithValue("@office", user.Office);

                await doctorCommand.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteUserAsync(int userId)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            const string checkAppointmentsSql = """
                SELECT COUNT(*)
                FROM appointments
                WHERE patient_id = @userId OR doctor_id = @userId;
                """;

            await using var checkCommand = new NpgsqlCommand(checkAppointmentsSql, connection, transaction);
            checkCommand.Parameters.AddWithValue("@userId", userId);

            var appointmentsCount = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());

            if (appointmentsCount > 0)
            {
                throw new Exception("Нельзя удалить пользователя, который связан с записями на прием.");
            }

            const string deleteMedCardSql = """
                DELETE FROM med_cards
                WHERE patient_id = @userId;
                """;

            await using var medCardCommand = new NpgsqlCommand(deleteMedCardSql, connection, transaction);
            medCardCommand.Parameters.AddWithValue("@userId", userId);
            await medCardCommand.ExecuteNonQueryAsync();

            const string deleteDoctorSql = """
                DELETE FROM doctors
                WHERE id = @userId;
                """;

            await using var doctorCommand = new NpgsqlCommand(deleteDoctorSql, connection, transaction);
            doctorCommand.Parameters.AddWithValue("@userId", userId);
            await doctorCommand.ExecuteNonQueryAsync();

            const string deletePatientSql = """
                DELETE FROM patients
                WHERE id = @userId;
                """;

            await using var patientCommand = new NpgsqlCommand(deletePatientSql, connection, transaction);
            patientCommand.Parameters.AddWithValue("@userId", userId);
            await patientCommand.ExecuteNonQueryAsync();

            const string deleteUserSql = """
                DELETE FROM users
                WHERE id = @userId;
                """;

            await using var userCommand = new NpgsqlCommand(deleteUserSql, connection, transaction);
            userCommand.Parameters.AddWithValue("@userId", userId);
            await userCommand.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private static string GetDiscriminatorByRole(string role)
    {
        return role switch
        {
            "Patient" => "Patient",
            "Doctor" => "Doctor",
            _ => "User"
        };
    }
}