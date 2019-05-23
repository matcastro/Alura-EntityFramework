using System;
using System.Collections.Generic;

namespace Alura.Loja.Testes.ConsoleApp
{
    public class Promocao
    {
        public int Id { get; internal set; }
        public string Descricao { get; internal set; }
        public DateTime DataInicial { get; internal set; }
        public DateTime DataFinal { get; internal set; }
        public List<PromocaoProduto> Produtos { get; set; }

        public Promocao()
        {
            Produtos = new List<PromocaoProduto>();
        }

        public void IncluiProduto(Produto p)
        {
            Produtos.Add(new PromocaoProduto() { Produto = p });
        }
    }
}