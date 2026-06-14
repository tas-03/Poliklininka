using Microsoft.EntityFrameworkCore;
using Poliklininka.Entities;

namespace Poliklininka.Infrastructure.EF;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Reception> Receptions { get; set; } = null!;
    public DbSet<Patient> Patients { get; set; } = null!;
    public DbSet<BloodGroup> BloodGroups { get; set; } = null!;
    public DbSet<MedCard> MedCards { get; set; } = null!;
    public DbSet<Doctor> Doctors { get; set; } = null!;
    public DbSet<MedService> MedServices { get; set; } = null!;
    public DbSet<Schedule> Schedules { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;
    public DbSet<VisitHistory> VisitHistories { get; set; } = null!;
    public DbSet<Analysis> Analyses { get; set; } = null!;
    public DbSet<AnalysisHistory> AnalysisHistories { get; set; } = null!;
    public DbSet<Recipe> Recipes { get; set; } = null!;
    public DbSet<RecipeHistory> RecipeHistories { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<AllergyPatient> AllergyPatients { get; set; } = null!;
    public DbSet<Allergy> Allergies { get; set; } = null!;
    public DbSet<ChronicDiseases> ChronicDiseases { get; set; } = null!;
    public DbSet<ÑronicDiseasesPatient> ÑronicDiseasesPatient { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<ChronicDiseases>(e =>
        {
            e.ToTable("chronic_diseases");
            e.HasKey(e => e.Id);
            e.Property(e => e.Id).HasColumnName("id").IsRequired();
            e.Property(e => e.Name).HasColumnName("name").IsRequired();
            e.Property(e => e.Code).HasColumnName("code").IsRequired();
        });

        modelBuilder.Entity<ÑronicDiseasesPatient>(e =>
        {
            e.ToTable("chronic_diseases_patient");
            e.HasKey(e => new { e.MedCardId, e.ChronicDiseasesId });
            e.Property(e => e.MedCardId).HasColumnName("medcard_id").IsRequired();
            e.Property(e => e.ChronicDiseasesId).HasColumnName("chronic_diseases_id").IsRequired();
            e.HasOne(e => e.MedCard).WithMany(e => e.HronicDiseasesPatient).HasForeignKey(e => e.MedCardId).OnDelete(DeleteBehavior.Cascade); ;
            e.HasOne(e => e.ChronicDiseases).WithMany(a => a.HronicDiseasesPatient).HasForeignKey(e => e.ChronicDiseasesId);
        });

        modelBuilder.Entity<AllergyPatient>(e =>
        {
            e.ToTable("allergy_patient");
            e.HasKey(e => new { e.MedCardId, e.AllergyId });
            e.Property(e => e.MedCardId).HasColumnName("medcard_id").IsRequired();
            e.Property(e => e.AllergyId).HasColumnName("allergy_id").IsRequired();
            e.HasOne(e => e.MedCard).WithMany(e => e.AllergyPatient).HasForeignKey(e => e.MedCardId).OnDelete(DeleteBehavior.Cascade); ;
            e.HasOne(e => e.Allergy).WithMany(a => a.AllergyPatient).HasForeignKey(e => e.AllergyId);
        }

            );
        modelBuilder.Entity<Allergy>(e =>
        {
            e.ToTable("allergies");
            e.HasKey(e => e.Id);
            e.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(e => e.Name).HasColumnName("name").IsRequired();
            e.Property(e => e.Code).HasColumnName("code").IsRequired();

        });

        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(e => e.Id);
            e.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(e => e.Login).HasColumnName("login").IsRequired();
            e.Property(e => e.Password).HasColumnName("password").IsRequired();
            e.Property(e => e.Role).HasColumnName("role").IsRequired();
            e.Property(e => e.Full_Name).HasColumnName("full_name").IsRequired();
            e.Property(e => e.Discriminator).HasColumnName("discriminator").HasMaxLength(50);

        });

        modelBuilder.Entity<Reception>(e =>
        {
            e.ToTable("receptions");
            e.HasKey(r => r.ReceptionId);
        });

        modelBuilder.Entity<Patient>(e =>
        {
            e.ToTable("patients");
            e.Property(p => p.Phone_number).HasColumnName("phone_number");
            e.Property(p => p.Insurance_Policy).HasColumnName("insurance_policy").IsRequired();
            e.Property(p => p.Address).HasColumnName("address");
        });


        modelBuilder.Entity<BloodGroup>(e =>
        {
            e.ToTable("blood_groups");
            e.HasKey(b => b.Id);
            e.Property(b => b.Id).HasColumnName("id");
            e.Property(b => b.Name).HasColumnName("name").IsRequired();
            e.Property(b => b.RhFactor).HasColumnName("rh_factor").IsRequired();
            e.Property(b => b.Office).HasColumnName("office").IsRequired();
        });

        modelBuilder.Entity<MedCard>(e =>
        {
            e.ToTable("med_cards");
            e.HasKey(m => m.Id);
            e.Property(m => m.Id).HasColumnName("id");
            e.Property(m => m.PatientId).HasColumnName("patient_id");
            e.Property(m => m.BloodGroupId).HasColumnName("blood_group_id");

            e.Property(m => m.OpenDate).HasColumnName("open_date");

            e.Property(m => m.Disability).HasColumnName("disability");
            e.Property(m => m.DateOfBirth).HasColumnName("date_of_birth");


            e.HasOne(m => m.Patient)
             .WithOne(p => p.MedCard)
             .HasForeignKey<MedCard>(m => m.PatientId).OnDelete(DeleteBehavior.Cascade); ;

            e.HasOne(m => m.BloodGroup)
             .WithMany(b => b.MedCards)
             .HasForeignKey(m => m.BloodGroupId);
        });


        modelBuilder.Entity<Doctor>(e =>
        {
            e.ToTable("doctors");
            e.Property(d => d.Specialization).HasColumnName("specialization").IsRequired();
            e.Property(d => d.Office).HasColumnName("office").IsRequired();

        });


        modelBuilder.Entity<MedService>(e =>
        {
            e.ToTable("med_services");
            e.HasKey(s => s.Id);
            e.Property(s => s.Id).HasColumnName("id");
            e.Property(s => s.ServiceName).HasColumnName("service_name").IsRequired();
            e.Property(s => s.Cost).HasColumnName("cost").HasPrecision(10, 2);
            e.Property(s => s.Category).HasColumnName("category").IsRequired();
        });


        modelBuilder.Entity<Schedule>(e =>
        {
            e.ToTable("schedules");
            e.HasKey(s => s.Id);
            e.Property(s => s.Id).HasColumnName("id");
            e.Property(s => s.DoctorId).HasColumnName("doctor_id");
            e.Property(s => s.Date).HasColumnName("date");
            e.Property(s => s.StartTime).HasColumnName("start_time");
            e.Property(s => s.EndTime).HasColumnName("end_time");
            e.Property(s => s.Office).HasColumnName("office").IsRequired();
            e.Property(s => s.SlotStatus).HasColumnName("slot_status").IsRequired();

            e.HasOne(s => s.Doctor)
             .WithMany(d => d.Schedules)
             .HasForeignKey(s => s.DoctorId);
        });

        modelBuilder.Entity<Appointment>(e =>
        {
            e.ToTable("appointments");
            e.HasKey(a => a.Id);
            e.Property(a => a.Id).HasColumnName("id");
            e.Property(a => a.PatientId).HasColumnName("patient_id");
            e.Property(a => a.DoctorId).HasColumnName("doctor_id");
            e.Property(a => a.ScheduleId).HasColumnName("schedule_id");
            e.Property(a => a.AppointmentDate).HasColumnName("appointment_date");
            e.Property(a => a.BookingStatus).HasColumnName("booking_status").IsRequired();
            e.Property(a => a.CreatedAt).HasColumnName("created_at");

            e.HasOne(a => a.Patient)
             .WithMany(p => p.Appointments)
             .HasForeignKey(a => a.PatientId);

            e.HasOne(a => a.Doctor)
             .WithMany(d => d.Appointments)
             .HasForeignKey(a => a.DoctorId);

            e.HasOne(a => a.Schedule)
             .WithMany(s => s.Appointments)
             .HasForeignKey(a => a.ScheduleId);

            e.Property(e => e.MedServiceId)
             .HasColumnName("med_service_id");

            e.HasOne(e => e.MedService)
             .WithMany(e => e.Appointments)
             .HasForeignKey(e => e.MedServiceId)
             .OnDelete(DeleteBehavior.Restrict);
        });


        modelBuilder.Entity<VisitHistory>(e =>
        {
            e.ToTable("visit_histories");
            e.HasKey(v => v.Id);
            e.Property(v => v.Id).HasColumnName("id");
            e.Property(v => v.AppointmentId).HasColumnName("appointment_id");
            e.Property(v => v.PatientId).HasColumnName("patient_id");
            e.Property(v => v.MedServiceId).HasColumnName("med_service_id");
            e.Property(v => v.VisitDate).HasColumnName("visit_date");
            e.Property(v => v.VisitTime).HasColumnName("visit_time");
            e.Property(v => v.VisitResults).HasColumnName("visit_results").IsRequired();

            e.HasOne(v => v.Appointment)
             .WithOne(a => a.VisitHistory)
             .HasForeignKey<VisitHistory>(v => v.AppointmentId);

            e.HasOne(v => v.Patient)
             .WithMany(p => p.VisitHistories)
             .HasForeignKey(v => v.PatientId);

            e.HasOne(v => v.MedService)
             .WithMany(s => s.VisitHistories)
             .HasForeignKey(v => v.MedServiceId);
        });


        modelBuilder.Entity<Analysis>(e =>
        {
            e.ToTable("analyses");
            e.HasKey(a => a.Id);
            e.Property(a => a.Id).HasColumnName("id");
            e.Property(a => a.AnalysisName).HasColumnName("analysis_name").IsRequired();
            e.Property(a => a.Description).HasColumnName("description").IsRequired();
        });


        modelBuilder.Entity<AnalysisHistory>(e =>
        {
            e.ToTable("analysis_histories");
            e.HasKey(a => a.Id);
            e.Property(a => a.Id).HasColumnName("id");
            e.Property(a => a.VisitHistoryId).HasColumnName("visit_history_id");
            e.Property(a => a.AnalysisId).HasColumnName("analysis_id");
            e.Property(a => a.Result).HasColumnName("result").IsRequired();

            e.HasOne(a => a.VisitHistory)
             .WithMany(v => v.AnalysisHistories)
             .HasForeignKey(a => a.VisitHistoryId);

            e.HasOne(a => a.Analysis)
             .WithMany(an => an.AnalysisHistories)
             .HasForeignKey(a => a.AnalysisId);
        });


        modelBuilder.Entity<Recipe>(e =>
        {
            e.ToTable("recipes");
            e.HasKey(r => r.Id);
            e.Property(r => r.Id).HasColumnName("id");
            e.Property(r => r.Description).HasColumnName("description").IsRequired();
        });


        modelBuilder.Entity<RecipeHistory>(e =>
        {
            e.ToTable("recipe_histories");
            e.HasKey(r => r.Id);
            e.Property(r => r.Id).HasColumnName("id");
            e.Property(r => r.VisitHistoryId).HasColumnName("visit_history_id");
            e.Property(r => r.RecipeId).HasColumnName("recipe_id");
            e.Property(r => r.Dosage).HasColumnName("dosage").IsRequired();
            e.Property(r => r.Duration).HasColumnName("duration").IsRequired();

            e.HasOne(r => r.VisitHistory)
             .WithMany(v => v.RecipeHistories)
             .HasForeignKey(r => r.VisitHistoryId);

            e.HasOne(r => r.Recipe)
             .WithMany(rc => rc.RecipeHistories)
             .HasForeignKey(r => r.RecipeId);
        });
    }
}
