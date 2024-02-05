using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http;
using PixelService.Extensions;
using PixelService.Tests.Fakes;

namespace PixelService.Tests.ExtensionsTests
{
    [TestClass]
    public class HttpContextExtensionsTests
    {
        [TestMethod]
        public void GetTrackingEvent_DefaultHttpContext_ReturnTrackingEventJustWithVisitDate()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            // Act
            var trackingEvent = httpContext.GetTrackingEvent();

            // Assert
            trackingEvent.Should().NotBeNull();
            trackingEvent.IpAddress.Should().BeNullOrEmpty();
            trackingEvent.Referrer.Should().BeNullOrEmpty();
            trackingEvent.UserAgent.Should().BeNullOrEmpty();
            trackingEvent.VisitDatetime.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void GetTrackingEvent_ValidDefaultHttpContext_ReturnTrackingEvent()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            const string validReferer = "ValidReferer";
            const string validUserAgent = "ValidUserAgent";
            const string validIpString = "192.168.0.1";
            httpContext.Request.Headers.Add("Referer", validReferer);
            httpContext.Request.Headers.Add("User-Agent", validUserAgent);

            var ipAddress = IPAddress.Parse(validIpString);
            httpContext.Connection.RemoteIpAddress = ipAddress;
            var connectionInfo = new DefaultConnectionInfo();
            connectionInfo.RemoteIpAddress = ipAddress;
            httpContext.Features.Set<IConnectionIdFeature>(connectionInfo);

            // Act
            var trackingEvent = httpContext.GetTrackingEvent();

            // Assert
            trackingEvent.Should().NotBeNull();
            trackingEvent.IpAddress.Should().Be(validIpString);
            trackingEvent.Referrer.Should().Be(validReferer);
            trackingEvent.UserAgent.Should().Be(validUserAgent);
            trackingEvent.VisitDatetime.Should().NotBeNullOrEmpty();
        }
    }
}
