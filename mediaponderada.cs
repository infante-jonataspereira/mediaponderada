using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        List<Aluno> listaAlunos = new List<Aluno>();
        string continuar = "s";

        Console.WriteLine("=== SISTEMA DE GESTÃO DE NOTAS ===");

        while (continuar.ToLower() == "s")
        {
            Console.Write("\nNome do Aluno: ");
            string nome = Console.ReadLine();
            
            Aluno novoAluno = new Aluno(nome);
            novoAluno.InserirNotas();
            listaAlunos.Add(novoAluno);

            Console.Write("\nDeseja inserir outro aluno? (s/n): ");
            continuar = Console.ReadLine();
        }

        Aluno.MostrarTabela(listaAlunos);
    }
}

class Aluno
{
    public string Nome { get; private set; }
    public double[] Trabalhos { get; private set; } = new double[4];
    public double[] Testes { get; private set; } = new double[2];
    public double NotaCompetencias { get; private set; } // Agora é apenas uma nota única

    public Aluno(string nome)
    {
        Nome = nome;
    }

    public void InserirNotas()
    {
        Console.WriteLine($"\n--- Inserir notas para o aluno: {Nome} ---");

        // 1. Trabalhos Práticos (10% cada)
        for (int i = 0; i < 4; i++)
        {
            Trabalhos[i] = LerNotaValidada($"  Trabalho Prático {i + 1} (0 a 20): ");
        }

        // 2. Testes Individuais (25% cada)
        for (int i = 0; i < 2; i++)
        {
            Testes[i] = LerNotaValidada($"  Teste Individual {i + 1} (0 a 20): ");
        }

        // 3. Competências Transversais (Nota única que vale 10%)
        NotaCompetencias = LerNotaValidada("  Nota das Competências Transversais (0 a 20): ");
    }

    // Métodos auxiliares para calcular as médias aritméticas simples para a tabela
    public double ObterMediaTrabalhos()
    {
        double soma = 0;
        foreach (var nota in Trabalhos) soma += nota;
        return soma / 4;
    }

    public double ObterMediaTestes()
    {
        double soma = 0;
        foreach (var nota in Testes) soma += nota;
        return soma / 2;
    }

    // Cálculo final mantém os pesos estipulados
    public double CalcularMediaFinal()
    {
        double somaTrabalhos = 0;
        foreach (var nota in Trabalhos) somaTrabalhos += nota;

        double somaTestes = 0;
        foreach (var nota in Testes) somaTestes += nota;

        // Pesos: (SomaTrabalhos * 10%) + (SomaTestes * 25%) + (NotaCompetencias * 10%)
        return (somaTrabalhos * 0.10) + (somaTestes * 0.25) + (NotaCompetencias * 0.10);
    }

    public static void MostrarTabela(List<Aluno> alunos)
    {
        Console.WriteLine("\n=================================================================================");
        Console.WriteLine("|       ALUNO        | MÉD. TRAB (40%) | MÉD. TEST (50%) | COMP. (10%) |  MÉDIA  | ESTADO  |");
        Console.WriteLine("=================================================================================");

        foreach (var aluno in alunos)
        {
            double mediaTrab = aluno.ObterMediaTrabalhos();
            double mediaTest = aluno.ObterMediaTestes();
            double notaComp = aluno.NotaCompetencias; // Mostra a nota direta, sem médias extras
            double mediaFinal = aluno.CalcularMediaFinal();
            
            string estado = mediaFinal >= 9.5 ? "APROVADO" : "REPROV.";

            Console.WriteLine($"| {aluno.Nome,-18} | {mediaTrab,15:F1} | {mediaTest,15:F1} | {notaComp,11:F1} | {mediaFinal,7:F2} | {estado,-7} |");
        }

        Console.WriteLine("=================================================================================");
    }

    private double LerNotaValidada(string mensagem)
    {
        double nota;
        while (true)
        {
            Console.Write(mensagem);
            string entrada = Console.ReadLine();
            
            if (double.TryParse(entrada, out nota) && nota >= 0 && nota <= 20)
            {
                return nota;
            }
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("    [ERRO] Nota inválida! O valor deve ser obrigatoriamente entre 0.0 e 20.0.");
            Console.ResetColor();
        }
    }
}
