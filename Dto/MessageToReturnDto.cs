using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Dto
{
    public class MessageToReturnDto
    {
        public int Id { get; set; }
        public int EmisorId { get; set; }
        public string NombreEmisor { get; set; }
        public string FotoEmisor { get; set; }
        public int DestinatarioId { get; set; }
        public string NombreDestinatario { get; set; }
        public string FotoDestinatario { get; set; }
        public string Contenido { get; set; }
        public bool Leido { get; set; }
        public DateTime? FechaLeido { get; set; }
        public DateTime FechaEnviado { get; set; }
    }
}
