namespace SocialNetwork.Core.Users
{
    public static class StringNormalizer
    {
        public static string Normalize(string s) => s.ToUpperInvariant();
    }
}