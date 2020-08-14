using BabyVest_Servico.DAO;
using Box.Festa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Box.Festa.Negocio
{
    public class PedidoBO
    {



        public static List<Pedido> ListarTodosPedidos()
        {
            List<Pedido> listaPedido = new List<Pedido>();
            using (var db = new APIContext())
            {
                listaPedido = db.PedidoDAO.ToList();

            }
            foreach (Pedido pedido in listaPedido)
            {
                pedido.Usuario = UsuarioBO.ObterUsuario(pedido.UsuarioId);
                if (pedido.SacolaId > 0)
                {
                    pedido.Sacola = SacolaBO.ObterSacola(pedido.SacolaId);
                    List<SacolaProduto> listaSacolaProduto = SacolaProdutoBO.ObterSacolaProduto(pedido.SacolaId);
                    if (listaSacolaProduto != null && listaSacolaProduto.Count > 0)
                    {
                        pedido.Sacola.Produtos.AddRange(listaSacolaProduto);
                        foreach (SacolaProduto sacProd in pedido.Sacola.Produtos)
                        {
                            sacProd.Produto = ProdutoBO.ObterProduto(sacProd.ProdutoId);

                        }
                    }
                }
            }
            return listaPedido;
        }
        public static List<Pedido> ListarPedido(string ano = "", string mes = "", string dia = "", bool admin = false)
        {
            List<Pedido> listaPedido = new List<Pedido>();
            using (var db = new APIContext())
            {
                listaPedido = db.PedidoDAO.ToList();
                listaPedido = listaPedido.Where(c => c.Data.Equals(dia + "/" + mes + "/" + ano)).ToList();
            }
            foreach (Pedido pedido in listaPedido)
            {
                pedido.Usuario = UsuarioBO.ObterUsuario(pedido.UsuarioId);
                if (pedido.SacolaId > 0)
                {
                    pedido.Sacola = SacolaBO.ObterSacola(pedido.SacolaId);
                    List<SacolaProduto> listaSacolaProduto = SacolaProdutoBO.ObterSacolaProduto(pedido.SacolaId);
                    if (listaSacolaProduto != null && listaSacolaProduto.Count > 0)
                    {
                        pedido.Sacola.Produtos.AddRange(listaSacolaProduto);
                        foreach(SacolaProduto sacProd in pedido.Sacola.Produtos)
                        {
                            sacProd.Produto = ProdutoBO.ObterProduto(sacProd.ProdutoId);

                        }
                    }
                }
            }
            return listaPedido;

        }

        public static Pedido ObterPedido(long id)
        {
            List<Pedido> lista = ListarTodosPedidos();
            Pedido pedido = lista.Where(c => c.Id == id).FirstOrDefault();
            if (pedido != null)
            {
                pedido.Usuario = pedido.Usuario = UsuarioBO.ObterUsuario(pedido.UsuarioId);
            }

            return pedido;
        }

        public static long GravarPedido(Pedido pedido)
        {
            pedido.UsuarioId = pedido.Usuario.Id;


            using (var db = new APIContext())
            {
                db.PedidoDAO.Add(pedido);

                db.SaveChanges();
                return pedido.Id;
            }

        }

        public static long EditarPedido(Pedido pedido)
        {
            pedido.UsuarioId = pedido.Usuario.Id;


            using (var db = new APIContext())
            {
                Pedido pedidoBanco = db.PedidoDAO.First(a => a.Id == pedido.Id);
                pedidoBanco.IdTransacao = pedido.IdTransacao;
                pedidoBanco.SacolaId = pedido.SacolaId;
                db.SaveChanges();
                return pedido.Id;
            }

        }
        public static Pedido ObterPedidoUsuario(long idUsuario)
        {
            List<Pedido> lista = ListarTodosPedidos();
            Pedido pedido = lista.Where(c => c.Usuario.Id == idUsuario).OrderByDescending(c => c.Id).FirstOrDefault();
            if (pedido != null)
            {
                pedido.Usuario = pedido.Usuario = UsuarioBO.ObterUsuario(pedido.UsuarioId);
            }
            return pedido;
        }

        public static List<Pedido> ListarPedidoUsuario(long idUsuario)
        {
            List<Pedido> lista = ListarTodosPedidos();
            lista = lista.Where(c => c.Usuario.Id == idUsuario).OrderByDescending(c => c.Id).ToList();
            if (lista != null)
            {
                foreach (Pedido pedido in lista)
                {
                    pedido.Usuario = pedido.Usuario = UsuarioBO.ObterUsuario(pedido.UsuarioId);
                    if (pedido.SacolaId > 0)
                    {
                        pedido.Sacola = SacolaBO.ObterSacola(pedido.SacolaId);
                        List<SacolaProduto> listaSacolaProduto = SacolaProdutoBO.ObterSacolaProduto(pedido.SacolaId);
                        if (listaSacolaProduto != null && listaSacolaProduto.Count > 0)
                        {
                            pedido.Sacola.Produtos.AddRange(listaSacolaProduto);
                            foreach (SacolaProduto sacProd in pedido.Sacola.Produtos)
                            {
                                sacProd.Produto = ProdutoBO.ObterProduto(sacProd.ProdutoId);

                            }
                        }
                    }
                }
            
            }
            return lista;
        }



        public static long ObterUltimoId(string ano = "", string mes = "", string dia = "")
        {
            List<Pedido> listaPedido = new List<Pedido>();




            listaPedido = ListarPedido();

            long idRetorno = 0;
            if (listaPedido != null && listaPedido.Count > 0)
            {
                idRetorno = listaPedido.OrderByDescending(c => c.Id).FirstOrDefault().Id;
            }

            return idRetorno;
        }

        public static void GravarStatusListaPedidoMesmoDia(List<Pedido> listaPedido)
        {
            if (listaPedido != null && listaPedido.Count > 0 && !string.IsNullOrEmpty(listaPedido[0].Data))
            {
                var data = listaPedido[0].Data.Replace("/", "");
                List<Pedido> pedidos = new List<Pedido>();


                pedidos = ListarTodosPedidos();
                if (pedidos == null)
                {
                    pedidos = new List<Pedido>();
                }
                bool teveAlteracao = false;
                foreach (Pedido pedido in listaPedido)
                {
                    if (pedido.Alterado != null && pedido.Alterado.Value)
                    {
                        teveAlteracao = true;
                        Pedido pedidoUsuario = new Pedido();
                        pedido.Usuario = UsuarioBO.ObterUsuarioEmail(pedido.Usuario.Email);
                        pedido.UsuarioId = pedido.Usuario.Id;

                        /*foreach (Pedido pedidoAntigo in pedidos)
                        {
                            if (pedidoAntigo.Id.Equals(pedido.Id))
                            {
                                pedidoAntigo.Sacola = pedido.Sacola;
                                pedidoAntigo.Usuario = pedido.Usuario;
                                pedidoAntigo.Data = pedido.Data;
                                pedidoAntigo.Id = pedido.Id;
                                pedidoAntigo.IdTransacao = pedido.IdTransacao;
                                pedidoAntigo.Status = pedido.Status;
                                pedidoAntigo.StatusTexto = pedido.StatusTexto;
                                pedidoUsuario = new Pedido(pedidoAntigo);
                                break;
                            }
                        }*/

                       /* Usuario novoUsuario = new Usuario(pedido.Usuario);
                        foreach (Pedido pedidoUsr in novoUsuario.ListaPedido)
                        {
                            if (pedidoUsuario != null && pedido.Id != 0 && pedidoUsuario.Id.Equals(pedidoUsr.Id))
                            {
                                pedidoUsr.IdTransacao = pedidoUsuario.IdTransacao;
                                pedidoUsr.Status = pedidoUsuario.Status;
                                pedidoUsr.StatusTexto = pedidoUsuario.StatusTexto;
                            }

                        }*/
                        PedidoBO.EditarPedido(pedido);
                        //IncluirPedidoUsuario(novoUsuario);
                    }
                }
                if (teveAlteracao)
                {


                }
            }
        }


        public static void IncluirPedidoUsuario(Usuario usuarioAdicionado)
        {


            Usuario usuario = UsuarioBO.ObterUsuario(usuarioAdicionado.Id);
            usuario.ListaPedido.Clear();
            usuario.ListaPedido.AddRange(usuarioAdicionado.ListaPedido);
            UsuarioBO.EditarUsuario(usuario);

        }
    }

}