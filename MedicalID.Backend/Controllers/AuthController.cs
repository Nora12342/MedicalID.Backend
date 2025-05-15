using MedicalID.Backend.Data;
using MedicalID.Backend.Dtos;
using MedicalID.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalID.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MedicalIDContext _context;
        private readonly IConfiguration _config;

        public AuthController(MedicalIDContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto dto)
        {
            // Check in Doctors
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserName == dto.UserName);

            if (doctor != null)
            {
                if (BCrypt.Net.BCrypt.Verify(dto.Password, (string)doctor.PasswordHash))
                {
                    var token = JwtHelper.GenerateToken(
                        doctor.DoctorID,
                        doctor.UserName,
                        "Doctor",
                        _config
                    );

                    return Ok(new
                    {
                        token,
                        user = new LoginResponseDto
                        {
                            UserId = doctor.DoctorID,
                            Role = "Doctor",
                            UserName = doctor.UserName,
                            FullName = $"{doctor.FName} {doctor.LName}"
                        }
                    });
                }
                return Unauthorized("Invalid password.");


            }

            // Check in Patients
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserName == dto.UserName);

            if (patient != null)
            {
                if (BCrypt.Net.BCrypt.Verify(dto.Password, (string)patient.PasswordHash))
                {
                    var token = JwtHelper.GenerateToken(
                        patient.PatientID,
                        patient.UserName,
                        "Patient",
                        _config
                    );

                    return Ok(new
                    {
                        token,
                        user = new LoginResponseDto
                        {
                            UserId = patient.PatientID,
                            Role = "Patient",
                            UserName = patient.UserName,
                            FullName = $"{patient.FName} {patient.LName}"
                        }
                    });
                }
                return Unauthorized("Invalid password.");
            }

            return NotFound("User not found.");
        }

        [HttpPost("register/doctor")]
        public async Task<ActionResult> RegisterDoctor(DoctorRegisterDto dto)
        {
            if (await _context.Doctors.AnyAsync(d => d.UserName == dto.UserName))
                return BadRequest("Username already exists for a doctor.");

            var doctor = new Doctor
            {
                DoctorID = dto.DoctorID,
                FName = dto.FName,
                LName = dto.LName,
                Specialization = dto.Specialization,
                UserName = dto.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RegionID = dto.RegionID
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return Ok("Doctor registered successfully.");
        }

        [HttpPost("register/patient")]
        public async Task<ActionResult> RegisterPatient(PatientRegisterDto dto)
        {
            if (await _context.Patients.AnyAsync(p => p.UserName == dto.UserName))
                return BadRequest("Username already exists for a patient.");

            var patient = new Patient
            {
                PatientID = dto.PatientID,
                MedicalID = dto.MedicalID,
                FName = dto.FName,
                LName = dto.LName,
                UserName = dto.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                DateOfBirth = dto.DateOfBirth,
                BloodType = dto.BloodType,
                EmergencyContact = dto.EmergencyContact,
                OrganDonorStatus = dto.OrganDonorStatus,
                RegionID = dto.RegionID
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return Ok("Patient registered successfully.");
        }
    }
}

