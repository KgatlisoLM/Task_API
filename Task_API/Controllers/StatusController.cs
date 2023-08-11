using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Task_API.Data;
using Task_API.Models;
using Task_API.Models.Dto;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly ApiResponse _response;

        public StatusController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<IActionResult> GetStatus()
        {
            var status = await _db.Statuses.ToListAsync();
            var data = _mapper.Map<List<Status>, List<StatusDto>>(status);
            _response.Result = data;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
