using System;  
using System.IO;  
using System.Security.Cryptography;  
using System.Text;  
  

public class DataCrypto {
    private SymmetricAlgorithm mobjCryptoService;

    public DataCrypto() {
        mobjCryptoService = new RijndaelManaged();  
    }

    private byte[] GetLegalKey() {
        string sTemp = "PuG(%&hY7x69H$yufI0456FtmOT5&fv4UFCy76*h%(HilJ$lhj!y6&(*jkP87jH7";
        mobjCryptoService.GenerateKey();  
        byte[] bytTemp = mobjCryptoService.Key;  
        int KeyLength = bytTemp.Length;  
        if (sTemp.Length > KeyLength)  
            sTemp = sTemp.Substring(0, KeyLength);  
        else if (sTemp.Length < KeyLength)  
            sTemp = sTemp.PadRight(KeyLength, ' ');  
        return ASCIIEncoding.ASCII.GetBytes(sTemp);  
    }

    private byte[] GetLegalIV() {
        string sTemp = "K2oqz*Lao4!rFLfb&95GNU86GfghUb#er57HBh(u%g6HJ($jhWk7&!hg4ui%$hjk";  
        mobjCryptoService.GenerateIV();  
        byte[] bytTemp = mobjCryptoService.IV;  
        int IVLength = bytTemp.Length;  
        if (sTemp.Length > IVLength)  
            sTemp = sTemp.Substring(0, IVLength);  
        else if (sTemp.Length < IVLength)  
            sTemp = sTemp.PadRight(IVLength, ' ');  
        return ASCIIEncoding.ASCII.GetBytes(sTemp);  
    }

    public string Encrypto(string Source) {
        byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);  
        MemoryStream ms = new MemoryStream();  
        mobjCryptoService.Key = GetLegalKey();  
        mobjCryptoService.IV = GetLegalIV();  
        ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();  
        CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);  
        cs.Write(bytIn, 0, bytIn.Length);  
        cs.FlushFinalBlock();  
        ms.Close();  
        byte[] bytOut = ms.ToArray();  
        return Convert.ToBase64String(bytOut);  
    }

    public string Decrypto(string Source)  {
        byte[] bytIn = Convert.FromBase64String(Source);  
        MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);  
        mobjCryptoService.Key = GetLegalKey();  
        mobjCryptoService.IV = GetLegalIV();  
        ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();  
        CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);  
        StreamReader sr = new StreamReader(cs);  
        return sr.ReadToEnd();  
    }
 }  
