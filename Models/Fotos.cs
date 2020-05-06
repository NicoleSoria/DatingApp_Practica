using System;

namespace DatingApp.API.Models
{
    public class Fotos
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public bool EsPrincipal { get; set; }
        public string PublicId { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }

    }
}