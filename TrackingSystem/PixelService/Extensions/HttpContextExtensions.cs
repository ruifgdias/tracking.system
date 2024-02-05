using System.Globalization;
using Shared.Events;

namespace PixelService.Extensions
{
    public static class HttpContextExtensions
    {
        public static TrackingEvent GetTrackingEvent(this HttpContext context)
        {
            var referrer = context.Request.Headers.Referer;
            var userAgent = context.Request.Headers.UserAgent;
            var ipAddress = context.Connection.RemoteIpAddress?.MapToIPv4();

            return new TrackingEvent(ipAddress?.ToString(), referrer, userAgent, DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture));
        }
    }
}
