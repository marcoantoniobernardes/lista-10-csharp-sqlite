using System.Data.Common;
using System.Data.SQLite;
class ContaBancaria
{
    private static HashSet<ContaBancaria> contas = new HashSet<ContaBancaria>();
    private static Random rand = new Random();
    private static SQLiteConnection connection;
    public int Numero { get; }
    public string Titular { get; }
    public float Saldo { get; private set; }

    private ContaBancaria(int numero, string titular, float saldo)
    {
        Numero = numero;
        Titular = titular;
        Saldo = saldo;
    }

    public static void CriarNovaConta(SQLiteConnection conexao)
    {
        Console.Write("Nome do Titular: ");
        string titular = Console.ReadLine();

        int numeroConta = rand.Next(1000, 2000);

        float saldo = 0;

        ContaBancaria novaConta = new ContaBancaria(numeroConta, titular, saldo);
        contas.Add(novaConta);

        string sql = "INSERT INTO tb_contas (id_conta, numero, titular, saldo) VALUES (@id, @numero, @titular, @saldo)";

        using (SQLiteCommand command = new SQLiteCommand(sql, conexao))
        {
            command.Parameters.AddWithValue("@id", novaConta.Numero);
            command.Parameters.AddWithValue("@numero", novaConta.Numero);
            command.Parameters.AddWithValue("@titular", novaConta.Titular);
            command.Parameters.AddWithValue("@saldo", novaConta.Saldo);
            command.ExecuteNonQuery();
        }

        Console.WriteLine($"\nNova conta criada!\nNúmero da Conta: {novaConta.Numero}\nTitular: {titular}");
    }

    public static void Depositar(int numeroConta, SQLiteConnection conexao)
    {
        ContaBancaria conta = ObterConta(numeroConta);
        if (conta != null)
        {
            Console.WriteLine("Valor do Depósito:");
            float valor = float.Parse(Console.ReadLine());
            conta.Saldo += valor;

            string sql = "UPDATE tb_contas SET saldo = @saldo WHERE numero = @numero";

            using (SQLiteCommand command = new SQLiteCommand(sql, conexao))
            {
                command.Parameters.AddWithValue("@saldo", conta.Saldo);
                command.Parameters.AddWithValue("@numero", conta.Numero);

                int rowsUpdated = command.ExecuteNonQuery();

                if (rowsUpdated > 0)
                {
                    Console.WriteLine($"Deposito foi efetuado");
                }
            }
        }
    }

    public static void Sacar(int numeroConta)
    {
        ContaBancaria conta = ObterConta(numeroConta);
        if (conta != null)
        {
            Console.WriteLine("Qual valor deseja sacar:");
            float valor = float.Parse(Console.ReadLine());
            if (valor > conta.Saldo)
            {
                Console.WriteLine($"Saldo insuficiente na conta {conta.Numero}. Operação de saque não realizada.");
            }
            else
            {
                conta.Saldo -= valor;
                Console.WriteLine($"Saque de {valor:C} realizado na conta {conta.Numero}. Novo saldo: {conta.Saldo:C}");
            }
        }
    }

    public static void ConsultarSaldo(int numeroConta)
    {
        ContaBancaria conta = ObterConta(numeroConta);
        if (conta != null)
        {
            Console.WriteLine($"Saldo na conta {conta.Numero} de {conta.Titular}: {conta.Saldo:C}");
        }
    }
    public static void ListarContas()
    {
        Console.WriteLine("\nLista de contas:");
        foreach (var conta in contas)
        {
            Console.WriteLine($"Número da Conta: {conta.Numero}, Titular: {conta.Titular}, Saldo: {conta.Saldo:C}");
        }
        Console.WriteLine();
    }

    private static ContaBancaria ObterConta(int numeroConta)
    {
        return contas.FirstOrDefault(c => c.Numero == numeroConta);
    }
    public static void Exibir(SQLiteConnection conexao)
    {

        string query = "SELECT numero, titular, saldo FROM tb_contas";

        using (SQLiteCommand command = new SQLiteCommand(query, conexao))
        {
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Itens no Estoque:");
                while (reader.Read())
                {
                    int numeroConta = reader.GetInt32(reader.GetOrdinal("numero"));
                    string titular = reader["titular"].ToString();
                    float saldo = reader.GetFloat(reader.GetOrdinal("saldo"));

                    Console.WriteLine($"Numero da Conta: {numeroConta}, Titular: {titular}, Saldo: {saldo}");
                }
            }
        }
    }
    public static void Iniciar(SQLiteConnection conexao)
    {

        string query = "SELECT numero, titular, saldo FROM tb_contas";

        List<ContaBancaria> novasContas = new List<ContaBancaria>();

        using (SQLiteCommand command = new SQLiteCommand(query, conexao))
        {
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int numeroConta = reader.GetInt32(reader.GetOrdinal("numero"));
                    string titular = reader["titular"].ToString();
                    float saldo = reader.GetFloat(reader.GetOrdinal("saldo"));

                    if (!contas.Any(c => c.Numero == numeroConta))
                    {
                        ContaBancaria novaConta = new ContaBancaria(numeroConta, titular, saldo);
                        novasContas.Add(novaConta);
                    }
                }
            }
        }
        contas = new HashSet<ContaBancaria>(contas.Union(novasContas));
    }
}