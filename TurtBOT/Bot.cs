﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace TurtBOT
{
    public static class Bot
    {
        private static BotConfig botConfig;
        
        private static DiscordSocketClient client = new DiscordSocketClient();

        private static readonly CommandService commands = new CommandService();

        public static void Initialize(string token, BotConfig config)
        {
            botConfig = config;
            client.Log += Log;
            client.MessageReceived += OnMessageReceived;
            
            commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            
            client.LoginAsync(TokenType.Bot, token);
            client.StartAsync();
        }
        
        private static Task Log(LogMessage msg)
        {
            switch (msg.Severity)
            {
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(msg.ToString());
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(msg.ToString());
                    break;
                default:
                    Console.WriteLine(msg.ToString());
                    break;
            }
            Console.ResetColor();
            return Task.CompletedTask;
        }

        static async Task OnMessageReceived(SocketMessage msgpar)
        {
            if (msgpar is SocketUserMessage msg && msg.Content.StartsWith(botConfig.Prefix) && !msg.Author.IsBot)
            {
                await commands.ExecuteAsync(new SocketCommandContext(client, msg), botConfig.Prefix.Length, null);
            }
        }
    }
}