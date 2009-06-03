using System;

namespace AfterDarkGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (AfterDarkGame game = new AfterDarkGame())
            {
                game.Run();
            }
        }
    }
}

