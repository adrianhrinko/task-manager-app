
using Shared.Domain;
using UserService.Domain.Clients;
using UserService.Domain.Entities;
using UserService.Domain.PubSub;
using UserService.Domain.Repositories;
using UserService.Domain.Services;

namespace UserService.Application.User;

public class UserService : IUserService
{
    private readonly IAuthProviderClient _authProviderClient;
    private readonly IUserRepository _userRepo;
    private readonly IMessagePublisher _messagePublisher;

    public UserService(IAuthProviderClient authProviderClient, IUserRepository userRepo, IMessagePublisher messagePublisher)
    {
        _authProviderClient = authProviderClient;
        _userRepo = userRepo;
        _messagePublisher = messagePublisher;
    }

    public async Task<Domain.Entities.User?> RegisterUserAsync(UserRegistration registration, CancellationToken ct)
    {
        var regResult = await _authProviderClient.RegisterUserAsync(registration, ct);

        if (regResult is not RegistrationResult.Success)
        {
            return null;
        }
        
        var now = DateTime.UtcNow;

        var newUser = new Domain.Entities.User(Guid.Empty, registration.Email, registration.FirstName,
            registration.LastName, now, now);
        
        var user = await _userRepo.CreateAsync(newUser, ct);

        await _messagePublisher.PublishEvent(new PubSubMessage<Shared.Domain.Entities.User>(user, MessageAction.Create), ct);

        return user;
    }
}