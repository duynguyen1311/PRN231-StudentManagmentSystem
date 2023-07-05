using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagingSystem_API.DTO;
using System.Security.Claims;

namespace StudentManagingSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PointController : ControllerBase
    {
        private readonly IPointRepository _repository;
        private readonly IMapper _mapper;

        public PointController(IPointRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        private string? GetUserIdFromConext()
        {
            var a = User.FindFirstValue(ClaimTypes.Sid);
            return a;
        }

        private string? GetNameFromConext()
        {
            return User.FindFirstValue(ClaimTypes.Name);
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] PointAddRequest rq)
        {
            try
            {
                rq.Id = Guid.NewGuid();
                rq.CreatedBy = GetNameFromConext();
                rq.CreatedDate = DateTime.Now;
                var map = _mapper.Map<Point>(rq);
                await _repository.Add(map);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] PointUpdateRequest rq)
        {
            try
            {
                rq.LastModifiedDate = DateTime.Now;
                rq.LastModifiedBy = GetNameFromConext();
                var map = _mapper.Map<Point>(rq);
                await _repository.Update(map);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] Guid Id)
        {
            try
            {
                await _repository.Delete(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] PointSearchRequest rq)
        {
            try
            {
                rq.studentId = Guid.Parse(GetUserIdFromConext());
                var res = await _repository.Search(rq.keyword, rq.semester, rq.subjectId, rq.studentId, rq.page, rq.pagesize);
                var map = _mapper.Map<PagedList<PointResponse>>(res);
                return Ok(map);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("detail")]
        public async Task<IActionResult> GetDetail([FromQuery] Guid Id)
        {
            try
            {
                var res = await _repository.GetById(Id);
                var map = _mapper.Map<PointResponse>(res);
                return Ok(map);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
