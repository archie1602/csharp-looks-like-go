namespace util;

static class StringExtensions
{
    extension(string str)
    {
        public string RequireNonEmpty(string fieldName)
        {
            var trimmed = str.Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
                throw new ArgumentException($"{fieldName} is required");
            return trimmed;
        }
    }
}
