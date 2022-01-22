using System;
using System.Collections.Generic;
using System.Text;

namespace Servicos
{
    public sealed class Produto
    {
        public Produto(string nome, DateTime dataDeConsulta, string valorAVista, string valorParcelado)
        {
            Nome = nome;
            DataDeConsulta = dataDeConsulta;
            ValorAVista = valorAVista;
            ValorParcelado = valorParcelado;
        }

        public string Nome { get; set; }
        public DateTime DataDeConsulta { get; set; }
        public string ValorAVista { get; set; }
        public string ValorParcelado { get; set; }
    }
}
