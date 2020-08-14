using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Box.Festa.Attribute;
using Box.Festa.Models;
using Box.Festa.Negocio;
using Box.Festa.Service;
using Microsoft.Reporting.WebForms;

namespace Box.Festa.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            ViewBag.Admin = admin;
            return View("Index");
        }
        public ActionResult AtualizarStatus()
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            ViewBag.Admin = admin;
            return View("Status");
        }

        public ActionResult FiltroRelatorio()
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            ViewBag.Admin = admin;
            return View("FiltroRelatorio");
        }

        public ActionResult ListarProduto()
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            ViewBag.Admin = admin;
            List<Produto> listaProduto = ProdutoBO.ListarTodosProdutos().OrderBy(c => c.Id).ToList();
            ViewBag.lista = listaProduto;
            ViewBag.TotalResultados = listaProduto.Count;
            return View("ListarProduto");
        }

        public ActionResult EditarProduto(string codigo)
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            ViewBag.Admin = admin;
            Produto produto = ProdutoBO.ObterProduto(codigo);
            return View("EditarProduto", produto);
        }

        [HttpPost]
        public ActionResult EditarProduto(Produto produto)
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            ViewBag.Admin = admin;
            if(produto.Id > 0)
            {
                ProdutoBO.EditarProduto(produto);
            }
            else
            {
                ProdutoBO.CadastrarProduto(produto);
            }
            
            TempData["Mensagem"] = " Produto cadastrado com sucesso.";
            return View("EditarProduto", produto);
        }

        public ActionResult ExcluirProduto(string codigo)
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            ViewBag.Admin = admin;
            Produto produto = ProdutoBO.ObterProduto(codigo);
            ProdutoBO.ExcluirProduto(produto);
            TempData["Mensagem"] = " Produto excluído com sucesso.";
           List<Produto> listaProduto = ProdutoBO.ListarTodosProdutos().OrderBy(c => c.Id).ToList();
            ViewBag.lista = listaProduto;
            ViewBag.TotalResultados = listaProduto.Count;
            return View("ListarProduto");
        }

        [HttpPost]
        [MultipleButton(Name = "atualizar", Argument = "Status")]
        public  ActionResult AtualizarStatus(PesquisaAdmin pesquisa)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Debug("-------------------------------Atualizar Status");
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            List<Pedido> listaPedidosAlterados = new List<Pedido>();
            string erro = "Não atualizou o dia: ";
            try
            {
                string data = pesquisa.Data.Replace("/", "");
               
                DateTime diaPesquisa = new DateTime(Int32.Parse(data.Substring(4, 4)), Int32.Parse(data.Substring(2, 2)), Int32.Parse(data.Substring(0, 2)));
                log.Debug("-------------------------------ListarPedido");
                List<Pedido> listaPedido = PedidoBO.ListarPedido(diaPesquisa.Year.ToString(), diaPesquisa.Month.ToString().PadLeft(2, '0'), diaPesquisa.Day.ToString().PadLeft(2, '0'), true);
                try
                {
                    log.Debug("-------------------------------Salvar1");
                    this.SalvarStatusAlterado(listaPedido, listaPedidosAlterados);
                }
                catch (Exception e)
                {
                   erro = erro + diaPesquisa+"/n";
                    log.Debug("-------------------------------Erro1:"+e.ToString());

                }
                listaPedido.Clear();
                DateTime diaPesquisa1 = diaPesquisa.AddDays(-1);
                listaPedido.AddRange(PedidoBO.ListarPedido(diaPesquisa1.Year.ToString(), diaPesquisa1.Month.ToString().PadLeft(2, '0'), diaPesquisa1.Day.ToString().PadLeft(2, '0'), true));
                try
                {
                    log.Debug("-------------------------------Salvar2");
                    this.SalvarStatusAlterado(listaPedido, listaPedidosAlterados);
                }
                catch (Exception e)
                {
                    log.Debug("-------------------------------Erro2:" + e.ToString());
                    erro = erro + diaPesquisa1 + "/n";
                }
            
                listaPedido.Clear();
                DateTime diaPesquisa2 = diaPesquisa.AddDays(-2);
                listaPedido.AddRange(PedidoBO.ListarPedido(diaPesquisa2.Year.ToString(), diaPesquisa2.Month.ToString().PadLeft(2, '0'), diaPesquisa2.Day.ToString().PadLeft(2, '0'), true));
                try
                {
                    log.Debug("-------------------------------Salvar3");
                    this.SalvarStatusAlterado(listaPedido, listaPedidosAlterados);
                }
                catch (Exception e)
                {
                    log.Debug("-------------------------------Erro3:" + e.ToString());
                    erro = erro + diaPesquisa2 + "/n";
                }
                listaPedido.Clear();

                DateTime diaPesquisa3 = diaPesquisa.AddDays(-3);
                listaPedido.AddRange(PedidoBO.ListarPedido(diaPesquisa3.Year.ToString(), diaPesquisa3.Month.ToString().PadLeft(2, '0'), diaPesquisa3.Day.ToString().PadLeft(2, '0'), true));
                try
                {
                    log.Debug("-------------------------------Salvar4");
                    this.SalvarStatusAlterado(listaPedido, listaPedidosAlterados);
                }
                catch (Exception e)
                {
                    log.Debug("-------------------------------Erro4:" + e.ToString());
                    erro = erro + diaPesquisa3 + "/n";
                }
                listaPedido.Clear();

                DateTime diaPesquisa4 = diaPesquisa.AddDays(-4);
                listaPedido.AddRange(PedidoBO.ListarPedido(diaPesquisa4.Year.ToString(), diaPesquisa4.Month.ToString().PadLeft(2, '0'), diaPesquisa4.Day.ToString().PadLeft(2, '0'), true));
                try
                {
                    log.Debug("-------------------------------Salvar5");
                    this.SalvarStatusAlterado(listaPedido, listaPedidosAlterados);
                }
                catch (Exception e)
                {
                    log.Debug("-------------------------------Erro5:" + e.ToString());
                    erro = erro + diaPesquisa4 + "/n";
                }
                listaPedido.Clear();

                DateTime diaPesquisa5 = diaPesquisa.AddDays(-5);
                listaPedido.AddRange(PedidoBO.ListarPedido(diaPesquisa5.Year.ToString(), diaPesquisa5.Month.ToString().PadLeft(2, '0'), diaPesquisa5.Day.ToString().PadLeft(2, '0'), true));
                try
                {

                    log.Debug("-------------------------------Salvar6");
                    this.SalvarStatusAlterado(listaPedido, listaPedidosAlterados);
                }
                catch (Exception e)
                {
                    log.Debug("-------------------------------Erro6:" + e.ToString());
                    erro = erro + diaPesquisa5 + "/n";
                }



                // https://dev.pagseguro.uol.com.br/docs/checkout-web-consulta#parametros-da-api
                /*
                 * https://ws.pagseguro.uol.com.br/v2/transactions/9E884542-81B3-4419-9A75-BCC6FB495EF1
                    ?email=suporte@lojamodelo.com.br
                    &token=95112EE828D94278BD394E91C4388F20
                  <transactionSearchResult>
                    <transactions>
                    <transaction>
                    <status>
                Codigo	Significado
                1	Aguardando pagamento: o comprador iniciou a transação, mas até o momento o PagSeguro não recebeu nenhuma informação sobre o pagamento.
                2	Em análise: o comprador optou por pagar com um cartão de crédito e o PagSeguro está analisando o risco da transação.
                3	Paga: a transação foi paga pelo comprador e o PagSeguro já recebeu uma confirmação da instituição financeira responsável pelo processamento.
                4	Disponível: a transação foi paga e chegou ao final de seu prazo de liberação sem ter sido retornada e sem que haja nenhuma disputa aberta.
                5	Em disputa: o comprador, dentro do prazo de liberação da transação, abriu uma disputa.
                6	Devolvida: o valor da transação foi devolvido para o comprador.
                7	Cancelada: a transação foi cancelada sem ter sido finalizada.
                8	Debitado: o valor da transação foi devolvido para o comprador.
                9	Retenção temporária: o comprador abriu uma solicitação de chargeback junto à operadora do cartão de crédito.*/
                //XmlDocument doc = new XmlDocument();
                // XmlNodeList infoElem = doc.GetElementsByTagName("status");
            }
            catch (Exception e)
            {
                TempData["Erro"] = e.ToString();
            }
            AtualizarStatus statusObj = new AtualizarStatus();
            statusObj.HoraAtualizacao = DateTime.Now;
            statusObj.UsuarioAlteracao = admin;
            statusObj.NrPedidosAlterados = listaPedidosAlterados.Count;
            foreach(Pedido pedido in listaPedidosAlterados)
            {
                AtualizarStatusPedido asPedido = new AtualizarStatusPedido();
                asPedido.PedidoId = pedido.Id;
                asPedido.Pedido = pedido;
                asPedido.AtualizarStatus = statusObj;
                statusObj.ListaPedidosAlterados.Add(asPedido);
            }
            //statusObj.ListaPedidosAlterados = listaPedidosAlterados;
            AtualizarStatusBO.CriarAtualizacaoStatus(statusObj);
            ViewBag.Status = statusObj;
            foreach (Pedido pedido in listaPedidosAlterados)
            {
                if (pedido.Status.Equals("3"))
                {
                    this.EnviarEmailEnviarPedido(pedido);
                }
            }
            if (!string.IsNullOrEmpty(erro) && !erro.Equals("Não atualizou o dia: "))
            {
                TempData["Erro"] = erro;
            }
            TempData["Mensagem"] = " Atualizacao efetuada com sucesso. Foram alterados:" + listaPedidosAlterados.Count + " registros.";
            return View("Status");
        }


        public void SalvarStatusAlterado(List<Pedido> listaPedido, List<Pedido> listaPedidosAlterados)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Debug("-------------------------------SalvarStatusAlterado");
            foreach (Pedido pedido in listaPedido)
            {
                if (!string.IsNullOrEmpty(pedido.IdTransacao))
                {
                    if (pedido.Status == null || (!pedido.Status.Equals("7") && !pedido.Status.Equals("3")))
                    {
                        // var caminhoPagSeguro = "https://ws.pagseguro.uol.com.br/v3/transactions/" + pedido.IdTransacao+ "?email=rapsch3@gmail.com&token=5269A70FB9FD41D3A2BDD1C305B142AE";
                        var caminhoPagSeguro = "https://ws.sandbox.pagseguro.uol.com.br/v3/transactions/" + pedido.IdTransacao + "?email=rapsch3@gmail.com&token=5269A70FB9FD41D3A2BDD1C305B142AE";
                        var xmlResult = "";
                        try
                        {
                          //  var task = new Task(() =>
                           // {
                                xmlResult = RestService<String>.AtualizarStatus(caminhoPagSeguro, "", "get");
                           // });
                            log.Debug("-------------------------------task Start");

                           // task.Start();
                           // task.Wait();

                            if (!string.IsNullOrEmpty(xmlResult))
                            {
                                var _doc = XDocument.Parse(xmlResult);
                                var hasErrors = _doc.Descendants("errors").Count() > 0;

                                if (!hasErrors)
                                {
                                    XElement status = _doc.Descendants("status").FirstOrDefault();
                                    if (status != null && (pedido.Status == null || pedido.Status != null && !pedido.Status.Equals(status.Value)))
                                    {
                                        pedido.Status = status.Value;
                                        pedido.StatusTexto = Pedido.PreencherStatus(int.Parse(pedido.Status));

                                        //ArquivoBO.GravarPedido(pedido);
                                        if (pedido.Status.Equals("3"))
                                        {
                                            pedido.ConfirmadoPagSeguro = true;
                                        }
                                        pedido.Alterado = true;
                                        listaPedidosAlterados.Add(pedido);
                                        // pedidosAlterados++;
                                    }
                                }
                                else
                                {
                                    TempData["Erro"] = "Erro no Pag Seguro " + xmlResult;

                                }
                            }
                        }catch(Exception e)
                        {
                            log.Debug("-------------------------------SalvarStatusAlterado task exception" + e.ToString());
                        }
                    }
                }
                else
                {
                    if (pedido.Status == null || !pedido.Status.Equals("7"))
                    {
                        pedido.Status = "7";
                        pedido.StatusTexto = Pedido.PreencherStatus(int.Parse(pedido.Status));
                        //ArquivoBO.GravarPedido(pedido);
                        pedido.Alterado = true;
                        listaPedidosAlterados.Add(pedido);
                        // pedidosAlterados++;
                    }
                }
            }
            log.Debug("-------------------------------SalvarStatusAlterado Gravar Lista");
            try
            {
                PedidoBO.GravarStatusListaPedidoMesmoDia(listaPedido);
            }catch(Exception e)
            {
                log.Debug("-------------------------------Exception Gravar Lista"+e.ToString());
                throw e;
            }
        }

        public void EnviarEmailEnviarPedido(Pedido pedido)
        {
            try
            {
                string produtos = "";
                produtos = "Endereço: " + pedido.Sacola.Rua + " - Complemento: " + (string.IsNullOrEmpty(pedido.Sacola.Complemento)? "-": pedido.Sacola.Complemento) + " - Cep: " + pedido.Sacola.Cep + " - Cidade:" + pedido.Sacola.Cidade + " - Estado: " + pedido.Sacola.Estado+" <br/>";

                foreach (SacolaProduto prod in pedido.Sacola.Produtos)
                {
                    produtos = produtos +"Código: "+prod.Produto.Codigo  +" - Produto: " + prod.Produto.Descricao + " - Quantidade: " + prod.Produto.Qtde + " - Valor: " + prod.Produto.Valor+"<br/>";
                }
                MailMessage m = new MailMessage();
                SmtpClient sc = new SmtpClient();
                m.From = new MailAddress("contato@boxfesta.com.br");
                m.To.Add("rapsch3@gmail.com");
                m.Subject = "[Contato Box Festa] Pedido Pago";
                m.IsBodyHtml = true;
                m.Body = string.Format("Pedido: {0}    Data: {1}    Transação: {2} <br/> Produtos para serem enviados: {3}", pedido.Id, pedido.Data + " " + pedido.Hora, pedido.IdTransacao, produtos);
                sc.Host = "smtp.zoho.com";
                sc.Port = 587;
                sc.Credentials = new System.Net.NetworkCredential("contato@boxfesta.com.br", "contato:Boxfesta");


                sc.EnableSsl = true;
                sc.Send(m);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult PesquisaPedido()
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            ViewBag.Admin = admin;
            return View("PesquisaPedido");
        }

        [HttpPost]
        [MultipleButton(Name = "pesquisa", Argument = "PedidoAdmin")]
        public ActionResult PesquisaPedido(PesquisaAdmin pesquisa)
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            string data = pesquisa.Data.Replace("/", "");
            List<Pedido> listaPedido =  PedidoBO.ListarPedido(data.Substring(4, 4), data.Substring(2, 2), data.Substring(0, 2), true);
            ViewBag.ListaPedido = listaPedido.OrderByDescending(c => c.Id);
            if (listaPedido == null || listaPedido.Count == 0)
            {
                TempData["Erro"] = "Pesquisa não retornou resultados.";
            }
            return View("PesquisaPedido", pesquisa);
        }

        public ActionResult PesquisaUsuario()
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            ViewBag.Admin = admin;
            return View("PesquisaUsuario");
        }

        [HttpPost]
        [MultipleButton(Name = "pesquisa", Argument = "Usuario")]
        public ActionResult PesquisaUsuario(PesquisaAdmin pesquisa)
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            Usuario usuario = UsuarioBO.ObterUsuario(pesquisa.Usuario);
            if (usuario == null)
            {
                TempData["Erro"] = "Pesquisa não retornou resultados.";
            }
            usuario.ListaPedido = PedidoBO.ListarPedidoUsuario(usuario.Id);
            usuario.ListaPedido = usuario.ListaPedido.OrderByDescending(c => c.Id).ToList();
            ViewBag.Usuario = usuario;
            return View("PesquisaUsuario", pesquisa);
        }
        public ActionResult Login()
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return View("Login");
            }

            return View("Index");
        }

        public ActionResult RelatorioProduto()
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return View("Login");
            }
            //var ds = ObterDados();

            var viewer = new Microsoft.Reporting.WebForms.ReportViewer();
            viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"report/RelatorioProduto.rdlc";
            
            ReportDataSource datasource = new ReportDataSource();
            datasource.Name = "DataSet1";
            datasource.Value = ProdutoBO.ListarTodosProdutos();
            viewer.LocalReport.DataSources.Add(datasource);
            //viewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("", (System.Data.DataTable)ds.NomeDaDataTable));
            viewer.SizeToReportContent = true;
            viewer.Width = System.Web.UI.WebControls.Unit.Pixel(800);
            viewer.Height = System.Web.UI.WebControls.Unit.Pixel(1200);
            ViewBag.ReportViewer = viewer;

            return View("Relatorio");
        }

        public ActionResult RelatorioListarProduto()
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return View("Login");
            }
            //var ds = ObterDados();

            var viewer = new Microsoft.Reporting.WebForms.ReportViewer();
            viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"report/RelatorioListaProduto.rdlc";

            ReportDataSource datasource = new ReportDataSource();
            datasource.Name = "DataSet1";
            datasource.Value = ProdutoBO.ListarTodosProdutos();
            viewer.LocalReport.DataSources.Add(datasource);
            //viewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("", (System.Data.DataTable)ds.NomeDaDataTable));
            viewer.SizeToReportContent = true;
            viewer.Width = System.Web.UI.WebControls.Unit.Pixel(800);
            viewer.Height = System.Web.UI.WebControls.Unit.Pixel(1200);
            ViewBag.ReportViewer = viewer;

            return View("Relatorio");
        }

        public ActionResult RelatorioListarUsuario()
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return View("Login");
            }
            //var ds = ObterDados();

            var viewer = new Microsoft.Reporting.WebForms.ReportViewer();
            viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"report/RelatorioListaUsuario.rdlc";

            ReportDataSource datasource = new ReportDataSource();
            datasource.Name = "DataSet1";
            datasource.Value = UsuarioBO.ListarTodosUsuarios();
            viewer.LocalReport.DataSources.Add(datasource);
            //viewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("", (System.Data.DataTable)ds.NomeDaDataTable));
            viewer.SizeToReportContent = true;
            viewer.Width = System.Web.UI.WebControls.Unit.Pixel(800);
            viewer.Height = System.Web.UI.WebControls.Unit.Pixel(1200);
            ViewBag.ReportViewer = viewer;

            return View("Relatorio");
        }

        public ActionResult RelatorioUsuario()
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return View("Login");
            }
            
            var viewer = new Microsoft.Reporting.WebForms.ReportViewer();
            viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"report/RelatorioUsuario.rdlc";

            ReportDataSource datasource = new ReportDataSource();
            datasource.Name = "DataSet1";
            datasource.Value = UsuarioBO.ListarTodosUsuarios();
            viewer.LocalReport.DataSources.Add(datasource);
            viewer.SizeToReportContent = true;
            viewer.Width = System.Web.UI.WebControls.Unit.Pixel(800);
            viewer.Height = System.Web.UI.WebControls.Unit.Pixel(1200);
            ViewBag.ReportViewer = viewer;

            return View("Relatorio");
        }

        public ActionResult RelatorioPedido()
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return View("Login");
            }

            var viewer = new Microsoft.Reporting.WebForms.ReportViewer();
            viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"report/RelatorioPedido.rdlc";

            ReportDataSource datasource = new ReportDataSource();
            datasource.Name = "DataSet1";
            datasource.Value = PedidoBO.ListarTodosPedidos();
            viewer.LocalReport.DataSources.Add(datasource);
            viewer.SizeToReportContent = true;
            viewer.Width = System.Web.UI.WebControls.Unit.Pixel(800);
            viewer.Height = System.Web.UI.WebControls.Unit.Pixel(1200);
            ViewBag.ReportViewer = viewer;

            return View("Relatorio");
        }


        [HttpPost]
        public ActionResult Login(Usuario administrador)
        {
            //string senha = Md5BO.RetornarMD5(administrador.Senha);
            Usuario admBanco = UsuarioBO.ObterUsuario(administrador.Email);
            if (admBanco == null || administrador == null ||  admBanco.Senha==null || !admBanco.Email.Equals(administrador.Email) || !admBanco.PerfilUsuario.Equals(EnumPerfilUsuario.Administrador) || !admBanco.Senha.Equals(Sha1BO.RetornarSHA(administrador.Senha)))
            {
                TempData["Erro"] = "Usuário ou senha inválida.";
                return new RedirectResult("~/Admin/Admin/Login");
            }
            HttpContext.Session["admin"] = administrador;
            return new RedirectResult("~/Admin/Admin/Index");
        }

    }
}
