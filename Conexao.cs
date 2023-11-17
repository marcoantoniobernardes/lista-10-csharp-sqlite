using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

public class Conexao
{
    private SQLiteConnection connection;

    // Correção do construtor
    public Conexao(string connectionString)
    {
        connection = new SQLiteConnection(connectionString);
    }

    public void AbrirConexao()
    {
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }
    }

    public SQLiteConnection GetConnection()
    {
        return connection;
    }

    public void FecharBanco()
    {
        if (connection != null)
        {
            connection.Close();
            connection.Dispose();
        }
    }
}