using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Data;
using FileHelpers;
using System.Web;
using System.Security.Cryptography;

namespace VerikaiSolution
{
    class Program
    {

        public static string GetTSV(string url)
        {
            string fileName = Path.Combine(Path.GetTempPath(), "data.tsv");
            WebClient aWebClient = new WebClient();
            aWebClient.DownloadFile(url, fileName);
            return fileName;
        }

        public static void SplitCSV()
        {
            var engine = new FileHelperAsyncEngine<Data>();

            string fileName = GetTSV("http://devchallenge.verikai.com/data.tsv");
            List<Data> csv = new List<Data>();
            using (engine.BeginReadFile(fileName))
            {
                foreach (Data csvData in engine)
                {
     
                    var today = DateTime.Today;
                    var dob = DateTime.Parse(csvData.dob);

                    // Calculate the age.
                    var age = today.Year - dob.Year;

                    // In case of leap year
                    if (dob.Date > today.AddYears(-age)) age--;
                    csvData.age = age;
                    csvData.cost = 0;
                    csv.Add(csvData);
                }
            }
            File.Delete(fileName);
            //give file a name and header text
            string uploadFilename = Path.Combine(Path.GetTempPath(), "unencrypted.tsv");
            engine.HeaderText = engine.GetFileHeader();
            
            using (engine.BeginWriteFile(uploadFilename))
            {
                foreach (Data csvData in csv)
                {
                    engine.WriteNext(csvData);
                }
            }

            Console.WriteLine("--------------------------------------------Task 1-------------------------------------------------------------\n");
            Console.WriteLine("Processed/Normalzied file outputted to unencrypted.tsv, Refer the path below to look up file");
            Console.WriteLine(uploadFilename);

            var sw = new StringWriter();

            using (engine.BeginWriteStream(sw))
            {
                foreach (Data csvData in csv)
                {
                    engine.WriteNext(csvData);
                }
            }

            string inputFile = sw.GetStringBuilder().ToString();
            //Console.WriteLine(sw.GetStringBuilder().ToString());

            //Encrypting the file
            Aes myAes = Aes.Create();
            myAes.Mode = CipherMode.CBC;
            myAes.Padding = PaddingMode.PKCS7;
            myAes.Key = Convert.FromBase64String("AXe8YwuIn1zxt3FPWTZFlAa14EHdPAdN9FaZ9RQWihc=");
            myAes.IV = Convert.FromBase64String("bsxnWolsAyO7kCfWuyrnqg==");
            byte[] encrypted = EncryptStringToBytes_Aes(inputFile, myAes.Key, myAes.IV);
            string output = Convert.ToBase64String(encrypted);


            Console.WriteLine("--------------------------------------------Task 2-------------------------------------------------------------\n");
            Console.WriteLine("Output the encrypted text");
            Console.WriteLine(output);

            //Decrypting the file 
            var decryptedString = DecryptString(output);
            //Console.WriteLine(decryptedString);


        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        public static string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String("AXe8YwuIn1zxt3FPWTZFlAa14EHdPAdN9FaZ9RQWihc=");
                aes.IV = Convert.FromBase64String("bsxnWolsAyO7kCfWuyrnqg==");
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }


        static void Main(string[] args)
        {

            SplitCSV();
            Console.ReadKey();
        }
    }
}
