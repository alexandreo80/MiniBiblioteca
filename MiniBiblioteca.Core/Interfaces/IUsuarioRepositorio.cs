using MiniBiblioteca.Core.Domain;

namespace MiniBiblioteca.Core.Interfaces;

public interface IUsuarioRepositorio
{
    void Adicionar(Usuario usuario);
    Usuario? BuscarPorId(string id);
    List<Usuario> ListarTodos();

}