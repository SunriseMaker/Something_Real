public static class __Strings
{
    public static string Quote(string s)
    {
        return Quote(s, "\"");
    }

    public static string Quote(string s, string quotes)
    {
        return Quote(s, quotes, quotes);
    }

    public static string Quote(string s, string left_quote, string right_quote)
    {
        return left_quote + s.Trim() + right_quote;
    }
}
