using BabyVest_Servico.DAO;
using Box.Festa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Box.Festa.Negocio
{
    public class AtualizarStatusPedidoBO
    {



        public static List<AtualizarStatusPedido> ListarAtualizarStatusPedido()
        {
            List<AtualizarStatusPedido> listaAtualizarStatusPedido = new List<AtualizarStatusPedido>();
            using (var db = new APIContext())
            {
                listaAtualizarStatusPedido = db.AtualizarStatusPedidoDAO.ToList();

            }
           
            return listaAtualizarStatusPedido;
        }
        

        public static List<AtualizarStatusPedido> ObterAtualizarStatusPedido(long idAtualizarStatus)
        {
            List<AtualizarStatusPedido> lista = ListarAtualizarStatusPedido();
            lista = lista.Where(c => c.AtualizarStatusId == idAtualizarStatus).ToList();
           
            return lista;
        }

       
        

    }

}