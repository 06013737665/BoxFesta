using BabyVest_Servico.DAO;
using Box.Festa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Box.Festa.Negocio
{
    public class SacolaProdutoBO
    {



        public static List<SacolaProduto> ListarSacolaProduto()
        {
            List<SacolaProduto> listaSacola = new List<SacolaProduto>();
            using (var db = new APIContext())
            {
                listaSacola = db.SacolaProdutoDAO.ToList();

            }
           
            return listaSacola;
        }
        

        public static List<SacolaProduto> ObterSacolaProduto(long idSacola)
        {
            List<SacolaProduto> lista = ListarSacolaProduto();
            lista = lista.Where(c => c.SacolaId == idSacola).ToList();
           
            return lista;
        }

       
                
    }

}