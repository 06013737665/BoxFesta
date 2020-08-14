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

namespace Box.Festa.Areas.Admin.Controllers
{
    public class ProdutoController : Controller
    {


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
            return View("Produto", produto);
        }

        public ActionResult IncluirProduto()
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            ViewBag.Admin = admin;
            Produto produto = new Produto();
            return View("Produto", produto);
        }

        [HttpPost]
        public ActionResult IncluirProduto(Produto produto)
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            ViewBag.Admin = admin;
            if (produto.Codigo.Equals("") || produto.Descricao.Equals("") || produto.Valor == 0 || produto.TemaRelacionado.Equals(""))
            {
                TempData["Mensagem"] = "Favor preencher todos os campos.";
                return View("Kit", produto);
            }
            produto.ehKit = false;
            produto.ehMaisVendido = true;
            produto.ehSale = false;
            ProdutoBO.CadastrarProduto(produto);

            TempData["Mensagem"] = " Produto cadastrado com sucesso.";
            return View("Produto", produto);
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
            if (produto.Codigo.Equals("") || produto.Descricao.Equals("") || produto.Valor == 0 || produto.TemaRelacionado.Equals(""))
            {
                TempData["Mensagem"] = "Favor preencher todos os campos.";
                return View("Kit", produto);
            }
            ProdutoBO.EditarProduto(produto);

            TempData["Mensagem"] = " Produto editado com sucesso.";
            return View("Produto", produto);
        }

        public ActionResult ExcluirProduto(string codigo)
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            ViewBag.Admin = admin;
            Produto produto = ProdutoBO.ObterProduto(codigo.ToString());
            ProdutoBO.ExcluirProduto(produto);
            TempData["Mensagem"] = " Produto excluído com sucesso.";
            List<Produto> listaProduto = ProdutoBO.ListarTodosProdutos().OrderBy(c => c.Id).ToList();
            ViewBag.lista = listaProduto;
            ViewBag.TotalResultados = listaProduto.Count;
            return View("ListarProduto");
        }
        public FileResult Imprimir(string codigo)
        {

            Produto pojo = ProdutoBO.ObterProduto(codigo);

            string caminho = pojo.Imprimir(pojo);
            FileInfo arquivoInfo = new FileInfo(caminho);
            byte[] fileBytes = System.IO.File.ReadAllBytes(caminho);
            string fileName = arquivoInfo.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }

    }
}
