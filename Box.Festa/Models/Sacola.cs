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
    [Table("Sacola")]
    public class Sacola : BasePojo
    {
        public Sacola()
        {
            Produtos = new List<SacolaProduto>();
            
            ProdutoSelecionado = new long[(ProdutoBO.ListarProduto()).Count];
            SedexFormatado = "0.00";
            TipoPagamento = "";
            // SedexFormatado = string.Format("{0:C}", 0);
            // SedexFormatado = string.Format("R${0:0.00#.##}", 0);
            //  SedexFormatado = 0.ToString("N2").Replace(",", "").Replace(".", "");
        }
        public long UsuarioId { get; set; }
        [NotMapped]
        public Usuario Usuario { get; set; }

        public List<SacolaProduto> Produtos { get; set; }

        [JsonIgnore]
        [NotMapped]
        public long[] ProdutoSelecionado { get; set; }

       public double TotalProduto { get; set; }
        public double TotalSacola { get; set; }
        public string Cep { get; set; }

        public bool MostrarEndereco { get; set; }
        public string Rua { get; set; }

        public string Complemento { get; set; }

        public string Cidade { get; set; }
        public string Estado { get; set; }

        public string Bairro { get; set; }

        public int? NumeroEndereco { get; set; }

        public double Sedex { get; set; }
        public string SedexFormatado { get; set; }

        public string TipoPagamento { get; set; }


      

        public int Qtde
        {
            get
            {
                return Produtos.Count;
            }
            
        }

    }
}