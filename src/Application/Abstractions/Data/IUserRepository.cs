using Domain.Users;
using SharedKernel;

namespace Application.Abstractions.Data;

public interface IUserRepository
{
    Task<bool> IsEmailUniqueAsync(string email);
    void Add(User user);
    Task<User?> GetByIdAsync(Guid id);
}
