using System;
using System.Collections.Generic;
using System.Text;

public static class FrenchNumberConverter
{
    private static string[] units = {
        "zéro", "un", "deux", "trois", "quatre", "cinq",
        "six", "sept", "huit", "neuf", "dix", "onze", "douze",
        "treize", "quatorze", "quinze", "seize"
    };

    private static string[] tens = {
        "", "", "vingt", "trente", "quarante",
        "cinquante", "soixante", "soixante-dix",
        "quatre-vingt", "quatre-vingt-dix"
    };

    private static string[] powersOfTen = {
        "", "mille", "million", "milliard", "billion", "billiard"
    };

    public static string ConvertToText(double number)
    {
        int wholePart = (int)number;
        int decimalPart = (int)((number - wholePart) * 100);

        string result = ConvertToText(wholePart);

        if (decimalPart > 0)
        {
            result += " virgule " + ConvertToText(decimalPart);
        }

        return result;
    }

    private static string ConvertToText(int number)
    {
        if (number < 0 || number > 999_999_999)
            throw new ArgumentOutOfRangeException("Number out of range (0-999,999,999)");

        if (number == 0)
            return units[0];

        List<string> groups = new List<string>();

        while (number > 0)
        {
            groups.Add(ConvertToTextGroup(number % 1000));
            number /= 1000;
        }

        StringBuilder result = new StringBuilder();

        for (int i = groups.Count - 1; i >= 0; i--)
        {
            if (!string.IsNullOrEmpty(groups[i]))
            {
                result.Append(groups[i]);
                result.Append(" ");
                result.Append(powersOfTen[i]);
                result.Append(" ");
            }
        }

        return result.ToString().Trim();
    }

    private static string ConvertToTextGroup(int number)
    {
        StringBuilder result = new StringBuilder();

        // hundreds
        if (number >= 100)
        {
            int hundreds = number / 100;
            result.Append(units[hundreds]);
            result.Append(" cent ");
            number %= 100;
        }

        // tens and units
        if (number >= 17)
        {
            int tensIndex = number / 10;
            result.Append(tens[tensIndex]);

            int unitsIndex = number % 10;
            if (unitsIndex != 1 && unitsIndex != 0)
            {
                result.Append("-");
                result.Append(units[unitsIndex]);
            }
            else if (unitsIndex == 1)
            {
                result.Append(" et un");
            }
        }
        else
        {
            result.Append(units[number]);
        }

        return result.ToString().Trim();
    }

    public static string currencyText(double number, string _currency, string _decimals)
    {
        int wholePart = (int)number;
        int decimalPart = (int)Math.Round((number - wholePart) * 100);

        string frenchText = string.Format("{0} {1}", ConvertToText(wholePart), _currency);

        if (decimalPart > 0)
        {
            frenchText += string.Format(" et {0} {1}", ConvertToText(decimalPart), _decimals);
        }

        return frenchText;
    }

}
