using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Servicos.Relatorio
{
    public class HistoricoProduto
    {
        public HistoricoProduto(string nomeProdto, string nomeLoja, string link, IEnumerable<PeriodoHistoricoProduto> peridos)
        {
            NomeProdto = nomeProdto;
            NomeLoja = nomeLoja;
            Link = link;
            Peridos = peridos;
        }

        public string NomeProdto { get; set; }
        public string NomeLoja { get; set; }
        public string Link { get; set; }
        public IEnumerable<PeriodoHistoricoProduto> Peridos { get; set; }

    }

    public class PeriodoHistoricoProduto
    {
        public PeriodoHistoricoProduto(double valorAvista, double valorParcelado, DateTime data)
        {
            ValorAvista = valorAvista;
            ValorParcelado = valorParcelado;
            Data = data;
        }

        public double ValorAvista { get; set; }
        public double ValorParcelado { get; set; }
        public DateTime Data { get; set; }

        public static PeriodoHistoricoProduto Novo(double valorAvista, double valorParcelado, DateTime data)
            => new PeriodoHistoricoProduto(valorAvista, valorParcelado, data);
    }

    public static class PeriodoHistoricoProdutoEnxtensao
    {
        public static double Minima(this IEnumerable<PeriodoHistoricoProduto> historicos)
        {
            return historicos
                    .OrderBy(h => h.ValorAvista)
                    .FirstOrDefault().ValorAvista;
        }

        public static double Maxima(this IEnumerable<PeriodoHistoricoProduto> historicos)
        {
            return historicos
                  .OrderByDescending(h => h.ValorAvista)
                  .FirstOrDefault().ValorAvista;
        }

        public static double Media(this IEnumerable<PeriodoHistoricoProduto> historicos)
        {
            var quantidade = historicos.Count();
            var soma = historicos.Select(historico => historico.ValorAvista).Sum();
            return (soma / quantidade);
        }

    }
}
