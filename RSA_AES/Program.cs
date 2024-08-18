using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Wybierz rodzaj szyfrowania: RSA lub AES");
        string choice = Console.ReadLine();

        if (choice.Equals("RSA", StringComparison.OrdinalIgnoreCase) || choice.Equals("R", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Czy chcesz podać własny tekst do zaszyfrowania? (T/N)");
            char yn = Console.ReadKey().KeyChar;
            Console.WriteLine();

            string plaintext;
            if (char.ToUpper(yn) == 'T')
            {
                Console.Write("Podaj tekst do zaszyfrowania: ");
                plaintext = Console.ReadLine();

                int bytes = GetRSAKeyLength();

                var stopwatch = Stopwatch.StartNew();

                EncryptDecryptRSA(plaintext, bytes);

                stopwatch.Stop();
                Console.WriteLine($"Czas szyfrowania RSA: {stopwatch.ElapsedMilliseconds} milisekund");
            }
            else
            {
                Console.Write("Podaj ilość haseł do zaszyfrowania: ");
                int ilosc = Convert.ToInt32(Console.ReadLine());

                int minTextLength, maxTextLength;
                Console.Write("Podaj minimalną długość tekstu: ");
                minTextLength = Convert.ToInt32(Console.ReadLine());
                Console.Write("Podaj maksymalną długość tekstu: ");
                maxTextLength = Convert.ToInt32(Console.ReadLine());

                int bytes = GetRSAKeyLength();
                int maxTextBytes = (int)Math.Floor((bytes - 384d) / 8);

                if (maxTextLength > maxTextBytes)
                {
                    Console.WriteLine($"Maksymalna długość tekstu dla wybranego klucza RSA ({bytes} bitów) wynosi {maxTextBytes} bajtów, musisz podać maksymalnie {maxTextBytes} .");
                    do
                    {
                        Console.WriteLine("Podaj nową maksymalną długość tekstu:");
                        maxTextLength = Convert.ToInt32(Console.ReadLine());
                    } while (maxTextLength > maxTextBytes);
                }

                var stopwatch = Stopwatch.StartNew();

                for (int i = 0; i < ilosc; i++)
                {
                    plaintext = GenerateRandomText(minTextLength, maxTextLength);
                    EncryptDecryptRSA(plaintext, bytes);
                }


                stopwatch.Stop();
                Console.WriteLine($"Czas szyfrowania RSA: {stopwatch.ElapsedMilliseconds} milisekund");
            }
        }
        else if (choice.Equals("AES", StringComparison.OrdinalIgnoreCase) || choice.Equals("A", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Czy chcesz podać własny tekst do zaszyfrowania? (T/N)");
            char yn = Console.ReadKey().KeyChar;
            Console.WriteLine();

            string plaintext;
            if (char.ToUpper(yn) == 'T')
            {
                Console.Write("Podaj tekst do zaszyfrowania: ");
                plaintext = Console.ReadLine();

                int keyBits = GetAESKeyLength();

                var stopwatch = Stopwatch.StartNew();

                EncryptDecryptAES(plaintext, keyBits);

                stopwatch.Stop();
                Console.WriteLine($"Czas szyfrowania AES: {stopwatch.ElapsedMilliseconds} milisekund");
            }
            else
            {
                Console.Write("Podaj ilość haseł do zaszyfrowania: ");
                int ilosc = int.Parse(Console.ReadLine());

                Console.Write("Podaj minimalną długość tekstu: ");
                int minTextLength = int.Parse(Console.ReadLine());
                Console.Write("Podaj maksymalną długość tekstu: ");
                int maxTextLength = int.Parse(Console.ReadLine());
                int keyBits = GetAESKeyLength();

                var stopwatch = Stopwatch.StartNew();

                for (int i = 0; i < ilosc; i++)
                {
                    string plaintextAES = GenerateRandomText(minTextLength, maxTextLength);
                    EncryptDecryptAES(plaintextAES, keyBits);
                }

                stopwatch.Stop();
                Console.WriteLine($"Czas szyfrowania AES: {stopwatch.ElapsedMilliseconds} milisekund");
            }
        }
        else
        {
            Console.WriteLine("Nieprawidłowy wybór.");
        }
        Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static int GetRSAKeyLength()
    {
        Console.WriteLine("Wybierz długość klucza RSA (w bitach):");
        Console.WriteLine("1. 512");
        Console.WriteLine("2. 1024");
        Console.WriteLine("3. 2048");
        Console.WriteLine("4. 4096");

        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1:
                return 512;
            case 2:
                return 1024;
            case 3:
                return 2048;
            case 4:
                return 4096;
            default:
                Console.WriteLine("Nieprawidłowy wybór. Ustawiam domyślną długość klucza RSA na 1024 bity.");
                return 1024;
        }
    }

    static int GetAESKeyLength()
    {
        Console.WriteLine("Wybierz długość klucza AES (w bitach):");
        Console.WriteLine("1. 128");
        Console.WriteLine("2. 192");
        Console.WriteLine("3. 256");

        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1:
                return 128;
            case 2:
                return 192;
            case 3:
                return 256;
            default:
                Console.WriteLine("Nieprawidłowy wybór. Ustawiam domyślną długość klucza AES na 128 bitów.");
                return 128;
        }
    }

    static void EncryptDecryptRSA(string plaintext, int bytes)
    {

        // Sprawdzenie, czy wartość bytes przekracza maksymalną obsługiwaną długość klucza RSA
        if (bytes < 512 || bytes > 4096)
        {
            Console.WriteLine("Nieprawidłowa długość klucza RSA. Długość klucza musi być w zakresie od 512 do 4096 bitów.");
            return;
        }
        using (var rsa = new RSACryptoServiceProvider(bytes))
        {
            // Generowanie kluczy
            string publicKey;
            string privateKey;
            publicKey = rsa.ToXmlString(false);
            privateKey = rsa.ToXmlString(true);

            Console.WriteLine("Prywatny klucz RSA: ");
            Console.WriteLine(privateKey);
            Console.WriteLine("Publiczny klucz RSA: ");
            Console.WriteLine(publicKey);

            // Szyfrowanie
            byte[] encryptedData;
            byte[] decryptedData;
            encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(plaintext), true);
            string encryptedText = BitConverter.ToString(encryptedData).Replace("-", "");

            Console.WriteLine("Zaszyfrowany tekst (w heksadecymalnym): ");
            Console.WriteLine(encryptedText);

            // Deszyfrowanie
            decryptedData = rsa.Decrypt(encryptedData, true);
            string decryptedText = Encoding.UTF8.GetString(decryptedData);

            Console.WriteLine("Tekst odszyfrowany: ");
            Console.WriteLine(decryptedText);

            // Sprawdzenie, czy tekst odszyfrowany jest taki sam jak tekst oryginalny
            if (plaintext == decryptedText)
            {
                Console.WriteLine("Tekst odszyfrowany jest identyczny z tekstem oryginalnym.");
            }
            else
            {
                Console.WriteLine("Tekst odszyfrowany nie jest identyczny z tekstem oryginalnym.");
            }
        }
    }

    static void EncryptDecryptAES(string plaintext, int keyBits)
    {

        // Sprawdzenie, czy wartość bytes przekracza maksymalną obsługiwaną długość klucza AES
        if (keyBits != 128 && keyBits != 192 && keyBits != 256)
        {
            Console.WriteLine("Nieprawidłowa długość klucza AES. Długość klucza musi być 128, 192 lub 256 bitów.");
            return;
        }
        using (var aes = new AesCryptoServiceProvider())
        {
            aes.KeySize = keyBits;
            aes.GenerateKey();
            aes.GenerateIV();

            byte[] key = aes.Key;
            byte[] iv = aes.IV;

            // Konwersja klucza AES na postać szesnastkową
            string strKey = BitConverter.ToString(key).Replace("-", "");

            Console.WriteLine("Klucz AES (szesnastkowo): ");
            Console.WriteLine(strKey);

            // Konwersja wektora inicjującego (IV) AES na postać szesnastkową
            string strIV = BitConverter.ToString(iv).Replace("-", "");

            Console.WriteLine("Wektor inicjujący (IV) (szesnastkowo): ");
            Console.WriteLine(strIV);

            byte[] encryptedData;
            byte[] decryptedData;

            Console.WriteLine("Tekst oryginalny: ");
            Console.WriteLine(plaintext);

            try
            {
                // Szyfrowanie
                using (var encryptor = aes.CreateEncryptor(key, iv))
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plaintext);
                    }
                    encryptedData = msEncrypt.ToArray();
                }

                // Konwersja zaszyfrowanego tekstu na postać szesnastkową
                string encryptedText = BitConverter.ToString(encryptedData).Replace("-", "");

                Console.WriteLine("Zaszyfrowany tekst (w heksadecymalnym): ");
                Console.WriteLine(encryptedText);

                // Deszyfrowanie
                using (var decryptor = aes.CreateDecryptor(key, iv))
                using (var msDecrypt = new MemoryStream(encryptedData))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    decryptedData = Encoding.UTF8.GetBytes(srDecrypt.ReadToEnd());
                }

                string decryptedText = Encoding.UTF8.GetString(decryptedData);

                Console.WriteLine("Tekst odszyfrowany: ");
                Console.WriteLine(decryptedText);

                // Sprawdzenie, czy tekst odszyfrowany jest taki sam jak tekst oryginalny
                if (plaintext == decryptedText)
                {
                    Console.WriteLine("Tekst odszyfrowany jest identyczny z tekstem oryginalnym.");
                }
                else
                {
                    Console.WriteLine("Tekst odszyfrowany nie jest identyczny z tekstem oryginalnym.");
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Błąd kryptograficzny: " + e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Błąd argumentu: " + e.Message);
            }
        }
    }

    static string GenerateRandomText(int minLength, int maxLength)
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        // Losowa długość tekstu w zakresie od minLength do maxLength
        int length = new Random().Next(minLength, maxLength + 1);

        // Losowa permutacja znaków
        var shuffledCharacters = characters.OrderBy(c => Guid.NewGuid()).ToArray();

        // Wybór pierwszych length znaków z permutacji
        string randomText = new string(shuffledCharacters.Take(length).ToArray());

        return randomText;
    }
}
