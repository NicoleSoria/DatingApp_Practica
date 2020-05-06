using AutoMapper;
using DatingApp.API.Dto;
using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.FotoUrl, op =>
              {
                  op.MapFrom(scr => scr.Fotos.FirstOrDefault(p => p.EsPrincipal).Url);
              })
               .ForMember(dest => dest.Edad, op =>
              {
                  op.ResolveUsing(d => d.FechaNacimiento.CalcularEdad());
              });

            CreateMap<User, UserForDetailedDto>()
                .ForMember(des => des.FotoUrl, op =>
               {
                   op.MapFrom(scr => scr.Fotos.FirstOrDefault(p => p.EsPrincipal).Url);
               })
                .ForMember(dest => dest.Edad , op =>
                {
                    op.ResolveUsing(d => d.FechaNacimiento.CalcularEdad());
                });

            CreateMap<Fotos, PhotosForDetailedDto>();

            CreateMap<UserForUpdateDto, User>();

            CreateMap<Fotos, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Fotos>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDto>()
                .ForMember(u => u.NombreEmisor, opt => 
                            opt.MapFrom(u => u.Emisor.KnowAs))
                .ForMember(m => m.FotoEmisor, opt =>  
                        opt.MapFrom(u => u.Emisor.Fotos.FirstOrDefault(p => p.EsPrincipal).Url))
                .ForMember( u => u.NombreDestinatario, opt => 
                        opt.MapFrom( u => u.Destinatario.KnowAs))
                .ForMember(m => m.FotoDestinatario, opt =>
                    opt.MapFrom(u => u.Destinatario.Fotos.FirstOrDefault(p => p.EsPrincipal).Url));
        }
    }
}
