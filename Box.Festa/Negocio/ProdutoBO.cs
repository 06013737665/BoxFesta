using BabyVest_Servico.DAO;
using Box.Festa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Box.Festa.Negocio
{
    public class ProdutoBO
    {
        public static List<Produto> ListarProduto()
        {
            List<Produto> listaProduto = new List<Produto>();
            using (var db = new APIContext())
            {
                listaProduto = db.ProdutoDAO.ToList();

            }
            
            return listaProduto;
        }

        public static void InserirProdutoInicial()
        {
            List<Produto> listaProduto = new List<Produto>();
            for (int i = 1; i < 16; i++)
            {
                Produto produto = new Produto();
                //produto.Id = i;
                produto.Codigo = i.ToString();

                switch (i)
                {
                    case 1: produto.ehKit = false; produto.ehMaisVendido = true; produto.ehSale = false; produto.TemaRelacionado = "copo,frozen"; produto.Valor = 5.00; produto.Descricao = "Copo Descartável Frozen"; produto.Imagem = "/img/product/Copo_Plastico_Elza.png"; produto.ImagemSacola = "/img/product/cart-product/Copo_Plastico_Elza.png"; produto.ImagemDetalhe = "/img/product/product-details/Copo_Plastico_Elza_detail"; produto.ImagemTab1Detalhe = "/img/product/product-details/Copo_Plastico_Elza_tab"; produto.DescricaoSimplificada = "Jogo de 10 copos plástico vermelhos."; produto.DescricaoCompleta = "Este item contêm 1 copo descartável da marca Copo BR."; break;
                    case 2: produto.ehKit = false; produto.ehMaisVendido = true; produto.ehSale = true; produto.TemaRelacionado = "copo,galinha"; produto.Valor = 5.00; produto.Descricao = "Copo Descartável Galinha Pintadinha"; produto.Imagem = "/img/product/Copo_Descartavel_galinha.png"; produto.ImagemSacola = "/img/product/cart-product/Copo_Descartavel_galinha.png"; produto.ImagemDetalhe = "/img/product/product-details/Copo_Descartavel_galinha_detail"; produto.ImagemTab1Detalhe = "/img/product/product-details/Copo_Descartavel_galinha_tab"; produto.DescricaoSimplificada = "Este item contêm 1 copo descartável da marca Copo BR."; break;
                    case 3: produto.ehKit = false; produto.ehMaisVendido = false; produto.ehSale = true; produto.TemaRelacionado = "copo,galinha"; produto.Valor = 5.00; produto.Descricao = "Copo Descartável Sorvete"; produto.Imagem = "/img/product/Copo_Sorvete.png"; produto.ImagemSacola = "/img/product/cart-product/Copo_Sorvete.png"; produto.ImagemDetalhe = "/img/product/product-details/Copo_Sorvete_detail"; produto.ImagemTab1Detalhe = "/img/product/product-details/Copo_Sorvete_tab"; produto.DescricaoSimplificada = "Copo descartável no formato de sorvete."; produto.DescricaoCompleta = "Este item contêm 1 copo descartável da marca Copo BR."; break;
                    case 4: produto.ehKit = false; produto.ehMaisVendido = false; produto.ehSale = true; produto.TemaRelacionado = "copo,lol"; produto.Valor = 5.00; produto.Descricao = "Copo Descartável LOL"; produto.Imagem = "/img/product/Copo_LOL.png"; produto.ImagemSacola = "/img/product/cart-product/Copo_LOL.png"; produto.ImagemDetalhe = "/img/product/product-details/Copo_LOL_detail"; produto.ImagemTab1Detalhe = "/img/product/product-details/Copo_LOL_tab"; produto.DescricaoSimplificada = "Copo descartável LOL."; produto.DescricaoCompleta = "Este item contêm 1 copo descartável da marca Copo BR."; break;
                    case 5: produto.ehKit = false; produto.ehMaisVendido = false; produto.ehSale = false; produto.TemaRelacionado = "copo,batman"; produto.Valor = 5.00; produto.Descricao = "Copo Descartável Batman"; produto.Imagem = "/img/product/Copo_Batman.png"; produto.ImagemSacola = "/img/product/cart-product/Copo_Batman.png"; produto.ImagemDetalhe = "/img/product/product-details/Copo_Batman_detail"; produto.ImagemTab1Detalhe = "/img/product/product-details/Copo_Batman_tab"; produto.DescricaoSimplificada = "Copo descartável Batman."; produto.DescricaoCompleta = "Este item contêm 1 copo descartável da marca Copo BR."; break;
                    case 6: produto.ehKit = false; produto.ehMaisVendido = true; produto.ehSale = false; produto.TemaRelacionado = "bala,galinha"; produto.Valor = 3.00; produto.Descricao = "Bala de Iogurte"; produto.Imagem = "/img/product/Bala_Iogurte.png"; produto.ImagemSacola = "/img/product/cart-product/Bala_Iogurte.png"; produto.ImagemDetalhe = "/img/product/product-details/Bala_Iogurte_detail"; produto.ImagemTab1Detalhe = "/img/product/product-details/Bala_Iogurte_tab"; produto.DescricaoSimplificada = "Um saco de bala de Iogurte."; produto.DescricaoCompleta = "Este item contêm um saco de balas sabor Iogurte da marca Bala BR."; break;
                    case 7: produto.ehKit = false; produto.ehMaisVendido = true; produto.ehSale = false; produto.TemaRelacionado = "balao,bala"; produto.Valor = 2.50; produto.Descricao = "Balões"; produto.Imagem = "/img/product/Balao.png"; produto.ImagemSacola = "/img/product/cart-product/Balao.png"; produto.ImagemDetalhe = "/img/product/product-details/Balao_detail"; produto.ImagemTab1Detalhe = "/img/product/product-details/Balao_tab"; produto.DescricaoSimplificada = "Item com um saco de balões coloridos."; produto.DescricaoCompleta = "Este item contêm um saco de balões coloridos da marca Balões BR."; break;
                    case 8: produto.ehKit = false; produto.ehMaisVendido = false; produto.ehSale = false; produto.TemaRelacionado = "chapeu,prato"; produto.Valor = 2.00; produto.Descricao = "Chapéu do Mickey"; produto.Imagem = "/img/product/Chapeu_Mickey.png"; produto.ImagemSacola = "/img/product/cart-product/Chapeu_Mickey.png"; produto.ImagemDetalhe = "/img/product/product-details/Chapeu_Mickey_detail"; produto.ImagemTab1Detalhe = "/img/product/product-details/Chapeu_Mickey_tab"; produto.DescricaoSimplificada = "Chapéu de papel do Mickey."; produto.DescricaoCompleta = "Este item contêm um chapéu de papel do Mickey da Marca Chapéu BR."; break;
                    case 9: produto.ehKit = false; produto.ehMaisVendido = false; produto.ehSale = false; produto.TemaRelacionado = "prato,bala,balao"; produto.Valor = 3.00; produto.Descricao = "Prato Plástico"; produto.Imagem = "/img/product/Prato_Plastico.png"; produto.ImagemSacola = "/img/product/cart-product/Prato_Plastico.png"; produto.ImagemDetalhe = "/img/product/product-details/Prato_Plastico_detail"; produto.ImagemTab1Detalhe = "/img/product/product-details/Prato_Plastico_tab"; produto.DescricaoSimplificada = "Jogo de 10 pratos de plástico brancos."; produto.DescricaoCompleta = "Este item contêm uma caixa com 10 pratos de plástico brancos da marca Prato BR."; break;
                    //case 10: produto.ehKit = true; produto.ehSale = false; produto.Valor = 50.00; produto.Descricao = "Kit Batman"; produto.Imagem = "/img/batman.jpg"; produto.ImagemSacola = "/img/product/cart-product/batman.jpg"; produto.ImagemDetalhe = "/img/product/product-details/batman_detail1.jpg"; produto.ImagemTab1Detalhe = "/img/product/product-details/batman_tab1.jpg"; produto.ImagemTab2Detalhe = "/img/productproduct-details/Copo_Plastico_tab2.png"; produto.ImagemTab3Detalhe = "/img/productproduct-details/Copo_Plastico_tab3.png"; produto.DescricaoSimplificada = "Kit Batman para decoração de festas."; produto.DescricaoCompleta = "Este item contêm um saco com 10 copos plásticos vermelhos da marca Copo BR."; break;
                    case 10: produto.ehKit = true; produto.ehMaisVendido = false; produto.ehSale = false; produto.TemaRelacionado = "kit,kits,batman,heroi"; produto.Valor = 50.00; produto.Descricao = "Kit Batman"; produto.Imagem = "/img/product/ícones_Batman.png"; produto.ImagemKit = "/img/blog/from-blog/ícones_Batman.png"; produto.ImagemSacola = "/img/product/cart-product/icones_Batman.png"; produto.ImagemDetalhe = "/img/product/product-details/batman_detail"; produto.ImagemTab1Detalhe = "/img/product/product-details/batman_tab"; produto.DescricaoSimplificada = "Kit Batman para decoração de festas."; produto.DescricaoCompleta = "Este item contêm um Kit, com uma faixa, dois balões e 10 copos do tema Batman."; break;
                    case 11: produto.ehKit = true; produto.ehMaisVendido = false; produto.ehSale = false; produto.TemaRelacionado = "kit,kits,frozen,elza"; produto.Valor = 50.00; produto.Descricao = "Kit LOL"; produto.Imagem = "/img/product/ícones_LOL.png"; produto.ImagemKit = "/img/blog/from-blog/ícones_LOL.png"; produto.ImagemSacola = "/img/product/cart-product/icones_LOL.png"; produto.ImagemDetalhe = "/img/product/product-details/lol_detail"; produto.ImagemTab1Detalhe = "/img/product/product-details/lol_tab"; produto.DescricaoSimplificada = "Kit LOL para decoração de festas."; produto.DescricaoCompleta = "Este item contêm um Kit, com uma faixa, dois balões e 10 copos do tema LOL."; break;
                    case 12: produto.ehKit = true; produto.ehMaisVendido = true; produto.ehSale = false; produto.TemaRelacionado = "kit,kits,aladdin,copo,genio"; produto.Valor = 50.00; produto.Descricao = "Kit Aladdin"; produto.Imagem = "/img/product/kit_aladdin.png"; produto.ImagemKit = "/img/blog/from-blog/ícones_Aladin.png"; produto.ImagemSacola = "/img/product/cart-product/icones_Aladin.png"; produto.ImagemDetalhe = "/img/product/product-details/aladdin_detail"; produto.ImagemTab1Detalhe = "/img/product/product-details/aladdin_tab"; produto.DescricaoSimplificada = "Kit Aladdin para decoração de festas."; produto.DescricaoCompleta = "Este item contêm um Kit, com uma faixa, dois balões e c10 copos do tema Aladdin."; break;
                    case 13: produto.ehKit = true; produto.ehMaisVendido = false; produto.ehSale = false; produto.TemaRelacionado = "kit,kits,mickey,disney"; produto.Valor = 50.00; produto.Descricao = "Kit Mickey"; produto.Imagem = "/img/product/icone_Mickey.jpg"; produto.ImagemKit = "/img/blog/from-blog/icone_Mickey.jpg"; produto.ImagemSacola = "/img/product/cart-product/icone_Mickey.jpg"; produto.ImagemDetalhe = "/img/product/product-details/Mickey_detail"; produto.ImagemTab1Detalhe = "/img/product/product-details/Mickey_tab"; produto.DescricaoSimplificada = "Kit Mickey para decoração de festas."; produto.DescricaoCompleta = "Este item contêm um Kit, com uma faixa, dois balões e 10 copos do tema Mickey."; break;
                    case 14: produto.ehKit = true; produto.ehMaisVendido = false; produto.ehSale = false; produto.TemaRelacionado = "kit,kits,minnie,disney"; produto.Valor = 50.00; produto.Descricao = "Kit Minnie"; produto.Imagem = "/img/product/minnie-site.jpg"; produto.ImagemKit = "/img/blog/from-blog/minnie-site.jpg"; produto.ImagemSacola = "/img/product/cart-product/minnie-site.jpg"; produto.ImagemDetalhe = "/img/product/product-details/Minnie_detail"; produto.ImagemTab1Detalhe = "/img/product/product-details/Minnie_tab"; produto.DescricaoSimplificada = "Kit Minnie para decoração de festas."; produto.DescricaoCompleta = "Este item contêm um Kit, com uma faixa, dois balões e 10 copos do tema Minnie."; break;
                    case 15: produto.ehKit = true; produto.ehMaisVendido = true; produto.ehSale = false; produto.TemaRelacionado = "kit,kits,moana,disney"; produto.Valor = 50.00; produto.Descricao = "Kit Moana"; produto.Imagem = "/img/product/icone_Moana.jpg"; produto.ImagemKit = "/img/blog/from-blog/icone_Moana.jpg"; produto.ImagemSacola = "/img/product/cart-product/icone_Moana.jpg"; produto.ImagemDetalhe = "/img/product/product-details/Moana_detail"; produto.ImagemTab1Detalhe = "/img/product/product-details/Moana_tab"; produto.DescricaoSimplificada = "Kit Moana para decoração de festas."; produto.DescricaoCompleta = "Este item contêm um Kit, com uma faixa, dois balões e c10 copos do tema Moana."; break;

                }

                listaProduto.Add(produto);
            }
            foreach (Produto produto in listaProduto)
            {
                using (var db = new APIContext())
                {
                    db.ProdutoDAO.Add(produto);
                    db.SaveChanges();
                }
            }
        }
        public static Produto ObterProduto(string codigo)
        {
            List<Produto> lista = ListarProduto();
            foreach (Produto produto in lista)
            {
                if (produto.Codigo.Equals(codigo))
                {
                    return produto;
                }
            }
            return null;
        }

        public static Produto ObterProduto(long id)
        {
            List<Produto> lista = ListarProduto();
            foreach (Produto produto in lista)
            {
                if (produto.Id.Equals(id))
                {
                    return produto;
                }
            }
            return null;
        }
        public static long CadastrarProduto(Produto produto)
        {
            using (var db = new APIContext())
            {
                db.ProdutoDAO.Add(produto);

                db.SaveChanges();
                return produto.Id;
            }
        }

        public static void ExcluirProduto(Produto produto)
        {
            using (var db = new APIContext())
            {
                db.ProdutoDAO.Remove(produto);

                db.SaveChanges();
               
            }
        }

        public static void EditarProduto(Produto produto)
        {
            using (var db = new APIContext())
            {
                Produto produtoBanco = db.ProdutoDAO.First(a => a.Id == produto.Id);
                produtoBanco.Descricao = produto.Descricao;
                produtoBanco.DescricaoCompleta = produto.DescricaoCompleta;
                produtoBanco.Valor = produto.Valor;
                produtoBanco.Imagem = produto.Imagem;
                               
                db.SaveChanges();
            }
        }
        public static List<Produto> ListarTodosProdutos()
        {
            List<Produto> listaProduto = new List<Produto>();
            using (var db = new APIContext())
            {
                listaProduto = db.ProdutoDAO.ToList();

            }
            return listaProduto;
        }

        

        
    }
}