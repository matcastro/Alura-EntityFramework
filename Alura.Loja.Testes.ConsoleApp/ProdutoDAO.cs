using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Alura.Loja.Testes.ConsoleApp
{
    public class ProdutoDAO : IDisposable
    {
        private LojaContext contexto;

        public ProdutoDAO()
        {
            contexto = new LojaContext();
        }

        public void Dispose()
        {
            contexto.Dispose();
        }

        public void Adicionar(Produto p)
        {
            contexto.Produtos.Add(p);
            contexto.SaveChanges();
        }

        public void Atualizar(Produto p)
        {
            contexto.Produtos.Update(p);
            contexto.SaveChanges();
        }

        public void Remover(Produto p)
        {
            contexto.Produtos.Remove(p);
            contexto.SaveChanges();
        }

        public IList<Produto> Produtos()
        {
            return contexto.Produtos.ToList();
        }
    }
}