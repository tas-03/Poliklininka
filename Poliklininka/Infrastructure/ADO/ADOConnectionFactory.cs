using Microsoft.Extensions.Configuration;
using Npgsql;
using System.IO;

namespace Poliklininka.Infrastructure.ADO;

public class ADOConnectionFactory
{
    public NpgsqlConnection CreateConnection()
    {
        var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();

        var connectionString = configuration.GetConnectionString("Default");
        return new NpgsqlConnection(connectionString);
    }
}
