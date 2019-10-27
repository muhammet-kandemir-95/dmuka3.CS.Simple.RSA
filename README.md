# dmuka3.CS.Simple.RSA

 This library provides you to encrypt/decrypt data with rsa algorithm.
 
 ## Example 1
 
  We will encrypt a text data with public key. Then, we will decrypt it using private key. Firstly, we need a RSA key which includes private key and public key. Afterwards, we will create a new RSA key via previous public key. Let's do it.
  
  ```csharp
// Creating a new RSA key randomly.
var rsaKey = new RSAKey(2048);
// Our text which will be encrypted.
string s = "Hello World";
// Text data is converted byte[].
var data = Encoding.UTF8.GetBytes(s);
// Created a new RSA key with public key and then encrypted data using it.
var e = new RSAKey(rsaKey.PublicKey).Encrypt(data);
// Decrypted data using first RSA key.
var d = Encoding.UTF8.GetString(rsaKey.Decrypt(e));
  ```
  
   It looks like so easy. That's right. Now, we will try another scenario. The scenario is to encrypt big data. What does it mean? You know if you are using 2048 bit RSA key, it means you can encrypt as many data as RSA key bit size. But our data is fairly big. What will we do to fix?
   
   For instance our RSA key's size is 2048(256 Byte) and our data's size is 5.000 byte. According to this data we will divide our data and for each part of data will be encrypted invidually. Thus, we will have some encrypted datas. For the decrypting, we will combine them in a generic list.
   
 ## Example 2
 
```csharp
// Creating a new RSA key randomly.
var rsaKey = new RSAKey(2048);
// Our text which will be encrypted.
for (int i = 0; i < 60000; i++)
    sb.Append(i.ToString());
string s = sb.ToString();
// Text data is converted byte[].
var data = Encoding.UTF8.GetBytes(s);
// Created a new RSA key with public key and then encrypted data using it.
var e = new RSAKey(rsaKey.PublicKey).Encrypt(data);
// Decrypted data using first RSA key.
var d = Encoding.UTF8.GetString(rsaKey.Decrypt(e));
  ```
