using Box.Festa.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BabyVest_Servico.DAO
{
    public class APIContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public APIContext() : base()
        {
          // Database.EnsureDeleted();
          // Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=.;database=BoxFesta;trusted_connection=true;");
        }
        /*public APIContext() : base("name=ConnectionString")
        {
         // Database.SetInitializer(new DropCreateDatabaseAlways<APIContext>());
            
        }*/
        
        

        public Microsoft.EntityFrameworkCore.DbSet<AtualizarStatus> AtualizarStatusDAO { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Kit> KitDAO { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Pedido> PedidoDAO { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<FormaPagamento> FormaPagamentoDAO { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Endereco> EnderecoDAO { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Sacola> SacolaDAO { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Usuario> UsuarioDAO { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<SacolaProduto> SacolaProdutoDAO { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<AtualizarStatusPedido> AtualizarStatusPedidoDAO { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Produto> ProdutoDAO { get; set; }

        

    }
}