using System.ComponentModel.DataAnnotations;

namespace to_do_app_dotnet.DTOs;

public class PasswordDTO {
    [Required]
    public string OldPassword {get;set;}
    [Required]
    public string NewPassword {get;set;}
}