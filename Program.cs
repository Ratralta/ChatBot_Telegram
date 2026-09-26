using ChatBotTelegram;
using ChatBotTelegram.Bot;
using ChatBotTelegram.Database;


using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;




apiKey my_api = new apiKey(); // dados de api 


var db = new Config("Database/banco.db"); // conectando com o banco de dados 
String[] db_sabores = db.getSpecificTupla("sabores","sabor"); // pegando tupla "sabor" e retornando como "string[]"
Dicionario_Bot bot_dicionario = new Dicionario_Bot(db_sabores); // criando dicionario pro bot 



using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient(my_api.getBotApiKey(), cancellationToken: cts.Token);
var me = await bot.GetMe();
bot.OnMessage += OnMessage;

// method that handle messages received by the bot:
async Task OnMessage(Message msg, UpdateType type)
{
    if (msg.Text is null) return;// we only handle Text messages here

    String[] mensagem_tratada_list =  msg.Text.ToLower().Trim().Split(" "); // tratando mensagem do usuario e separando palavras por espaços e colocando numa "String[]"

    foreach(String i_palavra in mensagem_tratada_list)
    {
        if(bot_dicionario.sabores.ContainsValue(i_palavra))
        {
        await bot.SendMessage(msg.Chat,"Tem esse sabor ai");  
        }
    }
    // let's echo back received text in the chat
    await bot.SendMessage(msg.Chat, $"{msg.From} said: {msg.Text}");
}

Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine(); // fazer aplicação continuar rodando ate receber input do usuário 
cts.Cancel(); // stop the bot