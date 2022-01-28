﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Servicos.Relatorio
{
    public class HistoricoProduto
    {
        public HistoricoProduto(string nomeProdto, IEnumerable<PeriodoHistoricoProduto> peridos)
        {
            NomeProdto = nomeProdto;
            Peridos = peridos;
        }

        public string NomeProdto { get; set; }
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
}