using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FridayNightFunkin
{
    public class SceneLoad : MonoBehaviour
    {
        AsyncOperation asyncOperation;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Image image;
        public static SceneLoad instance { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void StartLoad(int SceneID)
        {
            StartCoroutine(LoadSceneCor(SceneID));
        }
        public void StartLoad(string sceneName)
        {
            StartCoroutine(LoadSceneCor(sceneName));
        }
        public void StartLoad(Scene scene)
        {
            StartCoroutine(LoadSceneCor(scene));
        }
        IEnumerator LoadSceneCor(Scene scene)
        {
            var index = Random.Range(0, sprites.Length);
            image.color = new Color(255, 255, 255, 255);
            image.sprite = sprites[index];
            asyncOperation = SceneManager.LoadSceneAsync(scene.buildIndex);
            while (!asyncOperation.isDone)
            {
                float progress = asyncOperation.progress / 0.9f;
                yield return 0;
            }
        }

        IEnumerator LoadSceneCor(int SceneID)
        {
            var index = Random.Range(0, sprites.Length);
            image.color = new Color(255, 255, 255, 255);
            image.sprite = sprites[index];
            asyncOperation = SceneManager.LoadSceneAsync(SceneID);
            while (!asyncOperation.isDone)
            {
                float progress = asyncOperation.progress / 0.9f;
                yield return 0;
            }
        }
        IEnumerator LoadSceneCor(string sceneName)
        {
            var index = Random.Range(0, sprites.Length);
            image.color = new Color(255, 255, 255, 255);
            image.sprite = sprites[index];
            asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            while (!asyncOperation.isDone)
            {
                float progress = asyncOperation.progress / 0.9f;
                yield return 0;
            }
        }

    }
}