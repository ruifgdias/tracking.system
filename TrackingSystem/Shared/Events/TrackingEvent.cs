namespace Shared.Events;

public record TrackingEvent(string IpAddress, string referrer, string UserAgent, string VisitDatetime);