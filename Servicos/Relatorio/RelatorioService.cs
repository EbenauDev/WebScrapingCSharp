﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicos.Extensoes;

namespace Servicos.Relatorio
{
    public interface IRelatorioService
    {
        Task<bool> GerarRelatorioPrecosAsync(string caminhoArquivoCSV);
    }

    public sealed class RelatorioService : IRelatorioService
    {
        public async Task<bool> GerarRelatorioPrecosAsync(string caminhoArquivoCSV)
        {
            if (File.Exists(caminhoArquivoCSV) == false)
            {
                return false;
            }
            var lista = new List<Produto>();
            using (var reader = new StreamReader(File.OpenRead(caminhoArquivoCSV)))
            {
                while (!reader.EndOfStream)
                {
                    var linha = await reader.ReadLineAsync();
                    var valores = linha.Split("#");
                    lista.Add(new Produto(
                            nome: valores[0],
                            valorAVista: valores[1],
                            valorParcelado: valores[2],
                            loja: valores[3],
                            disponibilidade: valores[4],
                            dataDeConsulta: valores[6].ConveterParaDateTime(),
                            link: valores[5]
                        ));
                }
            }
            var produtosAgrupados = lista.GroupBy(produto => produto.Nome, (chave, produtos) => new
            {
                NomeProduto = chave,
                Periodos = produtos.Select(p => new
                {
                    ValorAVista = p.GetValorAVista(),
                    ValorParcelado = p.GetValorParcelado(),
                    Data = p.DataDeConsulta
                })
            });
            var historicos = new List<HistoricoProduto>();
            foreach (var produto in produtosAgrupados)
            {
                historicos.Add(new HistoricoProduto(produto.NomeProduto,
                                                    peridos: produto.Periodos
                                                            .Select(p => PeriodoHistoricoProduto
                                                                           .Novo(p.ValorAVista, p.ValorParcelado, p.Data))));
            }
            return true;
        }
    }
}