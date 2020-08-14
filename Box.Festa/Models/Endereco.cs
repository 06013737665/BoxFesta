using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Box.Festa.Models
{
    [Table("Endereco")]
    public class Endereco : BasePojo
    {

        //

        public long UsuarioId { get; set; }
        [NotMapped]
        public Usuario Usuario { get; set; }

        public string Rua { get; set; }

        public int? Numero { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        public string Cep { get; set; }

        public string Imprimir(Endereco endereco)
        {
            string cabecalho;
            cabecalho = "Cep;Rua;Numero;Bairro;Cidade;Estado;";
            string caminho = System.AppDomain.CurrentDomain.BaseDirectory + endereco.Cep + ".xls";
            StreamWriter arquivo = new StreamWriter(caminho);

            arquivo.WriteLine(cabecalho);

            string linha = endereco.Cep + ";" + endereco.Rua + ";" + endereco.Numero + ";" + endereco.Bairro + ";" + endereco.Cidade + ";" + endereco.Estado + ";";

            arquivo.WriteLine(linha);
            arquivo.Flush();
            arquivo.Close();
            return caminho;
            
        }
    }
}