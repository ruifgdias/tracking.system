namespace PixelService.Extensions;

public static partial class ApiResults
{
    private readonly static string base64String = "R0lGODlhAQABAIAAAP///wAAACwAAAAAAQABAAACAkQBADs=";
    private readonly static byte[] gifBytes = Convert.FromBase64String(base64String);

    public static IResult EmptyGif()
    {
        var memoryStream = new MemoryStream(gifBytes);
        return Results.Stream(memoryStream, "image/gif", "px.gif"); ;
    }
}
