namespace MagicVilla_Api.Logging
{
    public class Logging : ILogging
    {
        public readonly static string ERROR = "error";
        public void Log(string message, string type)
        {
            if (type == ERROR)
            {
                Console.WriteLine($"{ERROR} -> {message}");
            }
            else 
            {
                Console.WriteLine(message);
            }
        }
    }
}
