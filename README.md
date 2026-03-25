## 🌟 Destaque

Este projeto faz parte da minha jornada de reciclagem profissional em C#/.NET.

🔗 [Ver projeto anterior: FinanceiroPessoal](https://github.com/alexandreo80/FinanceiroPessoal)

---

# Projeto MiniBiblioteca - Sistema de Gerenciamento de Empréstimos

[![.NET](https://img.shields.io/badge/.NET-9.0-purple?logo=dotnet)](https://dotnet.microsoft.com)
[![C#](https://img.shields.io/badge/C%23-12-blue?logo=csharp)](https://learn.microsoft.com/pt-br/dotnet/csharp)
[![SOLID](https://img.shields.io/badge/SOLID-✅-green)](https://en.wikpedia.org/wiki/SOLID)
[![LICENSE](https://img.shields.io/badge/license-MIT-blue)](LICENSE)

Sistema de gerenciamento de empréstimos de livros desenvolvido para **praticar arquitetura limpa, princípios SOLID e Design Patterns** em C# .NET 9.

🎯 **Propósito** Projeto de estudo focado em qualidade de código, não em funcionalidades completas.

---

## 🚀 Funcionalidades

| Funcionalidade | Status |
| :--- | :--- |
| Cadastrar Livro | ✅ Concluído |
| Cadastrar Usuário | ✅ Concluído |
| Emprestar Livro (com validações) | ⏳ Em andamento |
| Devolver Livro (com cálculo de multa) | ⏳ Em andamento |
| Listar Empréstimos por Usuário | ✅ Concluído |
| Persistência em JSON | ✅ Concluído |
| Testes Unitários | ⏳ Em andamento |

---

## 🏗️ Arquitetura do Projeto

```text
MiniBiblioteca/
├── MiniBiblioteca.Core/ # Regra de negócio e domínio
│ ├── Domain/ # Entidades (Livro, Usuario, Emprestimo)
│ ├── Interfaces/ # Contratos (Repositórios)
│ ├── Services/ # Regras de negócio (EmprestimoService)
│ └── Common/ # Utilitários (Result<T>, etc.)
├── MiniBiblioteca.Infrastructure/ # Implementações (Repositórios JSON)
├── MiniBiblioteca.Tests/ # Testes unitários (xUnit)
└── README.md
```

### Diagrama de Dependências

```text
┌─────────────────────────────────────────────────────────────┐
│ Program.cs │
│ (Composition Root) │
└───────────────────────┬─────────────────────────────────────┘
│ Injeta Dependências
▼
┌─────────────────────────────────────────────────────────────┐
│ EmprestimoService │
│ (Regra de Negócio - Não sabe persistência) │
│ │
│ ILivroRepositorio ← Interface │
│ IUsuarioRepositorio ← Interface │
│ IEmprestimoRepositorio ← Interface │
└───────────────────────┬─────────────────────────────────────┘
│ Implementa
▼
┌─────────────────────────────────────────────────────────────┐
│ RepositorioDeArquivo (Infrastructure) │
│ (Persistência em JSON - Detalhes técnicos) │
└─────────────────────────────────────────────────────────────┘
```

--- 

## 🎯 Princípios SOLID Aplicados

| Princípio | Como Foi Aplicado | Exemplo |
| :--- | :--- | :--- |
| **S** - Single Responsibility | Cada classe tem uma responsabilidade única | `EmprestimoService` só cuida de empréstimos |
| **O** - Open/Closed | Extensível sem modificação | Novo repositório (ex: SQL) sem mudar Service |
| **L** - Liskov Substitution | Subclasses substituíveis sem quebrar | `RepositorioJson` e `RepositorioSql` trocáveis |
| **I** - Interface Segregation | Interfaces específicas por cliente | `ILivroRepositorio`, `IUsuarioRepositorio` separados |
| **D** - Dependency Inversion | Dependência de abstrações | Service depende de interfaces, não de classes concretas |

---

## 🎨 Design Patterns Utilizados

| Pattern | Onde | Por Que |
| :--- | :--- | :--- |
| **Repository** | `IRepositorio` + implementações | Abstrair acesso a dados |
| **Dependency Injection** | Construtor do `EmprestimoService` | Desacoplar dependências |
| **Result Object** | `Common/Result<T>` | Evitar exceções em fluxos esperados |
| **Domain Model** | `Domain/` | Entidades com lógica de negócio |

---

## 🛠️ Tecnologias

| Tecnologia | Versão | Finalidade |
| :--- | :--- | :--- |
| .NET | 9.0 | Framework principal |
| C# | 12 | Linguagem (Records, Pattern Matching) |
| xUnit | 2.8+ | Testes unitários |
| System.Text.Json | Nativo | Serialização JSON |
| Git | - | Versionamento |

---

## 🚀 Como Rodar o Projeto

### Pré-requisitos
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Git instalado

### Passos

```bash
# 1. Clonar repositório
git clone https://github.com/alexandreo80/MiniBiblioteca.git
cd MiniBiblioteca

# 2. Restaurar pacotes
dotnet restore

# 3. Rodar o projeto
cd MiniBiblioteca.Core
dotnet run
```
---

## 🧪 Exemplo de Uso

```csharp
var service = new EmprestimoService(livroRepo, usuarioRepo, emprestimoRepo);
var resultado = service.Emprestar("978-0-123456-78-9", "USR001");

if (resultado.Sucesso)
    Console.WriteLine("✅ Empréstimo realizado!");
else
    Console.WriteLine($"❌ Erro: {resultado.Erro}");
```

---

### 2. Roadmap Visual

## 🗺️ Roadmap

- [x] Semana 1: Estrutura SOLID
- [x] Semana 2: Persistência JSON
- [ ] Semana 3: Validações + Devolução com multa
- [ ] Semana 4: Testes unitários (xUnit)

---

## 👨‍💻 Autor

| | |
| :--- | :--- |
| **Nome** | Alexandre de Oliveira |
| **GitHub** | [@alexandreo80](https://github.com/alexandreo80) |
| **LinkedIn** | [Seu Perfil](https://linkedin.com/in/seu-perfil) |