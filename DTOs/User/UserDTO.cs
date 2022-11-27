using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace to_do_app_dotnet.DTOs.User
{
    public class UserDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}