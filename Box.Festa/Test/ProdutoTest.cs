using Box.Festa.Models;
using Box.Festa.Negocio;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;

namespace Box.Festa.Test
{
    [TestFixture]
    public class ProdutoTest
    {
        [Test]
        public void TestListarTodosProdutos()
        {
            List<Produto> listaProduto = ProdutoBO.ListarTodosProdutos();
            Assert.IsTrue(listaProduto != null && listaProduto.Count>1);
        }

        [Test]
        public void TestObetrProduto()
        {
            Produto produto = ProdutoBO.ObterProduto("1");
            Assert.IsTrue(produto != null && !produto.Descricao.Equals(""));
        }
    }
}