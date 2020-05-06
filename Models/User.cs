using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Genero { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string KnowAs { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime UltimaVezActivo { get; set; }
        public string Introduccion { get; set; }
        public string Buscando { get; set; }
        public string Intereses { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public virtual ICollection<Fotos> Fotos { get; set; }

        public virtual ICollection<Like> Likers { get; set; }
        public virtual ICollection<Like> Likees { get; set; }
        public virtual  ICollection<Message> MensajeRecibido { get; set; }
        public virtual ICollection<Message> MensajeEnviado { get; set; }


    }
}
