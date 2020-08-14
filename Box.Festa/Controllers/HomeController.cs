using Box.Festa.Attribute;
using Box.Festa.GoogleApi;
using Box.Festa.Models;
using Box.Festa.Negocio;
using Box.Festa.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Box.Festa.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string produtosSelecionados = "")
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Debug("-------------------------------Inicio Box Festa");
            PreencherSacola(produtosSelecionados);
            Sacola sacola = (Sacola)HttpContext.Session["sacola"];
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            // this.CriarPedido("06013737665");

            List<Produto> listaProduto = ProdutoBO.ListarProduto();
            if (sacola == null)
            {
                sacola = new Sacola();
            }
            if (usuario != null)
            {
                sacola.Usuario = usuario;
            }
            sacola = this.LimparEndereco(sacola);
            sacola.Sedex = 0;
            sacola.TotalProduto = 0;
            foreach (SacolaProduto produto in sacola.Produtos)
            {
                sacola.TotalProduto = sacola.TotalProduto + (produto.Produto.Valor * produto.Produto.Qtde);
            }

            sacola.TotalSacola = sacola.TotalProduto;
            List<Produto> listaTodosProdutosAvulsos = listaProduto.Where(c => c.ehKit == false).ToList();
            ViewBag.TotalAvulso = listaTodosProdutosAvulsos.Count;
            ViewBag.ListaProduto = listaTodosProdutosAvulsos;
            ViewBag.ListaProdutoSale = listaProduto.Where(c => c.ehSale == true);
            ViewBag.ListaKit = listaProduto.Where(c => c.ehKit == true);
            ViewBag.ListaProdutoArray = sacola.ProdutoSelecionado;
            ViewBag.ValorProdutoSelecionado = sacola.TotalSacola;
            ViewBag.QtdeProdutoSelecionado = sacola.Produtos.Count;
            ViewBag.ProdutosSacola = "";
            ViewBag.Usuario = usuario;
            HttpContext.Session["sacola"] = sacola;

            return View("Index", sacola);
        }

        public ActionResult Products(string listaProduto, string secao = "0")
        {
            PreencherSacola(listaProduto);
            Sacola sacola = (Sacola)HttpContext.Session["sacola"];


            List<Produto> listaProdutos = ProdutoBO.ListarProduto();
            if (sacola == null)
            {
                sacola = new Sacola();

            }
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];

            sacola = this.LimparEndereco(sacola);
            sacola.Sedex = 0;
            sacola.TotalProduto = 0;
            foreach (SacolaProduto produto in sacola.Produtos)
            {
                sacola.TotalProduto = sacola.TotalProduto + (produto.Produto.Valor * produto.Produto.Qtde);
            }
            sacola.TotalSacola = sacola.TotalProduto;
            List<Produto> listaTodosProdutosAvulsos = listaProdutos.Where(c => c.ehKit == false).ToList();
            ViewBag.TotalAvulso = listaTodosProdutosAvulsos.Count;
            ViewBag.ListaProduto = listaTodosProdutosAvulsos;
            ViewBag.ListaProdutoSale = listaProdutos.Where(c => c.ehSale == true);
            ViewBag.ListaProdutoMaisVendido = listaProdutos.Where(c => c.ehMaisVendido == true);
            ViewBag.ListaKit = listaProdutos.Where(c => c.ehKit == true);
            ViewBag.ListaProdutoArray = sacola.ProdutoSelecionado;
            ViewBag.ValorProdutoSelecionado = sacola.TotalSacola;
            ViewBag.ProdutosSacola = "";
            ViewBag.Secao = secao;

            ViewBag.QtdeProdutoSelecionado = sacola.Produtos.Count;
            ViewBag.Usuario = usuario;
            HttpContext.Session["sacola"] = sacola;
            return View(sacola);
        }


        public ActionResult Cart(string listaProduto, string code = "")
        {
            this.PreencherSacola(listaProduto);
            Sacola sacola = (Sacola)HttpContext.Session["sacola"];
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];

            //sacola.SedexFormatado = string.Format("R${0:0.00#,##}", 0);
            // sacola.SedexFormatado =  0.ToString().Replace(".","").Replace(",","");
            if (sacola == null)
            {
                sacola = new Sacola();
            }
            sacola.TotalProduto = 0;
            foreach (SacolaProduto produto in sacola.Produtos)
            {
                sacola.TotalProduto = sacola.TotalProduto + (produto.Produto.Valor * produto.Produto.Qtde);
                sacola.TotalSacola = sacola.TotalProduto + sacola.Sedex;
            }
            ViewBag.QtdeProdutoSelecionado = sacola.Produtos.Count;
            ViewBag.ProdutosSacola = "";
            ViewBag.CodePagSeguro = code;
            if (usuario == null || string.IsNullOrEmpty(usuario.Nome))
            {
                ViewBag.CodePagSeguro = "";
            }
            HttpContext.Session["sacola"] = sacola;
            return View("Cart", sacola);

        }

        public ActionResult DetalheProduto(string codigo, string produtosSacola)
        {
            this.PreencherSacola(produtosSacola);
            Sacola sacola = (Sacola)HttpContext.Session["sacola"];
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            if (sacola == null)
            {
                sacola = new Sacola();
            }
            Produto produto = new Produto();
            if (!String.IsNullOrEmpty(codigo))
            {
                produto = ProdutoBO.ObterProduto(codigo);
            }
            if (sacola.ProdutoSelecionado != null)
            {
                ViewBag.QtdeProdutoSelecionado = sacola.Produtos.Count;
                ViewBag.ValorProdutoSelecionado = sacola.TotalProduto;

            }
            else
            {
                ViewBag.QtdeProdutoSelecionado = 0;
                ViewBag.ValorProdutoSelecionado = 0d;

            }
            ViewBag.ListaProdutoArray = new long[sacola.Produtos.Count];
            List<Produto> listaProdutosRelacionado = new List<Produto>();
            List<Produto> listaProdutos = ProdutoBO.ListarProduto();
            if (produto != null)
            {
                string[] temas = produto.TemaRelacionado.Split(',');
                foreach (Produto prod in listaProdutos)
                {
                    for (int i = 0; i < temas.Length; i++)
                    {
                        if (prod.TemaRelacionado.Contains(temas[i]))
                        {
                            if (!listaProdutosRelacionado.Contains(prod) && !produto.Codigo.Equals(prod.Codigo))
                            {
                                listaProdutosRelacionado.Add(prod);
                            }
                        }
                    }
                }
            }
            ViewBag.ProdutosRelacionados = listaProdutosRelacionado;
            ViewBag.usuario = usuario;
            HttpContext.Session["sacola"] = sacola;
            return View("DetalheProduto", produto);
        }

        public ActionResult Login()
        {
            Sacola sacola = (Sacola)HttpContext.Session["sacola"];
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            if(usuario!=null && usuario.Id != 0)
            {
                return new RedirectResult("~/User/Usuario/Index");
            }
            if (sacola == null)
            {
                sacola = new Sacola();
            }



            ViewBag.ListaProdutoArray = new long[sacola.Produtos.Count];
            ViewBag.ValorProdutoSelecionado = 0d;
            ViewBag.ListaProdutoArray = sacola.ProdutoSelecionado;
            ViewBag.ValorProdutoSelecionado = sacola.TotalSacola;


            ViewBag.QtdeProdutoSelecionado = sacola.Produtos.Count;
            ViewBag.Usuario = usuario;
            return View("Login", usuario);
        }

        public ActionResult Compras()
        {
            Sacola sacola = (Sacola)HttpContext.Session["sacola"];
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            if (sacola == null)
            {
                sacola = new Sacola();
            }

            if (usuario == null || string.IsNullOrEmpty(usuario.Nome))
            {
                return new RedirectResult("~/Home/Login");
            }


            ViewBag.ListaProdutoArray = new long[sacola.Produtos.Count];
            ViewBag.ValorProdutoSelecionado = 0d;
            ViewBag.ListaProdutoArray = sacola.ProdutoSelecionado;
            ViewBag.ValorProdutoSelecionado = sacola.TotalSacola;


            ViewBag.QtdeProdutoSelecionado = sacola.Produtos.Count;
            usuario = UsuarioBO.ObterUsuario(usuario.Email);
            return View("Compras", usuario);
        }

        [HttpPost]
        [MultipleButton(Name = "logar", Argument = "Google")]
        public ActionResult LogarComGoogle(Usuario usuario)
        {
            Sacola sacola = (Sacola)HttpContext.Session["sacola"];
            HttpContext.Session["usuario"] = usuario;
            if (sacola == null)
            {
                sacola = new Sacola();
            }

            ViewBag.ListaProdutoArray = new long[sacola.Produtos.Count];
            ViewBag.ValorProdutoSelecionado = 0d;
            ViewBag.ListaProdutoArray = sacola.ProdutoSelecionado;
            ViewBag.ValorProdutoSelecionado = sacola.TotalSacola;
            ViewBag.QtdeProdutoSelecionado = sacola.Produtos.Count;
            try
            {
                return this.SyncToGoogleCalendar();
            }
            catch (Exception e)
            {
                TempData["Erro"] = e.Message;
            }
            return View("Login", usuario);
        }

        [HttpPost]
        [MultipleButton(Name = "logar", Argument = "Login")]
        public ActionResult LogarComLogin(Usuario usuario)
        {
            Sacola sacola = (Sacola)HttpContext.Session["sacola"];
            HttpContext.Session["usuario"] = usuario;
            if (sacola == null)
            {
                sacola = new Sacola();
            }

            ViewBag.ListaProdutoArray = new long[sacola.Produtos.Count];
            ViewBag.ValorProdutoSelecionado = 0d;
            ViewBag.ListaProdutoArray = sacola.ProdutoSelecionado;
            ViewBag.ValorProdutoSelecionado = sacola.TotalSacola;
            ViewBag.QtdeProdutoSelecionado = sacola.Produtos.Count;
            Usuario usuarioBanco = UsuarioBO.ObterUsuario(usuario.Email);
            if (usuarioBanco.Email.Equals(usuario.Email) && usuarioBanco.Senha.Equals(Sha1BO.RetornarSHA(usuario.Senha)))
            {
                HttpContext.Session["usuario"] = usuarioBanco;

                if (sacola != null && sacola.TipoPagamento.Equals("1"))
                {
                    if (!String.IsNullOrEmpty(usuario.Email))
                    {
                        return PagSeguro();
                    }
                    else
                    {
                        TempData["Erro"] = "Este Cep não pode efetuar compra.";
                        return new RedirectResult("~/Home/Cart");
                    }
                }
                else
                {
                    TempData["Mensagem"] = "Usuário logado com sucesso.";
                    return new RedirectResult("~/Home/Index");
                }
            }
            else
            {
                TempData["Erro"] = "Usuário ou senha incorretos.";

            }


            return View("Login", usuario);
        }

        [HttpPost]
        [MultipleButton(Name = "logar", Argument = "Criar")]
        public ActionResult LogarComCriar(Usuario usuario)
        {
            Sacola sacola = (Sacola)HttpContext.Session["sacola"];
            HttpContext.Session["usuario"] = usuario;
            if (sacola == null)
            {
                sacola = new Sacola();
            }

            ViewBag.ListaProdutoArray = new long[sacola.Produtos.Count];
            ViewBag.ValorProdutoSelecionado = 0d;
            ViewBag.ListaProdutoArray = sacola.ProdutoSelecionado;
            ViewBag.ValorProdutoSelecionado = sacola.TotalSacola;
            ViewBag.QtdeProdutoSelecionado = sacola.Produtos.Count;
            try
            {
                usuario.NovoCpf = usuario.NovoCpf.Replace(".", "").Replace("-", "");
                usuario.Telefone = usuario.Telefone.Replace("(", "").Replace(")", "").Replace("-", "");
                if (!UsuarioBO.ExisteUsuario(usuario.NovoEmail))
                {
                    if (!ValidarInsercaoUsuario(usuario))
                    {
                        TempData["Erro"] = "Campos obrigatórios não preenchidos.";
                        return View("Login", usuario);
                    }

                    string[] nomes = usuario.Nome.Split(' ');
                    if (nomes.Length < 2 || string.IsNullOrEmpty(nomes[1].Trim()))
                    {
                        TempData["Erro"] = "Nome inválido.";
                        return View("Login", usuario);
                    }
                    if (usuario.NovaSenha.Length < 6)
                    {
                        TempData["Erro"] = "Senha tem que ter no mínimo 6 caracteres.";
                        return View("Login", usuario);
                    }
                    if (usuario.NovoCpf.Length != 11)
                    {
                        TempData["Erro"] = "Cpf inválido.";
                        return View("Login", usuario);
                    }
                    if (!usuario.NovaSenha.Equals(usuario.ConfirmarSenha))
                    {
                        TempData["Erro"] = "Senhas não conferem.";
                        return View("Login", usuario);
                    }
                    usuario.Cpf = usuario.NovoCpf;
                    usuario.Email = usuario.NovoEmail;
                    usuario.Senha = Sha1BO.RetornarSHA(usuario.NovaSenha);
                    usuario.PrimeiroNome = usuario.Nome.Split(' ')[0];
                    UsuarioBO.CadastrarUsuario(usuario);
                    HttpContext.Session["usuario"] = usuario;
                    if (sacola != null && sacola.TipoPagamento.Equals("1"))
                    {
                        if (!String.IsNullOrEmpty(usuario.Email))
                        {
                            return PagSeguro();
                        }
                        else
                        {
                            TempData["Erro"] = "Este Cep não pode efetuar compra.";
                            return new RedirectResult("~/Home/Cart");
                        }
                    }
                    else
                    {
                        TempData["Mensagem"] = "Usuário logado com sucesso.";
                        return new RedirectResult("~/Home/Index");
                    }
                }
            }
            catch (Exception e)
            {
                TempData["Erro"] = e.Message;
            }
            return View("Login", usuario);
        }

        

        public ActionResult AdicionarProduto(string codigo, string qtde, string pessoas = "0")
        {
            Sacola sacola = (Sacola)HttpContext.Session["sacola"];
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            sacola.TotalProduto = 0;
            Produto produto = new Produto();
            if (!String.IsNullOrEmpty(codigo))
            {
                produto = ProdutoBO.ObterProduto(codigo);
                produto.Qtde = Int32.Parse(qtde);

                if (produto.ehKit)
                {
                    if (pessoas == "0")
                    {
                        TempData["Erro"] = "Favor selecionar a quantidade de pessoas.";
                        return new RedirectResult("~/Home/DetalheProduto?codigo=" + codigo + "&listaProduto=");
                    }
                    produto.QtdePessoas = Int32.Parse(pessoas);
                    produto.Descricao = produto.Descricao + " - " + produto.QtdePessoas + " pessoas.";
                    if (produto.QtdePessoas == 10)
                    {
                        produto.Valor = produto.Valor;
                    }
                    else if (produto.QtdePessoas == 20)
                    {
                        produto.Valor = (produto.Valor * 2) - 5;
                    }
                    else if (produto.QtdePessoas == 30)
                    {
                        produto.Valor = (produto.Valor * 3) - 10;
                    }
                    else if (produto.QtdePessoas == 40)
                    {
                        produto.Valor = (produto.Valor * 4) - 15;
                    }
                    else if (produto.QtdePessoas == 50)
                    {
                        produto.Valor = (produto.Valor * 5) - 20;
                    }
                }
                SacolaProduto sacProd = new SacolaProduto();
                sacProd.Produto = produto;
                sacProd.ProdutoId = produto.Id;
                sacProd.Sacola = sacola;
                sacProd.SacolaId = sacola.Id;

                sacola.Produtos.Add(sacProd);
                for (int i = 0; i < sacola.ProdutoSelecionado.Length - 1; i++)
                {
                    if (sacola.ProdutoSelecionado[i].Equals(produto.Codigo))
                    {
                        sacola.ProdutoSelecionado[i] = long.Parse(produto.Codigo);
                    }
                }

            }
            ViewBag.QtdeProdutoSelecionado = sacola.Produtos.Count;
            foreach (SacolaProduto produto2 in sacola.Produtos)
            {
                sacola.TotalProduto = sacola.TotalProduto + (produto2.Produto.Valor * produto2.Produto.Qtde);
                sacola.TotalSacola = sacola.TotalProduto + sacola.Sedex;
            }
            HttpContext.Session["sacola"] = sacola;
            return new RedirectResult("~/Home/Cart?listaProduto=");
        }
        public ActionResult ExcluirProduto(string codigo)
        {
            Sacola sacola = (Sacola)HttpContext.Session["sacola"];
            sacola.TotalProduto = 0;
            List<Produto> listaProdutoSelecionado = new List<Produto>();
            if (!String.IsNullOrEmpty(codigo))
            {
                Produto produto = ProdutoBO.ObterProduto(codigo);
                foreach (SacolaProduto prod in sacola.Produtos)
                {
                    if (!prod.Produto.Equals(produto))
                    {
                        listaProdutoSelecionado.Add(prod.Produto);


                    }
                }
                sacola.Produtos.Clear();
                foreach (Produto prod in listaProdutoSelecionado)
                {
                    SacolaProduto sacProd = new SacolaProduto();
                    sacProd.Produto = prod;
                    sacProd.ProdutoId = prod.Id;
                    sacProd.Sacola = sacola;
                    sacProd.SacolaId = sacola.Id;
                    sacola.Produtos.Add(sacProd);
                }
                foreach (SacolaProduto prod2 in sacola.Produtos)
                {
                    sacola.TotalProduto = sacola.TotalProduto + (prod2.Produto.Valor * prod2.Produto.Qtde);
                }
                for (int i = 0; i < sacola.ProdutoSelecionado.Length; i++)
                {
                    if (sacola.ProdutoSelecionado[i].Equals(codigo))
                    {
                        sacola.ProdutoSelecionado[i] = 0;
                    }
                }


            }
            sacola.TotalSacola = sacola.TotalProduto + sacola.Sedex;
            ViewBag.QtdeProdutoSelecionado = sacola.Produtos.Count;
            HttpContext.Session["sacola"] = sacola;
            return View("Cart", sacola);
        }

        public ActionResult AtualizarQuantidade(string codigo, string qtde)
        {
            Sacola sacola = (Sacola)HttpContext.Session["sacola"];
            sacola.TotalProduto = 0;
            List<Produto> listaProdutoSelecionado = new List<Produto>();
            int qtdeInt = 0;
            Int32.TryParse(qtde, out qtdeInt);
            if (qtdeInt == null || qtdeInt <= 0)
            {
                TempData["Erro"] = "Quantidade invalida.";
            }
            else
            {
                if (!String.IsNullOrEmpty(codigo))
                {
                    Produto produto = ProdutoBO.ObterProduto(codigo);
                    foreach (SacolaProduto prod in sacola.Produtos)
                    {
                        if (prod.Produto.Codigo.Equals(codigo))
                        {
                            prod.Produto.Qtde = Int32.Parse(qtde);
                        }
                    }
                    foreach (SacolaProduto prod2 in sacola.Produtos)
                    {
                        sacola.TotalProduto = sacola.TotalProduto + (prod2.Produto.Valor * prod2.Produto.Qtde);
                    }
                    for (int i = 0; i < sacola.ProdutoSelecionado.Length; i++)
                    {
                        if (sacola.ProdutoSelecionado[i].Equals(codigo))
                        {
                            sacola.ProdutoSelecionado[i] = 0;
                        }
                    }


                }
            }
            sacola.TotalSacola = sacola.TotalProduto + sacola.Sedex;
            ViewBag.QtdeProdutoSelecionado = sacola.Produtos.Count;
            HttpContext.Session["sacola"] = sacola;
            return View("Cart", sacola);
        }

        [HttpPost]
        [MultipleButton(Name = "cart", Argument = "Sedex")]
        public async Task<ActionResult> CalcularSedex(Sacola sacola)
        {
            string cep = sacola.Cep;
            int numero = sacola.NumeroEndereco == null ? 0 : sacola.NumeroEndereco.Value;
            string complemento = sacola.Complemento;
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            sacola = (Sacola)HttpContext.Session["sacola"];
            sacola.Cep = cep;

            sacola.Sedex = 0;
            if (string.IsNullOrEmpty(sacola.Cep))
            {
                TempData["Erro"] = "Cep é obrigatório.";
                return new RedirectResult("~/Home/Cart");
            }

            ActionResult result = await Sedex(sacola);
            return result;
        }


        [HttpPost]
        [MultipleButton(Name = "cart", Argument = "Checkout")]
        public async Task<ActionResult> Cart(Sacola sacola)
        {
            string cep = sacola.Cep;
            int numero = sacola.NumeroEndereco == null ? 0 : sacola.NumeroEndereco.Value;
            string complemento = sacola.Complemento;
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            sacola = (Sacola)HttpContext.Session["sacola"];
            
            sacola.Cep = cep;

            if (string.IsNullOrEmpty(sacola.Cep))
            {
                TempData["Erro"] = "Cep é obrigatório.";
                return new RedirectResult("~/Home/Cart");
            }
            if (numero == 0)
            {
                TempData["Erro"] = "Número é obrigatório.";
                return new RedirectResult("~/Home/Cart");
            }
            sacola.NumeroEndereco = numero;
            sacola.Complemento = complemento;
            Pedido pedido = new Pedido();
            pedido.Sacola = sacola;
            sacola.TipoPagamento = "1";
            HttpContext.Session["pedido"] = pedido;
            HttpContext.Session["sacola"] = sacola;
            if (usuario == null || string.IsNullOrEmpty(usuario.Nome))
            {
                return new RedirectResult("~/Home/Login");
            }
            else
            {
                return PagSeguro();
            }
            //    if (sacola.Rua.Equals("Rua San Marino"))
            //    {
            //        return PagSeguro();
            //    }



        }
        private async Task<Cep> RetornaCEPAsync(string cep)
        {
            string restUrl = "http://viacep.com.br/ws/" + cep + "/json/";

            var tasks = new List<Task<Cep>>();
            RestService<Cep> service = new RestService<Cep>();
            tasks.Add(service.RefreshDataAsync(restUrl, new object[1]));
            Cep[] listaResult = await Task.WhenAll(tasks);
            var resultado = listaResult[0];
            return resultado;
        }

        public async Task<ActionResult> Sedex(Sacola sacola)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            string cep = sacola.Cep;
            //Recupera Endereço
            try
            {
                var task = await RetornaCEPAsync(sacola.Cep.Replace("-", ""));
                if (!String.IsNullOrEmpty(task.logradouro))
                {
                    sacola.Rua = task.logradouro;
                    sacola.Estado = task.uf;
                    sacola.Bairro = task.bairro;
                    sacola.Cidade = task.localidade;
                    var sedex = new CalcularPrecoPrazo.CalcPrecoPrazoWSSoapClient("CalcPrecoPrazoWSSoap");
                    decimal valorDeclarado = 0;
                    if(sacola.TotalProduto < 20.5)
                    {
                        valorDeclarado = new decimal(20.5);
                    }
                    else
                    {
                        valorDeclarado = new decimal(sacola.TotalProduto);
                    }
                    CalcularPrecoPrazo.cResultado resposta = sedex.CalcPrecoPrazo("", "", "04014", "31035536", sacola.Cep, "0.3", 1, new decimal(30), new decimal(30), new decimal(30), new decimal(30), "N", valorDeclarado, "N");
                    sacola.MostrarEndereco = true;
                    if (!sacola.Cidade.Equals("Belo Horizonte") || sacola.Cidade.Equals("Contagem") || sacola.Cidade.Equals("Betim") || sacola.Cidade.Equals("Lagoa Santa") || sacola.Cidade.Equals("Santa Luzia"))
                    {
                        sacola.Sedex = double.Parse(resposta.Servicos[0].Valor.Replace(",", "."), CultureInfo.InvariantCulture);
                        if(sacola.Sedex == 0)
                        {
                            sacola.Sedex = 20.5;
                        }
                    }
                    else
                    {
                        sacola.Sedex =  0;
                    }
                    //  String[] valores = resposta.Servicos[0].Valor.Split(',');
                    sacola.SedexFormatado = resposta.Servicos[0].Valor;
                    // sacola.SedexFormatado = string.Format("R${0:0.00#,##}", sacola.Sedex);
                    //sacola.SedexFormatado = sacola.Sedex.ToString().Replace(".", "").Replace(",", "");
                    sacola.TotalSacola = sacola.TotalProduto + sacola.Sedex;
                    sacola.NumeroEndereco = null;
                }
                else
                {
                    sacola = this.LimparEndereco(sacola);
                    sacola.Cep = cep;
                    TempData["Erro"] = "Cep não encontrado.";
                    log.Debug("------------------------------ CEP - NOK " + "Cep não encontrado.");
                }
            }
            catch (Exception e)
            {
                TempData["Erro"] = e.ToString();
                log.Debug("------------------------------ CEP - NOK " + e.ToString());
                throw e;

            }

            ViewBag.QtdeProdutoSelecionado = sacola.Qtde;
            HttpContext.Session["sacola"] = sacola;
            return new RedirectResult("~/Home/Cart");
        }


        [HttpPost]
        public ActionResult Contact(Email email)
        {
            try
            {
                this.EnviarEmailContato(email);
                // data.Dispose();
                TempData["Mensagem"] = " Email enviado com sucesso";

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateMessageWithAttachment(): {0}",
                      ex.ToString());
                TempData["Erro"] = ex.Message;
            }

            return View("");
        }

        

        public void EnviarEmailContato(Email email)
        {
            try
            {
                MailMessage m = new MailMessage();
                SmtpClient sc = new SmtpClient();
                m.From = new MailAddress("contato@boxfesta.com.br");
                m.To.Add("contato@boxfesta.com.br");
                m.Subject = "[Contato Box Festa] " + email.Assunto;
                m.IsBodyHtml = true;
                m.Body = string.Format("Nome: {0} <br/> Email: {1} <br/> Mensagem: {2}", email.Nome, email.EmailContato ,email.Mensagem);
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



        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Us()
        {


            return View();
        }

        public ActionResult FinalizarPedido(string id_transacao = "")
        {
            Pedido pedido = (Pedido)HttpContext.Session["pedido"];
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            if(pedido == null && usuario !=null)
            {
                pedido = PedidoBO.ObterPedidoUsuario(usuario.Id);
            }
            if (!string.IsNullOrEmpty(id_transacao) && pedido != null && pedido.Id != 0)
            {
                pedido = PedidoBO.ObterPedido(pedido.Id);
                pedido.Sacola = SacolaBO.ObterSacola(pedido.SacolaId);
                pedido.IdTransacao = id_transacao;
                PedidoBO.EditarPedido(pedido);
            }
            HttpContext.Session["pedido"] = null;
            HttpContext.Session["sacola"] = null;
            ViewBag.ValorProdutoSelecionado = 0;
            ViewBag.QtdeProdutoSelecionado = 0;
            ViewBag.ProdutosSacola = "";
            ViewBag.Usuario = usuario;

            return View("", pedido);
        }

        public ActionResult Pesquisa(string listaProduto, string buscar = "")
        {
            string[] palavrasBuscar = buscar.ToLower().Split(' ');

            PreencherSacola(listaProduto);
            Sacola sacola = (Sacola)HttpContext.Session["sacola"];


            List<Produto> listaProdutos = ProdutoBO.ListarProduto();
            if (sacola == null)
            {
                sacola = new Sacola();

            }
            sacola = this.LimparEndereco(sacola);
            sacola.Sedex = 0;
            sacola.TotalProduto = 0;
            foreach (SacolaProduto produto in sacola.Produtos)
            {
                sacola.TotalProduto = sacola.TotalProduto + (produto.Produto.Valor * produto.Produto.Qtde);
            }
            sacola.TotalSacola = sacola.TotalProduto;
            List<Produto> listaTodosProdutos = listaProdutos.ToList();

            List<Produto> listaTodosProdutosFiltrados = new List<Produto>();
            if (palavrasBuscar.Length > 0 && !palavrasBuscar[0].Equals(""))
            {
                foreach (Produto prod in listaTodosProdutos)
                {
                    foreach (string palavraBuscar in palavrasBuscar)
                    {
                        if (palavraBuscar.Length > 2 && !palavraBuscar.Equals("para"))
                        {
                            if (prod.TemaRelacionado.Contains(palavraBuscar))
                            {
                                if (!listaTodosProdutosFiltrados.Contains(prod))
                                {
                                    listaTodosProdutosFiltrados.Add(prod);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                listaTodosProdutosFiltrados.AddRange(listaTodosProdutos);
            }

            ViewBag.TotalAvulso = listaTodosProdutosFiltrados.Count;
            ViewBag.ListaProduto = listaTodosProdutosFiltrados;
            ViewBag.ListaProdutoArray = sacola.ProdutoSelecionado;
            ViewBag.ValorProdutoSelecionado = sacola.TotalSacola;
            ViewBag.ProdutosSacola = "";


            ViewBag.QtdeProdutoSelecionado = sacola.Produtos.Count;
            HttpContext.Session["sacola"] = sacola;
            return View(sacola);
        }

        public ActionResult SyncToGoogleCalendar()
        {
            GoogleOauthTokenService.OauthToken = "";
            if (string.IsNullOrWhiteSpace(GoogleOauthTokenService.OauthToken))
            {
                var redirectUri = GoogleCalendarSyncer.GetOauthTokenUri(this);
                return Redirect(redirectUri);
            }
            /*else
            {
                var success = GoogleCalendarSyncer.SyncToGoogleCalendar(this);
                if (!success)
                {
                    return Json("Token was revoked. Try again.",JsonRequestBehavior.AllowGet);
                }
            }*/
            return new RedirectResult("~/Home/Login");
        }

        public ActionResult PagSeguro()
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Debug("-------------------------------PagSeguro");
            Sacola sacola = (Sacola)HttpContext.Session["sacola"];
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            Pedido pedido = (Pedido)HttpContext.Session["pedido"];
            string code = "";
            if (pedido == null)
            {
                pedido = new Pedido();
                long ultimoId = PedidoBO.ObterUltimoId();
                pedido.Id = long.Parse(PreencherId(ultimoId.ToString()));
            }

            try
            {
                //var url = "https://ws.pagseguro.uol.com.br/v2/checkout";
                var url = "https://ws.sandbox.pagseguro.uol.com.br/v2/checkout";
                var content = "";
                content += "email=rapsch3@gmail.com";
                //content += "email=v11367235410983738939@sandbox.pagseguro.com.br";
                // content += "&token=a88fbdb1-2df5-45dc-af24-c7f64e303653b58a0b24483fb536ac84be8b536da9d9f8b9-10a6-4177-a75c-5ae7e249ad1c";
                content += "&token=5269A70FB9FD41D3A2BDD1C305B142AE";
                content += "&currency=BRL";
                content += "&reference=" + pedido.Id;



                string[] telefones = new string[2];
                string ddd = "(31)";
                string tel = "99304-7540";
                string nome = usuario.Nome;
                string email = "c14625593130199418727@sandbox.pagseguro.com.br";
                string telefone = ddd + tel;
                if (!String.IsNullOrWhiteSpace(telefone) && telefone.Contains(")") && telefone.Contains("-"))
                {
                    telefones = telefone.Split(')');
                    ddd = telefones[0].Replace("(", "").Replace(")", "");
                    tel = telefones[1].Replace(")", "").Replace("-", "");
                    //    content += "&reference = " + pedido.Id;
                    content += "&senderName=" + nome;
                    content += "&senderAreaCode=" + ddd;
                    content += "&senderPhone=" + tel;
                    content += "&senderEmail=" + email;
                    content += "&shippingType=2";
                    content += "&shippingCost=" + sacola.Sedex.ToString("N2").Replace(",", ".");
                    content += "&shippingAddressStreet=" + sacola.Rua;
                    content += "&shippingAddressNumber=" + sacola.NumeroEndereco;
                    content += "&shippingAddressDistrict=" + sacola.Bairro;
                    content += "&shippingAddressPostalCode=" + sacola.Cep;
                    content += "&shippingAddressCity=" + sacola.Cidade;
                    content += "&shippingAddressState=" + sacola.Estado;
                    content += "&shippingAddressCountry=BRA";
                    content = RecuperarProdutos(content, sacola.Produtos);

                    var xmlResult = "";
                    var task = new Task(() =>
                    {
                        xmlResult = RestService<String>.AccessTheWebAsync(url, content, "post").Result;
                    });
                    task.Start();
                    task.Wait();

                    if (!string.IsNullOrEmpty(xmlResult))
                    {
                        var _doc = XDocument.Parse(xmlResult);
                        var hasErrors = _doc.Descendants("errors").Count() > 0;

                        if (!hasErrors)
                        {
                            log.Debug("-------------------------------PagSeguro - OK - Checkout Efetuado sem erros");
                            log.Debug("-------------------------------Criar Pedido");
                            CriarPedido(usuario.Email, pedido.Id);
                            log.Debug("-------------------------------Pedido - OK - Pedido criado sem Erros");
                            var checkout = _doc.Descendants("checkout");
                            code = checkout.FirstOrDefault().Element("code").Value;
                            //_navegacao.NavigateTo("PagSeguroUOL", code);
                            //  PagSeguro pagseguro = new PagSeguro();
                            //  pagseguro.Content = "https://pagseguro.uol.com.br/v2/checkout/payment.html?code=" + code;
                            //  HttpContext.Session["sacola"] = null;
                            //    return new RedirectResult(pagseguro.Content);
                            //    return View("Cart", sacola);
                        }
                        else
                        {
                            TempData["Erro"] = "Erro no Pag Seguro " + xmlResult;
                            log.Debug("-------------------------------Pedido - NOK " + xmlResult);
                        }
                    }
                }
                else
                {
                    // page.DisplayAlert("Erro", "Formato de telefone inválido(XX)XXXXX-XXXX.", "Ok");
                    TempData["Erro"] = "Erro no Web Service";
                    ViewBag.Ocorrencias = "Erro no Pag Seguro ";
                    log.Debug("-------------------------------Pedido - NOK " + "Erro no Pag Seguro ");
                }
            }
            catch (Exception ex)
            {
                log.Debug("-------------------------------PagSeguro - NOK " + ex.ToString());
                TempData["Erro"] = ex.Message;
                return new RedirectResult("~/Home/Cart");
            }


            return new RedirectResult("~/Home/Cart?listaProduto=''&code=" + code);
        }

        private string RecuperarProdutos(string content, List<SacolaProduto> listaProduto)
        {
            if (listaProduto != null && listaProduto.Count > 0)
            {
                int j = 1;
                foreach (SacolaProduto produto in listaProduto)
                {
                    content += "&itemId" + j + "=" + produto.Produto.Codigo.ToString();
                    content += "&itemDescription" + j + "=" + produto.Produto.Descricao;
                    content += "&itemAmount" + j + "=" + (produto.Produto.Valor).ToString("N2").Replace(",", ".");
                    content += "&itemQuantity" + j + "=" + produto.Produto.Qtde;

                    j++;
                }

            }
            return content;
        }

        private bool ValidarInsercaoUsuario(Usuario usuario)
        {
            if (string.IsNullOrEmpty(usuario.NovoCpf))
            {
                return false;
            }

            if (string.IsNullOrEmpty(usuario.NovoEmail))
            {
                return false;
            }

            if (string.IsNullOrEmpty(usuario.Nome))
            {
                return false;
            }

            if (string.IsNullOrEmpty(usuario.NovaSenha))
            {
                return false;
            }

            if (string.IsNullOrEmpty(usuario.Telefone))
            {
                return false;
            }

            return true;
        }
        private void PreencherSacola(string sacolaProdutos)
        {
            Sacola sacola = (Sacola)HttpContext.Session["sacola"];

            //sacola.SedexFormatado = string.Format("R${0:0.00#,##}", 0);
            // sacola.SedexFormatado =  0.ToString().Replace(".","").Replace(",","");
            if (sacola == null)
            {
                sacola = new Sacola();
            }
            sacola.TotalProduto = 0;
            if (!String.IsNullOrEmpty(sacolaProdutos) && sacolaProdutos.Contains(','))
            {
                string[] listaProdutoArray = sacolaProdutos.Split(',');
                for (int i = 0; i < listaProdutoArray.Length - 1; i++)
                {

                    // Primeiro Preenche os produtos da sessao e dps da requisição
                    Produto produto = ProdutoBO.ObterProduto(listaProdutoArray[i]);


                    if (!sacola.Produtos.Any(c => c.ProdutoId == produto.Id))
                    {
                        produto.Qtde = 1;
                        SacolaProduto sacolaProd = new SacolaProduto();
                        sacolaProd.Sacola = sacola;
                        sacolaProd.SacolaId = sacola.Id;
                        sacolaProd.Produto = produto;
                        sacolaProd.ProdutoId = produto.Id;
                        sacola.Produtos.Add(sacolaProd);

                    }
                    else
                    {
                        SacolaProduto sacolaProduto = sacola.Produtos.Where(c => c.Produto.Codigo.Equals(listaProdutoArray[i])).First();
                        produto = sacolaProduto.Produto;
                        produto.Qtde++;
                    }

                     

                }
                //Retira os duplicados
                List<Produto> listaSemDuplicado = new List<Produto>();
                foreach (SacolaProduto prod in sacola.Produtos)
                {
                    if (!listaSemDuplicado.Contains(prod.Produto))
                    {
                        listaSemDuplicado.Add(prod.Produto);
                    }
                    else
                    {
                        Produto produto = listaSemDuplicado.Where(c => c.Codigo.Equals(prod.Produto.Codigo)).FirstOrDefault();
                        produto.Qtde++;
                    }
                }
                sacola.Produtos.Clear();
                foreach (Produto produto in listaSemDuplicado) {
                    SacolaProduto sacolaProd = new SacolaProduto();
                    sacolaProd.Produto = produto;
                    sacolaProd.ProdutoId = produto.Id;
                    sacolaProd.Sacola = sacola;
                    sacolaProd.SacolaId = sacola.Id;
                    sacola.Produtos.Add(sacolaProd);
                }
                //Fim do Retira duplicação
                foreach (SacolaProduto prod in sacola.Produtos)
                {
                    sacola.TotalProduto = sacola.TotalProduto + (prod.Produto.Valor * prod.Produto.Qtde);
                }
                sacola.TotalSacola = sacola.TotalProduto + sacola.Sedex;
                sacola.ProdutoSelecionado = new long[(ProdutoBO.ListarProduto()).Count];

            }
            HttpContext.Session["sacola"] = sacola;
        }
        private Sacola LimparEndereco(Sacola sacola)
        {
            sacola.Sedex = 0;
            // sacola.SedexFormatado = sacola.Sedex.ToString().Replace(",","").Replace(".","");
            // sacola.SedexFormatado = string.Format("R${0:0.00#,##}", sacola.Sedex);
            //sacola.SedexFormatado = string.Format("{0:C}", sacola.Sedex);
            sacola.Rua = "";
            sacola.Cep = "";
            sacola.Cidade = "";
            sacola.Estado = "";
            sacola.NumeroEndereco = 0;
            sacola.MostrarEndereco = false;

            return sacola;
        }


        private Usuario CriarUsuario()
        {
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            if (usuario == null || string.IsNullOrEmpty(usuario.Nome))
            {
                usuario = new Usuario();
                usuario.Cpf = usuario.Cpf;
                usuario.Nome = "Teste teste";
                usuario.Senha = "teste@teste.com";
                usuario.Telefone = "31999999999";
                usuario.Senha = "teste";
            }
            else
            {
                usuario.Cpf = usuario.Cpf;
                usuario.Nome = usuario.Nome;
                usuario.Senha = Sha1BO.RetornarSHA(usuario.Senha);
                usuario.Telefone = usuario.Telefone;
                usuario.Email = usuario.Email;
            }

            UsuarioBO.CadastrarUsuario(usuario);
            return usuario;
        }

        private void CriarPedido(string cpf = "", long idPedido = 0)
        {
            Sacola sacola = (Sacola)HttpContext.Session["sacola"];
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            if (usuario != null && String.IsNullOrEmpty(usuario.Cpf))
            {
                usuario = UsuarioBO.ObterUsuario(usuario.Email);
            }
            if (usuario == null || !UsuarioBO.ExisteUsuario(usuario.Email))
            {
                usuario = CriarUsuario();
            }


            Pedido pedido = new Pedido();
            pedido.Id = idPedido;
            pedido.Data = DateTime.Now.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year;
            pedido.Hora = DateTime.Now.Hour.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0');
            pedido.Usuario = usuario;
            pedido.Sacola = sacola;
            foreach (SacolaProduto sacProd in pedido.Sacola.Produtos)
            {
                sacProd.Id = 0;
                sacProd.ProdutoId = sacProd.Produto.Id;
            }
            pedido.Sacola.Id = 0;
            PedidoBO.GravarPedido(pedido);
            pedido.SacolaId = pedido.Sacola.Id;
            PedidoBO.EditarPedido(pedido);
            
            HttpContext.Session["pedido"] = pedido;
            HttpContext.Session["usuario"] = usuario;
        }

        private string PreencherId(string ultimoId)
        {
            if (ultimoId.Length > 7)
            {
                ultimoId = ultimoId.Substring(9);
            }
            return DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + (Int32.Parse(ultimoId) + 1).ToString().PadLeft(9, '0');
        }
    }
}