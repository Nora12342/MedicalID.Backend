using System.Reflection.Emit;
using MedicalID.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Data
{
    public class MedicalIDContext : DbContext
    {
        public MedicalIDContext(DbContextOptions<MedicalIDContext> options)
       : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<MedicalCondition> MedicalConditions { get; set; }
        public DbSet<AccessLog> AccessLogs { get; set; }

        public DbSet<RecordHistory> RecordHistories { get; set; }
        public DbSet<AskDoctor> AskDoctors { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Region> Regions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<AccessLog>()
                .HasKey(a => a.LogID);

            modelBuilder.Entity<Allergy>()
               .HasKey(a => new { a.PatientID, a.Allergen });

            modelBuilder.Entity<AskDoctor>()
              .HasKey(a => new { a.PatientID, a.MessageID });

            modelBuilder.Entity<MedicalCondition>()
             .HasKey(a => new { a.PatientID, a.MedConditionID });

            modelBuilder.Entity<RecordHistory>()
            .HasKey(a => new { a.PatientID, a.RecordID });
        }
    

    }

}
