using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        string connectionString = "Data Source=DB_BANK_PAIVA.db;Version=3;";
        Conexao co = new Conexao(connectionString);
        co.AbrirConexao();

        while (true)
        {
            Console.WriteLine("Escolha uma opção:");
            Console.WriteLine("1. Criar conta");
            Console.WriteLine("2. Depositar dinheiro");
            Console.WriteLine("3. Sacar dinheiro");
            Console.WriteLine("4. Verificar saldo");
            Console.WriteLine("5. Listar contas");
            Console.WriteLine("6. Sair");
            ContaBancaria.Iniciar(co.GetConnection());

            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    ContaBancaria.CriarNovaConta(co.GetConnection());
                    break;

                case "2":
                    Console.WriteLine("Digite o número da conta para depositar:");
                    int numeroDeposito = int.Parse(Console.ReadLine());
                    ContaBancaria.Depositar(numeroDeposito, co.GetConnection());
                    break;

                case "3":
                    Console.WriteLine("Digite o número da conta para sacar:");
                    int numeroSaque = int.Parse(Console.ReadLine());
                    ContaBancaria.Sacar(numeroSaque);
                    break;

                case "4":
                    Console.WriteLine("Digite o número da conta para verificar o saldo:");
                    int numeroConsulta = int.Parse(Console.ReadLine());
                    ContaBancaria.ConsultarSaldo(numeroConsulta);
                    break;

                case "5":
                    ContaBancaria.Exibir(co.GetConnection());
                    ContaBancaria.ListarContas();
                    break;

                case "6":
                    Console.WriteLine("Saindo ...\n");
                    co.FecharBanco();
                    return;

                default:
                    Console.WriteLine("Opção inválida. Tente novamente.\n");
                    break;
            }
        }
    }
}