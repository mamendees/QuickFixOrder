using QuickFix.Fields;
using QuickFix;

namespace OrderGenerator;
public class OrderGeneratorApp : MessageCracker, IApplication
{
    Session? _session = null;
    public IInitiator? MyInitiator = null;

    private readonly Random randNum;

    public OrderGeneratorApp()
    {
        randNum = new();
    }

    public void OnCreate(SessionID sessionID)
    {
        _session = Session.LookupSession(sessionID);
    }

    public void OnLogon(SessionID sessionID) { Console.WriteLine("Logon - " + sessionID.ToString()); }
    public void OnLogout(SessionID sessionID) { Console.WriteLine("Logout - " + sessionID.ToString()); }

    public void FromAdmin(Message message, SessionID sessionID) { }
    public void ToAdmin(Message message, SessionID sessionID) { }

    public void FromApp(Message message, SessionID sessionID)
    {
        try
        {
            Crack(message, sessionID);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            Console.WriteLine(ex.StackTrace);
        }
    }

    public void ToApp(Message message, SessionID sessionID)
    {
        Console.WriteLine();
    }

    public void OnMessage(QuickFix.FIX44.ExecutionReport m, SessionID s)
    {
        Console.WriteLine("Received execution report");
    }

    public void OnMessage(QuickFix.FIX44.OrderCancelReject m, SessionID s)
    {
        Console.WriteLine("Received order cancel reject");
    }

    public void Run()
    {
        while (true)
        {
            try
            {
                Thread.Sleep(1000);
                var newOrderSingle = GetRandomNewOrderSingle();
                SendMessage(newOrderSingle);

            }
            catch (Exception e)
            {
                Console.WriteLine("Message Not Sent: " + e.Message);
                Console.WriteLine("StackTrace: " + e.StackTrace);
            }
        }
    }

    public QuickFix.FIX44.NewOrderSingle GetRandomNewOrderSingle()
    {
        var newOrderSingle = new QuickFix.FIX44.NewOrderSingle()
        {
            Symbol = GetRandomSymbol(),
            Side = GetRandomSide(),
            OrderQty = GetRandomQuantity(),
            Price = GetRandomPrice(),

            ClOrdID = new ClOrdID(new Guid().ToString()),
            TransactTime = new TransactTime(DateTime.Now),
            OrdType = new OrdType(OrdType.MARKET)
        };

        return newOrderSingle;
    }

    #region Private Methods
    private Symbol GetRandomSymbol()
    {
        var listSymbol = new List<string> { "PETR4", "VALE3", "VIIA4" };
        var randomSymbol = GetRandomElement(listSymbol);
        return new Symbol(randomSymbol);
    }

    private Side GetRandomSide()
    {
        var listSide = new List<char> { Side.BUY, Side.SELL };
        var randomSide = GetRandomElement(listSide);
        return new Side(randomSide);
    }

    private OrderQty GetRandomQuantity()
    {
        int minValue = 1;
        int maxValue = 100000;
        uint randomQuantity = (uint)randNum.Next(minValue, maxValue);
        return new OrderQty(randomQuantity);
    }

    private Price GetRandomPrice()
    {
        var minValue = 0.01;
        var maxValue = 1000;
        var value = randNum.NextDouble() * (minValue - maxValue) + maxValue;
        var price = decimal.Round((decimal)value, 2);
        if (price == maxValue) return GetRandomPrice();
        return new Price(price);
    }

    private T GetRandomElement<T>(IList<T> list)
    {
        return list[randNum.Next(list.Count)];
    }

    private void SendMessage(Message message)
    {
        if (_session is not null && message is not null)
        {
            message.Header.GetString(Tags.BeginString);
            _session.Send(message);
        }
        else
        {
            Console.WriteLine("Can't send message: session not created.");
        }
    }
    #endregion
}