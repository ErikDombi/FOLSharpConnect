using Ical.Net.CalendarComponents;

namespace FOLSharpConnect.API.Models;

public class CalEvent
{
    public CalEvent(CalendarEvent @event)
    {
        Name = @event.Summary;
        Class = @event.Location;
        Start = @event.DtStart.AsDateTimeOffset.AddHours(-4);
        End = @event.DtEnd.AsDateTimeOffset.AddHours(-4);
        Type = @event.Summary switch
        {
            string a when a.EndsWith(" - Availability Ends") => EventType.AvailabilityEnds,
            string b when b.EndsWith(" - Quiz Due") => EventType.QuizDue,
            string c when c.EndsWith(" - Content Available") => EventType.ContentAvailable,
            string d when d.EndsWith(" - Discussion Due") => EventType.DiscussionDue,
            string e when e.EndsWith(" - Due") => EventType.SubmissionDue,
            _ => EventType.Other
        };
        Summary = Type == EventType.Other ? Name : Name.Substring(0, Name.LastIndexOf('-')).Trim();
    }
    
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Class { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public EventType Type { get; set; }
}