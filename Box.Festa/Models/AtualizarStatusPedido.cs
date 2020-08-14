using Box.Festa.Negocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Box.Festa.Models
{
    [Table("AtualizarStatusPedido")]
    public class AtualizarStatusPedido : BasePojo
    {
        public AtualizarStatusPedido() 
        {
            
        }
        public long AtualizarStatusId { get; set; }
        public long PedidoId { get; set; }

        [NotMapped]
        public AtualizarStatus AtualizarStatus { get; set; }
        [NotMapped]
        public Pedido Pedido { get; set; }

    }
}