using System.Data; 
using System.Data.SQLite; // ADO.NET namespace

namespace ChatBotTelegram.database;


public class Config
{
public SQLiteConnection? conn {get;set;}
private Table[] _tables_array = // lista de tabelas presentes no DB 
{
new Table("sabores",["id","sabor"])
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


    public void printTable(int tabela_index,int[] tuplas_index_to_print)
    {
    if(conn == null) return;

    Table sql_table = _tables_array[tabela_index]; // tabela especifica que será printada 

    String sql_comando = $"""
    SELECT * FROM {sql_table.name};
    """;

    SQLiteCommand query = conn.CreateCommand();
    query.CommandText = sql_comando;
    SQLiteDataReader query_result = query.ExecuteReader();
    
    Console.WriteLine("Tabela : " + sql_table.name);
        while(query_result.Read())
        {  
            foreach(int i in tuplas_index_to_print)
            {
            Console.Write($"{sql_table.tuplas[i].ToString()} : {query_result[sql_table.tuplas[i]]}. ");
            }
        Console.Write("\n");
        }
    }
}