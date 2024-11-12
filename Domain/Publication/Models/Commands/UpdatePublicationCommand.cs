namespace Domain.Publication.Models.Commands;

public record UpdatePublicationCommand(
    int UserId = 0,
    int PublicationId = 0,
    string Title = "",
    string Description = "",
    float Price = 0f,
    string Location = "",
    int PropertyType = 0,
    int OperationType = 0,
    int Bathrooms = 0,
    int AntiqueType = 0,
    int Size = 0,
    int Rooms = 0,
    int Garages = 0
    );