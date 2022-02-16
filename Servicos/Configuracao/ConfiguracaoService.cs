namespace Servicos.Configuracao
{

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
