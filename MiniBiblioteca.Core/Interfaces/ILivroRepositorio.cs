using MiniBiblioteca.Core.Domain;

namespace MiniBiblioteca.Core.Interfaces;

public interface ILivroRepositorio
{
    void Adicionar(Livro livro);
    Livro? BuscarPorIsbn(string isbn);
    List<Livro> ListarTodos();
}