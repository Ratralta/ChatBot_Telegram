using System;
using ChatBotTelegram.Bot.Cliente;
using ChatBotTelegram.Bot.Dicionario;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

using ChatBotTelegram.Bot.Dicionario.Pedidos;

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
    var botoes = new InlineKeyboardMarkup("pizza","bebida");
    var teste = await bot.SendMessage(msg.Chat, "O que deseja pedir?",replyMarkup: botoes);
    }

    public async Task PerguntarSaboresPizzaAsync(Message msg,ChatId chatId,String[] sabores_array)
    {
    clienteMeneger.getClienteByChatId(chatId).estado_do_pedido = Cliente.Cliente.estadoPedidoEnum.em_pizza_item; // mudando estado do pedido
    InlineKeyboardButton[] botao_itens = sabores_array.Select(c => InlineKeyboardButton.WithCallbackData(c)).ToArray(); // transformando "botao_opcoes" em algo pra ser lido em "SendMessage"
    InlineKeyboardMarkup botao = new InlineKeyboardMarkup(botao_itens);
        
        await bot.SendMessage(msg.Chat, "Qual sabor deseja na pizza? (Escollha 2 no maximo)",replyMarkup: botao);     
    }

    public async Task PrintarTodosPedidos()
    {
        foreach(var i_cliente in clienteMeneger.clientes)
        {
        Console.WriteLine("Cliente : " + i_cliente.chat_id);
            foreach(var i_pedido in i_cliente.pedido.pedidos_list)
            {
            Console.WriteLine("Pedido : " + i_pedido);
            }
        }
    }
}