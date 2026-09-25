using System;

namespace ChatBotTelegram.Bot;

public class Dicionario_Bot
{
public Dictionary<int,String> saudacoes;
    public Dicionario_Bot()
    {
    this.saudacoes = new Dictionary<int, string>()
    {
        {0,"ola"},
        {1,"oi"},
        {2,"gostaria"}
    };
    }
}
