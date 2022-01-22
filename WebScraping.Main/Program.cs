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
            var pichau = new PichauWebScraping();
            //https://www.pichau.com.br/placa-de-video-asus-geforce-rtx-3060-v2-oc-12gb-gddr6-dual-192-bit-dual-rtx3060-o12g-v2
            //var placaDeVideoKabum = kabum.LerPaginasAsync("https://www.kabum.com.br/produto/273163/placa-de-video-zotac-gaming-nvidia-geforce-rtx-2060-6gb-gddr6-ray-tracing-led-branco-zt-t20600r-10m").GetAwaiter().GetResult();
            pichau.LerPaginasAsync("https://www.pichau.com.br/placa-de-video-asus-geforce-rtx-3060-v2-oc-12gb-gddr6-dual-192-bit-dual-rtx3060-o12g-v2").GetAwaiter().GetResult();
        }
    }

}
