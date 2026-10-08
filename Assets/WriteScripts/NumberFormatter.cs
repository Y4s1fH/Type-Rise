using UnityEngine;

public static class NumberFormatter
{
    private static readonly string[] suffixes = { "", "K", "M", "B", "T", "Qa", "Qi" };

    public static string FormatValue(float value)
    {
        if (value < 1000f)
        {
            // 1000-dən kiçikdirsə, kəsr olub-olmadığını yoxlayırıq
            if (value % 1 != 0)
            {
                int whole = Mathf.FloorToInt(value);
                int decimalPart = Mathf.FloorToInt((value - whole) * 10f);
                return whole + "." + decimalPart;
            }
            else
            {
                return Mathf.FloorToInt(value).ToString();
            }
        }

        int suffixIndex = 0;
        while (value >= 1000f && suffixIndex < suffixes.Length - 1)
        {
            value /= 1000f;
            suffixIndex++;
        }

        // F1 və ya F0 yazmadan tam və kəsr hissəsini özümüz riyazi olaraq düzəldirik:
        float truncated = Mathf.Floor(value * 10f) / 10f;
        int mainPart = Mathf.FloorToInt(truncated);
        int fractionalPart = Mathf.FloorToInt((truncated - mainPart) * 10f);

        // Əgər kəsr hissəsi 0-dırsa, sadəcə tam rəqəmi və hərfi qaytar (məs: 5K)
        if (fractionalPart == 0)
        {
            return mainPart + suffixes[suffixIndex];
        }

        // Əgər kəsr varsa, nöqtə ilə birləşdir (məs: 6.7K)
        return mainPart + "." + fractionalPart + suffixes[suffixIndex];
    }
}