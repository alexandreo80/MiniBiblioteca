using MiniBiblioteca.Core.Domain;
using MiniBiblioteca.Core.Interfaces;
using MiniBiblioteca.Core.Common;

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
        // Validar se livro existe
        var livro = _livroRepo.BuscarPorIsbn(isbn);
        if (livro == null)
            return ResultadoOperacao<Emprestimo>.CriarErro("Livro não encontrado");

        // Validar se usuário existe
        var usuario = _usuarioRepo.BuscarPorId(usuarioId);
        if (usuario == null)
            return ResultadoOperacao<Emprestimo>.CriarErro("Usuário não encontrado");

        // Validar se livro já está emprestado
        var emprestimosAtivos = _emprestimoRepo.ListarEmprestimosAtivos();
        if (emprestimosAtivos.Any(e => e.LivroIsbn == isbn))
            return ResultadoOperacao<Emprestimo>.CriarErro("Livro já está emprestado");

        // Criar empréstimo
        var emprestimo = new Emprestimo
        (
            Id: Guid.NewGuid().ToString(),
            LivroIsbn: isbn,
            UsuarioId: usuarioId,
            DataEmprestimo: DateTime.Now,
            DataDevolucao: null
        );

        _emprestimoRepo.Adicionar(emprestimo);
        return ResultadoOperacao<Emprestimo>.CriarSucesso(emprestimo);
    }
}