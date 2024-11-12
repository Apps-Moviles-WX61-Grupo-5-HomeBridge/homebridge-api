namespace Domain.IAM.Models.Commands;

public record UpdateUserCommand(
    int UserId = 0,
    string Name = "",
    string Description = "",
    string PhotoUrl = ""
);