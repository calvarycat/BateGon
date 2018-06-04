using System.Collections.Generic;
using LitJson;
using UnityEngine;

public static class Preferences
{
    public static void LoadPreferences()
    {
        LoadSetting();
    }

    /// <summary>
    ///     Setting
    /// </summary>
    public static Setting CurrentSetting;

    private static Setting LoadSetting()
    {
        CurrentSetting = SaveGameManager.LoadData<Setting>(GameTags.SettingDataKey);
        if (CurrentSetting == null)
        {
            CurrentSetting = new Setting();
            SaveSetting();
        }
        return CurrentSetting;
    }

    public static void SaveSetting()
    {
        SaveGameManager.SaveData(GameTags.SettingDataKey, CurrentSetting);
    }
}

public class Setting
{
    public bool EnableSound;

    public Setting()
    {
        EnableSound = true;
    }
}