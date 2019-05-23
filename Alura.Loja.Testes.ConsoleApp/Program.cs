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
            var cliente = new Cliente();
            cliente.Nome = "Nome Teste";
            cliente.Endereco = new Endereco()
            {
                Rua = "Rua 1",
                Numero = 1,
                Complemento = "sobrado",
                Bairro = "Dom João",
                Cidade = "Cidade do Cabo"
            };

            using (var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                var entries = contexto.ChangeTracker.Entries();

                contexto.Add(cliente);
                ImprimeEstados(entries);
                contexto.SaveChanges();
                ImprimeEstados(entries);
            }
        }

        private static void NParaN()
        {
            var promocao = new Promocao();
            promocao.Descricao = "Páscoa Feliz";
            promocao.DataInicial = DateTime.Now;
            promocao.DataFinal = DateTime.Now.AddMonths(3);
            var p1 = new Produto()
            {
                Nome = "Barra de Chocolate",
                Categoria = "Alimento",
                PrecoUnitario = 5.0,
                Unidade = "gramas"
            };

            var p2 = new Produto()
            {
                Nome = "Boneca Bebê",
                Categoria = "Brinquedo",
                PrecoUnitario = 89.90,
                Unidade = "Unidade"
            };

            var p3 = new Produto()
            {
                Nome = "Ovo de Páscoa",
                Categoria = "Alimento",
                PrecoUnitario = 35.0,
                Unidade = "gramas"
            };

            promocao.IncluiProduto(p1);
            promocao.IncluiProduto(p2);
            promocao.IncluiProduto(p3);

            using (var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                var entries = contexto.ChangeTracker.Entries();

                var promo = contexto.Promocoes.First();
                ImprimeEstados(entries);
                contexto.Remove(promo);
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
                Console.WriteLine($"{e.State} - {e.Entity}");
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
