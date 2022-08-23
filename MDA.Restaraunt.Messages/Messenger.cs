namespace MDA.Restaraunt.Messages
{
    /// <summary>
    /// Класс печати сообщений
    /// </summary>
    public static class Messenger
    {
        /// <summary>
        /// Печать сообщения в консоль
        /// </summary>
        /// <param name="txt">Текст сообщения</param>
        /// <param name="color">Цвет сообщения</param>
        public static void PrintTxt(string txt, MsgColor color)
        {
            PaintMessage(color);
            Console.WriteLine(txt + "\n");
            Console.ResetColor();
        }

        /// <summary>
        /// Печать сообщения в консоль с задержкой
        /// </summary>
        /// <param name="txt">Текст сообщения</param>
        /// <param name="color">Цвет сообщения</param>
        public static void PrintMsgSleep(string txt, MsgColor color)
        {
            PaintMessage(color);
            Console.WriteLine(txt + "\n");
            Thread.Sleep(5000);
            Console.ResetColor();

        }

        /// <summary>
        /// Печать сообщения в консоль с задержкой
        /// </summary>
        /// <param name="txt">Текст сообщения</param>
        /// <param name="color">Цвет сообщения</param>
        public async static Task PrintMsgSleepAsync(string txt, MsgColor color)
        {

            Console.WriteLine(txt + "\n");
            //await Task.Delay(5000);
            Console.ResetColor();

        }

        private static void PaintMessage(MsgColor color)
        {
            switch (color)
            {
                case MsgColor.Normal:
                    Console.ResetColor();
                    break;
                case MsgColor.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case MsgColor.Answer:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case MsgColor.Info:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                default:
                    Console.ResetColor();
                    break;
            }
        }

    }
}
