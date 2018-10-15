using System.IO;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class GameUtility{
    public static Resolution[] resolutions;
    static GameUtility()
    { 
        resolutions=new Resolution[Screen.resolutions.Length+1];
        Resolution rs = new Resolution();
        rs.width = 1024;
        rs.height = 720;
        resolutions[0] = rs;
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            resolutions[i + 1] = Screen.resolutions[i];
        }
    }


    public static string VectorToStr( Vector3 vec ) {
        return string.Format( "{0}, {1}, {2}", vec.x, vec.y, vec.z );
    }

    public static Vector3 StrToVector3( string vStr ) {
        Vector3 vec = Vector3.zero;
        int vIndex = vStr.IndexOf( "," );
        try{
            vec.x = float.Parse( vStr.Substring( 0, vIndex ) );
            vStr = vStr.Substring( vIndex + 1 );

            vIndex = vStr.IndexOf( "," );
            vec.y = float.Parse( vStr.Substring( 0, vIndex ) );
            vStr = vStr.Substring( vIndex + 1 );

            vec.z = float.Parse( vStr );
        }
        catch( System.Exception ){
            return Vector3.zero;
        }

        return vec;
    }

    public static string GetMD5( byte[] buffer ){
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] binaryResult = md5.ComputeHash( buffer );
        string stringResult = "";
        for ( int i = 0; i < binaryResult.Length; i++ ) {
            stringResult += binaryResult[ i ].ToString( "x2" );
        }
        return stringResult;
    }
}

