using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AES_256 : MonoBehaviour
{
    // 키로 사용하기 위한 암호 정의
    private static readonly string PASSWORD = "3ds1s334e4dcc7c4yz4554e732983h";

    // 인증키 정의
    private static readonly string KEY = PASSWORD.Substring(0, 128 / 8);

    // 암호화
    public static string AESEncrypt128(string plain)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plain);

        RijndaelManaged myRijndael = new RijndaelManaged();
        myRijndael.Mode = CipherMode.CBC;
        myRijndael.Padding = PaddingMode.PKCS7;
        myRijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream();

        ICryptoTransform encryptor = myRijndael.CreateEncryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));

        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
        cryptoStream.FlushFinalBlock();

        byte[] encryptBytes = memoryStream.ToArray();

        string encryptString = Convert.ToBase64String(encryptBytes);

        cryptoStream.Close();
        memoryStream.Close();

        return encryptString;
    }

    // 복호화
    public static string AESDecrypt128(string encrypt)
    {
        byte[] encryptBytes = Convert.FromBase64String(encrypt);

        RijndaelManaged myRijndael = new RijndaelManaged();
        myRijndael.Mode = CipherMode.CBC;
        myRijndael.Padding = PaddingMode.PKCS7;
        myRijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream(encryptBytes);

        ICryptoTransform decryptor = myRijndael.CreateDecryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));

        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

        byte[] plainBytes = new byte[encryptBytes.Length];

        int plainCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

        string plainString = Encoding.UTF8.GetString(plainBytes, 0, plainCount);

        cryptoStream.Close();
        memoryStream.Close();

        return plainString;
    }
    private void Start()
    {

        string asdasd = GameManager.gameManager_Instance.squad.ToString();
        Debug.Log("asdasda : " + asdasd);
        string str = "원본 문자 정보";
        Debug.Log("plain : " + str);
        //bin파일로 저장
        SaveSystem.Save<string>(str, Application.dataPath + "/str.bin");
        //저장된 bin파일 
        string goldFilePath = Application.dataPath + "/str.bin";
        string str1 = File.ReadAllText(goldFilePath);
        //bin파일 내용 암호화
        string str2 = AESEncrypt128(str1);
        Debug.Log("AES128 encrypted : " + str2);
        //암호화된 내용 텍스트파일로 저장
        string filepath = Application.dataPath + "/str.txt";
        System.IO.File.WriteAllText(filepath, str2, Encoding.Default);


        /*//암호화된 텍스트파일 읽어오기
        string filepath2 = Application.dataPath + "/str.txt";
        string encrypted_file = System.IO.File.ReadAllText(filepath2);
        //복호화
        string str3 = AESDecrypt128(encrypted_file);
        Debug.Log("AES128 decrypted : " + str3);

        byte[] byteArray = Encoding.UTF8.GetBytes(str3);
        MemoryStream stream = new MemoryStream(byteArray);

        BinaryFormatter formatter = new BinaryFormatter();

        string data = (string)formatter.Deserialize(str3);

        Debug.Log("bin decrypted : " + data);*/
    }
}
