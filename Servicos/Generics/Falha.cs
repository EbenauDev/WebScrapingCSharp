using System;

namespace Servicos.Generics
{
    public sealed class Falha
    {
        public Falha(string mensagem, string detalhes, Exception exception)
        {
            Mensagem = mensagem;
            Detalhes = detalhes;
            Exception = exception;
        }


        public Falha(string mensagem, string detalhes)
        {
            Mensagem = mensagem;
            Detalhes = detalhes;
        }

        public Falha(string mensagem, Exception exception)
        {
            Mensagem = mensagem;
            Exception = exception;
        }

        public Falha(string mensagem)
        {
            Mensagem = mensagem;
        }

        public static Falha Nova(string mensagem)
            => new Falha(mensagem);

        public static Falha NovaComDetalhes(string mensagem, string detalhes)
          => new Falha(mensagem, detalhes);


        public static Falha NovaComException(string mensagem, Exception exception)
            => new Falha(mensagem, exception);

        public string Mensagem { get; }
        public string Detalhes { get; set; }
        public Exception Exception { get; }
    }
}
