using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Dto
{
    public class UserForListDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Genero { get; set; }
        public int Edad { get; set; }
        public string KnowAs { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime UltimaVezActivo { get; set; }
        public string Introduccion { get; set; }
        public string Buscando { get; set; }
        public string Intereses { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public string FotoUrl { get; set; }
    }
}
