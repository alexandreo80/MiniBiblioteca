using System.Text.Json;
using MiniBiblioteca.Core.Domain;
using MiniBiblioteca.Core.Interfaces;

namespace MiniBiblioteca.Core.Infrastructure;

public class EmprestimoRepositorioJson : IEmprestimoRepositorio
{
    private readonly string _caminhoArquivo = "Data/emprestimos.json";
    private List<Emprestimo> _emprestimos;

    public EmprestimoRepositorioJson()
    {
        _emprestimos = CarregarDoArquivo();
    }

    public void Adicionar(Emprestimo emprestimo)
    {
        _emprestimos.Add(emprestimo);
        SalvarNoArquivo();
    }

    public Emprestimo? BuscarPorId(string id)
    {
        return _emprestimos.FirstOrDefault(e => e.Id == id);
    }

    public void Remover(string id)
    {
        var emprestimo = _emprestimos.FirstOrDefault(e => e.Id == id);
        if (emprestimo is not null)
        {
            _emprestimos.Remove(emprestimo);
            SalvarNoArquivo();
        }
    }

    public List<Emprestimo> ListarPorUsuario(string usuarioId)
    {
        return _emprestimos.Where(e => e.UsuarioId == usuarioId).ToList();
    }

    public List<Emprestimo> ListarEmprestimosAtivos()
    {
        return _emprestimos.Where(e => e.DataDevolucao == null).ToList();
    }

    private List<Emprestimo> CarregarDoArquivo()
    {
        if (!File.Exists(_caminhoArquivo))
        {
            return new List<Emprestimo>();
        }

        var json = File.ReadAllText(_caminhoArquivo);
        return JsonSerializer.Deserialize<List<Emprestimo>>(json) ?? new List<Emprestimo>();
    }

    private void SalvarNoArquivo()
    {
        Directory.CreateDirectory("Data");

        var json = JsonSerializer.Serialize(_emprestimos, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(_caminhoArquivo, json);
    }
}