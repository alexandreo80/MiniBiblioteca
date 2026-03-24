namespace MiniBiblioteca.Core.Domain;

public record Emprestimo
(
    string Id,
    string LivroIsbn,
    string UsuarioId,
    DateTime DataEmprestimo,
    DateTime? DataDevolucao,
    int PrazoDias =7
)

{
    // Método de domínio: calcula multa
    public decimal CalcularMulta(decimal valorPorDia = 2.0m)
    {
        if (DataDevolucao == null) return 0m;

        var diasAtraso = (DataDevolucao.Value - DataEmprestimo).Days - PrazoDias;
        return diasAtraso > 0 ? diasAtraso * valorPorDia : 0m;
    }
};