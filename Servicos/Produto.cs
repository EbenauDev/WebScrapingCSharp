using System;

namespace Servicos
{
    public sealed class Produto
    {
        public Produto(string nome, string valorAVista, string valorParcelado, string loja, string disponibilidade, DateTime dataDeConsulta, string link)
        {
            Nome = nome;
            ValorAVista = valorAVista;
            ValorParcelado = valorParcelado;
            Loja = loja;
            Disponibilidade = disponibilidade;
            DataDeConsulta = dataDeConsulta;
            Link = link;
        }

        public string Nome { get; set; }
        public string ValorAVista { get; set; }
        public string ValorParcelado { get; set; }
        public string Loja { get; set; }
        public string Disponibilidade { get; set; }
        public DateTime DataDeConsulta { get; set; }
        public string Link { get; set; }
    }
}
