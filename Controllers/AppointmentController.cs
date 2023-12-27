using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;
using PetShop.Services.AppointmentService;

namespace PetShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Create([FromBody] AppointmentDto request)
        {
            return await _appointmentService.Create(request);
        }

        [HttpGet("all/{user_id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetByUser([FromRoute] string user_id)
        {
            return await _appointmentService.GetByUser(user_id);
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Cancel([FromRoute] int id, Appointment2Dto request)
        {
            return await _appointmentService.Cancel(id, request);
        }

        [HttpGet("get-all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAdmin()
        {
            return await _appointmentService.GetAll();
        }


        [HttpPut("updateStatus/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, Appointment1Dto request)
        {
            return await _appointmentService.UpdateAdmin(id, request);
        }
    }
}
