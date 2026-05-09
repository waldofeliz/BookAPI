namespace Domain.Entities;

public class Author
{
    public Guid Id { get; private set; } =  Guid.NewGuid();
    public string FirstName { get; private set; } = default;
    public string LastName { get; private set; } = default;


    private Author() {} //EF Core
    
    
}