using System;

namespace DatabaseV3
{
    /// <summary>
    /// The main entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point.
        /// </summary>
        /// <param name="args">The arguments to the application.</param>
        public static void Main(string[] args)
        {
            Logger.Init(string.Empty, LogLevel.Debug);
            using (Network network = new Network(int.Parse(args[0])))
            {
                foreach (var s in args[1].Split(','))
                {
                    network.Connect(new NetworkNode(s));
                }

                while (Console.ReadLine() != "exit")
                {
                }

                network.Shutdown();
            }
        }
    }
}