using System;

namespace ChatBotTelegram.Bot.Dicionario.Pedidos;

public class PedidoPizza : PedidoModel
{
    public int sabor_i = 0;
    public String[] sabor = new String[2];
    public double preco;
    public bool pedido_finalizado = false; 
    /*
    public PedidoPizza(int values_size) : base(values_size)
    {
    }
    */
}
