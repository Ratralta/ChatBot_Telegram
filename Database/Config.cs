using System.Data; 
using System.Data.SQLite; // ADO.NET namespace

namespace ChatBotTelegram.Database;


public class Config
{
public SQLiteConnection? conn {get;set;}
private Dictionary<String,String[]> _tables_dicionario = new Dictionary<string, String[]> // dicionario de tabelas presentes no DB
{
    {"sabores",["id","sabor"]}
}; 

    public Config(String data_base_localizacao)
    {
        try
        {
        if(!File.Exists(data_base_localizacao)) throw new Exception("Não existe arquivo : " + data_base_localizacao); // se db não existir 
        this.conn = new SQLiteConnection("Data Source=" + data_base_localizacao); // definindo parâmetros para a conextão com um banco
        conn.Open();
        }
        catch(Exception e)
        {
        Console.WriteLine("ERRO CONFIG :" + e);
        }
    }


    public void printTable(String tabela_key)
    {
    if(conn == null) return;
    if(!_tables_dicionario.ContainsKey(tabela_key)) {Console.WriteLine($"Não existe essa tabela_key ({tabela_key})"); return;}  // se não tiver essa key


    String[] table_tuplas = _tables_dicionario.GetValueOrDefault(tabela_key,[]); // tabela especifica que será printada 

    String sql_comando = $"""
    SELECT * FROM {tabela_key};
    """;

    SQLiteCommand query = conn.CreateCommand();
    query.CommandText = sql_comando;
    SQLiteDataReader query_result = query.ExecuteReader();
    
    Console.WriteLine("Tabela : " + tabela_key);
        while(query_result.Read())
        {  
            for(int i=0;i<table_tuplas.Length;i++)
            {
            Console.Write($"{table_tuplas[i]} : {query_result[table_tuplas[i]]} |");
            }
        Console.Write("\n");
        }
    }

    public String[] getSpecificTupla(String tabela_key,String tupla_name)
    {
    if(conn == null) return [];
    if(!_tables_dicionario.ContainsKey(tabela_key)) {Console.WriteLine($"Não existe essa tabela_key ({tabela_key})"); return [];}  // se não tiver essa key

    String[] table_tuplas = _tables_dicionario.GetValueOrDefault(tabela_key,[]); // tabela especifica que será printada 
    List<String> table_tuplas_values = new List<string>(); 

    String sql_comando = $"""
    SELECT * FROM {tabela_key};
    """;

    SQLiteCommand query = conn.CreateCommand();
    query.CommandText = sql_comando;
    SQLiteDataReader query_result = query.ExecuteReader();

        while(query_result.Read())
        {
        table_tuplas_values.Add(query_result.GetValue(tupla_name).ToString());    
        }
    return table_tuplas_values.ToArray();
    }
}