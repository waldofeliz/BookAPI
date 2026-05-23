namespace Domain.Entities;

public class Author
{
    public Guid Id { get; private set; } =  Guid.NewGuid();
    public string FirstName { get; private set; } = default;
    public string LastName { get; private set; } = default;
    public string? Biography { get; private set; }
    public DateTime BirthDate { get; private set; } = default;
    public bool State { get; private set; } = default;
    public string CreateBy { get; private set; }
    public DateTime CreateOn { get; private set; }
    public string? ModifiedBy { get; private set; }
    public DateTime ModifiedOn { get; private set; }
    public int Version { get; private set; }
    
    private Author() {} //EF Core

    public Author(string firstName, string lastName, string biography,  DateTime birthDate, string createBy)
    {
        
    }
    
    
}