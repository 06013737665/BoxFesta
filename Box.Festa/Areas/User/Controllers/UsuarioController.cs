using System;
using System.Collections.Generic;
using System.IO;
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
using Plugin.Media.Abstractions;

namespace Box.Festa.Areas.User.Controllers
{
    public class UsuarioController : Controller
    {


        public ActionResult Index()
        {
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            
            PreencherViewBag();

            return View("User", usuario);
        }

        [HttpPost]
        public ActionResult Index(Usuario usuario)
        {
            Usuario usuarioSessao = (Usuario)HttpContext.Session["usuario"];
            this.PreencherViewBag();
            usuario.Id = usuarioSessao.Id;
            if (usuario.Endereco != null && usuario.Endereco.Id > 0) {
                usuario.Endereco = EnderecoBO.ObterEndereco(usuario.Endereco.Id, usuarioSessao.Id);
            }
            if (usuario.FormaPagamento != null && usuario.FormaPagamento.Id > 0)
            {
                usuario.FormaPagamento = FormaPagamentoBO.ObterFormaPagamento(usuario.FormaPagamento.Id, usuarioSessao.Id);
            }
            UsuarioBO.EditarUsuario(usuario);

            TempData["Mensagem"] = "Usuário cadastrado com sucesso.";
            return View("User", usuario);
        }
        public ActionResult Sair()
        {
            HttpContext.Session.Remove("usuario");
            HttpContext.Session.Remove("sacola");

            return new RedirectResult("~/Home/Index");
        }

        public FileResult Imprimir()
        {
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            PreencherViewBag();
            string caminho = usuario.Imprimir(usuario);
            FileInfo arquivoInfo = new FileInfo(caminho);
            byte[] fileBytes = System.IO.File.ReadAllBytes(caminho);
            string fileName = arquivoInfo.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
           
        }

        private void PreencherViewBag()
        {
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

            sacola.TotalProduto = 0;
            foreach (SacolaProduto produto in sacola.Produtos)
            {
                sacola.TotalProduto = sacola.TotalProduto + (produto.Produto.Valor * produto.Produto.Qtde);
            }

            sacola.TotalSacola = sacola.TotalProduto;
            List<Produto> listaTodosProdutosAvulsos = listaProduto.Where(c => c.ehKit == false).ToList();
            List<Endereco> listaEndereco = EnderecoBO.ListarEndereco(usuario.Id);
            List<FormaPagamento> listaFormaPagamento = FormaPagamentoBO.ListarFormaPagamento(usuario.Id);
            ViewBag.ComboEndereco = new SelectList(listaEndereco, "Id", "Rua"); 
            ViewBag.ComboFormaPagamento = new SelectList(listaFormaPagamento, "Id", "Numero"); 

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
        }
    }
}
