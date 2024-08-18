Key features:

Encryption algorithm selection:

The user can choose between the RSA (Rivest-Shamir-Adleman) and AES (Advanced Encryption Standard) algorithm.
RSA encryption:

Custom text encryption: The user can enter their own text, which will be encrypted using RSA.
Random text generation: The user can specify the number of random texts to be generated and encrypted, with the possibility to set the minimum and maximum length of the generated text.
The software supports different RSA key lengths (512, 1024, 2048 or 4096 bits).
AES encryption:

Encryption of custom text: As with RSA, the user can enter their own text for encryption with AES.
Random text generation: The user can generate multiple random texts for encryption with AES, with specific length parameters.
The software supports different AES key lengths (128, 192 or 256 bits).
Measurement of execution time:

The application measures and displays the time taken to perform the encryption and decryption processes for both algorithms.
Key management:

For RSA, the application generates and displays the public and private keys used for encryption and decryption.
For AES, the programme generates and displays the encryption key and initialisation vector (IV) in hexadecimal format.
Text validation:

After encryption and decryption, the application checks that the decrypted text matches the original text.

Language:

The program is written in C# and uses the System.Security.Cryptography namespace for cryptographic operations. 
It also uses other standard C# libraries such as System.IO, System.Diagnostics and System.Text to handle file operations, 
performance measurement and text encoding.

Objective:

The application provides a simple demonstration of encryption and decryption using the RSA and AES algorithms. 
It allows users to experiment with different key lengths and see the impact on encryption time. 
It is also a practical example of learning how to implement cryptographic operations in C#.

Usage:

To use the application, compile and run the program in .NET. Then simply follow the instructions, selecting an encryption method 
and enter your own text or let the application generate random text for encryption. 
The application will display the keys, the encrypted text and the decrypted text for verification.

![rsa1](https://github.com/user-attachments/assets/c862ec78-8b75-4ef1-b5a3-20918f7c0456)
![rsa2](https://github.com/user-attachments/assets/44b23173-47f3-4165-b7aa-f7d597ba05f1)
![rsa3](https://github.com/user-attachments/assets/aac204f5-f362-4382-a831-dcb6e0a32bbc)
![rsa33](https://github.com/user-attachments/assets/3440c090-5e71-48ea-8782-0b997523bf0b)
