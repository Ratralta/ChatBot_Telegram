using System.Data; 
using System.Data.SQLite; // ADO.NET namespace


namespace ChatBotTelegram.DataBase;
public class Config
{
public SQLiteConnection? conn;
    public Config(String data_base_localizacao)
    {
        try
        {
        if(!File.Exists(data_base_localizacao)) throw new Exception("Não existe banco de dados " + data_base_localizacao); // se db não existir 

        this.conn = new SQLiteConnection("Data Source=" + data_base_localizacao); // definindo parâmetros para a conextão com um banco
        conn.Open();
        }
        catch(Exception e)
        {
        Console.WriteLine("ERRO CONFIG :" + e);
        }
    }
}