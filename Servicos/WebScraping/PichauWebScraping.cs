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
    public sealed class PichauWebScraping : ILerPaginas
    {
        public PichauWebScraping()
        {

        }

        public async Task<Produto> LerPaginasAsync(string enderecoURl)
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.Headers.Clear();
                    webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.71 Safari/537.36");
                    var paginaBaixada = await Task.FromResult(webClient.DownloadString(enderecoURl));

                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(paginaBaixada);
                    var tituloItem = htmlDocument.DocumentNode.Descendants("h1")
                                                    .Where(node => node.GetAttributeValue("data-cy", "")
                                                    .Contains("product-page-title"))
                                                    .FirstOrDefault()
                                                    .InnerText;
                    var produtoEstaEsgotado = htmlDocument.DocumentNode.Descendants("span")
                                        .Where(node => node.InnerText.Contains("Produto Indisponível"))
                                        .ToList();

                    if (produtoEstaEsgotado == null || produtoEstaEsgotado.Any())
                    {
                        return new Produto(tituloItem,
                                           valorAVista: "Esgotado",
                                           valorParcelado: "Esgotado",
                                           "Loja Pichau",
                                            disponibilidade: "Produto indisponível",
                                           dataDeConsulta: DateTime.Now,
                                           link: enderecoURl);
                    }
                    var valorAVista = htmlDocument.DocumentNode.Descendants("div")
                                                   .Where(node => node.InnerText.Contains("R$"))
                                                   .ToList()[9].InnerText;
                    var valorParcelado = htmlDocument.DocumentNode
                                                    .Descendants("div")
                                                    .Where(node => node.InnerText.Contains("R$"))
                                                    .ToList()[12].InnerText;
                    return new Produto(tituloItem,
                                       valorAVista,
                                       valorParcelado,
                                       loja: "Loja Pichau",
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
}
