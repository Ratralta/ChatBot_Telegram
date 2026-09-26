using System;

namespace ChatBotTelegram.database;


/// <summary>
/// simboliza uma "Tabela" do "Banco de Dados"  
/// </summary>
public class Table
{
    public String[] tuplas;
    public String name;

    public Table(String name,String[] tuplas)
    {
    this.tuplas = tuplas;
    this.name = name;
    }
}
