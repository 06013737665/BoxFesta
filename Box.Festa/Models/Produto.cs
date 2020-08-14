using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Box.Festa.Models
{
    [Table("Produto")]
    public class Produto : BasePojo
    {
        public Produto()
        {
            this.Qtde = 1;
        }
        public string Codigo { get; set; }

        public string TemaRelacionado { get; set; }
        public string Descricao { get; set; }

        public string DescricaoSimplificada { get; set; }

        [JsonIgnore]
        public string DescricaoCompleta { get; set; }

        [JsonIgnore]
        public string Imagem { get; set; }

        [JsonIgnore]
        public string ImagemKit { get; set; }

        public string ImagemSacola { get; set; }

        [JsonIgnore]
        public string ImagemDetalhe { get; set; }

        [JsonIgnore]
        public string ImagemTab1Detalhe { get; set; }

        public double Valor { get; set; }


        public Kit Kit { get; set; }

        public bool ehKit { get; set; }

        public bool ehMaisVendido { get; set; }

        public bool ehSale { get; set; }

        public decimal Altura { get; set; }

        public decimal Largura { get; set; }

        public decimal Comprimento { get; set; }


        public decimal Diametro { get; set; }

        public int Qtde { get; set; }

        public int QtdePessoas { get; set; }
        //public List<SacolaProduto> ListaSacola { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as Produto;

            if (this.Codigo.Equals(item.Codigo) || this.Id.Equals(item.Id))
            {
                return true;
            }
            return false;
        }

        public string Imprimir(Produto produto)
        {
            string cabecalho;
            cabecalho = "Codigo;Descricao;Valor;";
            string caminho = System.AppDomain.CurrentDomain.BaseDirectory + produto.Codigo + ".xls";
            StreamWriter arquivo = new StreamWriter(caminho);

            arquivo.WriteLine(cabecalho);

            string linha = produto.Codigo + ";" + produto.Descricao + ";" + produto.Valor + ";";

            arquivo.WriteLine(linha);
            arquivo.Flush();
            arquivo.Close();
            return caminho;
            
        }
    }

}