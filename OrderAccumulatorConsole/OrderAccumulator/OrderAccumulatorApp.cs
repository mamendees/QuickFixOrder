using QuickFix;
using QuickFix.Fields;

namespace OrderAccumulator;
internal class OrderAccumulatorApp : MessageCracker, IApplication
{
    int orderID = 0;
    int execID = 0;

    private readonly decimal Limit = 1000000;
    private List<Company> Companies;
    private string GenOrderID() { return (++orderID).ToString(); }
    private string GenExecID() { return (++execID).ToString(); }

    public OrderAccumulatorApp()
    {
        Companies = new List<Company>()
        {
            new Company("PETR4", Limit),
            new Company("VALE3", Limit),
            new Company("VIIA4", Limit)
        };
    }

    public void FromApp(Message message, SessionID sessionID)
    {
        Crack(message, sessionID);
    }

    public void ToApp(Message message, SessionID sessionID) { }

    public void FromAdmin(Message message, SessionID sessionID) { }
    public void OnCreate(SessionID sessionID) { }
    public void OnLogout(SessionID sessionID) { }
    public void OnLogon(SessionID sessionID) { }
    public void ToAdmin(Message message, SessionID sessionID) { }

    public void OnMessage(QuickFix.FIX44.NewOrderSingle n, SessionID s)
    {
        Symbol symbol = n.Symbol;
        Side side = n.Side;
        OrderQty orderQty = n.OrderQty;
        Price price = n.Price;
        OrdType ordType = n.OrdType;
        ClOrdID clOrdID = n.ClOrdID;
        Console.WriteLine($"Symbol: {symbol} - Side: {side} - Quantity: {orderQty} - Price: {price}");

        Company company = Companies.First(x => x.Name.Equals(symbol.getValue()));
        bool updateFinancialExpo = company.UpdateFinancialExpo(side, price, orderQty);

        Message message = CreateReturnMessage(updateFinancialExpo, n);

        try
        {
            Session.SendToTarget(message, s);
        }
        catch (SessionNotFound ex)
        {
            Console.WriteLine("Session not found exception!");
            Console.WriteLine(ex.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private Message CreateReturnMessage(bool updateFinancialExpo, QuickFix.FIX44.NewOrderSingle n)
    {
        Symbol symbol = n.Symbol;
        Side side = n.Side;
        OrderQty orderQty = n.OrderQty;
        Price price = n.Price;
        ClOrdID clOrdID = n.ClOrdID;

        if (updateFinancialExpo)
        {
            QuickFix.FIX44.ExecutionReport exReport = new(
                new OrderID(GenOrderID()),
                new ExecID(GenExecID()),
                new ExecType(ExecType.FILL),
                new OrdStatus(OrdStatus.FILLED),
                symbol,
                side,
                new LeavesQty(0),
                new CumQty(orderQty.getValue()),
                new AvgPx(price.getValue())
            );

            exReport.Set(clOrdID);
            exReport.Set(symbol);
            exReport.Set(n.OrderQty);
            exReport.Set(new LastQty(orderQty.getValue()));
            exReport.Set(new LastPx(price.getValue()));

            return exReport;
        }

        return new QuickFix.FIX44.OrderCancelReject(
          new OrderID(GenOrderID()),
          new ClOrdID(new Guid().ToString()),
          new OrigClOrdID(clOrdID.getValue()),
          new OrdStatus(OrdStatus.REJECTED),
          new CxlRejResponseTo(CxlRejResponseTo.ORDER_CANCEL_REPLACE_REQUEST))
        {
            CxlRejReason = new CxlRejReason(CxlRejReason.OTHER),
            Text = new Text("PRICE EXCEEDS LIMIT")
        };
    }
}