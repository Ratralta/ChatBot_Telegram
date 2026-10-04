using System;

namespace ChatBotTelegram.Bot.Database;

public class PizzaDB
{
    public String[] sabores; 
    public double[] preco;

    public PizzaDB(String[] array_sabores, double[] array_precos)
    {
        this.sabores = array_sabores;
        this.preco = array_precos;
    }
}