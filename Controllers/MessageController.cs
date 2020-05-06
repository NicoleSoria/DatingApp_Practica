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
    [Route("api/users/{userId}/[Controller]")]
    [ApiController]
    public class MessageController: ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public MessageController(IDatingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var mensajeRepo = await _repo.GetMessage(id);

            if (mensajeRepo == null)
                return NotFound();

            return Ok(mensajeRepo);
        }

        [HttpGet]
        public async Task<IActionResult> GetMensajeUsuario(int userId, [FromQuery]MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageParams.UserId = userId;

            var mensajeRepo = await _repo.GetMessageUsuario(messageParams);

            var mensaje = _mapper.Map<IEnumerable<MessageToReturnDto>>(mensajeRepo);

            Response.AddPagination(mensajeRepo.CurrentPage, mensajeRepo.PageSize, mensajeRepo.TotalCount, mensajeRepo.TotalPages);

            return Ok(mensaje);
        }

        [HttpGet("conversacion/{destinatarioId}")]
        public async Task<IActionResult> GetConversacion(int userId, int destinatarioId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var conversacionRepo = await _repo.GetConversacion(userId, destinatarioId);

            var conversacion = _mapper.Map<IEnumerable<MessageToReturnDto>>(conversacionRepo);

            return Ok(conversacion);
        }

        [HttpPost]
        public async Task<IActionResult> CrearMensaje(int userId, MessageForCreationDto mensajeCreado)
        {
            var emisor = await _repo.GetUser(userId);

            if (emisor.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            mensajeCreado.EmisorId = userId;
   
            var destinatario = await _repo.GetUser(mensajeCreado.DestinatarioId);

            if (destinatario == null)
            {
                return BadRequest("No se ecnontro el usuario");
            }

            var mensaje = _mapper.Map<Message>(mensajeCreado);

            _repo.Add(mensaje);


            if (await _repo.SaveAll())
            {
                var mensajeReturn = _mapper.Map<MessageToReturnDto>(mensaje);

                return CreatedAtRoute("GetMessage", new { userId, id = mensaje.Id }, mensajeReturn);
            }

            throw new Exception("Error al guardar el mensaje");

        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMensaje(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var mensajeRepo = await _repo.GetMessage(id);

            if (mensajeRepo.EmisorId == userId)
                mensajeRepo.EmisorEliminar = true;

            if (mensajeRepo.DestinatarioId == userId)
                mensajeRepo.DestinatarioEliminar = true;

            if(mensajeRepo.EmisorEliminar && mensajeRepo.DestinatarioEliminar)
            {
                _repo.Delete(mensajeRepo);
            }

            if(await _repo.SaveAll())
            {
                return NoContent();
            }

            throw new Exception("Error al eliminar mensaje");
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> LeerMensaje(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var mensajeRepo = await _repo.GetMessage(id);

            if (mensajeRepo.DestinatarioId != userId)
            {
                return Unauthorized();
            }

            mensajeRepo.Leido = true;
            mensajeRepo.FechaLeido = DateTime.Now;

            if(await _repo.SaveAll())
            {
                return NoContent();
            }

            throw new Exception("No se pudo leer el mensaje");
        }
    }
}
