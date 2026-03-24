using System.Text.Json;
using MiniBiblioteca.Core.Domain;
using MiniBiblioteca.Core.Interfaces;

namespace MiniBiblioteca.Core.Infrastructure;

public class LivroRepositorioJson : ILivroRepositorio
{
    private readonly string _caminhoArquivo = "Data/livros.json";
    private List<Livro> _livros;

    public LivroRepositorioJson()
    {
        _livros = CarregarDoArquivo();
    }

    public void Adicionar(Livro livro)
    {
        _livros.Add(livro);
        SalvarNoArquivo();
    }

    public Livro? BuscarPorIsbn(string isbn)
    {
        return _livros.FirstOrDefault(l => l.Isbn == isbn);
    }

    public List<Livro> ListarTodos()
    {
        return _livros;
    }

    private List<Livro> CarregarDoArquivo()
    {
        if (!File.Exists(_caminhoArquivo))
        {
            return new List<Livro>();
        }

        var json = File.ReadAllText(_caminhoArquivo);
        return JsonSerializer.Deserialize<List<Livro>>(json) ?? new List<Livro>();
    }

    private void SalvarNoArquivo()
    {
        // Garantir que a pasta Data existe
        Directory.CreateDirectory("Data");

        var json = JsonSerializer.Serialize(_livros, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(_caminhoArquivo, json);
    }
}