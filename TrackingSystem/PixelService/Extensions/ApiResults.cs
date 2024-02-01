namespace PixelService.Extensions;

public static partial class ApiResults
{
    public static IResult EmptyGif()
    {
        var base64String = "R0lGODlhAQABAIAAAP///wAAACwAAAAAAQABAAACAkQBADs=";
        var gifBytes = Convert.FromBase64String(base64String);
        var memoryStream = new MemoryStream(gifBytes);

        return Results.Stream(memoryStream, "image/gif", "px.gif");
    }
}
