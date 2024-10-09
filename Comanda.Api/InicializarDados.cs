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
                        Descricao = "XIS RATO",
                        PossuiPreparo = true,
                        // M eh pra deixar decimal
                        Preco = 20.00M,
                        Titulo = ""
                    },
                    new CardapioItem()
                    {
                        Descricao = "XIS URUBU",
                        PossuiPreparo = true,
                        Preco = 15.00M,
                        Titulo = ""
                    },
                    new CardapioItem()
                    {
                        Descricao = "AGUA CLORIFICADA",
                        PossuiPreparo = false,
                        Preco = 7.00M,
                        Titulo = ""
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

                if (!banco.ComandaItems.Any())
                {
                    banco.ComandaItems.AddRange(
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
                        );
                }
            }

            banco.SaveChanges();
        }
    }
}
