using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dto;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[Controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _repo.GetUser(currentUserId);

            userParams.UserId = currentUserId;

            if (string.IsNullOrEmpty(userParams.Genero))
            {
                userParams.Genero = userFromRepo.Genero == "male" ? "female" : "male";
            }

            var users = await _repo.GetUsers(userParams);

            var usuarios = _mapper.Map<IEnumerable<UserForListDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usuarios);
        }

        [HttpGet("{Id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int Id)
        {
            var user = await _repo.GetUser(Id);

            var usuario = _mapper.Map<UserForDetailedDto>(user);

            return Ok(usuario);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userUpdate)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userUpdate, userFromRepo);
            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Guardar cambios del usuario {id} fallo");
        }

        [HttpPost("{id}/like/{destinatarioId}")]
        public async Task<IActionResult> LikeUser(int id, int destinatarioId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var like = await _repo.GetLike(id, destinatarioId);

            if (like != null)
            {
                return BadRequest("Ya le diste MG al usuario");
            }

            if( await _repo.GetUser(destinatarioId) == null)
            {
                return NotFound();
            }

            like = new Like
            {
                LikerId = id,
                LikeeId = destinatarioId
            };

            _repo.Add<Like>(like);

            if(await _repo.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Error al dar MG al usuario");
        }

    }
}
