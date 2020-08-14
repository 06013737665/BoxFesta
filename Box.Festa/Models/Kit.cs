using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;

namespace Box.Festa.Models
{
    [Table("Kit")]
    public class Kit : BasePojo
    {

        
        public string Codigo { get; set; }

        public int QtdePessoas { get; set; }

        public int Qtde { get; set; }

        public string Imprimir(Produto produto)
        {
            string cabecalho;
            cabecalho = "Codigo;Descricao;Valor;QtdePessoas";
            string caminho = System.AppDomain.CurrentDomain.BaseDirectory + produto.Codigo + ".xls";
            StreamWriter arquivo = new StreamWriter(caminho);

            arquivo.WriteLine(cabecalho);

            string linha = produto.Codigo + ";" + produto.Descricao + ";" + produto.Valor + ";" + produto.Kit.QtdePessoas + ";";

            arquivo.WriteLine(linha);
            arquivo.Flush();
            arquivo.Close();
            return caminho;

        }
    }
}