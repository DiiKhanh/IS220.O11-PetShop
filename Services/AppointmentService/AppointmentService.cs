using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetShop.Data;
using PetShop.DTOs;
using PetShop.Helpers;
using PetShop.Models;
using PetShop.Services.EmailService;

namespace PetShop.Services.AppointmentService
{
    public class AppointmentService : IAppointmentService
    {
        private readonly PetShopDbContext _context;
        private readonly IMapper _mapper;

        public AppointmentService(PetShopDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Create(AppointmentDto request)
        {
            var existingAppointment = _context.Appointment
           .FirstOrDefault(a => a.Date == request.Date && a.Hour == request.Hour);

            if (existingAppointment != null)
            {
                return ResponseHelper.BadRequest("The appointment that has already been booked.");
            }

            // Check if the dog already has an active appointment
            var existingDogAppointment = _context.Appointment
                .FirstOrDefault(a => a.Dog_item_id == request.Dog_item_id && !new[] { "Completed", "Cancelled" }.Contains(a.Status));

            if (existingDogAppointment != null)
            {
                var errorMessage = $"The dog is scheduled for an appointment on {existingDogAppointment.Date} at {existingDogAppointment.Hour}:00.";
                return ResponseHelper.BadRequest(errorMessage);
            }


            // Create a new appointment with userId
            var newAppointment = new Appointment
            {
                User_id = request.User_id,
                Status = "Pending",
                Is_cancel = false,
                Date = request.Date,
                Hour = request.Hour,
                Description = request.Description,
                Phone_number = request.Phone_number,
                User_name = request.User_name,
                Dog_item_id = request.Dog_item_id,
                Service=request.Service,
            };

            await _context.Appointment.AddAsync(newAppointment);
            await _context.SaveChangesAsync();
            return ResponseHelper.Ok(new
            {
                status = 200,
                data = "Dang ky thanh cong!"
            });
        }

        public async Task<IActionResult> GetByUser(string user_id)
        {
            var appointments = await _context.Appointment.Where(d => d.User_id == user_id).ToListAsync();
            if (appointments is null) return ResponseHelper.NotFound();
            List<object> responselist = new List<object>();
            appointments.ForEach(appointment =>
            {
               
                object response = new
                {
                    appointment.User_id,
                    appointment.Status,
                    appointment.Is_cancel,
                    appointment.Date,
                    appointment.Hour,
                    appointment.Description,
                    appointment.Phone_number,
                    appointment.User_name,
                    appointment.Service,
                    appointment.Dog_item_id,
                    appointment.Appointment_id,
                    appointment.Result
                };
                responselist.Add(response);
            });
            return ResponseHelper.Ok(responselist);
        }

        public async Task<IActionResult> Cancel(int id, Appointment2Dto request)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment ==  null)
            {
                return ResponseHelper.NotFound();
            }
            if (appointment.Status != "Pending")
            {
                return ResponseHelper.BadRequest("Lịch hẹn đã được xác nhận!");
            }

            appointment.Is_cancel = true;
            appointment.Status = "Cancelled";
            appointment.Result = request.Result;
            await _context.SaveChangesAsync();
            return ResponseHelper.Ok(new
            {
                status = 200,
                data = "Dang ky thanh cong!"
            });

        }

        public async Task<IActionResult> GetAll()
        {
            var checkouts = await _context.Appointment.ToListAsync();
            if (checkouts is null) return ResponseHelper.NotFound();
            List<object> responselist = new List<object>();
            checkouts.ForEach(appointment =>
            {
                object response = new
                {
                    appointment.User_id,
                    appointment.Status,
                    appointment.Is_cancel,
                    appointment.Date,
                    appointment.Hour,
                    appointment.Description,
                    appointment.Phone_number,
                    appointment.User_name,
                    appointment.Service,
                    appointment.Dog_item_id,
                    appointment.Appointment_id,
                    appointment.Result,
                    appointment.CreateAt,
                    appointment.UpdatedAt,
                };
                responselist.Add(response);
            });
            return ResponseHelper.Ok(responselist);
        }

        public async Task<IActionResult> UpdateAdmin(int id, Appointment1Dto request)
        {

            var appointment =
                await _context.Appointment.FirstOrDefaultAsync(x => x.Appointment_id == id);
            if (appointment is null) return ResponseHelper.NotFound();

            try
            {
                appointment.Status = request.Status;
                appointment.Result = request.Result;
                appointment.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return ResponseHelper.Ok(new
                {
                    appointment.User_id,
                    appointment.Status,
                    appointment.Is_cancel,
                    appointment.Date,
                    appointment.Hour,
                    appointment.Description,
                    appointment.Phone_number,
                    appointment.User_name,
                    appointment.Service,
                    appointment.Dog_item_id,
                    appointment.Appointment_id,
                    appointment.Result,
                    appointment.CreateAt,
                    appointment.UpdatedAt,
                });
            }
            catch (Exception)
            {
                return ResponseHelper.BadRequest("Không thể cập nhật. Vui lòng thử lại");
            }
        }
    }
}
