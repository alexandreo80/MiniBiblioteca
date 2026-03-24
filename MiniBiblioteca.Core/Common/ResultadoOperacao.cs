namespace MiniBiblioteca.Core.Common;

public class ResultadoOperacao<T>
{
    public bool Sucesso { get; private set; }
    public string? Erro { get; private set; }
    public T? Dados { get; private set; }

    // ← Métodos renomeados para evitar conflito!
    public static ResultadoOperacao<T> CriarSucesso(T dados) => 
        new() { Sucesso = true, Dados = dados };
    
    public static ResultadoOperacao<T> CriarErro(string mensagem) => 
        new() { Sucesso = false, Erro = mensagem };
}