using System;
using System.Reflection.Metadata;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatBotTelegram;

public class TelegramChatBot
{
    private apiKey my_api = new apiKey(); 
    private TelegramBotClient bot = new TelegramBotClient("8826244768:AAG8KlSmZiz9CJH27uq-_NS9igme5_sd3O4"); 

    public void consolePrint()
    {
    var me = this.bot.GetMe();
    Console.WriteLine($"Hello, World! I am user {me.Id} .");
    }
}
