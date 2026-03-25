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
    System.Console.WriteLine("4. Devolver Livro");
    System.Console.WriteLine("5. Listar Livros");
    System.Console.WriteLine("6. Listar Usuários");
    System.Console.WriteLine("7. Listar Empréstimos");
    System.Console.WriteLine("8. Sair\n");

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
            System.Console.WriteLine("Usuário cadastrado!\n");

            break;

        case "3":
            Console.Write("\nISBN do Livro: ");
            var isbnEmprestimo = Console.ReadLine();
            Console.Write("ID do Usuário: ");
            var usuarioIdEmprestimo = Console.ReadLine();

            var resultado = emprestimoService.Emprestar(isbnEmprestimo, usuarioIdEmprestimo);
            if (resultado.Sucesso)
                System.Console.WriteLine("Emprestimo Realizado!\n");
            else
                System.Console.WriteLine($"{resultado.Erro}\n");

            break;

        case "4":
            Console.Write("ID do empréstimo: ");
            var emprestimoId = Console.ReadLine();

            var resultadoDevolucao = emprestimoService.Devolver(emprestimoId);
            if (resultadoDevolucao.Sucesso)
            {
                var multa = resultadoDevolucao.Dados;
                if (multa > 0)
                    System.Console.WriteLine($"Livro devolvido! Multa: {multa:F2}\n");
                else
                    System.Console.WriteLine("Livro devolvido no prazo! Sem  multa.\n");
            }
            else
            {
                System.Console.WriteLine($"Erro: {resultadoDevolucao.Erro}\n");
            }

            break;

        case "5":
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
            System.Console.WriteLine();

            break;

        case "6":
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

        case "7":
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

        case "8":
            System.Console.WriteLine("Até logo!");
            return;
    }
}