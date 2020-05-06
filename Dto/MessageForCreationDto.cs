using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Dto
{
    public class MessageForCreationDto
    {
        public int EmisorId { get; set; }
        public int DestinatarioId { get; set; }
        public DateTime FechaEnviado { get; set; }
        public string Contenido { get; set; }
        public MessageForCreationDto()
        {
            FechaEnviado = DateTime.Now;
        }
    }
}
