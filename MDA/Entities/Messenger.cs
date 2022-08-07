﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDA.Entities
{
    internal static class Messenger
    {
        public static void PrintTxt(string txt)
        {
            Console.WriteLine(txt + "\n");
        }

        public static void PrintError(string txt)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(txt + "\n");
            Console.ResetColor();
        }

        public static void PrintAnswer(string txt)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(txt + "\n");
            Console.ResetColor();
        }

        public static void PrintMsgSleep(string txt)
        {
            
            Console.WriteLine(txt + "\n");
            Thread.Sleep(5000);

        }
        public async static Task PrintMsgSleepAsync(string txt)
        {

            Console.WriteLine(txt + "\n");
            await Task.Delay(5000);

        }

    }
}
