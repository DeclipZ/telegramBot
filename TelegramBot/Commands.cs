using System;

using System.IO;
using System.Net.NetworkInformation;


using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    class Commands
    {
        private ZaycevNet.Parser parser = new ZaycevNet.Parser();
        private TelegramBotClient Bot;
        private ReplyKeyboardHide hideKeyboard = new ReplyKeyboardHide();

        public Commands(TelegramBotClient Bot)
        {
            this.Bot = Bot;
        }

        public void command(string message, Chat chat, User user)
        {
            try{
                if (!message.StartsWith(Constants.COMMAND_CHAR)) { return; }
                message = message.Substring(Constants.COMMAND_CHAR.Length);
                if (message.StartsWith("help"))
                {
                    commandHelp(chat, user);
                }
                if (message.StartsWith("about"))
                {
                    commandAbout(chat, user);
                }
                if (message.StartsWith("music"))
                {
                    if (message.Length < 6)
                    {
                        Bot.SendTextMessageAsync(chat.Id,
                            "# MUSIC #\n\nUsage: /music <Song name>",
                            replyMarkup: hideKeyboard);
                        return;
                    }
                    message = message.Substring(6);
                    commandMusic(chat, message, user);
                }
                if (message.StartsWith("ping"))
                {
                    commandPing(chat, user);
                }
            }
            catch(Exception ex){
                Console.WriteLine(ex);
            }
        }

        private void commandPing(Chat chat, User user)
        {
            Console.WriteLine(user.Username + " used ping command");
            Ping ping = new Ping();
            PingReply pingReply = ping.Send("cdndl.zaycev.net");
            Console.WriteLine(pingReply.RoundtripTime.ToString() + "ms");
            string textPing = String.Format("# PING #\n\n{0}ms", pingReply.RoundtripTime.ToString());
            Bot.SendTextMessageAsync(chat.Id,
                textPing,
                replyMarkup: hideKeyboard);

        }
        
        private void commandHelp(Chat chat, User user)
        {
            Console.WriteLine(user.Username + " used help command");
            var keyboard = new ReplyKeyboardMarkup(new[]{
                new[]{
                    new KeyboardButton("/help"),
                },
                new[]{
                    new KeyboardButton("/about"),
                },
                new[]{
                    new KeyboardButton("/music"),
                },
                new[]{
                    new KeyboardButton("/ping"),
                },
            });
            Bot.SendTextMessageAsync(chat.Id,
                "# HELP #\n\nHere's a list of commands:\n/help - show list of commands\n/about - about bot and creator\n/music - request bot to search music\n/ping - get the server ping",
                replyMarkup: keyboard);
        }

        private void commandAbout(Chat chat, User user)
        {
            Console.WriteLine(user.Username + " used about command");
            string textAbout = String.Format("# ABOUT #\n\nHello, my name is MysteriosBot. My creator is @{0}\nUse /help to get list of commands.\n\nBot version: {1}", Constants.BOT_CREATOR, Constants.BOT_VERSION);
            Bot.SendTextMessageAsync(chat.Id,
                textAbout,
                replyMarkup: hideKeyboard);
        }

        private void commandMusic(Chat chat, string songName, User user)
        {
            Console.WriteLine(user.Username + " used music command");
            ZaycevNet.Song song = parser.getSong(songName);
            if (song == null)
            {
                Bot.SendTextMessage(chat.Id,
                    "# MUSIC #\n\nI\'m sorry, I can\'t find this song.\nTry to write in a different way.");
            }
            FileToSend audio = new FileToSend();
            audio.Content = song.Audio;
            audio.Filename = song.Performer + " - " + song.Title;
            Bot.SendAudioAsync(chat.Id,
                audio,
                60,
                song.Performer,
                song.Title,
                replyMarkup: hideKeyboard);
        }
    }
}
