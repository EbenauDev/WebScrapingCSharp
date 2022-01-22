using System.Threading.Tasks;

namespace Servicos
{
    public interface ILerPaginas
    {
        Task<Produto> LerPaginasAsync(string enderecoURl);
    }
}
