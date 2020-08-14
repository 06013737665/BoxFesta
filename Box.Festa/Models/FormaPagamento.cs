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
    [Table("FormaPagamento")]
    public class FormaPagamento : BasePojo
    {

        
        
        public long UsuarioId { get; set; }
        [NotMapped]
        public Usuario Usuario { get; set; }
        public string Numero { get; set; }

        public string NomeProprietario { get; set; }

        public int? Codigo { get; set; }

        public string Validade { get; set; }

        public string CpfProprietario { get; set; }

        public string Imprimir(FormaPagamento forma)
        {
            string cabecalho;
            cabecalho = "Numero;Proprietario;CPF;Validade;Codigo;";
            string caminho = System.AppDomain.CurrentDomain.BaseDirectory + forma.Numero + ".xls";
            StreamWriter arquivo = new StreamWriter(caminho);

            arquivo.WriteLine(cabecalho);

            string linha = forma.Numero + ";" + forma.NomeProprietario + ";" + forma.CpfProprietario + ";" + forma.Validade + ";" + forma.Codigo + ";";

            arquivo.WriteLine(linha);
            arquivo.Flush();
            arquivo.Close();
            return caminho;
            
        }
    }
}