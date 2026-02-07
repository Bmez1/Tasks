using SharedKernel;

namespace Domain.Users;

public sealed class User : Entity
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string PasswordHash { get; private set; }

    private User() { }

    private User(Guid id, string email, string firstName, string lastName, string passwordHash)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PasswordHash = passwordHash;
    }

    public static User Create(string email, string firstName, string lastName, string passwordHash)
    {
        return new User(Guid.NewGuid(), email, firstName, lastName, passwordHash);
    }
}
