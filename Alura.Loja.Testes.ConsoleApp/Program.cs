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
            //GravarUsandoAdoNet();
            GravarUsandoEntity();
            RecuperarProdutos();
            AtualizarProduto();
            RecuperarProdutos();
            ExcluirProdutos();
            RecuperarProdutos();
        }

        private static void AtualizarProduto()
        {
            using(var dao = new ProdutoDAO())
            {
                var produto = dao.Produtos().First();
                produto.Nome += " - Editado";
                dao.Atualizar(produto);
            }
        }

        private static void ExcluirProdutos()
        {
            using(var dao = new ProdutoDAO())
            {
                var produtos = dao.Produtos();
                foreach (var item in produtos)
                {
                    dao.Remover(item);
                }
            }
        }

        private static void RecuperarProdutos()
        {
            using(var dao = new ProdutoDAO())
            {
                var produtos = dao.Produtos();
                Console.WriteLine($"Encontrados {produtos.Count} produto(s).");
                foreach (var item in produtos)
                {
                    Console.WriteLine($"{item.Nome}");
                }
            }
        }

        private static void GravarUsandoEntity()
        {
            Produto p = new Produto();
            p.Nome = "Harry Potter e a Ordem da Fênix";
            p.Categoria = "Livros";
            p.Preco = 19.89;

            using (var dao = new ProdutoDAO())
            {
                dao.Adicionar(p);
            }
        }

        private static void GravarUsandoAdoNet()
        {
            Produto p = new Produto();
            p.Nome = "Harry Potter e a Ordem da Fênix";
            p.Categoria = "Livros";
            p.Preco = 19.89;

            using (var repo = new ProdutoDAO())
            {
                repo.Adicionar(p);
            }
        }
    }
}
