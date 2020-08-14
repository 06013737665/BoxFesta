using Box.Festa.GoogleApi;
using Box.Festa.Models;
using Box.Festa.Negocio;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Plus.v1;
using Google.Apis.Plus.v1.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Box.Festa.Controllers
{
    public class AuthCallbackController : Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController
    {
        protected override FlowMetadata FlowData
        {
            get { return flowMetaData; }
        }

        private FlowMetadata flowMetaData { get; }

        public AuthCallbackController()
        {
            var dataStore = new DataStore();
            var clientID = WebConfigurationManager.AppSettings["GoogleClientID"];
            var clientSecret = WebConfigurationManager.AppSettings["GoogleClientSecret"];
            flowMetaData = new GoogleAppFlowMetaData(dataStore, clientID, clientSecret);
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Debug("-------------------------------Contrutor AUthCalback");
        }

        public AuthCallbackController(FlowMetadata flow)
        {
            flowMetaData = flow;
        }

        public override async Task<ActionResult> IndexAsync(AuthorizationCodeResponseUrl authorizationCode, CancellationToken taskCancellationToken)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                log.Debug("-------------------------------IdenxAsync");
                if (string.IsNullOrEmpty(authorizationCode.Code))
                {
                    var errorResponse = new TokenErrorResponse(authorizationCode);

                    return OnTokenError(errorResponse);
                }

                var returnUrl = Request.Url.ToString();
                returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("?"));

                var token = await Flow.ExchangeCodeForTokenAsync(UserId, authorizationCode.Code, returnUrl,
                    taskCancellationToken);

                UserCredential credentials = new UserCredential(this.FlowData.Flow, UserId, token);
                var urlAuth = "https://www.googleapis.com/oauth2/v2/userinfo?access_token=" + token.AccessToken;
                 HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(urlAuth);
                UsuarioGoogle googleUser = new UsuarioGoogle();
                if (response.IsSuccessStatusCode)
                {
                    googleUser = await response.Content.ReadAsAsync<UsuarioGoogle>();
                }
                

                /*var plusService = new PlusService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credentials,
                    ApplicationName = "Box Festa",
                });*/
                
                 log.Debug("-------------------------------Plus service");
                Sacola sacola = (Sacola)HttpContext.Session["sacola"];
                Usuario usuario = new Usuario();
                //BaseClientService.Initializer ini = new BaseClientService.Initializer { ApiKey = "AIzaSyBkXzq40hrcCImIxKSpNxQvr7zL37gN6qM" };
                //PlusService plusService = new PlusService(ini);
                //if (plusService != null)
                //{
                //PeopleResource.GetRequest prgr = plusService.People.Get("me");
                //Person googleUser = prgr.Execute();
                //Person me = plusService.People.Get(UserId).Execute();
                //PeopleResource.GetRequest personRequest = plusService.People.Get(UserId);
                //Person googleUser = personRequest.Execute();
                //usuario.Email = googleUser.Emails.FirstOrDefault().Value;
                //usuario.Nome = googleUser.Name.GivenName +" " + googleUser.Name.FamilyName;
                //usuario.PrimeiroNome = googleUser.Name.GivenName;
                usuario.Email = googleUser.email;
                usuario.Nome = googleUser.given_name + " " + googleUser.family_name;
                usuario.PrimeiroNome = googleUser.given_name;
                Usuario usuarioBanco = UsuarioBO.ObterUsuarioEmail(usuario.Email);
                if(usuarioBanco == null || usuarioBanco.Id == 0)
                {
                    UsuarioBO.CadastrarUsuario(usuario);
                }
                usuario = UsuarioBO.ObterUsuarioEmail(usuario.Email);
                HttpContext.Session["usuario"] = usuario;
                //}
                // Person me = plusService.People.Get(UserId).Execute();
                /*var success = GoogleCalendarSyncer.SyncToGoogleCalendar(this);
                if (!success)
                {
                    ViewData["Erro"] = "Token foi revogado. Tente novamente.";
                    return new RedirectResult("~/Home/Login");
                }*/
                if (!UsuarioBO.ExisteUsuario(usuario.Email))
                {
                    UsuarioBO.CadastrarUsuario(usuario);
                    if (sacola != null && sacola.TipoPagamento.Equals("1"))
                    {
                        // if (sacola.Rua.Equals("Rua San Marino") && !String.IsNullOrEmpty(usuario.Email))
                        if (!String.IsNullOrEmpty(usuario.Email))
                        {
                            return new RedirectResult("~/Home/PagSeguro"); 
                        }
                        else
                        {
                            TempData["Erro"] = "Este Cep não pode efetuar compra.";
                            return new RedirectResult("~/Home/Cart");
                        }
                    }
                    else
                    {
                        ViewData["Mensagem"] = "Usuário criado com sucesso.";
                        return new RedirectResult("~/Home/Index");
                    }
                }
                else
                {
                    if (sacola != null && sacola.TipoPagamento.Equals("1"))
                    {
                        if (!String.IsNullOrEmpty(usuario.Email))
                        {
                            return new RedirectResult("~/Home/PagSeguro");
                        }
                        else
                        {
                            TempData["Erro"] = "Este Cep não pode efetuar compra.";
                            return new RedirectResult("~/Home/Cart");
                        }
                    }
                    else
                    {
                        ViewData["Mensagem"] = "Usuário logado com sucesso.";
                        return new RedirectResult("~/Home/Index");
                    }

                }

                   
                return new RedirectResult("~/Home/Index");
            }
            catch (Exception e)
            {
                ViewData["Erro"] = e.Message;
            }
            return new RedirectResult("~/Home/Login");
        }

        protected override ActionResult OnTokenError(TokenErrorResponse errorResponse)
        {
            ViewData["Erro"] = "Token foi revogado. Tente novamente.";
            return new RedirectResult("~/Home/Login");
        }
    }
}