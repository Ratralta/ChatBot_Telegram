using System.Net.Quic;
using System.Text.Encodings.Web;
using ChatBotTelegram;
using ChatBotTelegram.Bot;
using ChatBotTelegram.Bot.Cliente;
using ChatBotTelegram.Bot.Dicionario;
using ChatBotTelegram.Database;

using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

apiKey my_api = new apiKey(); // dados de api 


var db = new Config("Database/banco.db"); // conectando com o banco de dados 
String[] db_sabores = db.getSpecificTupla("sabores","sabor"); // pegando tupla "sabor" e retornando como "string[]"



ClienteMeneger clienteMeneger = new ClienteMeneger();
var bot_dicionario = new Dicionario_Bot(db_sabores);
using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient(my_api.getBotToken(), cancellationToken: cts.Token);
var me = await bot.GetMe();
var botFunctions = new BotFunctions(bot,bot_dicionario,clienteMeneger); // funções que serão lidas nas task

bot.OnError += OnError;
bot.OnMessage += OnMessage;
bot.OnUpdate += OnUpdate;

Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel(); // stop the bot

// method to handle errors in polling or in your OnMessage/OnUpdate code
async Task OnError(Exception exception, HandleErrorSource source)
{
    Console.WriteLine(exception); // just dump the exception to the console
}

// method that handle messages received by the bot:
async Task OnMessage(Message msg, UpdateType type)
{
    await botFunctions.AskStartPedidoAsync(msg,db_sabores);
}

// method that handle other types of updates received by the bot:
async Task OnUpdate(Update update)
{
    if (update is { CallbackQuery: { } query }) // non-null CallbackQuery
    {
        ChatId query_chat_id = query.Message.Chat.Id;
        if(query.Data == "sim") 
        {
            if(!clienteMeneger.hasThisChatId(query_chat_id) || clienteMeneger.clientes.Count == 0) // se NÃO tiver esse cliente ou nenhum 
            {
            await botFunctions.StartPedidoAsync(query_chat_id); // inicia pedido
            await botFunctions.PeguntarItemPedidoAsync(query.Message,query_chat_id); // perguntar pedido e colocar no estado "escolhendo_item"            
            }
        }
        if(query.Data == "pizza")
        {
            if(botFunctions.clienteMeneger.hasThisChatId(query.Message.Chat.Id.ToString())) // se existir esse cliente 
            {
                foreach(var cliente in clienteMeneger.clientes)
                {
                    if(cliente.estado_do_pedido == Cliente.estadoPedidoEnum.escolhendo_item) // se cliente tiver no estado "escolhendo_item"
                    {
                    await botFunctions.PerguntarSaboresPizzaAsync(query.Message,query_chat_id,db_sabores);            
                    }
                }
            }
        }
        //await bot.AnswerCallbackQuery(query.Id, $"You picked {query.Data} from {query.Id}"); // pop up na tela 
        //await bot.SendMessage(query.Message!.Chat, $"User {query.Id} clicked on {query.Data}");
    }
    if (update.CallbackQuery.Data == "sim")
    {
    //Console.WriteLine("Fui lido");    
    }
    
}