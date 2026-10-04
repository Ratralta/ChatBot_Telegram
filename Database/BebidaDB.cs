using System;

namespace ChatBotTelegram.Database;

public class BebidaDB
{
    public String[] sabores; 
    public double[] preco;

    public BebidaDB(String[] array_sabores, double[] array_precos)
    {
        this.sabores = array_sabores;
        this.preco = array_precos;
    }
}
