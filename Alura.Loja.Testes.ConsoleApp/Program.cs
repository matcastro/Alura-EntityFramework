using Microsoft.EntityFrameworkCore;
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
            using (var contexto = new LojaContext())
            {
                LogSql(contexto);
                //Produtos da Promocao
                var promocao = contexto.Promocoes
                    .Include(p => p.Produtos)
                    .ThenInclude(pp => pp.Produto)
                    .FirstOrDefault();

                Console.WriteLine($"Produtos da promoção {promocao.Descricao}");
                foreach (var item in promocao.Produtos)
                {
                    Console.WriteLine(item.Produto);
                }

                //Endereço do Cliente
                var cliente = contexto.Clientes
                    .Include(c => c.Endereco)
                    .FirstOrDefault();
                Console.WriteLine($"Endereço do cliente {cliente.Nome}:");
                Console.WriteLine($"{cliente.Endereco.Rua}");

                //Compras do Produto
                var produto = contexto.Produtos
                    .Where(p => p.Id == 3002)
                    .Include(p => p.Compras)
                    .First();
                Console.WriteLine($"Compras do produto {produto.Nome}");
                foreach (var compra in produto.Compras)
                {
                    Console.WriteLine($"{compra.Quantidade} {compra.Produto.Unidade} de {compra.Produto.Nome} no valor de {compra.Preco}");
                }

                //Compras acima de 2 reais do Produto
                var p1 = contexto.Produtos
                    .Where(p => p.Id == 3002)
                    .FirstOrDefault();
                contexto.Entry(p1)
                    .Collection(p => p.Compras)
                    .Query()
                    .Where(c => c.Preco > 3.0)
                    .Load();
                Console.WriteLine($"Compras do produto {produto.Nome} com condicional");
                foreach (var compra in p1.Compras)
                {
                    Console.WriteLine($"{compra.Quantidade} {compra.Produto.Unidade} de {compra.Produto.Nome} no valor de {compra.Preco}");
                }
            }
        }

        private static void IncluirPromocao()
        {
            var promocao = new Promocao()
            {
                Descricao = "Queima de estoque!",
                DataInicial = new DateTime(2019, 1, 1),
                DataFinal = new DateTime(2019, 1, 31)
            };
            using (var contexto = new LojaContext())
            {
                LogSql(contexto);
                var produtos = contexto.Produtos
                    .Where(p => p.Categoria.Equals("Bebidas"))
                    .ToList();

                foreach (var item in produtos)
                {
                    promocao.IncluiProduto(item);
                }

                contexto.Promocoes.Add(promocao);
                //ImprimeEstados(contexto.ChangeTracker.Entries());
                contexto.SaveChanges();

            }
        }

        private static void IncluirBebidas(LojaContext contexto)
        {
            var p1 = new Produto()
            {
                Categoria = "Bebidas",
                Nome = "Café",
                PrecoUnitario = 1.0,
                Unidade = "Xícara"
            };

            var p2 = new Produto()
            {
                Categoria = "Bebidas",
                Nome = "Refrigerante",
                PrecoUnitario = 6.5,
                Unidade = "Litros"
            };

            var p3 = new Produto()
            {
                Categoria = "Bebidas",
                Nome = "Suco",
                PrecoUnitario = 2.35,
                Unidade = "Litros"
            };

            var p4 = new Produto()
            {
                Categoria = "Bebidas",
                Nome = "Leite",
                PrecoUnitario = 2.79,
                Unidade = "Caixa"
            };
            contexto.Produtos.AddRange(p1, p2, p3, p4);
            contexto.SaveChanges();
        }

        private static void UmParaUm()
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
                LogSql(contexto);

                var entries = contexto.ChangeTracker.Entries();

                contexto.Add(cliente);
                ImprimeEstados(entries);
                contexto.SaveChanges();
                ImprimeEstados(entries);
            }
        }

        private static void LogSql(LojaContext contexto)
        {
            var serviceProvider = contexto.GetInfrastructure();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddProvider(SqlLoggerProvider.Create());
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
