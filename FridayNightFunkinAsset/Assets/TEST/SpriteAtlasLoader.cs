using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

[ExecuteAlways]
public class SpriteAtlasLoader : MonoBehaviour
{
    public string xmlFileName = "checkboxThingie.xml"; // Имя XML-файла в StreamingAssets
    public string textureFileName = "checkboxThingie.png"; // Имя текстуры
    public SerializedDictionary<string, Sprite> sprites = new SerializedDictionary<string, Sprite>();

    public bool isSave;

    void Update()
    {
        LoadSprites();
    }

    void LoadSprites()
    {
        if (isSave)
        {
            string xmlPath = Path.Combine(Application.streamingAssetsPath, xmlFileName);
            string texturePath = Path.Combine(Application.streamingAssetsPath, textureFileName);

            // Загружаем текстуру
            byte[] fileData = File.ReadAllBytes(texturePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            texture.filterMode = FilterMode.Point; // Оставляем пиксельное качество

            // Читаем XML
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);

            XmlNodeList subTextures = xmlDoc.SelectNodes("//SubTexture");
            foreach (XmlNode node in subTextures)
            {
                string name = node.Attributes["name"].Value;
                int x = int.Parse(node.Attributes["x"].Value);
                int y = int.Parse(node.Attributes["y"].Value);
                int width = int.Parse(node.Attributes["width"].Value);
                int height = int.Parse(node.Attributes["height"].Value);

                // Создаём спрайт
                Rect rect = new Rect(x, texture.height - y - height, width, height);
                Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 100);
                sprites[name] = sprite;
            }

            Debug.Log("Загружено " + sprites.Count + " спрайтов");
            isSave = false;
        }
    }

    public Sprite GetSprite(string name)
    {
        return sprites.ContainsKey(name) ? sprites[name] : null;
    }
}
