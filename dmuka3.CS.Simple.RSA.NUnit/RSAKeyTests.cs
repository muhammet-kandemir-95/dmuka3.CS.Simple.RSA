using NUnit.Framework;
using System.Text;

namespace dmuka3.CS.Simple.RSA.NUnit
{
    public class RSAKeyTests
    {
        [Test]
        public void NormalData()
        {
            var rsaKey = new RSAKey(2048);
            string s = "Hello World";
            var data = Encoding.UTF8.GetBytes(s);
            var e = new RSAKey(rsaKey.PublicKey).Encrypt(data);
            var d = Encoding.UTF8.GetString(rsaKey.Decrypt(e));
            Assert.AreEqual(d, s);
        }

        [Test]
        public void BigData()
        {
            var rsaKey = new RSAKey(2048);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 6000; i++)
                sb.Append((i % 10).ToString());
            string s = sb.ToString();
            var data = Encoding.UTF8.GetBytes(s);
            var e = new RSAKey(rsaKey.PublicKey).Encrypt(data);
            var d = Encoding.UTF8.GetString(rsaKey.Decrypt(e));
            Assert.AreEqual(d, s);
        }
    }
}