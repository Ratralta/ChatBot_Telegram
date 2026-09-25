using ChatBotTelegram;
using ChatBotTelegram.Bot;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;



apiKey my_api = new apiKey();
Dicionario_Bot bot_dicionario = new Dicionario_Bot();

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient(my_api.getBotApiKey(), cancellationToken: cts.Token);
var me = await bot.GetMe();
bot.OnMessage += OnMessage;

// method that handle messages received by the bot:
async Task OnMessage(Message msg, UpdateType type)
{
    if (msg.Text is null) return;	// we only handle Text messages here

    String[] mensagem_tratada_list =  msg.Text.ToLower().Trim().Split(" ");

    foreach(String i in mensagem_tratada_list)
    {
        if (bot_dicionario.saudacoes.ContainsValue(i))
        {
        await bot.SendMessage(msg.Chat,"Saudação");  
        } 
    }

    // let's echo back received text in the chat
    await bot.SendMessage(msg.Chat, $"{msg.From} said: {msg.Text}");
}




Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel(); // stop the bot



