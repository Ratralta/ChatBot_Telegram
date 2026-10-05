using System;
using ChatBotTelegram.Bot.Cliente;
using ChatBotTelegram.Bot.Dicionario;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

using ChatBotTelegram.Bot.Dicionario.Pedidos;
using ChatBotTelegram.Bot.Database;
using ChatBotTelegram.Database;
using System.Runtime.CompilerServices;

namespace ChatBotTelegram.Bot;

public class BotFunctions
{
    public TelegramBotClient bot;
    public Dicionario_Bot bot_dicionario;
    public ClienteMeneger clienteMeneger;

    public BotFunctions(TelegramBotClient telegramBotClient, Dicionario_Bot dicionario_Bot, ClienteMeneger clienteMeneger)
    {
    this.bot = telegramBotClient;
    this.bot_dicionario = dicionario_Bot;
    this.clienteMeneger = clienteMeneger;
    }

    public async Task AskStartPedidoAsync(Message msg, String[] botao_opcoes)
    {
        if(clienteMeneger.hasThisChatId(msg.Chat.Id)){return;} // se TIVER esse chat id
        
        Dictionary<String,String>? intencoes_user = bot_dicionario.getIntencoes(msg.Text);
        if(intencoes_user == null) {Console.WriteLine("Não identifiquei intenção"); return;}
        foreach(var i_intecao in intencoes_user)
        {
            if(i_intecao.Key == Dicionario_Bot.intencoes_enum.saudacao.ToString()) // se identificar uma saudação
            {
            await bot.SendMessage(msg.Chat, "Ola!, deseja realizar um pedido?",replyMarkup: new InlineKeyboardMarkup("sim","não"));
            }
        }
    }
    public async Task StartPedidoAsync(ChatId chatId)
    {
    var new_cliente = clienteMeneger.newCliente(chatId); // criando novo cliente
    }
    public async Task PeguntarItemPedidoAsync(Message msg,ChatId chatId)
    {
    clienteMeneger.getClienteByChatId(chatId).estado_do_pedido = Cliente.Cliente.estadoPedidoEnum.escolhendo_item; // mudando estado do pedido
    var botoes = new InlineKeyboardMarkup();
    botoes.AddNewRow("pizza");
    botoes.AddNewRow("bebida");
    botoes.AddNewRow("cancelar");

    String text_message = "O que deseja pedir?";
        if(clienteMeneger.getClienteByChatId(chatId).pedido.pedidos_list.Count > 0) // se cliente possuir algum pedido
        {
        botoes.AddNewRow("Amostrar meus pedidos");
        botoes.AddNewRow("prosseguir");
        text_message = "Algo mais ?";
        }
    await bot.SendMessage(msg.Chat, text_message,replyMarkup: botoes);
    }


    public async Task PerguntarSaboresPizzaAsync(Message msg,ChatId chatId)
    {
    clienteMeneger.getClienteByChatId(chatId).estado_do_pedido = Cliente.Cliente.estadoPedidoEnum.em_pizza_item; // mudando estado do pedido
    
    PizzaDB my_pizzaDB = clienteMeneger.DBpizza; // pegando informações do DB relacionado a pizza 

    var botoes = new InlineKeyboardButton[my_pizzaDB.sabores.Length]; // criando um botão com "my_pizzaDB.sabores.Length" itens

        for(int i=0;i<my_pizzaDB.sabores.Length;i++)
        {
            botoes[i] = InlineKeyboardButton.WithCallbackData( // definindo "texto" e "callbackData" de cada botão 
            text:$"{my_pizzaDB.sabores[i]} (R$ : {my_pizzaDB.preco[i]})",
            callbackData:my_pizzaDB.sabores[i]
            ); 
        }

    await bot.SendMessage(msg.Chat, "Qual sabor deseja na pizza? (Escollha 2 no maximo)",replyMarkup: botoes);     
    }
    public async Task PerguntarBebidasBebidaAsync(Message msg,ChatId chatId)
    {
    clienteMeneger.getClienteByChatId(chatId).estado_do_pedido = Cliente.Cliente.estadoPedidoEnum.em_bebida_item; // mudando estado do pedido
    
    BebidaDB my_bebidaDB = clienteMeneger.DBbebida; // pegando informações do DB relacionado a pizza 

    var botoes = new InlineKeyboardButton[my_bebidaDB.sabores.Length]; // criando um botão com "my_pizzaDB.sabores.Length" itens

        for(int i=0;i<my_bebidaDB.sabores.Length;i++)
        {
            botoes[i] = InlineKeyboardButton.WithCallbackData( // definindo "texto" e "callbackData" de cada botão 
            text:$"{my_bebidaDB.sabores[i]} (R$ : {my_bebidaDB.preco[i]})",
            callbackData:my_bebidaDB.sabores[i]
            ); 
        }

    await bot.SendMessage(msg.Chat, "Qual bebida deseja ?",replyMarkup: botoes);     
    }

    public async void ChecarItemPedido(CallbackQuery query, Cliente.Cliente cliente) // checa se apertou nos botões de "PerguntarSaboresPizzaAsync" ou "PerguntarBebidasBebidaAsync"
    {
    ChatId query_chat_id = query.Message.Chat.Id;
        for(int i=0;i<clienteMeneger.DBpizza.sabores.Length && cliente.estado_do_pedido == Cliente.Cliente.estadoPedidoEnum.em_pizza_item;i++) // checando pizzas
        {
        var DBitem = clienteMeneger.DBpizza;
        String i_sabor = DBitem.sabores[i];
            if(query.Data == i_sabor) // botão apertado de sabor de pizza 
            {
                foreach(PedidoModel i_pizza in cliente.pedido.pedidos_list) // vendo todos os pedidos por pizza 
                {
                    if(!i_pizza.pedido_finalizado) // se "pedido_finalizado" for FALSE 
                    {
                        if(i_pizza.i_value < i_pizza.values.Length)
                        {
                        i_pizza.values[i_pizza.i_value] = query.Data; // enserindo valor 
                        i_pizza.preco += DBitem.preco[i]; // enserindo valor
                        
                        i_pizza.i_value += 1;
                        if(i_pizza.i_value < i_pizza.values.Length) 
                        {await PerguntarSaboresPizzaAsync(query.Message,query_chat_id);} 
                        }
                        if(i_pizza.i_value >= i_pizza.values.Length)
                        {
                        //await bot.SendMessage(query_chat_id,"Sua pizza de sabor " + i_pizza.retornarArrayAsString<String>(i_pizza.values));
                        i_pizza.pedido_finalizado = true; // encerrando esse pedido de pizza 
                        await PeguntarItemPedidoAsync(query.Message,query_chat_id);
                        }
                    }
                }
            }
        }
        for(int i=0;i<clienteMeneger.DBbebida.sabores.Length && cliente.estado_do_pedido == Cliente.Cliente.estadoPedidoEnum.em_bebida_item;i++) // checando bebidas
        {
        var DBitem = clienteMeneger.DBbebida;
        String i_sabor = DBitem.sabores[i];

            if(query.Data == i_sabor) // botão apertado de sabor de pizza 
            {
                foreach(PedidoModel i_bebida in cliente.pedido.pedidos_list) // vendo todos os pedidos por pizza 
                {
                    if(!i_bebida.pedido_finalizado) // se "pedido_finalizado" for FALSE 
                    {
                        if(i_bebida.i_value < i_bebida.values.Length)
                        {
                        i_bebida.values[i_bebida.i_value] = query.Data; // inserindo valor ao obj 
                        i_bebida.preco += DBitem.preco[i]; // inserindo valor ao obj
                        
                        i_bebida.i_value += 1; 
                            if(i_bebida.i_value < i_bebida.values.Length)
                            {await PeguntarItemPedidoAsync(query.Message,query_chat_id);} 
                        }
                        if(i_bebida.i_value >= i_bebida.values.Length)
                        {
                        //await bot.SendMessage(query_chat_id,"Sua pizza de sabor " + i_bebida.retornarArrayAsString<String>(i_bebida.values));
                        i_bebida.pedido_finalizado = true; // encerrando esse pedido de pizza 
                        await PeguntarItemPedidoAsync(query.Message,query_chat_id);
                        }
                    }
                }
            }
        }
    }

    public async void PeguntarDelivery(ChatId chatId)
    {
    String mensagem = "Ok, você irá buscar ou delivery?";
    var cliente = clienteMeneger.getClienteByChatId(chatId);
    cliente.estado_do_pedido = Cliente.Cliente.estadoPedidoEnum.pergutando_se_delivery; // mudando de estado de pedido

    var botao =  new InlineKeyboardMarkup("buscar","delivery");
    botao.AddNewRow("cancelar");


    await bot.SendMessage(chatId, mensagem,replyMarkup: botao);
    }

    public async void PerguntarEndereco(ChatId chatId)
    { 
    var cliente = clienteMeneger.getClienteByChatId(chatId);
        if(cliente.pedido.endereco == null)
        {
        cliente.estado_do_pedido = Cliente.Cliente.estadoPedidoEnum.definindo_endereco_delivery;
        bot.SendMessage(chatId, "Digite seu endereço");
        }
        else
        {
        var botao =  new InlineKeyboardMarkup("confirmar endereco","corrigir endereco");
        String mensagem = "Seu endereço é : " + "''" + cliente.pedido.endereco +"'', está correto?";
        await bot.SendMessage(chatId, mensagem,replyMarkup: botao);
        }
    }

    public async void ChecarConfimarEndereco(ChatId chatId)
    {
    var cliente = clienteMeneger.getClienteByChatId(chatId);
        if(cliente.pedido.endereco != null)
        {
        var botao =  new InlineKeyboardMarkup("confirmar endereco","corrigir endereco");

        bot.SendMessage(chatId, "Seu endereço é : " + cliente.pedido.endereco);
        
        }
    }

    public async void ChecarDeliveryOuBuscar(CallbackQuery query, Cliente.Cliente cliente)
    {
    ChatId chatId = cliente.chat_id;
        if(query.Data == "delivery" && cliente.estado_do_pedido == Cliente.Cliente.estadoPedidoEnum.pergutando_se_delivery)
        {
        PerguntarEndereco(chatId);
        }
        if(query.Data == "buscar" && cliente.estado_do_pedido == Cliente.Cliente.estadoPedidoEnum.pergutando_se_delivery)
        {
        PerguntarMetodoDePagamento(chatId);
        }

        if(query.Data == "corrigir endereco" && cliente.estado_do_pedido == Cliente.Cliente.estadoPedidoEnum.definindo_endereco_delivery)
        {
        cliente.pedido.endereco = null; // zerando endereço 
        PerguntarEndereco(chatId);
        }
        if(query.Data == "confirmar endereco" && cliente.estado_do_pedido == Cliente.Cliente.estadoPedidoEnum.definindo_endereco_delivery)
        {
        PerguntarMetodoDePagamento(chatId);
        }
    }

    public async void PerguntarMetodoDePagamento(ChatId chatId)
    {
    var cliente = clienteMeneger.getClienteByChatId(chatId);
    cliente.estado_do_pedido = Cliente.Cliente.estadoPedidoEnum.definindo_metodo_de_pagamento;
    
    Pedido.enum_tipo_de_pagamentos[] tipo_de_pagamento_array = (Pedido.enum_tipo_de_pagamentos[])Enum.GetValues(typeof(Pedido.enum_tipo_de_pagamentos));
    var botoes = new InlineKeyboardButton[tipo_de_pagamento_array.Length]; // criando um botão com "my_pizzaDB.sabores.Length" itens
    String mensagem = "Escolha um método de pagamento : ";

        for(var i=0;i<tipo_de_pagamento_array.Length;i++)
        {
        botoes[i] = tipo_de_pagamento_array[i].ToString();
        }
    
    await bot.SendMessage(chatId, mensagem,replyMarkup: botoes);    
    }

    public void PrintarTodosPedidos()
    {
        foreach(var i_cliente in clienteMeneger.clientes)
        {
        Console.WriteLine("Cliente : " + i_cliente.chat_id);
            foreach(var i_pedido in i_cliente.pedido.pedidos_list)
            {
                String values = i_pedido.retornarArrayAsString<String>(i_pedido.values);
                Console.WriteLine("Pedido : " + values );                
            }
        }
    }
}