using Core;

namespace Api
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new MicroServiceHost();
            host.Run();
        }
    }
}
