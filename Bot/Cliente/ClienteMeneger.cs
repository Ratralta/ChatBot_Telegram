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
        if(clientes.All(e=> e.chat_id == chat_id))
        {
        return true;
        } // se tiver o chat diferente 
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
    double preco_total = 0;

        foreach(PedidoModel i_pedido in getClienteByChatId(chatId).pedido.pedidos_list)
        {
        retorno += i_pedido.pedido_name +" : "+ i_pedido.retornarArrayAsString(i_pedido.values) + ", R$" + i_pedido.preco + "\n";
        preco_total += i_pedido.preco;
        }
    retorno += "Preço total : R$" + preco_total;

    return retorno;
    }

    public double getClientePedidosPreco(ChatId chatId)
    {
    double preco_total = 0;

        foreach(PedidoModel i_pedido in getClienteByChatId(chatId).pedido.pedidos_list)
        {
        preco_total += i_pedido.preco;
        }

    return preco_total;
    }

    public String getAllInfoCliente(ChatId chatId)
    {
    Cliente cliente = getClienteByChatId(chatId);
    String texto_retorno = "";

    texto_retorno += "Pedidos : " + getClientePedidos(chatId); // todos os pedidos 

    texto_retorno +=  cliente.pedido.endereco != null ? "\n Endereço : " + cliente.pedido.endereco : "\nEndereço : Vai buscar"; // enderecos
    texto_retorno += "\nMétodo de pagamento : " + cliente.pedido.modo_de_pagamento; // metodo de pagamento 

    return texto_retorno;
    }

    public void printarClientes()
    {
        foreach(Cliente i_cliente in clientes)
        {
        Console.WriteLine("Cliente :" + i_cliente.chat_id);
        }
    }
}
