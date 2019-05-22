using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Loja.Testes.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var produto = new Produto()
            {
                Nome = "Pão Francês",
                Categoria = "Alimento",
                PrecoUnitario = 0.4,
                Unidade = "Unidade"
            };

            var compra = new Compra()
            {
                Quantidade = 6,
                Produto = produto                
            };
            compra.Preco = compra.Quantidade * compra.Produto.PrecoUnitario;
            using (var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                var entries = contexto.ChangeTracker.Entries();

                contexto.Add(compra);
                ImprimeEstados(entries);
                contexto.SaveChanges();
                ImprimeEstados(entries);
            }
        }

        private static void ImprimeEstados(IEnumerable<EntityEntry> entries)
        {
            Console.WriteLine("=======");
            foreach (var e in entries)
            {
                Console.WriteLine(e.State);
            }
        }

        private static void AdicionarProduto(LojaContext contexto)
        {
            var produto = new Produto()
            {
                Nome = "Pasta de Dente",
                Categoria = "Higiene",
                PrecoUnitario = 1.99
            };
            contexto.Add(produto);
            contexto.SaveChanges();
        }
    }
}
