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
    public class FormaPagtoController : Controller
    {
        public ActionResult ListarFormaPagamento()
        {
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            this.PreencherViewBag();

            List<FormaPagamento> listaFormaPagamento = FormaPagamentoBO.ListarFormaPagamento(usuario.Id).OrderBy(c => c.Id).ToList();
            ViewBag.lista = listaFormaPagamento;
            ViewBag.TotalResultados = listaFormaPagamento.Count;
            return View("ListarFormaPagto");
        }

        public ActionResult EditarFormaPagamento(long id)
        {
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            this.PreencherViewBag();

            FormaPagamento formaPagamento = FormaPagamentoBO.ObterFormaPagamento(id, usuario.Id);
            formaPagamento.Usuario = usuario;
            formaPagamento.UsuarioId = usuario.Id;
            return View("FormaPagamento", formaPagamento);
        }

        public ActionResult IncluirFormaPagamento()
        {
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            this.PreencherViewBag();
            FormaPagamento formaPagamento = new FormaPagamento();
            formaPagamento.UsuarioId = usuario.Id;
            return View("FormaPagamento", formaPagamento);
        }

        [HttpPost]
        public ActionResult IncluirFormaPagamento(FormaPagamento formaPagamento)
        {
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            this.PreencherViewBag();
            if (formaPagamento.Numero.Equals("") || formaPagamento.CpfProprietario.Equals("") || formaPagamento.NomeProprietario.Equals("") || formaPagamento.Validade.Equals("") || formaPagamento.Codigo.Equals(""))
            {
                TempData["Mensagem"] = "Favor preencher todos os campos.";
                return View("FormaPagamento", formaPagamento);
            }
            formaPagamento.Usuario = usuario;
            formaPagamento.UsuarioId = usuario.Id;
            formaPagamento.CpfProprietario = formaPagamento.CpfProprietario.Replace(".", "").Replace("-", "");
            FormaPagamentoBO.InserirFormaPagamento(formaPagamento);

            TempData["Mensagem"] = "Forma de Pagamento cadastrado com sucesso.";
            return View("FormaPagamento", formaPagamento);
        }

        [HttpPost]
        public ActionResult EditarFormaPagamento(FormaPagamento formaPagamento)
        {
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            this.PreencherViewBag();
            if (formaPagamento.Numero.Equals("") || formaPagamento.CpfProprietario.Equals("") || formaPagamento.NomeProprietario.Equals("") || formaPagamento.Validade.Equals("") || formaPagamento.Codigo.Equals(""))
            {
                TempData["Mensagem"] = "Favor preencher todos os campos.";
                return View("FormaPagamento", formaPagamento);
            }
            formaPagamento.CpfProprietario = formaPagamento.CpfProprietario.Replace(".", "").Replace("-", "");
            formaPagamento.UsuarioId = usuario.Id;
            FormaPagamentoBO.EditarFormaPagamento(formaPagamento);

            TempData["Mensagem"] = " Forma de Pagamento editado com sucesso.";
            return View("FormaPagamento", formaPagamento);
        }

        public ActionResult ExcluirFormaPagamento(long id)
        {
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];
            this.PreencherViewBag();
            FormaPagamento formaPagamento = FormaPagamentoBO.ObterFormaPagamento(id, usuario.Id);
            FormaPagamentoBO.ExcluirFormaPagamento(formaPagamento);
            TempData["Mensagem"] = " Forma de Pagamento excluído com sucesso.";
            List<FormaPagamento> listaFormaPagamento = FormaPagamentoBO.ListarFormaPagamento(usuario.Id).OrderBy(c => c.Id).ToList();
            ViewBag.lista = listaFormaPagamento;
            ViewBag.TotalResultados = listaFormaPagamento.Count;
            return View("ListarFormaPagto");
        }
        public FileResult Imprimir(long id)
        {
            Usuario usuario = (Usuario)HttpContext.Session["usuario"];

            FormaPagamento pojo = FormaPagamentoBO.ObterFormaPagamento(id, usuario.Id);
            PreencherViewBag();
            string caminho = pojo.Imprimir(pojo);
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