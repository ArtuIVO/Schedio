using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Util.Store;

public class GoogleCalendarService
{
#pragma warning disable CS8632 // La anotación para tipos de referencia que aceptan valores NULL solo debe usarse en el código dentro de un contexto de anotaciones "#nullable".
    private CalendarService? _calendarService;
#pragma warning restore CS8632 // La anotación para tipos de referencia que aceptan valores NULL solo debe usarse en el código dentro de un contexto de anotaciones "#nullable".

    public void InitializeService()
    {
        var credentialsJson = @"
        {
            ""web"": {
                ""client_id"": ""300468690395-1kv84dlu3gk2uagbevc2t0qtkla18mah.apps.googleusercontent.com"",
                ""project_id"": ""schedio-440117"",
                ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
                ""token_uri"": ""https://oauth2.googleapis.com/token"",
                ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
                ""client_secret"": ""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""
            }
        }";

        var credential = GoogleCredential.FromJson(credentialsJson)
            .CreateScoped(CalendarService.Scope.Calendar);

        _calendarService = new CalendarService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "SCHEDIO",
        });
    }

    public async Task CreateScheduledEvent(string summary, string description, DateTime startDateTime, DateTime endDateTime, string timeZone, string email)
    {
        if (_calendarService == null)
        {
            throw new InvalidOperationException("Calendar service is not initialized.");
        }

        var newEvent = new Event()
        {
            Summary = summary,
            Description = description,
            Start = new EventDateTime()
            {
                DateTimeRaw = startDateTime.ToString("o"),
                TimeZone = timeZone
            },
            End = new EventDateTime()
            {
                DateTimeRaw = endDateTime.ToString("o"),
                TimeZone = timeZone
            },
            Attendees = new List<EventAttendee>()
            {
                new EventAttendee() { Email = email }
            },
            Reminders = new Event.RemindersData()
            {
                UseDefault = false,
                Overrides = new List<EventReminder>()
                {
                    new EventReminder() { Method = "email", Minutes = 30 },
                    new EventReminder() { Method = "popup", Minutes = 10 }
                }
            }
        };

        await _calendarService.Events.Insert(newEvent, "primary").ExecuteAsync();
    }
}
