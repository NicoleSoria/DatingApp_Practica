using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T Entity) where T : class
        {
            _context.Add(Entity);
        }

        public void Delete<T>(T Entity) where T : class
        {
            _context.Remove(Entity);
        }

        public async Task<IEnumerable<Message>> GetConversacion(int userId, int destinatarioId)
        {
            //var mensaje = await _context.Mensajes
            //    .Include(u => u.Emisor).ThenInclude(p => p.Fotos)
            //    .Include(U => U.Destinatario).ThenInclude(p => p.Fotos)
            //    .Where(m => m.DestinatarioId == userId && m.DestinatarioEliminar == false && m.EmisorId == destinatarioId 
            //            || m.DestinatarioId == destinatarioId  && m.EmisorId == userId && m.EmisorEliminar == false)
            //    .OrderBy(m => m.FechaEnviado)
            //    .ToListAsync();

            var mensaje = await _context.Mensajes
               .Where(m => m.DestinatarioId == userId && m.DestinatarioEliminar == false && m.EmisorId == destinatarioId
                       || m.DestinatarioId == destinatarioId && m.EmisorId == userId && m.EmisorEliminar == false)
               .OrderBy(m => m.FechaEnviado)
               .ToListAsync();

            return mensaje;
        }

        public async Task<Fotos> GetFoto(int Id)
        {
            var photo = await _context.Fotos.FirstOrDefaultAsync(p => p.Id == Id);
            return photo;
        }

        public async Task<Fotos> GetFotoPrincipal(int userId)
        {
            return await _context.Fotos.Where(u => u.UserId == userId)
                        .FirstOrDefaultAsync(p => p.EsPrincipal );
        }

        public async Task<Like> GetLike(int userId, int destinatarioId)
        {
            return await _context.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == destinatarioId);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Mensajes.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetMessageUsuario(MessageParams messageParams)
        {
            //var messages = _context.Mensajes.Include(u => u.Emisor).ThenInclude(p => p.Fotos)
            //                                  .Include(u => u.Destinatario).ThenInclude(p => p.Fotos)
            //                                  .AsQueryable();

            var messages = _context.Mensajes.AsQueryable();

            switch (messageParams.ContenidoMensaje)
            {
                case "Recibidos":
                    messages = messages.Where(u => u.DestinatarioId == messageParams.UserId && u.DestinatarioEliminar == false);
                    break;
                case "Enviados":
                    messages = messages.Where(u => u.EmisorId == messageParams.UserId && u.EmisorEliminar == false);
                    break;

                default:
                    messages = messages.Where(u => u.DestinatarioId == messageParams.UserId && u.DestinatarioEliminar == false && u.Leido == false);
                    break;
            }

            messages = messages.OrderByDescending(d => d.FechaEnviado);
            return await PagedList<Message>.CreatedAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<User> GetUser(int Id)
        {
            //var usuario = await _context.Users.Include(p => p.Fotos).FirstOrDefaultAsync(u => u.Id == Id);
            var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
            return usuario;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            //var usuarios = _context.Users.Include(p => p.Fotos).OrderByDescending(u => u.UltimaVezActivo).AsQueryable();
            var usuarios = _context.Users.OrderByDescending(u => u.UltimaVezActivo).AsQueryable();

            usuarios = usuarios.Where(u => u.Id != userParams.UserId);
            usuarios = usuarios.Where(u => u.Genero == userParams.Genero);

            if (userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                usuarios = usuarios.Where(u => userLikers.Contains(u.Id));
            }

            if (userParams.Likees)
            {
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                usuarios = usuarios.Where(u => userLikees.Contains(u.Id));
            }

            if(userParams.EdadMin != 18 || userParams.EdadMax != 95)
            {
                var minDate = DateTime.Today.AddYears(-userParams.EdadMax - 1);
                var maxDate = DateTime.Today.AddYears(-userParams.EdadMin);

                usuarios = usuarios.Where(u => u.FechaNacimiento >= minDate && u.FechaNacimiento <= maxDate);
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "FechaRegistro":
                        usuarios = usuarios.OrderByDescending(u => u.FechaRegistro);
                        break;

                    default:
                        usuarios = usuarios.OrderByDescending(u => u.UltimaVezActivo);
                        break;
                }
            }

            return await PagedList<User>.CreatedAsync(usuarios,userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }


        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
        {
            //var user = await _context.Users.Include(x => x.Likers)
            //                               .Include(x => x.Likees)
            //                               .FirstOrDefaultAsync(u => u.Id == id);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (likers)
            {
                return user.Likers.Where(u => u.LikeeId == id).Select(i => i.LikerId);
            }
            else
            {
                return user.Likees.Where(u => u.LikerId == id).Select(i => i.LikeeId);
            }
        }
    }
}
