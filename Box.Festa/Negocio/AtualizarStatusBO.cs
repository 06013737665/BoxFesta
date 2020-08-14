using BabyVest_Servico.DAO;
using Box.Festa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Box.Festa.Negocio
{
    public class AtualizarStatusBO
    {
        public static void CriarAtualizacaoStatus(AtualizarStatus status)
        {
            using (var db = new APIContext())
            {
                db.AtualizarStatusDAO.Add(status);
                db.SaveChanges();

            }

        }
        


        public static void InserirEndereco(Endereco endereco)
        {
           
                using (var db = new APIContext())
                {
                    db.EnderecoDAO.Add(endereco);
                    db.SaveChanges();
                }
            
        }
        
        
    }
}