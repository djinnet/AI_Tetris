using AITetris.Classes;
using AITetris.Stores;
using System;
using System.IO;
using System.Text.Json;

namespace AITetris.Services;

/// <summary>
/// This service is dealing with read and write json (input/output)
/// </summary>
public class JsonIOService
{
    public static bool Write<T>(T value) where T : class
    {
        try
        {
            string? path = FileStore.SettingJsonFileLocation;
            if (string.IsNullOrEmpty(path)) return false;
            string json = JsonSerializer.Serialize(value, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(path, json);
            return true;
        }
        catch (Exception)
        {
            //log exception
            return false;
        }
    }

    public static string? Read()
    {
        // Try to read the settings JSON file
        string? path = FileStore.SettingJsonFileLocation;
        if(string.IsNullOrEmpty(path)) return string.Empty;
        return File.ReadAllText(path);
    }

    public static bool CheckSetting(Settings? settings = null)
    {
        try
        {
            //this checks if the file exists. Much better approach than try-catch approach for exception if file didnt exists or anything related to that.
            if (File.Exists(FileStore.SettingJsonFileLocation))
            {
                string? jsonvalue = Read();
                //Here we could have used the jsonvalue to deserialize to settings and we can write it back into the file again.
                //or we simply check if the json string value isnt empty or null.
                return !string.IsNullOrEmpty(jsonvalue);
            }
            else
            {
                //if setting file doesnt exists, then we makes one
                settings ??= new Settings();
                return Write(settings);
            }
        }
        catch (Exception)
        {
            //log the exception here.

            return false;
        }
    }
}
