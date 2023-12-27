using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;

namespace PetShop.Services.AppointmentService
{
    public interface IAppointmentService
    {
        Task<IActionResult> Create(AppointmentDto request);
        Task<IActionResult> GetByUser(string user_id);
        Task<IActionResult> Cancel(int id, Appointment2Dto request);
        Task<IActionResult> GetAll();
        Task<IActionResult> UpdateAdmin(int id, Appointment1Dto request);
    }
}
