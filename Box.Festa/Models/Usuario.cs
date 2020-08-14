using Box.Festa.Negocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Box.Festa.Models
{
    [Table("Usuario")]
    public class Usuario : BasePojo
    {
        public Usuario()
        {
            ListaPedido = new List<Pedido>();
        }

        public Usuario(Usuario usuario)
        {
            this.Nome = usuario.Nome;
            this.Cpf = usuario.Cpf;
            this.Email = usuario.Email;
            this.Senha = usuario.Senha;
            this.Telefone = usuario.Telefone;
            this.ListaPedido = usuario.ListaPedido;
        }
        public string Nome { get; set; }

        public string PrimeiroNome { get; set; }

        public string Cpf { get; set; }

        public string Email { get; set; }

        public string NovoEmail { get; set; }

        public EnumPerfilUsuario PerfilUsuario { get; set; }

        public string Senha { get; set; }

        [JsonIgnore]
        public string NovoCpf { get; set; }

        [JsonIgnore]
        public string NovaSenha { get; set; }

        [JsonIgnore]
        public string ConfirmarSenha { get; set; }

        public string Telefone { get; set; }

        public DateTime DataInsercao { get; set; }
        public long EnderecoId { get; set; }
        [NotMapped]
        public Endereco Endereco { get; set; }
        public long FormaPagamentoId { get; set; }
        [NotMapped]
        public FormaPagamento FormaPagamento { get; set; }

        public List<Pedido> ListaPedido { get; set; }

        public string Imprimir(Usuario usuario)
        {
            string cabecalho;
            cabecalho = "Cpf;Nome;Telefone;Email;";
            string caminho = System.AppDomain.CurrentDomain.BaseDirectory + usuario.Cpf + ".xls";
            StreamWriter arquivo = new StreamWriter(caminho);

            arquivo.WriteLine(cabecalho);

            string linha = usuario.Cpf + ";" + usuario.Nome + ";" + usuario.Telefone + ";" + usuario.Email + ";";

            arquivo.WriteLine(linha);
            arquivo.Flush();
            arquivo.Close();
            return caminho;
        }
    }
}