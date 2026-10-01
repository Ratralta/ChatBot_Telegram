using System;
using Telegram.Bot.Types;

namespace ChatBotTelegram.Bot.Cliente;

public class ClienteMeneger
{
    public List<Cliente> clientes = new List<Cliente>();

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
}
