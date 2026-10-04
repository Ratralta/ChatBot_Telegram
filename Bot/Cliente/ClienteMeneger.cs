using System;
using System.Security.Cryptography.X509Certificates;
using ChatBotTelegram.Bot.Database;
using ChatBotTelegram.Bot.Dicionario.Pedidos;
using ChatBotTelegram.Database;
using Telegram.Bot.Types;

namespace ChatBotTelegram.Bot.Cliente;

public class ClienteMeneger
{
    public List<Cliente> clientes = new List<Cliente>();
    public PizzaDB DBpizza; 
    public BebidaDB DBbebida;

    public ClienteMeneger(PizzaDB DBpizza,BebidaDB DBbebida)
    {
    this.DBpizza = DBpizza;
    this.DBbebida = DBbebida;
    }

    public Cliente newCliente(ChatId chatId)
    {
    var cliente = new Cliente(chatId);
    clientes.Add(cliente);
    return cliente;
    }
    
    public bool hasThisChatId(ChatId chat_id)
    {
        if(clientes.Count == 0) {return false;}
        if(clientes.All(e=> e.chat_id == chat_id)){ return true;} // se tiver o chat diferente 
        return false;        
    }

    public Cliente? getClienteByChatId(ChatId chatId)
    {
        foreach(var cliente in clientes)
        {
            if(cliente.chat_id == chatId)
            {
            return cliente;
            }
        }
    return null;
    }

    public String getClientePedidos(ChatId chatId)
    {
    String retorno = "";

        foreach(PedidoModel i_pedido in getClienteByChatId(chatId).pedido.pedidos_list)
        {
        retorno += i_pedido.pedido_name +" : "+ i_pedido.retornarArrayAsString(i_pedido.values) + ", Preço : R$" + i_pedido.preco + "\n";
        }

    return retorno;
    }
}
