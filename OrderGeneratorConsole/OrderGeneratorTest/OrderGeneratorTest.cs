using OrderGenerator;

namespace OrderGeneratorTest;
public class OrderGeneratorTest
{
    [Fact]
    public void VerifyQuantityCorrectRange()
    {
        //Quantidade: valor positivo inteiro aleatório menor que 100.000
        var OrderGenenatorApp = new OrderGeneratorApp();

        var overRange = false;

        for (int i = 1; i < 999999; i++)
        {
            var value = OrderGenenatorApp.GetRandomNewOrderSingle().OrderQty.getValue();

            if (value < 1 || value >= 100000)
            {
                overRange = true;
                break;
            }
        }

        Assert.False(overRange);
        Assert.False(overRange);
    }

    [Fact]
    public void VerifyQuantityMinValue()
    {
        //Quantidade: valor positivo inteiro aleatório menor que 100.000
        var OrderGenenatorApp = new OrderGeneratorApp();

        bool hasMinValue = false;
        for (int i = 1; i < 999999; i++)
        {
            var value = OrderGenenatorApp.GetRandomNewOrderSingle().OrderQty.getValue();

            if (value == 1)
            {
                hasMinValue = true;
                break;
            }
        }

        Assert.True(hasMinValue);
    }

    [Fact]
    public void VerifyQuantityMaxValue()
    {
        //Quantidade: valor positivo inteiro aleatório menor que 100.000
        var OrderGenenatorApp = new OrderGeneratorApp();

        bool hasMaxValue = false;
        for (int i = 1; i < 999999; i++)
        {
            var value = OrderGenenatorApp.GetRandomNewOrderSingle().OrderQty.getValue();

            if (value == 99999)
            {
                hasMaxValue = true;
                break;
            }
        }

        Assert.True(hasMaxValue);
    }

    [Fact]
    public void VerifyPriceCorrectRange()
    {
        //Preço: valor positivo decimal múltiplo de 0.01 e menor que 1.000
        var OrderGenenatorApp = new OrderGeneratorApp();

        var overRangeValue = false;
        var overMultipleValue = false;
        for (int i = 0; i < 999999; i++)
        {
            var value = OrderGenenatorApp.GetRandomNewOrderSingle().Price.getValue();

            if (value <= 0 || value >= 1000)
            {
                overRangeValue = true;
                break;
            }

            if (value % (decimal)0.01 != 0)
            {
                overMultipleValue = true;
                break;
            }
        }

        Assert.False(overRangeValue);
        Assert.False(overMultipleValue);
    }

    [Fact]
    public void VerifyPriceNotCorrectRange()
    {
        //Preço: valor positivo decimal múltiplo de 0.01 e menor que 1.000
        var OrderGenenatorApp = new OrderGeneratorApp();

        var overRangeValue = false;
        var overMultipleValue = false;
        for (int i = 0; i < 999999; i++)
        {
            var value = OrderGenenatorApp.GetRandomNewOrderSingle().Price.getValue();

            if (value < 1 || value > 999)
            {
                overRangeValue = true;
            }

            if (value % (decimal)0.02 != 0)
            {
                overMultipleValue = true;
            }
        }

        Assert.True(overRangeValue);
        Assert.True(overMultipleValue);
    }

    [Fact]
    public void VerifyPriceMinValue()
    {
        //Preço: valor positivo decimal múltiplo de 0.01 e menor que 1.000
        var OrderGenenatorApp = new OrderGeneratorApp();

        bool hasMinValue = false;
        for (int i = 0; i < 999999; i++)
        {
            var value = OrderGenenatorApp.GetRandomNewOrderSingle().Price.getValue();

            if (value == (decimal)0.01)
            {
                hasMinValue = true;
            }
        }

        Assert.True(hasMinValue);
    }

    [Fact]
    public void VerifyPriceMaxValue()
    {
        //Preço: valor positivo decimal múltiplo de 0.01 e menor que 1.000
        var OrderGenenatorApp = new OrderGeneratorApp();

        bool hasMaxValue = false;
        for (int i = 0; i < 999999; i++)
        {
            var value = OrderGenenatorApp.GetRandomNewOrderSingle().Price.getValue();

            if (value == (decimal)999.99)
            {
                hasMaxValue = true;
            }
        }

        Assert.True(hasMaxValue);
    }
}