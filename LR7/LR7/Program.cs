using System;

namespace LR7
{
    class Program
    {
        static void Main(string[] args)
        {
            SignVerify();
            //EncryptDecrypt();
        }

        public static void EncryptDecrypt()
        {
            var message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.";
            EllipticCurve ellipticCurve = new EllipticCurve();
            ECDSA ellipticCurveDsa = new ECDSA(ellipticCurve);

            var keyPair = ellipticCurveDsa.GenerateKeyPair();
            var cypher = ECIE.Encryption(message, ellipticCurve, keyPair.publicKey);

            var decrypt = ECIE.Decryption(cypher, keyPair.privateKey, ellipticCurve);

            Console.WriteLine(decrypt);

        }

        public static void SignVerify()
        {
            EllipticCurve ellipticCurve = new EllipticCurve();
            ECDSA ellipticCurveDsa = new ECDSA(ellipticCurve);

            var keyPair = ellipticCurveDsa.GenerateKeyPair();

            string message = "Secret Message on Seven Labaratory Work";
            var signature = ellipticCurveDsa.SignMessage(keyPair.privateKey, message);

            var verification = ellipticCurveDsa.VerifySignature(keyPair.publicKey, message, signature);
            Console.WriteLine(verification);

            string message1 = "Hello world!!";

            var verification1 = ellipticCurveDsa.VerifySignature(keyPair.publicKey, message1, signature);
            Console.WriteLine(verification1);
        }
    }
}
