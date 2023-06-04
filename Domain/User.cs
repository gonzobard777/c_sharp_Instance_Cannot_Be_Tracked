namespace Domain;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string Phone { get; set; }
    public bool IsActive { get; set; }

    public int RoleId { get; set; }
    public Role? Role { get; set; }

    public int CompanyId { get; set; }
    public Company? Company { get; set; }
}