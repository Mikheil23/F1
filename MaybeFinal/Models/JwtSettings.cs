namespace MaybeFinal.Models
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public double ExpirationDays { get; internal set; }
    }
}
