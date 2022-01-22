using Servicos;
using System;
using System.Threading.Tasks;

namespace WebScraping.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Carregando valores...");
            var kabum = new KabumWebScraping();
            kabum.LerPaginasAsync("https://www.kabum.com.br/produto/273163/placa-de-video-zotac-gaming-nvidia-geforce-rtx-2060-6gb-gddr6-ray-tracing-led-branco-zt-t20600r-10m").GetAwaiter().GetResult();
        }
    }

}
