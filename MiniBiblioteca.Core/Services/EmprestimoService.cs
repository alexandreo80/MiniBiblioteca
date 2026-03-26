using MiniBiblioteca.Core.Domain;
using MiniBiblioteca.Core.Interfaces;
using MiniBiblioteca.Core.Common;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;

namespace MiniBiblioteca.Core.Services;

public class EmprestimoService
{
    private readonly ILivroRepositorio _livroRepo;
    private readonly IUsuarioRepositorio _usuarioRepo;
    private readonly IEmprestimoRepositorio _emprestimoRepo;

    public EmprestimoService(ILivroRepositorio livroRepo, IUsuarioRepositorio usuarioRepo, IEmprestimoRepositorio emprestimoRepo)
    {
        _livroRepo = livroRepo;
        _usuarioRepo = usuarioRepo;
        _emprestimoRepo = emprestimoRepo;
    }

    public ResultadoOperacao<Emprestimo> Emprestar(string isbn, string usuarioId)
    {
        // 1. Validar se livro existe
        var livro = _livroRepo.BuscarPorIsbn(isbn);
        if (livro is null)
            return ResultadoOperacao<Emprestimo>.CriarErro("Livro não encontrado.");

        // 2. Validar se usuário existe
        var usuario = _usuarioRepo.BuscarPorId(usuarioId);
        if (usuario is null)
            return ResultadoOperacao<Emprestimo>.CriarErro("Usuário não encontrado.");

        // 3. Validar se livro já está emprestado
        var emprestimosAtivos = _emprestimoRepo.ListarEmprestimosAtivos();
        if (emprestimosAtivos.Any(e => e.LivroIsbn == isbn))
            return ResultadoOperacao<Emprestimo>.CriarErro("Livro já está emprestado.");

        // 4. Criar empréstimo e salvar o empréstimo
        var emprestimo = new Emprestimo
        (
            Id: Guid.NewGuid().ToString(),
            LivroIsbn: isbn,
            UsuarioId: usuarioId,
            DataEmprestimo: DateTime.Now,
            DataDevolucao: null // Ainda não foi devolvido
        );

        _emprestimoRepo.Adicionar(emprestimo);
        return ResultadoOperacao<Emprestimo>.CriarSucesso(emprestimo);
    }

    public ResultadoOperacao<decimal> Devolver(string emprestimoId, DateTime? dataDevolucao = null)
    {
        // 1. Buscar o empréstimo
        var emprestimo = _emprestimoRepo.BuscarPorId(emprestimoId);
        if (emprestimo is null)
        {
            return ResultadoOperacao<decimal>.CriarErro("Empréstimo não encontrado.");
        }

        // 2. Validar se já foi devolvido
        if (emprestimo.DataDevolucao.HasValue)
        {
            return ResultadoOperacao<decimal>.CriarErro("Este livro já foi devolvido.");
        }

        // 3. Marcar data de devolução
        var dataDevolucaoEfetiva = dataDevolucao ?? DateTime.Now;
        var emprestimoAtualizado = emprestimo with { DataDevolucao = dataDevolucaoEfetiva };

        // 4. Calcular multa (se houver atraso)
        var multa = CalcularMulta(emprestimoAtualizado);

        // 5. Atualizar no repositório (precisa de método Update ou remover/adicionar)
        // Simplificação: remover o antigo e adicionar o resultado
        // (Em produção, implementar método Update na interface)
        _emprestimoRepo.Remover(emprestimoId);  // Método precisa ser criado
        _emprestimoRepo.Adicionar(emprestimoAtualizado);

        return ResultadoOperacao<decimal>.CriarSucesso(multa);
    }

    private decimal CalcularMulta(Emprestimo emprestimo)
    {
        if (!emprestimo.DataDevolucao.HasValue)
            return 0;

        var diasAtraso = (emprestimo.DataDevolucao.Value.Date - emprestimo.DataEmprestimo.Date).Days - emprestimo.PrazoDias;

        if (diasAtraso <= 0)
            return 0;

        const decimal valorPordia = 2.0m;
        return diasAtraso * valorPordia;
    }
}