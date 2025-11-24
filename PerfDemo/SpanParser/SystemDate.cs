// PSEUDOCODE / PLAN (detailed):
// - Define a record struct `SystemDate` with fields: Year, Month, Day, Hour, Minute, Second.
// - Provide `ToString()` for convenient display.
// - Provide `Parse(string input)` that throws a FormatException on failure.
// - Provide `ParseFast(string input)` that uses spans and throws on failure (mirrors Point3D style).
// - Implement a `TryParseFast(ReadOnlySpan<char> input, out SystemDate result)` that returns bool and fills result:
//   - Trim the input span.
//   - If the trimmed span contains a 'T' and matches the exact layout "YYYY-MM-DDTHH:MM:SS" (length 19):
//       - Validate separators at expected positions ('-' at 4 and 7, 'T' at 10, ':' at 13 and 16).
//       - Parse 4-digit year and 2-digit month, day, hour, minute, second by converting chars to integers (no allocations).
//       - Validate basic ranges (month 1-12, day 1-31, hour 0-23, minute 0-59, second 0-59).
//   - Else if the span matches "HH:MM:SS" (length 8):
//       - Validate ':' separators at 2 and 5.
//       - Parse hour, minute, second as two-digit numbers.
//       - Set Year/Month/Day to 0 (or default) to indicate time-only value.
//   - Else return false.
// - Implement small helper functions to parse 2 and 4 digits and to validate ranges.
// - `ParseFast` and `Parse` call `TryParseFast` and throw `FormatException` if it returns false.
// - Keep implementation allocation-free (use ReadOnlySpan<char> and simple digit math).
//
// This file implements the above plan.

using System;

namespace PerfDemo.Examples;

public record struct SystemDate(int Year, int Month, int Day, int Hour, int Minute, int Second)
{
    public override string ToString()
    {
        if (Year != 0 || Month != 0 || Day != 0)
            return $"{Year:D4}-{Month:D2}-{Day:D2}T{Hour:D2}:{Minute:D2}:{Second:D2}";
        return $"{Hour:D2}:{Minute:D2}:{Second:D2}";
    }

    public static SystemDate ParseFast(string input)
    {
        if (TryParseFast(input, out var dt))
            return dt;
        throw new FormatException("Input in incorrect format");
    }

    public static bool TryParseFast(ReadOnlySpan<char> input, out SystemDate result)
    {
        result = default;
        var s = input.Trim();
        if (s.Length == 0)
            return false;

        // ISO full datetime: "YYYY-MM-DDTHH:MM:SS" (length 19)
        if (s.Length == 19 && s[4] == '-' && s[7] == '-' && s[10] == 'T' && s[13] == ':' && s[16] == ':')
        {
            if (!Parse4Digits(s, 0, out int year)) return false;
            if (!Parse2Digits(s, 5, out int month)) return false;
            if (!Parse2Digits(s, 8, out int day)) return false;
            if (!Parse2Digits(s, 11, out int hour)) return false;
            if (!Parse2Digits(s, 14, out int minute)) return false;
            if (!Parse2Digits(s, 17, out int second)) return false;

            if (!ValidateDateTimeParts(year, month, day, hour, minute, second)) return false;

            result = new SystemDate(year, month, day, hour, minute, second);
            return true;
        }

        // Time-only: "HH:MM:SS" (length 8)
        if (s.Length == 8 && s[2] == ':' && s[5] == ':')
        {
            if (!Parse2Digits(s, 0, out int hour)) return false;
            if (!Parse2Digits(s, 3, out int minute)) return false;
            if (!Parse2Digits(s, 6, out int second)) return false;

            if (!ValidateTimeParts(hour, minute, second)) return false;

            result = new SystemDate(0, 0, 0, hour, minute, second);
            return true;
        }

        // Could add more tolerant parsing (variable widths) here if desired.
        return false;
    }

    private static bool Parse2Digits(ReadOnlySpan<char> s, int pos, out int value)
    {
        value = 0;
        if (pos + 2 > s.Length) return false;
        char c1 = s[pos];
        char c2 = s[pos + 1];
        if (c1 < '0' || c1 > '9' || c2 < '0' || c2 > '9') return false;
        value = (c1 - '0') * 10 + (c2 - '0');
        return true;
    }

    private static bool Parse4Digits(ReadOnlySpan<char> s, int pos, out int value)
    {
        value = 0;
        if (pos + 4 > s.Length) return false;
        for (int i = 0; i < 4; i++)
        {
            char c = s[pos + i];
            if (c < '0' || c > '9') return false;
            value = value * 10 + (c - '0');
        }
        return true;
    }

    private static bool ValidateTimeParts(int hour, int minute, int second)
    {
        if (hour < 0 || hour > 23) return false;
        if (minute < 0 || minute > 59) return false;
        if (second < 0 || second > 59) return false;
        return true;
    }

    private static bool ValidateDateTimeParts(int year, int month, int day, int hour, int minute, int second)
    {
        if (year < 1 || year > 9999) return false;
        if (month < 1 || month > 12) return false;
        if (day < 1 || day > 31) return false; // basic check; not validating month-specific days/leap years here
        return ValidateTimeParts(hour, minute, second);
    }
}   