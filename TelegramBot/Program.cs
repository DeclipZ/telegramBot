using System;


using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace TelegramBot
{
    class Program
    {
        private static TelegramBotClient Bot = new TelegramBotClient(Constants.BOT_API_KEY);
        private static Commands Commands = new Commands(Bot);
        
        
        static void Main(string[] args)
        {
            Bot.OnMessage += BotOnMessageReceived;
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
            
        }

        private static void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            try
            {
                User user = Bot.GetUpdatesAsync().Result[0].Message.From;
                var message = messageEventArgs.Message;
                string messageText = message.Text;
                Commands.command(messageText, message.Chat, user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
