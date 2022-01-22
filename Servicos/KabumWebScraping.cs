using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
                var pagina = await Task.FromResult(webClient.DownloadString(enderecoURl));
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
