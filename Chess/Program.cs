using System;

namespace Chess
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Chess game = new Chess())
            {
                game.Run();
            }
        }
    }
}

