using OrderGenerator;
using QuickFix;
using QuickFix.Transport;

internal class Program
{
    [STAThread]
    private static void Main()
    {
        Console.WriteLine("====================");
        Console.WriteLine("ORDER GENERATOR");
        Console.WriteLine("====================");

        try
        {
            string file = "ordergenerator.cfg";
            SessionSettings settings = new(file);
            OrderGeneratorApp application = new();
            IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
            ILogFactory logFactory = new ScreenLogFactory(settings);
            SocketInitiator initiator = new(application, storeFactory, settings, logFactory);

            application.MyInitiator = initiator;

            initiator.Start();
            application.Run();
            initiator.Stop();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }
    }
}