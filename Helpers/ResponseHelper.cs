using Microsoft.AspNetCore.Mvc;

namespace PetShop.Helpers
{
    public static class ResponseHelper
    {
        public static IActionResult Error()
        {
            return new ObjectResult(new { status = 500, message = "Oops! Something wrong!" })
            {
                StatusCode = 500
            };
        }

        public static IActionResult BadRequest(string message)
        {
            return new ObjectResult(new { status = 400, message })
            {
                StatusCode = 400
            };
        }

        public static IActionResult Ok(object data)
        {
            return new ObjectResult(data)
            {
                StatusCode = 200
            };
        }

        public static IActionResult Created(object data)
        {
            return new ObjectResult(data)
            {
                StatusCode = 201
            };
        }

        public static IActionResult Unauthorized()
        {
            return new ObjectResult(new { status = 401, message = "Unauthorized" })
            {
                StatusCode = 401
            };
        }

        public static IActionResult NotFound()
        {
            return new ObjectResult(new { status = 404, message = "Resource not found" })
            {
                StatusCode = 404
            };
        }
    }
}
