using Box.Festa.Negocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Box.Festa.Models
{
    [Table("AtualizarStatus")]
    public class AtualizarStatus : BasePojo
    {
        public AtualizarStatus()
        {
            ListaPedidosAlterados = new List<AtualizarStatusPedido>();
            UsuarioAlteracao = new Usuario();
        }



        public DateTime HoraAtualizacao { get; set; }

        public Usuario UsuarioAlteracao { get; set; }

        public int NrPedidosAlterados { get; set; }


        public List<AtualizarStatusPedido> ListaPedidosAlterados { get; set; }

    }
}