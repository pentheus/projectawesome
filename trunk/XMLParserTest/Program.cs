using System;

namespace XMLParserTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ParserTest game = new ParserTest())
            {
                game.Run();
            }
        }
    }
}

