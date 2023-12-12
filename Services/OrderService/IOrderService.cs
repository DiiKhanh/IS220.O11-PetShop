using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;
using PetShop.Models;

namespace PetShop.Services.OrderService
{
    public interface IOrderService
    {
        Task<IActionResult> createOrder(ShipInfo request);
        Task<IActionResult> getAllOrder();
        Task<IActionResult> getAllOrderAdmin();
        Task<IActionResult> cancelOrder(int oid);
        Task<IActionResult> getOrder(int oid);
        Task<IActionResult> getOrderAdmin(int oid);
        Task<IActionResult> updateOrder(int oid,OrderDto request);
        Task<IActionResult> updateShipInfo(UpdateShipInfoDto request, int sid);
    }
}
