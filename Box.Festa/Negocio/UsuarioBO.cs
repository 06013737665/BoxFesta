using BabyVest_Servico.DAO;
using Box.Festa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Box.Festa.Negocio
{
    public class UsuarioBO
    {
        public static void CadastrarUsuario(Usuario usuario)
        {
            usuario.DataInsercao = DateTime.Now;
            if (usuario.FormaPagamento!=null) {
                usuario.FormaPagamentoId = usuario.FormaPagamento.Id;
            }
            if (usuario.Endereco != null)
            {
                usuario.EnderecoId = usuario.Endereco.Id;
            }
            using (var db = new APIContext())
            {
                db.UsuarioDAO.Add(usuario);
                db.SaveChanges();

            }
        }

        public static bool ExisteUsuario(string email)
        {
            Usuario usuario = ObterUsuarioEmail(email);
            if (usuario != null && usuario.Id > 0)
            {
                return true;
            }

            return false;
        }
        public static void CadastrarAdministradorInicial()
        {
            Usuario usuario = new Usuario();
            usuario.Email = "teste@teste.com.br";
            //usuario.Senha = Md5BO.RetornarMD5("teste");
            usuario.Senha = Sha1BO.RetornarSHA("teste");
            usuario.PerfilUsuario = EnumPerfilUsuario.Administrador;
            usuario.Nome = "teste";
            usuario.DataInsercao = DateTime.Now;
            using (var db = new APIContext())
            {
                db.UsuarioDAO.Add(usuario);
                db.SaveChanges();

            }
        }
        public static void EditarUsuario(Usuario usuario)
        {
            if (usuario.FormaPagamento != null)
            {
                usuario.FormaPagamentoId = usuario.FormaPagamento.Id;
            }
            if (usuario.Endereco != null)
            {
                usuario.EnderecoId = usuario.Endereco.Id;
            }
            using (var db = new APIContext())
            {
                Usuario usuarioBanco = db.UsuarioDAO.First(a => a.Id == usuario.Id);
                usuarioBanco.Nome = usuario.Nome;
                usuarioBanco.Email = usuario.Email;

                db.SaveChanges();
            }
        }

        public static Usuario RecuperarUsuario(string cpf)
        {
            Usuario retorno = new Usuario();
            using (var db = new APIContext())
            {
                retorno = db.UsuarioDAO.Where(c => c.Cpf == cpf).FirstOrDefault();
                if (retorno != null)
                {
                    retorno.FormaPagamento = FormaPagamentoBO.ObterFormaPagamento(retorno.FormaPagamentoId, retorno.Id);
                    retorno.Endereco = EnderecoBO.ObterEndereco(retorno.EnderecoId, retorno.Id);
                }
            }
            return retorno;
        }

        public static Usuario ObterUsuario(long id)
        {
            Usuario retorno = new Usuario();
            using (var db = new APIContext())
            {
                retorno = db.UsuarioDAO.Where(c => c.Id == id).FirstOrDefault();
                if (retorno != null)
                {
                    retorno.FormaPagamento = FormaPagamentoBO.ObterFormaPagamento(retorno.FormaPagamentoId, retorno.Id);
                    retorno.Endereco = EnderecoBO.ObterEndereco(retorno.EnderecoId, retorno.Id);
                }
            }
            return retorno;
        }

        public static Usuario ObterUsuario(string email)
        {
            Usuario retorno = new Usuario();
            using (var db = new APIContext())
            {
                retorno = db.UsuarioDAO.Where(c => c.Email == email).FirstOrDefault();
                if (retorno != null)
                {
                    retorno.FormaPagamento = FormaPagamentoBO.ObterFormaPagamento(retorno.FormaPagamentoId, retorno.Id);
                    retorno.Endereco = EnderecoBO.ObterEndereco(retorno.EnderecoId, retorno.Id);
                }
            }
            return retorno;
        }

        public static Usuario ObterUsuarioEmail(string email)
        {
            Usuario retorno = new Usuario();
            using (var db = new APIContext())
            {
                retorno = db.UsuarioDAO.Where(c => c.Email == email).FirstOrDefault();
                if (retorno != null)
                {
                    retorno.FormaPagamento = FormaPagamentoBO.ObterFormaPagamento(retorno.FormaPagamentoId, retorno.Id);
                    retorno.Endereco = EnderecoBO.ObterEndereco(retorno.EnderecoId, retorno.Id);
                }
            }
            return retorno;
        }

        public static List<Usuario> ListarTodosUsuarios()
        {
            List<Usuario> listaUsuario = new List<Usuario>();
            using (var db = new APIContext())
            {
                listaUsuario = db.UsuarioDAO.ToList();

            }
            return listaUsuario;
        }


    }
}