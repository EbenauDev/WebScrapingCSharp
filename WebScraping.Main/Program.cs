using Microsoft.Extensions.DependencyInjection;
using Servicos;
using Servicos.Configuracao;
using Servicos.Relatorio;
using Servicos.WebScraping;
using System;
using System.Collections.Generic;
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
            Perguntar();
            int opcao = Convert.ToInt32(Console.ReadLine());

            while (opcao > 2 || opcao < 0)
            {
                Perguntar();
                opcao = Convert.ToInt32(Console.ReadLine());
            }

            if (opcao == 1)
            {
                await RealizarPesquisaPrecosAsync(webScrapingService);
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
            Console.WriteLine("O que você deseja fazer?");
            Console.WriteLine("[ 1 ] Para gerar relátorio dos preços das GPU`s");
            Console.WriteLine("[ 2 ] Para gerar relátorio comparando ospreços de GPU`s");
        }

        public async static Task<int> RealizarPesquisaPrecosAsync(IWebScrapingService webScrapingService)
        {
            Console.WriteLine("Carregando valores...");
            var configuracao = new ConfiguracaoService(new List<Parametro> {
                new Parametro(EWebSite.Kabum, "https://www.kabum.com.br/produto/273163/placa-de-video-zotac-gaming-nvidia-geforce-rtx-2060-6gb-gddr6-ray-tracing-led-branco-zt-t20600r-10m"),
                new Parametro(EWebSite.Kabum, "https://www.kabum.com.br/produto/148656/placa-de-video-zotac-gaming-geforce-rtx-3060-twin-edge-15-gbps-12gb-gddr6-ray-tracing-zt-a30600e-10m"),
                new Parametro(EWebSite.Kabum, "https://www.kabum.com.br/produto/302389/placa-de-video-zotac-geforce-rtx-3060-ti-twin-edge-lhr-8gb-gddr6-256-bits-zt-a30610e-10mlhr"),
                new Parametro(EWebSite.Kabum, "https://www.kabum.com.br/produto/100863/placa-de-video-gigabyte-nvidia-geforce-gtx-1660-ti-oc-6g-gddr6-gv-n166toc-6gd"),
                new Parametro(EWebSite.Kabum, "https://www.kabum.com.br/produto/105132/placa-de-video-gigabyte-gtx-1660-super-oc-nvidia-geforce-6g-gddr6-gv-n166soc-6gd"),
                new Parametro(EWebSite.Kabum, "https://www.kabum.com.br/produto/192397/placa-de-video-asus-dual-rx-6600-xt-o8g-16-gbps-8gb-gddr6-90yv0gn1-m0na00"),
                new Parametro(EWebSite.Kabum, "https://www.kabum.com.br/produto/241298/placa-de-video-asus-amd-radeon-dual-rx-6700-xt-oc-12g-12gb-gddr6-ray-tracing-amd-rdna-2-architecture-90yv0g83-m0na00"),
                new Parametro(EWebSite.Kabum, "https://www.kabum.com.br/produto/150482/placa-de-video-asus-amd-radeon-rx-6700-xt-16-gbps-12gb-gddr6-dual-rx6700xt-12g"),
                new Parametro(EWebSite.Pichau, "https://www.pichau.com.br/placa-de-video-gigabyte-radeon-rx-6600-xt-gaming-oc-8gb-gddr6-128-bit-gv-r66xtgaming-oc-8gd"),
                new Parametro(EWebSite.Pichau, "https://www.pichau.com.br/placa-de-video-asus-geforce-gtx-1660-ti-tuf-gaming-oc-6gb-gddr6-192-bit-tuf-gtx1660ti-o6g-evo-gaming"),
                new Parametro(EWebSite.Pichau, "https://www.pichau.com.br/placa-de-video-asus-geforce-rtx-3060-ti-v2-oc-lhr-8gb-gddr6-dual-256-bit-dual-rtx3060ti-o8g-v2"),
                new Parametro(EWebSite.Pichau, "https://www.pichau.com.br/placa-de-video-asus-geforce-rtx-3060-ti-tuf-gaming-oc-lhr-8gb-gddr6-256-bit-tuf-rtx3060ti-o8g-v2-gaming"),
                new Parametro(EWebSite.Pichau, "https://www.pichau.com.br/placa-de-video-asus-geforce-rtx-3060-v2-oc-12gb-gddr6-dual-192-bit-dual-rtx3060-o12g-v2"),
                new Parametro(EWebSite.Pichau, "https://www.pichau.com.br/placa-de-video-asus-geforce-rtx-2060-dual-oc-edition-6gb-gddr6-192-bit-dual-rtx2060-o6g-evo"),
                new Parametro(EWebSite.Pichau, "https://www.pichau.com.br/placa-de-video-asus-radeon-rx-6600-xt-dual-oc-edition-8gb-gddr6-128-bit-dual-rx6600xt-o8g"),
            });

            if (await webScrapingService.LerPaginasAsync(configuracao) is var resultado && resultado == null)
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

        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScoped<ILerPaginas, KabumWebScraping>()
                .AddScoped<ILerPaginas, PichauWebScraping>()
                .AddScoped<IWebScrapingService, WebScrapingService>()
                .AddScoped<IWebScrapingStrategy, WebScrapingStrategy>()
                .AddScoped<IRelatorioService, RelatorioService>();
        }
    }

}
