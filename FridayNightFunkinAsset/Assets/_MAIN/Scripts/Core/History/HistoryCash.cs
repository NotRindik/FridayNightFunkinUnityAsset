using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

namespace History
{
    public class HistoryCash : MonoBehaviour
    {
        public static Dictionary<string, (object asset, int staleIndex)> loadedAssets = new Dictionary<string, (object asset, int staleIndex)>();

        public static T TryLoadObject<T>(string key)
        {
            object resources = null;

            if (loadedAssets.ContainsKey(key))
                resources = (T)loadedAssets[key].asset;
            else
            {
                resources = Resources.Load(key);
                if (resources != null)
                {
                    loadedAssets[key] = (resources, 0);
                }
            }

            if (resources != null)
            {
                if (resources is T)
                    return (T)resources;
                else
                    Debug.LogWarning($"Retrived object '{key}'wasn`t the expected type!");
            }

            Debug.LogWarning($"Could not load object from cache '{key}'");
            return default;
        }

        public static TMP_FontAsset LoadFont(string key) => TryLoadObject<TMP_FontAsset>(key);
        public static AudioClip LoadAudio(string key) => TryLoadObject<AudioClip>(key);
        public static Texture2D LoadImage(string key) => TryLoadObject<Texture2D>(key);
        public static VideoClip LoadVideo(string key) => TryLoadObject<VideoClip>(key);
    }
}