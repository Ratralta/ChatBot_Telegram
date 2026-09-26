using System;

namespace ChatBotTelegram.Bot;

public class Dicionario_Bot
{
public Dictionary<int,String> saudacoes;
public Dictionary<String,String> sabores;
    public Dicionario_Bot(String[] sabores)
    {
    this.saudacoes = new Dictionary<int, string>()
    {
        {0,"ola"},
        {1,"oi"},
        {2,"gostaria"}
    };
    this.sabores = new Dictionary<string, string>(){};  
        for(int i=0;i<sabores.Length;i++) {this.sabores.Add(sabores[i],sabores[i]);} // adicionando sabor em cada index do parâmetro "sabores[]"
    }
}