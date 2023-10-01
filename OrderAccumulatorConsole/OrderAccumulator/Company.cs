using QuickFix.Fields;

namespace OrderAccumulator;
public class Company
{
    public Company(string name, decimal limit)
    {
        Name = name;
        FinancialExpo = 0;
        Limit = limit;
    }

    public string Name { get; init; }
    public decimal FinancialExpo { get; private set; }
    public decimal Limit { get; init; }

    public bool UpdateFinancialExpo(Side side, Price price, OrderQty orderQty)
    {
        var totalValue = price.getValue() * orderQty.getValue();

        if (!CanUpdateFinancialExpo(totalValue))
            return false;

        UpdateFinancialExpo(side, totalValue);
        return true;
    }

    private void UpdateFinancialExpo(Side side, decimal totalValue)
    {
        switch (side.getValue())
        {
            case Side.SELL:
                FinancialExpo -= totalValue;
                break;
            case Side.BUY:
                FinancialExpo += totalValue;
                break;
            default:
                throw new Exception("Side Error");
        }
    }

    private bool CanUpdateFinancialExpo(decimal totalValue)
    {
        Console.WriteLine($"Name: {Name} - FinancialExpo: {FinancialExpo} - Total Price: {totalValue:c}");
        return (decimal.Abs(FinancialExpo) + totalValue) <= Limit;
    }
}
