namespace Domain.Publication.Models.Commands;

public record UpdateImageListCommand(
    int PublicationId = 0,
    List<string>? ImageList = null
    );