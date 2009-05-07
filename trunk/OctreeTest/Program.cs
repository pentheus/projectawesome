using System;

namespace OctreeTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (OctreeTest game = new OctreeTest())
            {
                game.Run();
            }
        }
    }
}

