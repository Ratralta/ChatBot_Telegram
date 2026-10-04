using System;

namespace ChatBotTelegram.Bot.Dicionario.Pedidos;

public class PedidoBebida : PedidoModel
{
    public int sabor_i = 0;
    public bool pedido_finalizado = false; 
    
    public PedidoBebida(int values_size, String pedido_name) : base(values_size,pedido_name)
    {
    } 
}
