using System.Net;
using Microsoft.AspNetCore.Connections.Features;

namespace PixelService.Tests.Fakes
{

    public class DefaultConnectionInfo : IConnectionIdFeature
    {
        public IPAddress RemoteIpAddress { get; set; }
        public IPAddress LocalIpAddress { get; set; }
        public int RemotePort { get; set; }
        public int LocalPort { get; set; }
        public string ConnectionId { get; set; }
    }
}
