using Servicos.Configuracao;

namespace Servicos.WebScraping
{
    public interface IWebScrapingStrategy
    {
        ILerPaginas DefinirWebScraping(EWebSite webSite);
    }

    public class WebScrapingStrategy : IWebScrapingStrategy
    {
        public ILerPaginas DefinirWebScraping(EWebSite webSite)
        {
            if (EWebSite.Kabum == webSite)
                return new KabumWebScraping();
            if (EWebSite.Pichau == webSite)
                return new PichauWebScraping();
            return null;
        }
    }
}
