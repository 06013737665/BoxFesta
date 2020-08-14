using Box.Festa.Models;
using Box.Festa.Negocio;
using NUnit.Framework;
using System.Text;

namespace Box.Festa.Test
{
    [TestFixture]
    public class UsuarioTest
    {
        [Test]
        public void TestObterUsuarioEmail()
        {
            Usuario usuario  = UsuarioBO.ObterUsuarioEmail("teste@teste.com.br");
            Assert.IsTrue(usuario!=null);
        }
    }
}