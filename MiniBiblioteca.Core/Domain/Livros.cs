namespace MiniBiblioteca.Core.Domain;

public record Livro
(
	string Isbn,
	string Titulo,
	string Autor,
	int AnoLancamento
);