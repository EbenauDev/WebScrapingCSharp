using System;
using System.Collections.Generic;
using System.Globalization;
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
            var lista = await LerProdutosAsync(caminhoArquivoCSV);
            var produtosAgrupados = lista.GroupBy(produto => produto.Nome, (chave, produtos) => new
            {
                Produto = produtos.Select(p => new { p.Nome, p.Loja, p.Link, }).FirstOrDefault(),
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
                historicos.Add(new HistoricoProduto(produto.Produto.Nome,
                                                    produto.Produto.Loja,
                                                    produto.Produto.Link,
                                                    peridos: produto.Periodos
                                                            .Select(p => PeriodoHistoricoProduto
                                                                           .Novo(p.ValorAVista, p.ValorParcelado, p.Data))));
            }
            await GerarRelatorioAsync(historicos);
            return true;
        }

        private async Task<IList<Produto>> LerProdutosAsync(string caminho)
        {
            if (File.Exists(caminho) == false)
            {
                throw new Exception("Caminho do arquivo inválido ou não existe");
            }
            var lista = new List<Produto>();
            using (var reader = new StreamReader(File.OpenRead(caminho)))
            {
                while (!reader.EndOfStream)
                {
                    var linha = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(linha))
                    {
                        throw new Exception("O arquivo está vázio ou contêm uma linha em branco");
                    }
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
            return lista;
        }

        private async Task<bool> GerarRelatorioAsync(IEnumerable<HistoricoProduto> historicos)
        {
            try
            {
                var arquivoTxt = new StringBuilder()
                        .AppendLine($"Relatório gerado em {DateTime.Now.ToString("dddd, dd 'de' MMMM 'às' HH:mm 'de' yyyy")}")
                        .AppendLine();
                foreach (var historico in historicos)
                {
                    arquivoTxt.AppendLine($"Produto: {historico.NomeProdto}");
                    arquivoTxt.AppendLine($"Preço médio: {historico.Peridos.Media().ToString("C", CultureInfo.CurrentCulture)}");
                    arquivoTxt.AppendLine($"Preço máximo: {historico.Peridos.Maxima().ToString("C", CultureInfo.CurrentCulture)}");
                    arquivoTxt.AppendLine($"Preço mínimo: {historico.Peridos.Minima().ToString("C", CultureInfo.CurrentCulture)}");
                    arquivoTxt.AppendLine($"Preço atual: {historico.Peridos.PrecoAtual().ToString("C", CultureInfo.CurrentCulture)}");
                    arquivoTxt.AppendLine($"Loja: {historico.NomeLoja}");
                    arquivoTxt.AppendLine($"Link: {historico.Link}");
                    arquivoTxt.AppendLine();
                }
                var areaDeTrabalhoPath = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
                await File.AppendAllTextAsync($"{areaDeTrabalhoPath}//relatorio-precos-{DateTime.Now.ToString("dd-MM-yyyy")}.txt", arquivoTxt.ToString(), Encoding.UTF8);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
