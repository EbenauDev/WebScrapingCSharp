using Servicos.Configuracao;
using Servicos.WebScraping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Servicos
{
    public interface IWebScrapingService
    {
        Task<IEnumerable<Produto>> LerPaginasAsync(ConfiguracaoService configuracao);
        Task<bool> GerarRelatorioAsync(IEnumerable<Produto> produtos);
        Task<bool> GerarRelatorioCSVAsync(IEnumerable<Produto> produtos);
    }

    public class WebScrapingService : IWebScrapingService
    {
        private readonly IWebScrapingStrategy _scrapingStrategy;
        public WebScrapingService(IWebScrapingStrategy scrapingStrategy)
        {
            _scrapingStrategy = scrapingStrategy;
        }

        public async Task<IEnumerable<Produto>> LerPaginasAsync(ConfiguracaoService configuracao)
        {
            var _lista = new List<Produto>();
            foreach (var paramero in configuracao.Parametros)
            {
                if (_scrapingStrategy.DefinirWebScraping(paramero.Site) is var scraping && scraping != null)
                    _lista.Add(await scraping.LerPaginasAsync(paramero.EnderecoURL));
            }
            return _lista;
        }

        public async Task<bool> GerarRelatorioAsync(IEnumerable<Produto> produtos)
        {
            try
            {
                var arquivoTxt = new StringBuilder();
                foreach (var produto in produtos)
                {
                    arquivoTxt.AppendLine($"Nome: {produto.Nome}");
                    arquivoTxt.AppendLine($"Preço a vista: {produto.ValorAVista}");
                    arquivoTxt.AppendLine($"Preço parcelado: {produto.ValorParcelado}");
                    arquivoTxt.AppendLine($"Loja: {produto.Loja}");
                    arquivoTxt.AppendLine($"Disponibilidade: {produto.Disponibilidade}");
                    arquivoTxt.AppendLine($"Link de Compra: {produto.Link}");
                    arquivoTxt.AppendLine($"Consultado: {produto.DataDeConsulta.ToString("dd/MM/yyyy 'às' HH:mm")}");
                    arquivoTxt.AppendLine($"==============================================================================");
                }
                var areaDeTrabalhoPath = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
                await File.AppendAllTextAsync($"{areaDeTrabalhoPath}//preco-placas-de-video-{DateTime.Now.ToString("dd-MM-yyyy")}.txt", arquivoTxt.ToString(), Encoding.UTF8);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> GerarRelatorioCSVAsync(IEnumerable<Produto> produtos)
        {
            try
            {
                var arquivoTxt = new StringBuilder();
                arquivoTxt.AppendLine("Produto# Valor a Vista# Valor Parcelado# Loja# Disponibilidade# Link de Compra# Data de Consulta");
                foreach (var produto in produtos)
                {
                    var novaLinha = $"{produto.Nome.Split(",")[0]}# {produto.ValorAVista}# {produto.ValorParcelado}# {produto.Loja}# {produto.Disponibilidade}# {produto.Link}# {produto.DataDeConsulta.ToString("dd/MM/yyyy 'às' HH:mm")}";
                    arquivoTxt.AppendLine(novaLinha);
                }
                var areaDeTrabalhoPath = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
                await File.AppendAllTextAsync($"{areaDeTrabalhoPath}//preco-placas-de-video-{DateTime.Now.ToString("dd-MM-yyyy")}.csv", arquivoTxt.ToString(), Encoding.UTF8);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
