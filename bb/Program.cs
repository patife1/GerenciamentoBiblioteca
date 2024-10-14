using System;
using System.Collections.Generic;

public class Livro
{
	public string Titulo { get; set; }
	public string Autor { get; set; }
	public string Genero { get; set; }
	public int QuantidadeDisponivel { get; set; }

	public Livro(string titulo, string autor, string genero, int quantidade)
	{
		Titulo = titulo;
		Autor = autor;
		Genero = genero;
		QuantidadeDisponivel = quantidade;
	}
}

public class Usuario
{
	public string Nome { get; set; }
	public int Id { get; set; }
	public bool IsAdmin { get; set; }
	public List<Livro> LivrosEmprestados { get; set; }

	public Usuario(string nome, int id, bool isAdmin)
	{
		Nome = nome;
		Id = id;
		IsAdmin = isAdmin;
		LivrosEmprestados = new List<Livro>();
	}

	public bool PodeEmprestar()
	{
		return LivrosEmprestados.Count < 3;
	}
}

public class Biblioteca
{
	private List<Livro> livros = new List<Livro>();
	private List<Usuario> usuarios = new List<Usuario>();
	public void AdicionarLivro(string titulo, string autor, string genero, int quantidade)
	{
		var livro = new Livro(titulo, autor, genero, quantidade);
		livros.Add(livro);
		Console.WriteLine("Livro adicionado com sucesso!");
	}

	public void RegistrarUsuario(string nome, bool isAdmin)
	{
		int id = usuarios.Count + 1;
		var usuario = new Usuario(nome, id, isAdmin);
		usuarios.Add(usuario);
		Console.WriteLine("Usuário registrado com sucesso!");
	}

	public void EmprestarLivro(string titulo, Usuario usuario)
	{
		var livro = livros.Find(l => l.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase) && l.QuantidadeDisponivel > 0);
		if (livro != null && usuario.PodeEmprestar())
		{
			livro.QuantidadeDisponivel--;
			usuario.LivrosEmprestados.Add(livro);
			Console.WriteLine($"Livro '{livro.Titulo}' emprestado para {usuario.Nome}.");
		}
		else
		{
			Console.WriteLine("Não é possível emprestar o livro.");
		}
	}

	public void DevolverLivro(string titulo, Usuario usuario)
	{
		var livro = usuario.LivrosEmprestados.Find(l => l.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase));
		if (livro != null)
		{
			livro.QuantidadeDisponivel++;
			usuario.LivrosEmprestados.Remove(livro);
			Console.WriteLine($"Livro '{livro.Titulo}' devolvido com sucesso.");
		}
		else
		{
			Console.WriteLine("Livro não encontrado entre os livros emprestados.");
		}
	}

	public void ListarLivros()
	{
		foreach (var livro in livros)
		{
			Console.WriteLine($"Título: {livro.Titulo}, Autor: {livro.Autor}, Gênero: {livro.Genero}, Disponível: {livro.QuantidadeDisponivel}");
		}
	}
}

class Program
{
	static void Main(string[] args)
	{
		Biblioteca biblioteca = new Biblioteca();
		Usuario admin = new Usuario("Admin", 1, true);
		biblioteca.RegistrarUsuario(admin.Nome, true);
		Usuario usuario = new Usuario("Usuário", 2, false);
		biblioteca.RegistrarUsuario(usuario.Nome, false);
		string comando;
		do
		{
			Console.WriteLine("\nMenu:");
			Console.WriteLine("1 - Adicionar Livro (Admin)");
			Console.WriteLine("2 - Emprestar Livro");
			Console.WriteLine("3 - Devolver Livro");
			Console.WriteLine("4 - Listar Livros");
			Console.WriteLine("0 - Sair");
			Console.Write("Escolha um comando: ");
			comando = Console.ReadLine();
			switch (comando)
			{
				case "1":
					if (admin.IsAdmin)
					{
						Console.Write("Título: ");
						string titulo = Console.ReadLine();
						Console.Write("Autor: ");
						string autor = Console.ReadLine();
						Console.Write("Gênero: ");
						string genero = Console.ReadLine();
						Console.Write("Quantidade: ");
						int quantidade = int.Parse(Console.ReadLine());
						biblioteca.AdicionarLivro(titulo, autor, genero, quantidade);
					}
					else
					{
						Console.WriteLine("Apenas administradores podem adicionar livros.");
					}

					break;
				case "2":
					Console.Write("Título do Livro: ");
					string tituloEmprestimo = Console.ReadLine();
					biblioteca.EmprestarLivro(tituloEmprestimo, usuario);
					break;
				case "3":
					Console.Write("Título do Livro: ");
					string tituloDevolucao = Console.ReadLine();
					biblioteca.DevolverLivro(tituloDevolucao, usuario);
					break;
				case "4":
					biblioteca.ListarLivros();
					break;
				case "0":
					Console.WriteLine("Saindo...");
					break;
				default:
					Console.WriteLine("Comando inválido.");
					break;
			}
		}
		while (comando != "0");
	}
}