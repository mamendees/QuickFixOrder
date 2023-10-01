using OrderAccumulator;
using QuickFix;

internal class Program
{
    [STAThread]
    private static void Main()
    {
        Console.WriteLine("====================");
        Console.WriteLine("ORDER ACCUMULATOR");
        Console.WriteLine("====================");

        try
        {
            string file = "orderaccumulator.cfg";

            SessionSettings settings = new(file);
            OrderAccumulatorApp executorApp = new();
            IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
            ILogFactory logFactory = new FileLogFactory(settings);
            ThreadedSocketAcceptor acceptor = new(executorApp, storeFactory, settings, logFactory);

            acceptor.Start();

            Console.WriteLine("press <enter> to quit");
            Console.Read();

            acceptor.Stop();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.ToString());
        }
    }
}