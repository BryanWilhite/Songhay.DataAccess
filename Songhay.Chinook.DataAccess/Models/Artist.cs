namespace Songhay.Chinook.DataAccess.Models;

public record Artist
{
    public int ArtistId { get; init; }

    public string? Name { get; init; }
}