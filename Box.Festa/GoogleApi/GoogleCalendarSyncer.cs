using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using static Google.Apis.Auth.OAuth2.Web.AuthorizationCodeWebApp;

namespace Box.Festa.GoogleApi
{
    public class GoogleCalendarSyncer
    {
        public static string GetOauthTokenUri(Controller controller)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Debug("-------------------------------controller"+controller);

            var authResult = GetAuthResult(controller);
            return authResult.RedirectUri;
        }



        private static AuthResult GetAuthResult(Controller controller)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Debug("-------------------------------getAuthResult");
            var dataStore = new DataStore();
            var clientID = WebConfigurationManager.AppSettings["GoogleClientID"];
            var clientSecret = WebConfigurationManager.AppSettings["GoogleClientSecret"];
            var appFlowMetaData = new GoogleAppFlowMetaData(dataStore, clientID, clientSecret);
            var factory = new AuthorizationCodeMvcAppFactory(appFlowMetaData, controller);
            var cancellationToken = new CancellationToken();
            var authCodeMvcApp = factory.Create();
            var authResultTask = authCodeMvcApp.AuthorizeAsync(cancellationToken);

            authResultTask.Wait();
            var result = authResultTask.Result;
            if (string.IsNullOrEmpty(result.RedirectUri))
            {
                var revokeTask = result.Credential.RevokeTokenAsync(cancellationToken);
                revokeTask.Wait();
                if (revokeTask.IsCompleted)
                {
                    authResultTask = authCodeMvcApp.AuthorizeAsync(cancellationToken);
                    authResultTask.Wait();
                    result = authResultTask.Result;
                }
            }
            
            return result;
        }

   /*     private static CalendarService InitializeService(AuthResult authResult)
        {
            var result = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = authResult.Credential,
                ApplicationName = "Box Festa"
            });
            return result;
        }

        private static string GetMainCalendarId(CalendarService service)
        {
            var calendarListRequest = new CalendarListResource.ListRequest(service);
            var calendars = calendarListRequest.Execute();
            var result = calendars.Items.First().Id;
            return result;
        }

        private static Event GetCalendarEvent()
        {
            var result = new Event();
            result.Summary = "Test Calendar Event Summary";
            result.Description = "Test Calendar Event Description";
            result.Sequence = 1;
            var eventDate = new EventDateTime();
            eventDate.DateTime = DateTime.UtcNow;
            result.Start = eventDate;
            result.End = eventDate;
            return result;
        }

        private static void SyncCalendarEventToCalendar(CalendarService service, Event calendarEvent, string calendarId)
        {
            var eventRequest = new EventsResource.InsertRequest(service, calendarEvent, calendarId);
            eventRequest.Execute();
        }*/
    }
}