using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using MiniBiblioteca.Core.Domain;
using MiniBiblioteca.Core.Interfaces;

namespace MiniBiblioteca.Core.Infrastructure;

public class UsuarioRepositorioJson : IUsuarioRepositorio
{
    private readonly string _caminhoArquivo = "Data/usuarios.json";
    private List<Usuario> _usuarios;

    public UsuarioRepositorioJson()
    {
        _usuarios = CarregarDoArquivo();
    }

    public void Adicionar(Usuario usuario)
    {
        _usuarios.Add(usuario);
        SalvarNoArquivo();
    }

    public Usuario? BuscarPorId(string id)
    {
        return _usuarios.FirstOrDefault(u => u.Id == id);
    }

    public List<Usuario> ListarTodos()
    {
        return _usuarios;
    }

    private List<Usuario> CarregarDoArquivo()
    {
        if (!File.Exists(_caminhoArquivo))
        {
            return new List<Usuario>();
        }

        var json = File.ReadAllText(_caminhoArquivo);
        return JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
    }

    private void SalvarNoArquivo()
    {
        Directory.CreateDirectory("Data");

        var json = JsonSerializer.Serialize(_usuarios, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(_caminhoArquivo, json);
    }
}