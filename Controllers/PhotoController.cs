using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Dto;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/user/{userId}/photos")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotoController(IDatingRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _repo = repo;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.Apisecret
               );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetFoto")]
        public async Task<IActionResult> GetFoto(int id)
        {
            var photoFromRepo = _repo.GetFoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();


            if(file.Length > 0)
            {
                using(var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                };
            };

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Fotos>(photoForCreationDto);

            if (!userFromRepo.Fotos.Any(u => u.EsPrincipal))
                photo.EsPrincipal = true;

            userFromRepo.Fotos.Add(photo);

            if (await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);

                return CreatedAtRoute("GetFoto", new { userId = userId, id = photo.Id }, photoToReturn);
            }

            return BadRequest("No se pudo agregar la foto");
        }


        [HttpPost("{id}/setearPrincipal")]
        public async Task<IActionResult> setearPrincipal(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _repo.GetUser(userId);

            if (!user.Fotos.Any(p => p.Id == id))
            {
                return Unauthorized();
            }

            var fotoRepo = await _repo.GetFoto(id);

            if (fotoRepo.EsPrincipal)
            {
                return BadRequest("La foto ya es la principal");
            }

            var fotoPrincipalActual = await _repo.GetFotoPrincipal(userId);

            fotoPrincipalActual.EsPrincipal = false;

            fotoRepo.EsPrincipal = true;

            if (await _repo.SaveAll())
                return NoContent();

            return BadRequest("No se pudo poner la foto como principal");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _repo.GetUser(userId);

            if (!user.Fotos.Any(p => p.Id == id))
            {
                return Unauthorized();
            }

            var fotoRepo = await _repo.GetFoto(id);

            if (fotoRepo.EsPrincipal)
            {
                return BadRequest("La foto es principal");
            }

            if(fotoRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(fotoRepo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    _repo.Delete(fotoRepo);
                }
            }

            if(fotoRepo.PublicId == null)
            {
                _repo.Delete(fotoRepo);
            }

            if( await _repo.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Error al eliminar foto");

        }
    }
}
