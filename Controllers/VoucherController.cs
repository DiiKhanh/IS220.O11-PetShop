using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;
using PetShop.Services.VoucherService;

namespace PetShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;
        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] VoucherDto request)
        {
            return await _voucherService.Create(request);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAdmin()
        {
            return await _voucherService.GetAllAdmin();
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return await _voucherService.Delete(id);
        }

        [HttpGet("get/{code}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetCode([FromRoute] string code)
        {
            return await _voucherService.GetCode(code);
        }

        [HttpGet("get-admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCodeAdmin([FromRoute] int id)
        {
            return await _voucherService.GetCodeAdmin(id);
        }

        [HttpPost("edit/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit([FromRoute] int id, VoucherDto request)
        {
            return await _voucherService.Edit(id, request);
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            return await _voucherService.List();
        }
    }
}
