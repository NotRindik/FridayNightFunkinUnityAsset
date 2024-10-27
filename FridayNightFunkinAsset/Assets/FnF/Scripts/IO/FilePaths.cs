using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilePaths
{
    private const string HOME_DIRECTORY_SYMBOL = "~/";

    public static readonly string root = $"{Application.dataPath}/gameData/";

    public static readonly string gameSaves = $"{runtimePath}Save Files/";

    public static readonly string resources_font = "Font/";

    public static readonly string resources_graphics = "Graphics/";
    public static readonly string resources_backgroundImages = $"{resources_graphics}BG Images/";
    public static readonly string resources_images = $"{resources_graphics}images/";
    public static readonly string resources_images_soundTray = $"{resources_images}soundtray/";
    public static readonly string resources_backgroundVideos = $"{resources_graphics}BG Videos/";
    public static readonly string resources_blendTextures = $"{resources_graphics}Transition Effects/";

    public static readonly string resources_audio = "Audio/";
    public static readonly string resources_sfx = $"{resources_audio}SFX/";
    public static readonly string resources_voices = $"{resources_audio}Voices/";
    public static readonly string resources_music = $"{resources_audio}Music/";
    public static readonly string resources_ambience = $"{resources_audio}Ambience/";

    public static readonly string resources_dialogueFile = $"Dialogue Files/";


    public static string GetPathToResources(string defaultPatch,string resourceName)
    {
        if (resourceName.StartsWith(HOME_DIRECTORY_SYMBOL))
            return resourceName.Substring(HOME_DIRECTORY_SYMBOL.Length);

        return defaultPatch + resourceName;
    }


    public static string runtimePath
    {
        get
        {
            #if UNITY_EDITOR
                return "Asset/appdata/";
            #else
                return Application.persistentDataPath + "/appdata/";
            #endif

        }
    }
}
