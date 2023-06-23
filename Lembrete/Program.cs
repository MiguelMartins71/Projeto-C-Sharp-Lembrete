using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaLembretes
{
    class Program
    {
        static List<Lembrete> listaLembretes = new List<Lembrete>();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("== Sistema de Lembretes ==");
                Console.WriteLine("1. Adicionar lembrete");
                Console.WriteLine("2. Deletar lembrete");
                Console.WriteLine("3. Mostrar lembretes");
                Console.WriteLine("4. Sair");
                Console.Write("Escolha uma opção: ");
                string opcao = Console.ReadLine();

                Console.Clear();
                switch (opcao)
                {
                    case "1":
                        AdicionarLembrete();
                        break;
                    case "2":
                        DeletarLembrete();
                        break;
                    case "3":
                        MostrarLembretes();
                        break;
                    case "4":
                        Console.WriteLine("Encerrando o programa...");
                        return;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void AdicionarLembrete()
        {
            Console.WriteLine("== Adicionar Lembrete ==");

            Console.Write("Nome do lembrete: ");
            string nome = Console.ReadLine();

            DateTime data;
            while (true)
            {
                Console.Write("Data do lembrete (dd/mm/aaaa): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out data))
                {
                    if (data.Date >= DateTime.Today)
                        break;

                    Console.WriteLine("A data deve ser no futuro. Tente novamente.");
                }
                else
                {
                    Console.WriteLine("Data inválida. Tente novamente.");
                }
            }

            Lembrete novoLembrete = new Lembrete(nome, data);
            AdicionarLembreteNaLista(novoLembrete);

            Console.WriteLine("Lembrete adicionado com sucesso!");
        }

        static void AdicionarLembreteNaLista(Lembrete lembrete)
        {
            Lembrete lembreteExistente = listaLembretes.FirstOrDefault(l => l.Data.Date == lembrete.Data.Date);

            if (lembreteExistente != null)
            {
                lembreteExistente.LembretesDoDia.Add(lembrete);
            }
            else
            {
                Lembrete novoDia = new Lembrete("", lembrete.Data.Date);
                novoDia.LembretesDoDia.Add(lembrete);
                listaLembretes.Add(novoDia);
                listaLembretes = listaLembretes.OrderBy(l => l.Data).ToList();
            }
        }

        static void DeletarLembrete()
        {
            Console.WriteLine("== Deletar Lembrete ==");

            Console.Write("Digite o índice do dia (começando de 0): ");
            if (int.TryParse(Console.ReadLine(), out int indiceDia))
            {
                if (indiceDia >= 0 && indiceDia < listaLembretes.Count)
                {
                    Lembrete dia = listaLembretes[indiceDia];
                    MostrarLembretesDoDia(dia);

                    Console.Write("Digite o índice do lembrete a ser deletado (começando de 0): ");
                    if (int.TryParse(Console.ReadLine(), out int indiceLembrete))
                    {
                        if (indiceLembrete >= 0 && indiceLembrete < dia.LembretesDoDia.Count)
                        {
                            dia.LembretesDoDia.RemoveAt(indiceLembrete);
                            if (dia.LembretesDoDia.Count == 0)
                                listaLembretes.RemoveAt(indiceDia);
                        }
                        else
                        {
                            Console.WriteLine("Índice de lembrete inválido.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Índice de lembrete inválido.");
                    }
                }
                else
                {
                    Console.WriteLine("Índice de dia inválido.");
                }
            }
            else
            {
                Console.WriteLine("Índice de dia inválido.");
            }
        }

        static void MostrarLembretes()
        {
            Console.WriteLine("== Lista de Lembretes ==");

            for (int i = 0; i < listaLembretes.Count; i++)
            {
                Lembrete dia = listaLembretes[i];
                Console.WriteLine($"Dia {i}: {dia.Data.ToShortDateString()}");
                MostrarLembretesDoDia(dia);
                Console.WriteLine();
            }
        }

        static void MostrarLembretesDoDia(Lembrete dia)
        {
            if (dia.LembretesDoDia.Count > 0)
            {
                for (int i = 0; i < dia.LembretesDoDia.Count; i++)
                {
                    Lembrete lembrete = dia.LembretesDoDia[i];
                    Console.WriteLine($"[{i}] {lembrete.Nome}");
                }
            }
            else
            {
                Console.WriteLine("Não há lembretes para este dia.");
            }
        }
    }

    class Lembrete
    {
        public string Nome { get; set; }
        public DateTime Data { get; set; }
        public List<Lembrete> LembretesDoDia { get; set; }

        public Lembrete(string nome, DateTime data)
        {
            Nome = nome;
            Data = data;
            LembretesDoDia = new List<Lembrete>();
        }
    }
}
