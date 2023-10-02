using OrderAccumulator;
using QuickFix.Fields;

namespace OrderAccumulatorTest;
public class CompanyTest
{
    Company Company;
    private readonly decimal Limit = 1000000;

    public CompanyTest()
    {
        Company = new Company("PETR4", Limit);
    }
    
    [Fact]
    public void SideSellCanUpdateExpoExactLimit()
    {
        var side = new Side(Side.SELL);
        var price = new Price(1000);
        var qty = new OrderQty(1000);
        Assert.Equal(0, Company.FinancialExpo);

        var returnValue = Company.UpdateFinancialExpo(side, price, qty);

        Assert.Equal(-(price.getValue() * qty.getValue()), Company.FinancialExpo);
        Assert.True(returnValue);
    }

    [Fact]
    public void SideSellCanUpdateExpoFinancialNegative()
    {
        var priceSell1 = new Price(500);
        var qtySell1 = new OrderQty(1000);

        var priceSell2 = new Price(500);
        var qtySell2 = new OrderQty(1000);

        Assert.Equal(0, Company.FinancialExpo);

        Company.UpdateFinancialExpo(new Side(Side.SELL), priceSell1, qtySell1);
        var resultActual = Company.UpdateFinancialExpo(new Side(Side.SELL), priceSell2, qtySell2);

        var resultExpected = -(priceSell1.getValue() * qtySell1.getValue()) - (priceSell2.getValue() * qtySell2.getValue());
        Assert.Equal(resultExpected, Company.FinancialExpo);
        Assert.True(resultActual);
    }

    [Fact]
    public void SideSellCanUpdateExpoFinancialPositive()
    {
        var priceBuy = new Price(1000);
        var qtyBuy = new OrderQty(1000);

        var priceSell = new Price(2000);
        var qtySell = new OrderQty(1000);

        Assert.Equal(0, Company.FinancialExpo);

        Company.UpdateFinancialExpo(new Side(Side.BUY), priceBuy, qtyBuy);
        var resultActual = Company.UpdateFinancialExpo(new Side(Side.SELL), priceSell, qtySell);

        var resultExpected = (priceBuy.getValue() * qtyBuy.getValue()) - (priceSell.getValue() * qtySell.getValue());
        Assert.Equal(resultExpected, Company.FinancialExpo);
        Assert.True(resultActual);
    }

    [Fact]
    public void SideBuyCanUpdateExpoExactLimit()
    {
        var side = new Side(Side.BUY);
        var price = new Price(1000);
        var qty = new OrderQty(1000);
        Assert.Equal(0, Company.FinancialExpo);

        var returnValue = Company.UpdateFinancialExpo(side, price, qty);

        Assert.Equal(price.getValue() * qty.getValue(), Company.FinancialExpo);
        Assert.True(returnValue);
    }

    [Fact]
    public void SideSellCantUpdateExpoOverLimit()
    {
        var side = new Side(Side.SELL);
        var returnValue = Company.UpdateFinancialExpo(side, new Price(1001), new OrderQty(1000));

        Assert.False(returnValue);
        Assert.Equal(0, Company.FinancialExpo);
    }

    [Fact]
    public void SideBuyCantUpdateExpoOverLimit()
    {
        var side = new Side(Side.BUY);
        var returnValue = Company.UpdateFinancialExpo(side, new Price(1001), new OrderQty(1000));

        Assert.False(returnValue);
        Assert.Equal(0, Company.FinancialExpo);
    }
    
    [Fact]
    public void SideErrorUpdateExpo()
    {
        var side = new Side(Side.CROSS);
        void action() => Company.UpdateFinancialExpo(side, new Price(100), new OrderQty(100));
        Exception exception = Assert.Throws<Exception>(action);
        Assert.Equal("Side Error", exception.Message);
    }
}