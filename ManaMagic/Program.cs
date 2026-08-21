using System;

#nullable enable

namespace ManaMagic
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            ManaMagicContext.Current.Run();
        }
    }
}