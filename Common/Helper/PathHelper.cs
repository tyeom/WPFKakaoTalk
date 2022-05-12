using System;
using System.IO;

namespace Common.Helper;

public class PathHelper
{
    static PathHelper()
    {
        if (Directory.Exists(GetLocalDirectory()) is false)
        {
            Directory.CreateDirectory(GetLocalDirectory());
        }
    }

    public static string GetLocalDirectory(string fileName)
    {
        return $"{System.AppDomain.CurrentDomain.BaseDirectory}Data\\{fileName}";
    }

    public static string GetLocalDirectory(string subDir, string fileName)
    {
        return $"{System.AppDomain.CurrentDomain.BaseDirectory}\\{subDir}\\{fileName}";
    }

    public static string GetLocalDirectory()
    {
        return System.AppDomain.CurrentDomain.BaseDirectory + "Data";
    }

    public static string GetLocalImagesDirectory()
    {
        return $"{System.AppDomain.CurrentDomain.BaseDirectory}\\Images\\";
    }
}