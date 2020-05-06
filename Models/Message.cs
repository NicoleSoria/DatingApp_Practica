using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int EmisorId { get; set; }
        public virtual User Emisor { get; set; }
        public int DestinatarioId { get; set; }
        public virtual User Destinatario { get; set; }
        public string Contenido { get; set; }
        public bool Leido { get; set; }
        public DateTime? FechaLeido { get; set; }
        public DateTime FechaEnviado { get; set; }
        public bool EmisorEliminar { get; set; }
        public bool DestinatarioEliminar { get; set; }

    }
}
