using Box.Festa.Models;
using Box.Festa.Negocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Box.Festa.Areas.User.Controllers
{
    public class EnderecoController : Controller
    {
        public ActionResult ListarEndereco()
        {
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            this.PreencherViewBag();

            List<Endereco> listaEndereco = EnderecoBO.ListarEndereco(usuario.Id).OrderBy(c => c.Id).ToList();
            ViewBag.lista = listaEndereco;
            ViewBag.TotalResultados = listaEndereco.Count;
            return View("ListarEndereco");
        }

        public ActionResult EditarEndereco(long id)
        {
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            this.PreencherViewBag();

            Endereco endereco = EnderecoBO.ObterEndereco(id, usuario.Id);
            endereco.Usuario = usuario;
            endereco.UsuarioId = usuario.Id;
            return View("Endereco", endereco);
        }

        public ActionResult IncluirEndereco()
        {
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            this.PreencherViewBag();
            Endereco endereco = new Endereco();
            return View("Endereco", endereco);
        }

        [HttpPost]
        public ActionResult IncluirEndereco(Endereco endereco)
        {
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            this.PreencherViewBag();
            if (endereco.Rua.Equals("") || endereco.Bairro.Equals("") || endereco.Numero == 0 || endereco.Cidade.Equals("") || endereco.Estado.Equals(""))
            {
                TempData["Mensagem"] = "Favor preencher todos os campos.";
                return View("Endereco", endereco);
            }
            endereco.Usuario = usuario;
            endereco.UsuarioId = usuario.Id;
            EnderecoBO.InserirEndereco(endereco);

            TempData["Mensagem"] = "Endereço cadastrado com sucesso.";
            return View("Endereco", endereco);
        }

        [HttpPost]
        public ActionResult EditarEndereco(Endereco endereco)
        {
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            this.PreencherViewBag();
            if (endereco.Rua.Equals("") || endereco.Bairro.Equals("") || endereco.Numero == 0 || endereco.Cidade.Equals("") || endereco.Estado.Equals(""))
            {
                TempData["Mensagem"] = "Favor preencher todos os campos.";
                return View("Endereco", endereco);
            }
            endereco.UsuarioId = usuario.Id;
            EnderecoBO.EditarEndereco(endereco);

            TempData["Mensagem"] = " Endereco editado com sucesso.";
            return View("Endereco", endereco);
        }

        public ActionResult ExcluirEndereco(long id)
        {
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            this.PreencherViewBag();
            Endereco endereco = EnderecoBO.ObterEndereco(id, usuario.Id);
            EnderecoBO.ExcluirEndereco(endereco);
            TempData["Mensagem"] = " Endereço excluído com sucesso.";
            List<Endereco> listaEndereco = EnderecoBO.ListarEndereco(usuario.Id).OrderBy(c => c.Id).ToList();
            ViewBag.lista = listaEndereco;
            ViewBag.TotalResultados = listaEndereco.Count;
            return View("ListarEndereco");
        }

        public FileResult Imprimir(long id)
        {
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];

            Endereco endereco = EnderecoBO.ObterEndereco(id, usuario.Id);
            PreencherViewBag();
            string caminho = endereco.Imprimir(endereco);
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