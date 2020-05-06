using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Dto
{
    public class UserForLoguinDto
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
    }
}
