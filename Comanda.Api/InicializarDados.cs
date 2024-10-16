using SistemaDeComandas.BancoDeDados;
using SistemaDeComandas.Modelos;

namespace Comanda.Api
{
    public static class InicializarDados
    {
        public static void Semear(ComandaContexto banco)
        {
            // Cardapio
            // se o cardapio items nao tem nenhum item cadastrado
            if(!banco.CardapioItems.Any())
            {
                // add range eh pra adicionar varias coisas, se fosse so add tinha que colocar varias vezes
                banco.CardapioItems.AddRange(
                    // aq n ta Vermelho
                    new CardapioItem()
                    {
                        Descricao = "mickey",
                        PossuiPreparo = true,
                        // M eh pra deixar decimal
                        Preco = 20.00M,
                        Titulo = "XIS RATO"
                    },
                    new CardapioItem()
                    {
                        Descricao = "bom",
                        PossuiPreparo = true,
                        Preco = 15.00M,
                        Titulo = "XIS URUBU"
                    },
                    new CardapioItem()
                    {
                        Descricao = "gostosa",
                        PossuiPreparo = false,
                        Preco = 7.00M,
                        Titulo = "AGUA CLORIFICADA"
                    },
                     new CardapioItem()
                     {
                         Descricao = "potassio",
                         PossuiPreparo = false,
                         Preco = 5.00M,
                         Titulo = "BANANA"
                     }
                    );
            }
            // INSERT INTO Cardapio (Columns) VALUES(1, "SALSICHA")

            if (!banco.Usuarios.Any())
            {
                banco.Usuarios.AddRange(
                    new Usuario()
                    {
                        emailUsuario = "admin@admin.com",
                        idFuncaoUsuario = 1,
                        nomeUsuario = "admin",
                        senhaUsuario = "admin"
                    }
                    );
            }

            if (!banco.Mesas.Any())
            {
                banco.Mesas.AddRange(
                    new Mesa()
                    {
                        NumeroMesa = 1,
                        SituacaoMesa = 1
                    },
                    new Mesa()
                    {
                        NumeroMesa = 2,
                        SituacaoMesa = 1
                    },
                    new Mesa()
                    {
                        NumeroMesa = 3,
                        SituacaoMesa = 1
                    },
                    new Mesa()
                    {
                        NumeroMesa = 4,
                        SituacaoMesa = 1
                    },
                    new Mesa()
                    {
                        NumeroMesa = 5,
                        SituacaoMesa = 1
                    }
                    );
            }

            if (!banco.Comandas.Any())
            {
                var comanda = new SistemaDeComandas.Modelos.Comanda() { NomeCliente = "Keller", NumeroMesa = 1, SituacaoComanda = 1};
                banco.Comandas.Add(comanda );

                // array com todas as comandas pre setadas
                ComandaItem[] listaArrayItem = {
                     new ComandaItem()
                        {
                            Comanda = comanda,
                            CardapioItemId = 1
                        },
                        new ComandaItem()
                        {
                            Comanda = comanda,
                            CardapioItemId = 2
                        }
                    };

                if (!banco.ComandaItems.Any())
                {

                    // Add Range eh pra adicionar mais de um
                    banco.ComandaItems.AddRange(listaArrayItem);
                }
                var pedidoCozinha1 = new PedidoCozinha() { Comanda = comanda };
                var pedidoCozinha2 = new PedidoCozinha() { Comanda = comanda };

                PedidoCozinhaItem[] pedidoCozinhaItems =
                {
                    new PedidoCozinhaItem{PedidoCozinha = pedidoCozinha1, ComandaItem = listaArrayItem[0]},
                    new PedidoCozinhaItem{PedidoCozinha = pedidoCozinha2, ComandaItem = listaArrayItem[1]}
                };

                banco.PedidoCozinhas.Add(pedidoCozinha1);
                banco.PedidoCozinhas.Add(pedidoCozinha2);
                banco.PedidoCozinhaItems.AddRange(pedidoCozinhaItems);
            }

            banco.SaveChanges();
        }
    }
}
