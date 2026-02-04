namespace Pustok.Business.Dtos;

public class JWTOptionsDto
{
    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiredDate { get; set; }
}
