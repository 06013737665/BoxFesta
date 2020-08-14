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
    public class KitController : Controller
    {


        public ActionResult ListarKit()
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            ViewBag.Admin = admin;
            List<Produto> listaProduto = ProdutoBO.ListarTodosProdutos().Where(c=>c.ehKit == true).OrderBy(c => c.Id).ToList();
            ViewBag.lista = listaProduto;
            ViewBag.TotalResultados = listaProduto.Count;
            return View("ListarKit");
        }

        public ActionResult EditarKit(string codigo)
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            ViewBag.Admin = admin;
            Produto produto = ProdutoBO.ObterProduto(codigo);
            return View("Kit", produto);
        }

        public ActionResult IncluirKit()
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            ViewBag.Admin = admin;
            Produto produto = new Produto();
            return View("Kit", produto);
        }

        [HttpPost]
        public ActionResult IncluirKit(Produto produto)
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
           
            if (produto.Codigo.Equals("") || produto.Descricao.Equals("") || produto.Valor==0 || produto.TemaRelacionado.Equals("") || produto.QtdePessoas == 0)
            {
                TempData["Mensagem"] = "Favor preencher todos os campos.";
                return View("Kit", produto);
            }
            ViewBag.Admin = admin;

            produto.ehKit = true;
            produto.ehMaisVendido = true;
            produto.ehSale = false;
            ProdutoBO.CadastrarProduto(produto);

            TempData["Mensagem"] = "Kit cadastrado com sucesso.";
            return View("Kit", produto);
        }

        [HttpPost]
        public ActionResult EditarKit(Produto produto)
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            if (produto.Codigo.Equals("") || produto.Descricao.Equals("") || produto.Valor == 0 || produto.TemaRelacionado.Equals("") || produto.QtdePessoas == 0)
            {
                TempData["Mensagem"] = "Favor preencher todos os campos.";
                return View("Kit", produto);
            }
            ViewBag.Admin = admin;

            ProdutoBO.EditarProduto(produto);

            TempData["Mensagem"] = " Kit editado com sucesso.";
            return View("Kit", produto);
        }

        public ActionResult ExcluirKit(string codigo)
        {
            Usuario admin = (Usuario)HttpContext.Session["admin"];

            if (admin == null)
            {
                return new RedirectResult("~/Admin/Admin/Login");
            }
            ViewBag.Admin = admin;
            Produto produto = ProdutoBO.ObterProduto(codigo.ToString());
            ProdutoBO.ExcluirProduto(produto);
            TempData["Mensagem"] = " Kit excluído com sucesso.";
            List<Produto> listaProduto = ProdutoBO.ListarTodosProdutos().Where(c=>c.ehKit== true).OrderBy(c => c.Id).ToList();
            ViewBag.lista = listaProduto;
            ViewBag.TotalResultados = listaProduto.Count;
            return View("ListarKit");
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
