using MiniBiblioteca.Core.Domain;
using MiniBiblioteca.Core.Interfaces;
using MiniBiblioteca.Core.Infrastructure;
using MiniBiblioteca.Core.Services;
using System.Runtime.CompilerServices;

// Composition Root: criar todas as dependências
ILivroRepositorio livroRepo = new LivroRepositorioJson();
IUsuarioRepositorio usuarioRepo = new UsuarioRepositorioJson();
IEmprestimoRepositorio emprestimoRepo = new EmprestimoRepositorioJson();

var emprestimoService = new EmprestimoService(livroRepo, usuarioRepo, emprestimoRepo);

Console.Clear();
System.Console.WriteLine(AppContext.BaseDirectory);

System.Console.WriteLine("========== MiniBiblioteca ==========\n");

// Menu Principal
while (true)
{
    System.Console.WriteLine("\n1. Cadastrar Livro");
    System.Console.WriteLine("2. Cadastrar Usuário");
    System.Console.WriteLine("3. Emprestar Livro");
    System.Console.WriteLine("4. Listar Livros");
    System.Console.WriteLine("5. Listar Usuários");
    System.Console.WriteLine("6. Listar Empréstimos");
    System.Console.WriteLine("7. Sair\n");

    Console.Write("Escolha uma opção: ");
    var escolha = Console.ReadLine();

    switch(escolha)
    {
        case "1":
            Console.Write("\nISBN: ");
            var isbn = Console.ReadLine();
            Console.Write("Título: ");
            var titulo = Console.ReadLine();
            Console.Write("Autor: ");
            var autor = Console.ReadLine();
            Console.Write("Ano: ");
            var ano = int.Parse(Console.ReadLine());

            Livro livro = new Livro(isbn, titulo, autor, ano);
            livroRepo.Adicionar(livro);
            System.Console.WriteLine("Livro Cadastrado!");

            break;
        
        case "2":
            Console.Write("\nID: ");
            var id = Console.ReadLine();
            Console.Write("Nome: ");
            var nome = Console.ReadLine();
            Console.Write("Email: ");
            var email = Console.ReadLine();
            Console.Write("CPF: ");
            var cpf = Console.ReadLine();

            Usuario usuario = new Usuario(id, nome, email, cpf);
            usuarioRepo.Adicionar(usuario);
            System.Console.WriteLine("Usuário cadastrado!");

            break;

        case "3":
            Console.Write("\nISBN do Livro: ");
            var isbnEmprestimo = Console.ReadLine();
            Console.Write("ID do Usuário: ");
            var usuarioIdEmprestimo = Console.ReadLine();

            var resultado = emprestimoService.Emprestar(isbnEmprestimo, usuarioIdEmprestimo);
            if (resultado.Sucesso)
                System.Console.WriteLine("Emprestimo Realizado!");
            else
                System.Console.WriteLine($"{resultado.Erro}");

            break;

        case "4":
            var livros = livroRepo.ListarTodos();
            if (!livros.Any())
                System.Console.WriteLine("Nenhum livro cadastrado.");
            else
            {
                System.Console.Write($"ISBN | ");
                System.Console.Write($"Título | ");
                System.Console.Write($"Autor | ");
                System.Console.Write($"Ano | ");

                foreach(var l in livros)
                {
                    System.Console.WriteLine();
                    Console.Write($"{l.Isbn} | ");
                    Console.Write($"{l.Titulo} | ");
                    Console.Write($"{l.Autor} | ");
                    Console.Write($"{l.AnoLancamento} | ");
                }
            }

            break;

        case "5":
            var usuarios = usuarioRepo.ListarTodos();

            if (!usuarios.Any())
                System.Console.WriteLine("Nenhum usuário cadastrado.");
            else
            {
            foreach (var u in usuarios)
                {
                    System.Console.WriteLine($"{u.Id} | {u.Nome} | {u.Email}");
                }
            }

            break;

        case "6":
            var emprestimos = emprestimoRepo.ListarEmprestimosAtivos();
            if (!emprestimos.Any())
                System.Console.WriteLine("Nenhum empréstimo cadastrado.");
            else
            {
            foreach(var e in emprestimos)
                {
                    System.Console.WriteLine($"Livro: {e.LivroIsbn} | Usuário: {e.UsuarioId} | Data: {e.DataEmprestimo}");
                }
            }

            break;

        case "7":
            System.Console.WriteLine("Até logo!");
            return;
    }
}