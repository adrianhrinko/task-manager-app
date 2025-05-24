using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Domain;

public class User(Guid id, string email, string firstName, string lastName)
{
    public Guid Id { get; set; } = id;
    public string Email { get; set; } = email;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
}