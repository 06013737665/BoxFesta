using BabyVest_Servico.DAO;
using Box.Festa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Box.Festa.Negocio
{
    public class SacolaBO
    {



        public static List<Sacola> ListarSacola()
        {
            List<Sacola> listaSacola = new List<Sacola>();
            using (var db = new APIContext())
            {
                listaSacola = db.SacolaDAO.ToList();

            }
           
            return listaSacola;
        }
        

        public static Sacola ObterSacola(long id)
        {
            List<Sacola> lista = ListarSacola();
            Sacola sacola = lista.Where(c => c.Id == id).FirstOrDefault();
           
            return sacola;
        }

       
                
    }

}