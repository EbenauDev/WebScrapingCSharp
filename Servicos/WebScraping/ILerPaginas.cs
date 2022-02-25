using Servicos.Generics;
using System.Threading.Tasks;

namespace Servicos
{
    public interface ILerPaginas
    {
        Task<Resultado<Produto, Falha>> LerPaginasAsync(string enderecoURl);
    }
}
