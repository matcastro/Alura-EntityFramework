﻿namespace Alura.Loja.Testes.ConsoleApp
{
    public class Endereco
    {
        public string Rua { get; set; }
        public string Cidade { get; internal set; }
        public string Bairro { get; internal set; }
        public string Complemento { get; internal set; }
        public int Numero { get; internal set; }
    }
}