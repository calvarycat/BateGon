using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Utility
{
    public static void DeleteFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath))
            return;

        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

        foreach (FileInfo file in directoryInfo.GetFiles())
        {
            file.Delete();
        }

        foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
        {
            directory.Delete(true);
        }
    }

    public static string GetMd5Hash(string input)
    {
        using (MD5 md5Hash = MD5.Create())
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }

    public static string StrikeThrough(string s)
    {
        string strikethrough = "";

        foreach (char c in s)
        {
            strikethrough = strikethrough + c + '\u0336';
        }

        return strikethrough;
    }

    public static Vector2 ConvertScreenPositionToCanvasPosition(Canvas canvas, Vector2 screenPosition)
    {
        Vector2 result = screenPosition;
        result.x -= Screen.width / 2f;
        result.y -= Screen.height / 2f;
        result /= canvas.scaleFactor;
        return result;
    }

    public static int RandomEnum<T>() where T : struct, IComparable, IConvertible, IFormattable
    {
        if (typeof(T).IsEnum)
            return Random.Range(0, Enum.GetNames(typeof(T)).Length);
        return 0;
    }

    public static Vector2 RandomVector2(Vector2 min, Vector2 max)
    {
        return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
    }
}