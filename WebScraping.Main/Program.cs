using Microsoft.Extensions.DependencyInjection;
using Servicos;
using Servicos.Configuracao;
using Servicos.WebScraping;
using System;
using System.Collections.Generic;
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

            Console.WriteLine("Carregando valores...");
            var configuracao = new ConfiguracaoService(new List<Parametro> {
                new Parametro(EWebSite.Kabum, "https://www.kabum.com.br/produto/273163/placa-de-video-zotac-gaming-nvidia-geforce-rtx-2060-6gb-gddr6-ray-tracing-led-branco-zt-t20600r-10m"),
                new Parametro(EWebSite.Kabum, "https://www.kabum.com.br/produto/148656/placa-de-video-zotac-gaming-geforce-rtx-3060-twin-edge-15-gbps-12gb-gddr6-ray-tracing-zt-a30600e-10m"),
                new Parametro(EWebSite.Kabum, "https://www.kabum.com.br/produto/302389/placa-de-video-zotac-geforce-rtx-3060-ti-twin-edge-lhr-8gb-gddr6-256-bits-zt-a30610e-10mlhr"),
                new Parametro(EWebSite.Kabum, "https://www.kabum.com.br/produto/100863/placa-de-video-gigabyte-nvidia-geforce-gtx-1660-ti-oc-6g-gddr6-gv-n166toc-6gd"),
                new Parametro(EWebSite.Kabum, "https://www.kabum.com.br/produto/105132/placa-de-video-gigabyte-gtx-1660-super-oc-nvidia-geforce-6g-gddr6-gv-n166soc-6gd"),
                new Parametro(EWebSite.Kabum, "https://www.kabum.com.br/produto/192397/placa-de-video-asus-dual-rx-6600-xt-o8g-16-gbps-8gb-gddr6-90yv0gn1-m0na00"),
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
                Console.WriteLine("Houve um problema ao coletar os preços das Placas de vídeos");
                return 0;
            }

            if (await webScrapingService.GerarRelatorioAsync(resultado) is var sucessoRelatorio && sucessoRelatorio == false)
            {
                Console.WriteLine("Houve um problema ao gerar o relatório de preços das Placas de vídeos");
                return 0;
            }

            return 0;
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScoped<ILerPaginas, KabumWebScraping>()
                .AddScoped<ILerPaginas, PichauWebScraping>()
                .AddScoped<IWebScrapingService, WebScrapingService>()
                .AddScoped<IWebScrapingStrategy, WebScrapingStrategy>();
        }
    }

}
