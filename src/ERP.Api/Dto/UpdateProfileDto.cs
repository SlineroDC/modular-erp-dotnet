namespace ERP.Api.DTo;

public class UpdateProfileDto
{
    // Data personal
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    // Change password
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
}
