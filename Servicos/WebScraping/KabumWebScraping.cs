using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Servicos
{
    public sealed class KabumWebScraping : ILerPaginas
    {
        public KabumWebScraping()
        {

        }

        public async Task<Produto> LerPaginasAsync(string enderecoURl)
        {
            try
            {
                var webClient = new WebClient();
                webClient.Headers.Clear();
                webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.71 Safari/537.36");
                var paginaBaixada = await Task.FromResult(webClient.DownloadString(enderecoURl));

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(paginaBaixada);



                var tituloItem = htmlDocument.DocumentNode.Descendants("h1")
                                                .FirstOrDefault()
                                                .InnerText;

                var produtoEstaEsgotado = htmlDocument.DocumentNode.Descendants("div")
                                             .Where(node => node.GetAttributeValue("id", "")
                                             .Contains("formularioProdutoIndisponivel"))
                                             .FirstOrDefault();


                if (produtoEstaEsgotado != null)
                {
                    return new Produto(tituloItem,
                                       valorAVista: "R$ 0,00",
                                       valorParcelado: "R$ 0,00",
                                       loja: "Lojas Kabum",
                                       disponibilidade: "Produto indisponível",
                                       dataDeConsulta: DateTime.Now,
                                       link: enderecoURl);
                }

                var valorAVista = htmlDocument.DocumentNode.Descendants("h4")
                                                .FirstOrDefault()
                                                .InnerText;
                var valorParcelado = htmlDocument.DocumentNode
                                                .Descendants("b")
                                                .Where(node => node.GetAttributeValue("class", "")
                                                .Contains("regularPrice"))
                                                .FirstOrDefault()
                                                .InnerText;
                return new Produto(tituloItem,
                                   valorAVista,
                                   valorParcelado,
                                   loja: "Lojas Kabum",
                                   disponibilidade: "Produto disponível",
                                   DateTime.Now,
                                   link: enderecoURl);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
