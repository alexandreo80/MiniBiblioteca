using MiniBiblioteca.Core.Domain;

namespace MiniBiblioteca.Core.Interfaces;

public interface IEmprestimoRepositorio
{
    void Adicionar(Emprestimo emprestimo);
    Emprestimo? BuscarPorId(string id);
    List<Emprestimo> ListarPorUsuario(string usuarioId);
    List<Emprestimo> ListarEmprestimosAtivos();
}