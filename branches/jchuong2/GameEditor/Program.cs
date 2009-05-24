using System;

namespace GameEditor
{
    static class Program
    {
        [STAThread]
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameEditor game = new GameEditor())
            {
                game.Run();
            }
        }
    }
}

