using UnityEngine;
using System.IO;

public static class GameStaticAccess {
    private static string defaultDataFolderName;
    
    public static string DataFolder {get; private set;}

    public static void SetDefaultDataFolder(){
        DataFolder = Directory.GetCurrentDirectory() +
            Path.DirectorySeparatorChar + defaultDataFolderName +
            Path.DirectorySeparatorChar;
        Directory.CreateDirectory(DataFolder);
    }

    static GameStaticAccess(){
        defaultDataFolderName = "Data";
        SetDefaultDataFolder();
    }
}
