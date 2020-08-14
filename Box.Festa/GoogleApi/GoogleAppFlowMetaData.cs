using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Calendar.v3;
using Google.Apis.Plus.v1;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Box.Festa.GoogleApi
{
    public class GoogleAppFlowMetaData : FlowMetadata
    {
        private IAuthorizationCodeFlow flow { get; set; }

        public override IAuthorizationCodeFlow Flow
        {
            get { return flow; }
        }

        public GoogleAppFlowMetaData(IDataStore dataStore, string clientID, string clientSecret)
        {
            var flowInitializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = clientID,
                    ClientSecret = clientSecret
                },
                Scopes = new[] {
                    PlusService.Scope.PlusLogin,
                    PlusService.Scope.PlusMe,
                  PlusService.Scope.UserinfoEmail,
    PlusService.Scope.UserinfoProfile },
    //CalendarService.Scope.Calendar },
                DataStore = dataStore
            };
            flow = new GoogleAuthorizationCodeFlow(flowInitializer);
           
        }

        public override string GetUserId(Controller controller)
        {
            return "usuarioBoxFesta";
        }

        public override string AuthCallback
        {
            get
            {
                return @"/AuthCallback/IndexAsync";
            }
        }
    }
}