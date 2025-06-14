using MedicalID.Backend.Models;
using MedicalID.Backend.Models.JoinModels;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<MedicalCondition> MedicalConditions { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<RecordHistory> RecordHistories { get; set; }
        public DbSet<RecordHistoryFile> RecordHistoryFiles { get; set; }
        public DbSet<AccessLog> AccessLogs { get; set; }
        public DbSet<AskDoctor> AskDoctors { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<PatientAllergy> PatientAllergies { get; set; }
        public DbSet<PatientCondition> PatientConditions { get; set; }
        public DbSet<PatientMedication> PatientMedications { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Specialization> Specializations { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<AccessLog>()
                .HasOne(a => a.PatientByMedicalID)
                .WithMany(p => p.AccessLogs) 
                .HasForeignKey(a => a.MedicalID)
                .HasPrincipalKey(p => p.MedicalID)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<AccessLog>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.AccessLogs)
                .HasForeignKey(a => a.DoctorID)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<RecordHistory>()
                .HasKey(r => r.RecordHistoryID);

            modelBuilder.Entity<RecordHistory>()
                .HasOne(r => r.AccessLog)
                .WithMany() 
                .HasForeignKey(r => r.LogID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RecordHistory>()
                .HasOne(rh => rh.Patient)
                
                .WithMany(p => p.RecordHistories) 
                .HasForeignKey(rh => rh.MedicalID)
                .HasPrincipalKey(p => p.MedicalID) 
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RecordHistory>()
                .HasOne(r => r.Doctor)
                .WithMany(d => d.RecordHistories)
                .HasForeignKey(r => r.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<RecordHistoryFile>()
                .HasOne(f => f.RecordHistory)
                .WithMany(r => r.RecordHistoryFiles)
                .HasForeignKey(f => f.RecordHistoryID)
                .OnDelete(DeleteBehavior.Cascade);


            
            modelBuilder.Entity<Hospital>()
                .HasOne(h => h.Region)
                .WithMany(r => r.Hospitals)
                .HasForeignKey(h => h.RegionID)
                .OnDelete(DeleteBehavior.Restrict);

           
            modelBuilder.Entity<PatientAllergy>()
                .HasKey(pa => new { pa.PatientID, pa.AllergyID });

            modelBuilder.Entity<PatientCondition>()
                .HasKey(pc => new { pc.PatientID, pc.ConditionID });

            modelBuilder.Entity<PatientMedication>()
                .HasKey(pm => new { pm.PatientID, pm.MedicationID });

            
            modelBuilder.Entity<PatientCondition>()
                .HasOne(pc => pc.Patient)
                .WithMany(p => p.PatientConditions) 
                .HasForeignKey(pc => pc.PatientID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PatientCondition>()
                .HasOne(pc => pc.Condition)
                .WithMany(mc => mc.PatientConditions) 
                .HasForeignKey(pc => pc.ConditionID)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<PatientMedication>()
                .HasOne(pm => pm.Patient)
                .WithMany(p => p.PatientMedications)
                .HasForeignKey(pm => pm.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PatientMedication>()
                .HasOne(pm => pm.Medication)
                .WithMany(m => m.PatientMedications)
                .HasForeignKey(pm => pm.MedicationID)
                .OnDelete(DeleteBehavior.Restrict);

          
            modelBuilder.Entity<AskDoctor>()
                .HasOne(ad => ad.Patient)
                .WithMany(p => p.AskDoctors)
                .HasForeignKey(ad => ad.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AskDoctor>()
                .HasOne(ad => ad.Doctor)
                .WithMany(d => d.AskDoctors)
                .HasForeignKey(ad => ad.DoctorID)
                .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<Region>()
                .HasOne(r => r.City)
                .WithMany(c => c.Regions)
                .HasForeignKey(r => r.CityID);

            
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorID)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Doctor>()
               .HasOne(d => d.Specialization)
               .WithMany(s => s.Doctors)
               .HasForeignKey(d => d.SpecializationID)
               .OnDelete(DeleteBehavior.Restrict);

           
            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.PatientID).IsUnique(); 

            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.MedicalID).IsUnique(); 
        }
    }
}
   
