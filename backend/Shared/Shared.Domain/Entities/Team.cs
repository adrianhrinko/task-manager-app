using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Domain.Entities;

public class Team(Guid id, string name)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
}