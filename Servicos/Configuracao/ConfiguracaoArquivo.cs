using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Servicos.Configuracao
{

    public interface IConfiguracaoArquivo
    {
        Task<IEnumerable<Parametro>> LerConfiguracaoArquivoAsync(string caminhoArquivo);
    }


    public sealed class ConfiguracaoArquivo : IConfiguracaoArquivo
    {
        public async Task<IEnumerable<Parametro>> LerConfiguracaoArquivoAsync(string caminhoArquivo)
        {
            if (File.Exists(caminhoArquivo) == false)
                throw new Exception("Caminho do arquivo inválido ou não existe");
            var lista = new List<Parametro>();
            using (var reader = new StreamReader(File.OpenRead(caminhoArquivo)))
            {
                while (!reader.EndOfStream)
                {
                    var linha = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(linha) == false)
                    {
                        var configuracoes = linha.Split("=");
                        lista.Add(new Parametro(Enum.Parse<EWebSite>(configuracoes[0]), configuracoes[1]));
                    }
                }
            }
            return lista;
        }
    }
}