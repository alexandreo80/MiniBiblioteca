using Xunit;
using Moq;
using MiniBiblioteca.Core.Domain;
using MiniBiblioteca.Core.Interfaces;
using MiniBiblioteca.Core.Services;
using System.Reflection;

namespace MiniBiblioteca.Tests.Services;

public class EmprestimoServiceTests
{
    private readonly Mock<ILivroRepositorio> _livroRepoMock;
    private readonly Mock<IUsuarioRepositorio> _usuarioRepoMock;
    private readonly Mock<IEmprestimoRepositorio> _emprestimoRepoMock;
    private EmprestimoService _service;

    public EmprestimoServiceTests()
    {
        _livroRepoMock = new Mock<ILivroRepositorio>();
        _usuarioRepoMock = new Mock<IUsuarioRepositorio>();
        _emprestimoRepoMock = new Mock<IEmprestimoRepositorio>();

        _service = new EmprestimoService(
            _livroRepoMock.Object,
            _usuarioRepoMock.Object,
            _emprestimoRepoMock.Object
        );
    }

    [Fact]
    public void Emprestar_LivroNaoExistente_RetornaErro()
    {
        // Arrange
        _livroRepoMock.Setup(r => r.BuscarPorIsbn("ISBN-INVALIDO"))
                        .Returns((Livro)null);
    
        // Act
        var resultado = _service.Emprestar("ISBN-INVALIDO", "USR00'");
    
        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Equal("Livro não encontrado.", resultado.Erro);
    }

    [Fact]
    public void Emprestar_UsuarioNaoExistente_RetornaErro()
    {
        // Arrange
        _livroRepoMock.Setup(u => u.BuscarPorIsbn("ISBN-123"))
                        .Returns(new Livro("ISBN-123", "Título", "Autor", 2020));
        _usuarioRepoMock.Setup(r => r.BuscarPorId("USR-INVALIDO"))
                        .Returns((Usuario)null);
    
        // Act
        var resultado = _service.Emprestar("ISBN-123", "USR-INVALIDO");
    
        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Equal("Usuário não encontrado.", resultado.Erro);
    }

    [Fact]
    public void Emprestar_LivroJaEmprestado_RetornaErro()
    {
        // Arrange
        var livro = new Livro("ISBN-123", "Título", "Autor", 2020);
        var usuario = new Usuario("USR001", "Nome", "email@test.com", "12345678900");

        _livroRepoMock.Setup(r => r.BuscarPorIsbn("ISBN-123")).Returns(livro);
        _usuarioRepoMock.Setup(r => r.BuscarPorId("USR001")).Returns(usuario);
        _emprestimoRepoMock.Setup(r => r.ListarEmprestimosAtivos())
                            .Returns(new List<Emprestimo>
                            {
                                new Emprestimo("EMP001", "ISBN-123", "USR001",
                                            DateTime.Now, null, 7)
                            });
    
        // Act
        var resultado = _service.Emprestar("ISBN-123", "USR001");
    
        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Equal("Livro já está emprestado.", resultado.Erro);
    }

    [Fact]
    public void Emprestar_TudoValido_RetornaSucesso()
    {
        // Arrange
        var livro = new Livro("ISBN-123", "Titulo", "Autor", 2020);
        var usuario = new Usuario("USR001", "Nome", "email@test.com", "12345678900");
        
        _livroRepoMock.Setup(r => r.BuscarPorIsbn("ISBN-123")).Returns(livro);
        _usuarioRepoMock.Setup(r => r.BuscarPorId("USR001")).Returns(usuario);
        _emprestimoRepoMock.Setup(r => r.ListarEmprestimosAtivos())
                           .Returns(new List<Emprestimo>());
        
        // Act
        var resultado = _service.Emprestar("ISBN-123", "USR001");
        
        // Assert
        Assert.True(resultado.Sucesso);
        Assert.NotNull(resultado.Dados);
        Assert.Equal("ISBN-123", resultado.Dados.LivroIsbn);
    }

    [Fact]
    public void Devolver_EmprestimoNaoExistente_RetornaErro()
    {
        // Arrange
        _emprestimoRepoMock.Setup(r => r.BuscarPorId("EMP-INVALIDO"))
                           .Returns((Emprestimo?)null);
        
        // Act
        var resultado = _service.Devolver("EMP-INVALIDO");
        
        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Equal("Empréstimo não encontrado.", resultado.Erro);
    }

    [Fact]
    public void Devolver_JaDevolvido_RetornaErro()
    {
        // Arrange
        var emprestimo = new Emprestimo(
            "EMP001", "ISBN-123", "USR001", 
            DateTime.Now.AddDays(-10), 
            DateTime.Now.AddDays(-3), // Já devolvido
            7
        );
        
        _emprestimoRepoMock.Setup(r => r.BuscarPorId("EMP001"))
                           .Returns(emprestimo);
        
        // Act
        var resultado = _service.Devolver("EMP001");
        
        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Equal("Este livro já foi devolvido.", resultado.Erro);
    }

    [Fact]
    public void Devolver_SemAtraso_SemMulta()
    {
        // Arrange: emprestado em 01/03, prazo 7 dias, devolvido em 07/03
        var emprestimo = new Emprestimo(
            "EMP001", "ISBN-123", "USR001",
            new DateTime(2024, 3, 1),
            new DateTime(2024, 3, 7), // Dentro do prazo
            7
        );
        
        _emprestimoRepoMock.Setup(r => r.BuscarPorId("EMP001"))
                           .Returns(emprestimo);
        _emprestimoRepoMock.Setup(r => r.Remover("EMP001"));
        _emprestimoRepoMock.Setup(r => r.Adicionar(It.IsAny<Emprestimo>()));
        
        // Act
        var resultado = _service.Devolver("EMP001");
        
        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Equal(0m, resultado.Dados); // Sem multa
    }

    [Fact]
    public void Devolver_ComAtraso_CalculaMultaCorretamente()
    {
        var ano = DateTime.Now.Year;
        var mes = DateTime.Now.Month;
        var dia = DateTime.Now.Day - 9;

        // Arrange: emprestado em 01/03, prazo 7 dias, devolvido em 15/03 (2 dias atraso)
        var emprestimo = new Emprestimo(
            "EMP001", "ISBN-123", "USR001",
            new DateTime(ano, mes, dia),
            DataDevolucao: null, // Ainda não foi devolvido
            7
        );
        
        _emprestimoRepoMock.Setup(r => r.BuscarPorId("EMP001"))
                           .Returns(emprestimo);
        _emprestimoRepoMock.Setup(r => r.Remover("EMP001"));
        _emprestimoRepoMock.Setup(r => r.Adicionar(It.IsAny<Emprestimo>()));
        
        // Act
        var resultado = _service.Devolver("EMP001");
        
        // Assert
        Assert.True(resultado.Sucesso);
        Assert.Equal(4.0m, resultado.Dados); // 2 dias × R$ 2,00
    }
}