using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EventsRepublic.Models.Mpesa
{
    public class RSAClass
    {
        
        public static string loadsslx509(string password)
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();

            string certificate = @"E:\Safcomcert\certg.cer";
            //load cert to a X509 
            //X509Certificate cert = X509Certificate.CreateFromCertFile(certificate);
            
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048))
            {
                /**get an instance of RSAParameters from ExportParameters function
                RSAParameters rsakeyinfo = RSA.ExportParameters(false);
                rsakeyinfo.Modulus = cert.GetPublicKey();
                var kj =Convert.ToBase64String(cert.GetPublicKey());
                //Import key parameters into RSA.
                RSA.ImportParameters(rsakeyinfo);
               return Convert.ToBase64String(RSA.Encrypt(Encoding.UTF8.GetBytes(password), RSAEncryptionPadding.Pkcs1)); 
    **/
                var plainTextBytes = Encoding.UTF8.GetBytes(password);
                PemReader pr = new PemReader(File.OpenText(certificate));
                X509Certificate cert = (X509Certificate)pr.ReadObject();
                //PKCS1 v1.5 paddings
                Pkcs1Encoding eng = new Pkcs1Encoding(new RsaEngine());
                eng.Init(true,cert.GetPublicKey());
                int length = plainTextBytes.Length;
                int blockSize = eng.GetInputBlockSize();
                List<byte> cipherTextBytes = new List<byte>();
                for (int chunkPosition = 0;
                    chunkPosition < length;
                    chunkPosition += blockSize)
                {
                    int chunkSize = Math.Min(blockSize, length - chunkPosition);
                    cipherTextBytes.AddRange(eng.ProcessBlock(
                        plainTextBytes, chunkPosition, chunkSize
                    ));
                }
                return Convert.ToBase64String(cipherTextBytes.ToArray());

            }
        }
        public static string RsaEncrypt(byte[]passwordToEncrypt, RSACryptoServiceProvider rsa)
        {
            byte[] encryptedsecuritycred;
            try
            {
              
               encryptedsecuritycred = rsa.Encrypt(passwordToEncrypt,false);
            }
            catch (Exception)
            {

                throw;
            }
            var h = Convert.ToBase64String(encryptedsecuritycred);
            return h;
        }
    }
}
