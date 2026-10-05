using System;
using ChatBotTelegram.Bot.Dicionario;
using ChatBotTelegram.Bot.Dicionario.Pedidos;
using Telegram.Bot.Types;

namespace ChatBotTelegram.Bot.Cliente;

public class Cliente
{
    public enum estadoPedidoEnum
    {
        escolhendo_item,
        em_pizza_item,
        em_bebida_item,
        perguntando_se_deseja_iniciar_pedido,
        pergutando_se_delivery,
        definindo_endereco_delivery,
        definindo_metodo_de_pagamento,
        pedido_finalizado
    }



    public bool atendimento_finalizado;
    public ChatId chat_id; 
    public Pedido pedido;
    public estadoPedidoEnum estado_do_pedido; 

    public Cliente(ChatId chat_id)
    {
    this.chat_id = chat_id;
    this.pedido = new Pedido();
    }


}
