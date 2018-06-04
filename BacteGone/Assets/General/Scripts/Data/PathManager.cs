﻿using UnityEngine;
using System.Collections;
using System.IO;

public static class PathManager
{
    private static string _dlc;

    public static string DLC
    {
        get
        {
            _dlc = Application.persistentDataPath + "/DLC/";

            if (!Directory.Exists(_dlc))
            {
                Directory.CreateDirectory(_dlc);
            }
            return _dlc;
        }
    }
}

public class GameTags
{
    public const string SettingDataKey = "SettingDataKey";
}

public class SceneName
{
    public const string Splash = "Splash";
    public const string Home = "Home";
    public const string Empty = "Empty";
}