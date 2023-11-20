namespace SEP_Web.Helper.Authentication;
public class Cryptography
{
    public static string EncryptPassword(string pass)
    {
        return BCrypt.Net.BCrypt.HashPassword(pass); // HashPassword utiliza o esquema OpenBSD BCrypt e geração de salto utilizando o método BCrypt.Net.BCrypt.GenerateSalt() para gerar o hash da senha; 
    }

    public static bool VerifyPasswordEncrypted(string password, string hashedPassword)
    {
        // VerifyPasswordEncrypted recebe por parâmetro duas strings que correspondem respectivamente a uma senha dada como entrada por um usuário, e um hash recuperado da base de dados para compará-las;
        
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword); // O método Verify retorna true ou false ao fazer uma comparação dada com base no "match" da senha informada e o "salt" gerado pela hash recuperada;
    }
}
