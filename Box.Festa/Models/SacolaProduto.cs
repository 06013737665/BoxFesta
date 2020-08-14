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
    [Table("SacolaProduto")]
    public class SacolaProduto : BasePojo
    {
        public SacolaProduto() 
        {
            
        }
        public long SacolaId { get; set; }
        public long ProdutoId { get; set; }

        [NotMapped]
        public Sacola Sacola { get; set; }
        [NotMapped]
        public Produto Produto { get; set; }

    }
}