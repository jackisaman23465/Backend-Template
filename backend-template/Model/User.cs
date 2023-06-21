using System.ComponentModel.DataAnnotations;

namespace backend_template.Model;

public class User
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string? Password { get; set; }
}