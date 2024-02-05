namespace Shared.Events;

public record TrackingEvent(string IpAddress, string Referrer, string UserAgent, string VisitDatetime);