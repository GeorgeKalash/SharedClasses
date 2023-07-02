using System;

public class NumberToFrenchTextConverter
{
    private static string[] units = { "", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf", "dix", "onze", "douze", "treize", "quatorze", "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf" };
    private static string[] tens = { "", "dix", "vingt", "trente", "quarante", "cinquante", "soixante", "soixante", "quatre-vingt", "quatre-vingt" };

    public static string convertToFrenchText(int number)
    {
        if (number == 0)
        {
            return "zéro";
        }
        else if (number < 0)
        {
            return "moins " + convertToFrenchText(Math.Abs(number));
        }
        else if (number < 20)
        {
            return units[number];
        }
        else if (number < 100)
        {
            int tensDigit = number / 10;
            int unitsDigit = number % 10;

            string text = tens[tensDigit];

            if (unitsDigit > 0)
            {
                if (tensDigit == 8)
                {
                    text += "-";
                }
                else
                {
                    text += "-";
                }

                text += convertToFrenchText(unitsDigit);
            }

            return text;
        }
        else if (number < 1000)
        {
            int hundredsDigit = number / 100;
            int remainingNumber = number % 100;

            string text = units[hundredsDigit] + " cent";

            if (remainingNumber > 0)
            {
                text += " " + convertToFrenchText(remainingNumber);
            }

            return text;
        }
        else if (number < 1000000)
        {
            int thousandsDigit = number / 1000;
            int remainingNumber = number % 1000;

            string text = convertToFrenchText(thousandsDigit) + " mille";

            if (remainingNumber > 0)
            {
                text += " " + convertToFrenchText(remainingNumber);
            }

            return text;
        }
        else
        {
            throw new ArgumentOutOfRangeException("number", "The number is too large to convert.");
        }
    }

    public static string convertToFrenchText(double number)
    {
        int wholePart = (int)number;
        int decimalPart = (int)Math.Round((number - wholePart) * 100);

        string frenchText = convertToFrenchText(wholePart);

        if (decimalPart > 0)
        {
            frenchText += " et " + convertToFrenchText(decimalPart) + " centimes";
        }

        return frenchText;
    }

    public static string currencyText(double number, string _currency, string _decimals)
    {
        int wholePart = (int)number;
        int decimalPart = (int)Math.Round((number - wholePart) * 100);

        string frenchText = string.Format("{0} {1}", convertToFrenchText(wholePart), _currency);

        if (decimalPart > 0)
        {
            frenchText += string.Format(" et {0} {1}", convertToFrenchText(decimalPart) , _decimals);
        }

        return frenchText;
    }
}
