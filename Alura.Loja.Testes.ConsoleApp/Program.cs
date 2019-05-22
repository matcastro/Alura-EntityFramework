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
            using(var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                // AdicionarProduto(contexto);
                var produtos = contexto.Produtos.ToList();
                foreach (var produto in produtos)
                {
                    Console.WriteLine(produto);
                }              

                var entries = contexto.ChangeTracker.Entries();
                ImprimeEstados(entries);
                produtos[0].Nome = "Teste";
                contexto.Update(produtos[0]);
                ImprimeEstados(entries);
                contexto.Remove(produtos[0]);
                ImprimeEstados(entries);
                produtos.Add(new Produto() { Nome = "Sorvete", Categoria = "Comida", PrecoUnitario = 5 });
                contexto.Add(produtos[1]);
                ImprimeEstados(entries);
                contexto.Remove(produtos[1]);
                ImprimeEstados(entries);

                var entry = contexto.Entry(produtos[1]);
                contexto.SaveChanges();
                Console.WriteLine(entry.State);
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
