using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using Task_API.Data;
using Task_API.Helpers;
using Task_API.Models;
using Task_API.Models.Dto;
using Task_API.Utility;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {

        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly ApiResponse _response;


        public TasksController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ApiResponse();
        }


        [HttpGet]
        public async Task<IActionResult> GetTasks(string searchString, int pageNumber = 1, int pageSize = 5)
        {
            IEnumerable<TaskDo> tasks = await _db.Tasks.Include(c => c.Status).ToListAsync();


            if (!string.IsNullOrEmpty(searchString))
            {
                tasks = tasks.Where(u => u.Title.ToLower().Contains(searchString.ToLower()));
            }

            Pagination pagination = new()
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalRecords = tasks.Count()
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));

            var data = _mapper.Map<IEnumerable<TaskDo>, List<TaskDto>>(tasks);

            var page = data.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            _response.Result = page;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }


        [HttpGet("{id:int}", Name = "GetTask")]
        public async Task<IActionResult> GetTask(int id)
        {
            if(id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            TaskDo taskDo = await _db.Tasks.Include(c => c.Status).FirstOrDefaultAsync(x => x.Id == id);

            if( taskDo == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }

            _response.Result = _mapper.Map<TaskDo, TaskDto>(taskDo);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse>> PostTask([FromForm] TaskCreateDto taskCreateDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _mapper.Map<TaskCreateDto, TaskDo>(taskCreateDto);

                    _db.Tasks.Add(data);
                    await _db.SaveChangesAsync();
                    _response.Result = data;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtAction("GetTask", new { id = data.Id }, _response);
                }else
                {
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }

            return _response;
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateTask([FromForm] TaskUpdateDto taskUpdateDto, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(taskUpdateDto == null || id != taskUpdateDto.Id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    TaskDo taskDoFromDb = await _db.Tasks.FindAsync(id);
                    if (taskDoFromDb == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    taskDoFromDb.Title = taskUpdateDto.Title;
                    taskDoFromDb.Description = taskUpdateDto.Description;
                    taskDoFromDb.StatusId = taskUpdateDto.StatusId;

                    _db.Tasks.Update(taskDoFromDb);
                    await _db.SaveChangesAsync();
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);

                }
                else
                {
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };

            }

            return _response;
        }




        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteTask(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                TaskDo task = await _db.Tasks.FindAsync(id);

                if (task == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                _db.Tasks.Remove(task);
                await _db.SaveChangesAsync();
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }

            return _response;
        }


    }
}
