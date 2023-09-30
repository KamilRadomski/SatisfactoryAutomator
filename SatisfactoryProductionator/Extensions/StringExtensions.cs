namespace SatisfactoryProductionator.Extensions
{
    public static class StringExtensions
    {
        public static string FormatCategoryName(this string name)
        {
            for (int i = name.Length - 1; i > 0; i--)
            {
                if (char.IsUpper(name[i]))
                {
                    name = name.Insert(i, ".");
                }
            }
            return name;
        }

        public static string FormatDisplayName(this string name)
        {
            return name.Replace(" - ", "-").Replace(" ", ".").Replace("..", ".");
        }

        public static string CondenseDisplayName(this string name)
        {
            return name.Replace("Inverted.", "Inv.");
        }
    }
}
