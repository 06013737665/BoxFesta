using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Box.Festa.Negocio
{
    public class Sha1BO
    {
        public static string RetornarSHA(string Senha)
        {
            using (SHA1 shaHash = SHA1.Create())
            {
                return RetonarHash(shaHash, Senha);
            }
        }

        public static bool ComparaSHA(string senhabanco, string Senha_SHA)
        {
            using (SHA1 shaHash = SHA1.Create())
            {
                var senha = RetornarSHA(senhabanco);
                if (VerificarHash(shaHash, Senha_SHA, senha))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private static string RetonarHash(SHA1 shaHash, string input)
        {
            byte[] data = shaHash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        private static bool VerificarHash(SHA1 shaHash, string input, string hash)
        {
            StringComparer compara = StringComparer.OrdinalIgnoreCase;

            if (0 == compara.Compare(input, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}