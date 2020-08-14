using BabyVest_Servico.DAO;
using Box.Festa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Box.Festa.Negocio
{
    public class FormaPagamentoBO
    {
        public static List<FormaPagamento> ListarFormaPagamento(long idUsuario)
        {
            List<FormaPagamento> listaFormaPagamento = new List<FormaPagamento>();
            using (var db = new APIContext())
            {
                listaFormaPagamento = db.FormaPagamentoDAO.Where(c=>c.UsuarioId== idUsuario).ToList();

            }
            
            return listaFormaPagamento;
        }

        public static void InserirFormaPagamento(FormaPagamento formaPagamento)
        {
           
                using (var db = new APIContext())
                {
                    db.FormaPagamentoDAO.Add(formaPagamento);
                    db.SaveChanges();
                }
            
        }
        public static FormaPagamento ObterFormaPagamento(long id, long idUsuario)
        {
            List<FormaPagamento> lista = ListarFormaPagamento(idUsuario);
            foreach (FormaPagamento formaPagamento in lista)
            {
                if (formaPagamento.Id.Equals(id))
                {
                    return formaPagamento;
                }
            }
            return null;
        }
        

        public static void ExcluirFormaPagamento(FormaPagamento formaPagamento)
        {
            using (var db = new APIContext())
            {
                db.FormaPagamentoDAO.Remove(formaPagamento);

                db.SaveChanges();
               
            }
        }

        public static void EditarFormaPagamento(FormaPagamento formaPagamento)
        {
            using (var db = new APIContext())
            {
                FormaPagamento formaPagamentoBanco = db.FormaPagamentoDAO.First(a => a.Id == formaPagamento.Id);
                formaPagamentoBanco.Numero = formaPagamento.Numero;
                formaPagamentoBanco.NomeProprietario = formaPagamento.NomeProprietario;
                formaPagamentoBanco.CpfProprietario = formaPagamento.CpfProprietario;
                formaPagamentoBanco.Validade = formaPagamento.Validade;
                formaPagamentoBanco.Codigo = formaPagamento.Codigo;
                db.SaveChanges();
            }
        }
        
    }
}