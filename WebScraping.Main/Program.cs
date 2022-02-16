using Microsoft.Extensions.DependencyInjection;
using Servicos;
using Servicos.Configuracao;
using Servicos.Relatorio;
using Servicos.WebScraping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace WebScraping.Main
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var serviceCollecion = new ServiceCollection();
            ConfigureServices(serviceCollecion);
            var serviceProvider = serviceCollecion.BuildServiceProvider();
            var webScrapingService = serviceProvider.GetService<IWebScrapingService>();
            var relatorioPrecos = serviceProvider.GetService<IRelatorioService>();
            var configuracaoArquivoService = serviceProvider.GetService<IConfiguracaoArquivo>();
            if (await EstaConectado() is var resultado && resultado == false)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Você não está conectado a internet");
                Console.WriteLine("Conecte-se a internet para continuar utilizando");
                return -1;
            }
            Perguntar();
            int opcao = Convert.ToInt32(Console.ReadLine());

            while (opcao > 2 || opcao < 0)
            {
                Perguntar();
                opcao = Convert.ToInt32(Console.ReadLine());
            }

            if (opcao == 1)
            {
                await RealizarPesquisaPrecosAsync(webScrapingService, configuracaoArquivoService);
            }

            if (opcao == 2)
            {
                await GerarRelatorioComparacaoPrecosAsync(relatorioPrecos);
            }

            return 0;
        }

        private static void Perguntar()
        {
            Console.Clear();
            Console.WriteLine("Tecle [ 1 ] Para gerar relátorio dos preços das GPU`s");
            Console.WriteLine("Tecle [ 2 ] Para gerar relátorio comparando os preços de GPU`s");
            Console.WriteLine("O que você deseja fazer?");
        }

        public async static Task<int> RealizarPesquisaPrecosAsync(IWebScrapingService webScrapingService, IConfiguracaoArquivo configuracaoArquivo)
        {
            Console.WriteLine("Informe o caminho do arquivo com o nome da loja e o link do produto conforme formato abaixo");
            Console.WriteLine("NomeLoja=LinkProduto");
            var caminhoArquivo = Console.ReadLine();

            if (await configuracaoArquivo.LerConfiguracaoArquivoAsync(caminhoArquivo) is var parametros && parametros.Any() == false) {
                Console.WriteLine("Houve um problema ao carregar o arquivo");
                return 0;
            }

            if (await webScrapingService.LerPaginasAsync(parametros) is var resultado && resultado == null)
            {
                Console.WriteLine("Houve um problema ao coletar os preços das placas de vídeos");
                return 0;
            }

            Console.WriteLine("Gerando relatório de preços em formato .TXT");
            if (await webScrapingService.GerarRelatorioAsync(resultado) is var sucessoRelatorio && sucessoRelatorio == false)
            {
                Console.WriteLine("Houve um problema ao gerar o relatório de preços das placas de vídeos");
                return 0;
            }
            Console.WriteLine("Relatório .TXT gerado com sucesso!");

            Console.WriteLine("Gerando relatório de preços em formato .CSV");
            if (await webScrapingService.GerarRelatorioCSVAsync(resultado) is var sucessoRelatorioCsv && sucessoRelatorioCsv == false)
            {
                Console.WriteLine("Houve um problema ao gerar o relatório CSV de preços das placas de vídeos");
                return 0;
            }
            Console.WriteLine("Relatório .CSV gerado com sucesso!");
            Console.WriteLine("Relatórios estão salvos na área de trabalho!");
            return 1;
        }
        public async static Task<int> GerarRelatorioComparacaoPrecosAsync(IRelatorioService relatorioService)
        {
            try
            {
                Console.WriteLine("Gerar relátorio comparando os precos");
                Console.WriteLine("Copie o caminho do arquivo .csv que contêm os preços: ");
                var caminhoArquivo = Console.ReadLine();

                if (await relatorioService.GerarRelatorioPrecosAsync(caminhoArquivo) is var sucessoRelatorioComparador && sucessoRelatorioComparador == false)
                {
                    Console.WriteLine("Houve um problema ao gerar o relatório CSV de comparação dos preços dos produtos");
                    return 0;
                }
                Console.WriteLine("Relatório de comparação de preços está salvo na área de trabalho!");
                Console.ReadKey();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Houve um problema ao gerar o relatório de comparação");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Execption: ", ex.Message.ToString());
                Console.WriteLine("StackTrace: ", ex.StackTrace.ToString());
                return -1;
            }

        }

        public static async Task<bool> EstaConectado()
        {
            try
            {
                var ping = new Ping();
                string host = "google.com";
                byte[] buffer = new byte[32];
                var reply = await Task.Run(() => ping.Send(host, timeout: 1000, buffer, new PingOptions()));
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScoped<ILerPaginas, KabumWebScraping>()
                .AddScoped<ILerPaginas, PichauWebScraping>()
                .AddScoped<IWebScrapingService, WebScrapingService>()
                .AddScoped<IWebScrapingStrategy, WebScrapingStrategy>()
                .AddScoped<IRelatorioService, RelatorioService>()
                .AddScoped<IConfiguracaoArquivo, ConfiguracaoArquivo>();
        }
    }

}
