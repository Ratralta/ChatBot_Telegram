using System;

namespace ChatBotTelegram.Bot.Dicionario.Pedidos;

public class PedidoPizza : PedidoModel
{
    public int sabor_i = 0;
    //public String[] sabor = new String[2];
    public bool pedido_finalizado = false; 
    
    
    public PedidoPizza(int values_size,String pedido_name) : base(values_size,pedido_name)
    {
    } 
       
}
