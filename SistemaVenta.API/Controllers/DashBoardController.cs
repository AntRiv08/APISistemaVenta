using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVenta.BLL.Servicios;
using SistemaVenta.API.Utilidad;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashBoardService _dashboardService;
        public DashBoardController(IDashBoardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet]
        [Route("Resumen")]
        public async Task<IActionResult> Resumen()
        {
            var response = new Response<DashBoardDTO>();
            try
            {
                response.status = true;
                response.value = await _dashboardService.Resumen();
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }
    }
}
