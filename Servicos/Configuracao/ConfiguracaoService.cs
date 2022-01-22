using System.Collections.Generic;

namespace Servicos.Configuracao
{
    public sealed class ConfiguracaoService
    {
        public ConfiguracaoService(IEnumerable<Parametro> parametros)
        {
            Parametros = parametros;
        }
        public IEnumerable<Parametro> Parametros { get; set; }
    }

    public sealed class Parametro
    {
        public Parametro(EWebSite site, string enderecoURL)
        {
            Site = site;
            EnderecoURL = enderecoURL;
        }
        public EWebSite Site { get; set; }
        public string EnderecoURL { get; set; }
    }

    public enum EWebSite
    {
        Pichau = 1,
        Kabum = 2
    }
}
