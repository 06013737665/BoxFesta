using Box.Festa.Negocio;
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
    [Table("Pedido")]
    public class Pedido : BasePojo
    {
        public Pedido()
        {
            Sacola sacola = new Sacola();
            Usuario Usuario = new Usuario();

        }

        public Pedido(Pedido pedido)
        {
            this.Sacola = pedido.Sacola;
            this.Usuario = pedido.Usuario;
            this.Data = pedido.Data;
            this.Id = pedido.Id;
        }
        public string Data { get; set; }

        public string Hora { get; set; }

        public long SacolaId { get; set; }

        public Sacola Sacola { get; set; }

        public long UsuarioId { get; set; }
        [NotMapped]
        public Usuario Usuario { get; set; }



        public string IdTransacao { get; set; }

        public string Status { get; set; }

        public string StatusTexto { get; set; }

        public bool ConfirmadoPagSeguro { get; set; }

        public bool? Alterado { get; set; }


        public static string PreencherStatus(int status)
        {/*
                 1	Aguardando pagamento: o comprador iniciou a transação, mas até o momento o PagSeguro não recebeu nenhuma informação sobre o pagamento.
            2	Em análise: o comprador optou por pagar com um cartão de crédito e o PagSeguro está analisando o risco da transação.
            3	Paga: a transação foi paga pelo comprador e o PagSeguro já recebeu uma confirmação da instituição financeira responsável pelo processamento.
            4	Disponível: a transação foi paga e chegou ao final de seu prazo de liberação sem ter sido retornada e sem que haja nenhuma disputa aberta.
            5	Em disputa: o comprador, dentro do prazo de liberação da transação, abriu uma disputa.
            6	Devolvida: o valor da transação foi devolvido para o comprador.
            7	Cancelada: a transação foi cancelada sem ter sido finalizada.
            8	Debitado: o valor da transação foi devolvido para o comprador.
            9	Retenção temporária: o comprador abriu uma solicitação de chargeback junto à operadora do cartão de crédito.*/


            switch (status)
            {
                case 1: return "Aguardando pagamento";
                case 2: return "Em análise";
                case 3: return "Paga";
                case 4: return "Disponível";
                case 5: return "Em disputa";
                case 6: return "Devolvida";
                case 7: return "Cancelada";
                case 8: return "Debitado";
                case 9: return "Retenção temporária";
                default: return "";
            }
        }


        public override bool Equals(object obj)
        {
            var pedido = obj as Pedido;
            return pedido != null &&
                   (Id == pedido.Id || (Data.Equals(pedido.Data) && Hora.Equals(pedido.Hora) && Sacola.TotalProduto == pedido.Sacola.TotalProduto));
        }

        public string Imprimir(Pedido pedido)
        {
            string cabecalho;
            cabecalho = "idTransacao;Data;Hora;Status;";
            int i = 0;
            foreach (SacolaProduto produto in pedido.Sacola.Produtos)
            {
                i++;
                cabecalho = cabecalho + "Produto" + i + "-Codigo;" + "Produto" + i + "-Descricao;" + "Produto" + i + "-Valor;";
            }
            string caminho = System.AppDomain.CurrentDomain.BaseDirectory + pedido.IdTransacao + ".xls";
            StreamWriter arquivo = new StreamWriter(caminho);

            arquivo.WriteLine(cabecalho);

            string linha = pedido.IdTransacao + ";" + pedido.Data + ";" + pedido.Hora + ";" + pedido.StatusTexto + ";";
            foreach (SacolaProduto produto in pedido.Sacola.Produtos)
            {
                linha = linha + produto.Produto.Codigo + ";" + produto.Produto.Descricao + ";" + produto.Produto.Valor + ";";
            }
            arquivo.WriteLine(linha);
            arquivo.Flush();
            arquivo.Close();
            return caminho;
            
        }
    }
}