namespace Domain;

public class Company
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Contacts { get; init; }
    public bool IsActive { get; set; }
    
    [System.Text.Json.Serialization.JsonIgnore]
    public ICollection<User> Users { get; set; }
    
    [System.Text.Json.Serialization.JsonIgnore]
    public ICollection<License> Licenses { get; set; }
}