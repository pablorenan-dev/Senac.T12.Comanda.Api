

using Microsoft.EntityFrameworkCore;
using SistemaDeComandas.Modelos;

namespace SistemaDeComandas.BancoDeDados
{
    //essa classe representa nosso banco
     //DbContext foi feita pela microsoft
    public class ComandaContexto : DbContext
    {

        //criar as variaveis que representam as tabelas
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<CardapioItem> CardapioItems { get; set; }
        public DbSet<Modelos.Comanda> Comandas { get; set; }
        public DbSet<ComandaItem> ComandaItems { get; set; }
        public DbSet<PedidoCozinha> PedidoCozinhas { get; set; }
        public DbSet<PedidoCozinhaItem> PedidoCozinhaItems { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        //metodo para configurar banco(para abrir rapido e so escrever override onconfiure e depois enter)
        //serve para configurar a conexao do banco de dados
        public ComandaContexto(DbContextOptions<ComandaContexto> options): base(options)
        {

        }

        //serve para configurar os relacionamentos das tabelas
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //acesso a entidade CardapioItem para determinar a nomenclatura

            //forcar valores do CardapioItem(deixar anti-burro, se alguem mudar algum valor no Cardapio item(int -> string num ID)o valor continuara sendo INTEGER no banco. - START
            modelBuilder.Entity<CardapioItem>()
                .Property(p => p.Id)
                .HasColumnType("INTEGER");

            modelBuilder.Entity<CardapioItem>()
                .Property(p => p.Titulo)
                .HasColumnType("TEXT");

            modelBuilder.Entity<CardapioItem>()
                .Property(p => p.Titulo)
                .HasColumnType("TEXT");

            modelBuilder.Entity<CardapioItem>()
                .Property(p => p.Preco)
                .HasConversion<decimal>()
                .HasColumnType("NUMERIC");

            modelBuilder.Entity<CardapioItem>()
                .Property(p => p.PossuiPreparo)
                .HasColumnType("INTEGER");

            //forcar valores do CardapioItem - END

            //forcar os valores da Comanda - START
            modelBuilder.Entity<Modelos.Comanda>()
                .Property(c => c.Id)
                .HasColumnType("INTEGER");

            modelBuilder.Entity<Modelos.Comanda>()
                .Property(c => c.NumeroMesa)
                .HasColumnType("INTEGER");

            modelBuilder.Entity<Modelos.Comanda>()
                .Property(c => c.NomeCliente)
                .HasColumnType("TEXT");

            modelBuilder.Entity<Modelos.Comanda>()
                .Property(c => c.SituacaoComanda)
                .HasColumnType("INTEGER");

            //forcar os valores da Comanda - END

            //Uma comanda possui muitos ComandasItems
            //E suas chave estraingeira é ComandaId
            modelBuilder.Entity<Modelos.Comanda>()
                .HasMany(c=>c.ComandaItems)
                .WithOne(ci=> ci.Comanda)
                .HasForeignKey(f => f.ComandaId);

        
            //forcar os valores do PedidoCozinhaItem - START
            modelBuilder.Entity<PedidoCozinhaItem>()
                .Property(c => c.Id)
                .HasColumnType("INTEGER");

            modelBuilder.Entity<PedidoCozinhaItem>()
                .Property(c => c.PedidoCozinhaId)
                .HasColumnType("INTEGER");

            modelBuilder.Entity<PedidoCozinhaItem>()
                .Property(c => c.ComandaItemId)
                .HasColumnType("INTEGER");

            //forcar os valores do PedidoCozinhaItem - END

            //Pedido cozinha item possui um comanda item
            //E sua chave estrangeira é ComadaItemId
            modelBuilder.Entity<PedidoCozinhaItem>()
                .HasOne(pci => pci.ComandaItem)
                .WithMany()
                .HasForeignKey(pci => pci.ComandaItemId);

            //forcar os valores do PedidoCozinha - START
            modelBuilder.Entity<PedidoCozinha>()
                .Property(c => c.Id)
                .HasColumnType("INTEGER");

            modelBuilder.Entity<PedidoCozinha>()
                .Property(c => c.ComandaId)
                .HasColumnType("INTEGER");

            modelBuilder.Entity<PedidoCozinha>()
                .Property(c => c.SituacaoId)
                .HasColumnType("INTEGER");

            //forcar os valores do PedidoCozinha - END

            //Pedido Cozinha com Pedido Cozinha item
            modelBuilder.Entity<PedidoCozinha>()
                .HasMany(cd=>cd.PedidoCozinhaItems)
                .WithOne(pci => pci.PedidoCozinha)
                .HasForeignKey(pci => pci.PedidoCozinhaId);

            //forcar valor do ComandaItem - START
            modelBuilder.Entity<ComandaItem>()
                .Property(ci => ci.Id)
                .HasColumnType("INTEGER");

            modelBuilder.Entity<ComandaItem>()
                .Property(ci => ci.CardapioItemId)
                .HasColumnType("INTEGER");

            modelBuilder.Entity<ComandaItem>()
                .Property(ci => ci.ComandaId)
                .HasColumnType("INTEGER");

            //forcar valor do ComandaItem - END

            //CardapioItem com CardapioItemId
            modelBuilder.Entity<ComandaItem>()
                .HasOne(ci => ci.CardapioItem)
                .WithMany()
                .HasForeignKey(ci => ci.CardapioItemId);

            modelBuilder.Entity<ComandaItem>()
                .HasOne(ci => ci.Comanda)
                .WithMany(ci => ci.ComandaItems)
                .HasForeignKey(ci => ci.CardapioItemId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
