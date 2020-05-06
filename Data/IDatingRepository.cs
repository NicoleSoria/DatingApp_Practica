using DatingApp.API.Helpers;
using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T Entity) where T : class;
        void Delete<T>(T Entity) where T : class;
        Task<bool> SaveAll();
        Task<PagedList<User>> GetUsers(UserParams userParams );
        Task<User> GetUser(int Id);
        Task<Fotos> GetFoto(int Id);
        Task<Fotos> GetFotoPrincipal(int userId);
        Task<Like> GetLike(int userId, int destinatarioId);
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetMessageUsuario(MessageParams messageParams);
        Task<IEnumerable<Message>> GetConversacion(int userId, int destinatarioId);

    }
}
