namespace Servicos.Generics
{
    public struct Resultado<TSucesso, TFalha>
    {
        internal Resultado(TSucesso sucesso)
        {
            EhFalha = false;
            Sucesso = sucesso;
            Falha = default;
        }

        internal Resultado(TFalha falha)
        {
            EhFalha = true;
            Sucesso = default;
            Falha = falha;
        }

        public TSucesso Sucesso { get; }
        public TFalha Falha { get; }
        public bool EhFalha { get; }
        public bool EhSucesso => !EhFalha;

        public static implicit operator Resultado<TSucesso, TFalha>(TFalha falha)
            => new Resultado<TSucesso, TFalha>(falha);

        public static implicit operator Resultado<TSucesso, TFalha>(TSucesso sucesso)
            => new Resultado<TSucesso, TFalha>(sucesso);
    }
}
