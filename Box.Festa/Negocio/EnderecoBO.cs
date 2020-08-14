using BabyVest_Servico.DAO;
using Box.Festa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Box.Festa.Negocio
{
    public class EnderecoBO
    {
        public static List<Endereco> ListarEndereco(long idUsuario)
        {
            List<Endereco> listaEndereco = new List<Endereco>();
            using (var db = new APIContext())
            {
                listaEndereco = db.EnderecoDAO.Where(c=>c.UsuarioId == idUsuario).ToList();

            }
            
            return listaEndereco;
        }

        public static void InserirEndereco(Endereco endereco)
        {
           
                using (var db = new APIContext())
                {
                    db.EnderecoDAO.Add(endereco);
                    db.SaveChanges();
                }
            
        }
        public static Endereco ObterEndereco(long id, long idUsuario)
        {
            List<Endereco> lista = ListarEndereco(idUsuario);
            foreach (Endereco endereco in lista)
            {
                if (endereco.Id.Equals(id))
                {
                    return endereco;
                }
            }
            return null;
        }
        

        public static void ExcluirEndereco(Endereco endereco)
        {
            using (var db = new APIContext())
            {
                db.EnderecoDAO.Remove(endereco);

                db.SaveChanges();
               
            }
        }

        public static void EditarEndereco(Endereco endereco)
        {
            using (var db = new APIContext())
            {
                Endereco enderecoBanco = db.EnderecoDAO.First(a => a.Id == endereco.Id);
                enderecoBanco.Rua = endereco.Rua;
                enderecoBanco.Numero = endereco.Numero;
                enderecoBanco.Bairro = endereco.Bairro;
                enderecoBanco.Cidade = endereco.Cidade;
                enderecoBanco.Estado = endereco.Estado;
                enderecoBanco.Cep = endereco.Cep;
                db.SaveChanges();
            }
        }
        
    }
}