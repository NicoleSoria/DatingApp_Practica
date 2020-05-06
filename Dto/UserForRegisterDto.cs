using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Dto
{
    public class UserForRegisterDto
    {
        [Required]
        public string username { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 4, ErrorMessage ="La contraseña debe tener entre 4 a 10 caracteres")]
        public string password { get; set; }

        [Required]
        public string genero { get; set; }

        [Required]
        public string knowAs { get; set; }

        [Required]
        public DateTime fechaNacimiento { get; set; }

        [Required]
        public string ciudad { get; set; }

        [Required]
        public string pais { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime ultimaVezActivo { get; set; }

        public UserForRegisterDto()
        {
            FechaRegistro = DateTime.Now;
            ultimaVezActivo = DateTime.Now;
        }

    }
}
