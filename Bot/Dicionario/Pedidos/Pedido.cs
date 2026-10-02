using System;

namespace ChatBotTelegram.Bot.Dicionario.Pedidos;

public class Pedido
{
    public enum enum_tipo_de_pagamentos
    {
    Credito,
    Debito,
    Pix,
    Dinheiro
    }

    public List<PedidoModel> pedidos_list = new List<PedidoModel>();


    public int preco;
    public String? endereco;
    public enum_tipo_de_pagamentos modo_de_pagamento; 


    public void AddPedido(PedidoModel pedido)
    {
    pedidos_list.Add(pedido);
    }
}