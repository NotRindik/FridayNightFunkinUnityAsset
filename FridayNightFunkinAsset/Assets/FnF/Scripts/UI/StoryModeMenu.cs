using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using AYellowpaper.SerializedCollections;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public enum DifficultLevel
{
    Easy,Normal,Hard,Erect,Nightmare
}

public interface IChangeDifficulty
{
    public void ChangeDifficult()
    {
    }
}

namespace FridayNightFunkin.UI
{
    public class StoryModeMenu : MenuBehaviour,IChangeDifficulty
    {
        [System.Serializable]
        public class LevelConfig
        {
            public string[] tracks;
            public string levelName;
            public SceneAsset levelScene;

            public Color backgroundColor = Color.white;
            public Sprite backGroundSpite;

            [SerializedDictionary("Id[-1;1]", "imageCharacters")]
            public SerializedDictionary<sbyte, GameObject> imageCharacters = new SerializedDictionary<sbyte, GameObject>();
        }
        [SerializeField] public LevelConfig[] levelConfigs;
        [SerializeField] private DifficultLevel difficult;
        [SerializeField] private RectTransform TrackContainer;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI LevelNameText;
        [SerializeField] private Image levelImageCharacter;
        [SerializeField] private GameObject instanceTrack;
        [SerializeField] private RectTransform levelButtonContainer;
        [SerializeField] private Animator difficultBar;
        [SerializeField] private Image difficultImage;
        [SerializedDictionary("difficult", "images")] public SerializedDictionary<DifficultLevel, Sprite> difficultImages = new SerializedDictionary<DifficultLevel, Sprite>();

        private GameObject lastSelectedGameObject;

        private Transform[] characterPos;

        public Button[] buttons { get; private set; }

        private int difficultIndex = 1;

        public void Init()
        {
            buttons = levelButtonContainer.GetComponentsInChildren<Button>(true);
            difficultIndex = 1;
            characterPos = new Transform[levelImageCharacter.transform.childCount];
            for (int i = 0; i < characterPos.Length; i++)
            {
                characterPos[i] = levelImageCharacter.transform.GetChild(i).transform;
            }
        }

        protected override void Update()
        {
            base.Update();
            ChangeDifficult();
            ChangeInformationByLevel();
        }

        public void ChangeDifficult()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                difficultBar.Play("Right");
                difficultIndex++;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                difficultBar.Play("Left");
                difficultIndex--;
            }

            if (difficultImages.Count - 1 < difficultIndex)
            {
                difficultIndex = 0;
            }
            else if (difficultIndex < 0)
            {
                difficultIndex = difficultImages.Count - 1;
            }

            if ((int)difficult != difficultIndex)
            {
                difficult = (DifficultLevel)difficultIndex;
                PlayerPrefs.SetInt("Difficult", difficultIndex);
                difficultImage.sprite = difficultImages[difficult];
                difficultImage.SetNativeSize();
                AudioManager.instance.PlaySoundEffect($"{FilePaths.resources_sfx}scrollMenu");
            }
        }

        private void ChangeInformationByLevel()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].gameObject == EventSystem.current.currentSelectedGameObject && lastSelectedGameObject != EventSystem.current.currentSelectedGameObject)
                {
                    if (levelConfigs.Length > i)
                    {
                        lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
                        scoreText.text = $"Level Score: {PlayerPrefs.GetInt($"{levelConfigs[i].levelScene.name}Score")}";
                        levelImageCharacter.color = levelConfigs[i].backgroundColor;
                        if (levelConfigs[i].backGroundSpite)
                        {
                            levelImageCharacter.sprite = levelConfigs[i].backGroundSpite;
                        }
                        LevelNameText.text = levelConfigs[i].levelName;

                        DeleteAllChilds(TrackContainer.gameObject);
                        sbyte characterArray = -1;
                        for (int k = 0; k < characterPos.Length; k++)
                        {
                            DeleteAllChilds(characterPos[k].gameObject);
                            if ((levelConfigs[i].imageCharacters.ContainsKey(characterArray)))
                                if (levelConfigs[i].imageCharacters[characterArray] != null)
                                    Instantiate(levelConfigs[i].imageCharacters[characterArray], characterPos[k].transform);
                            characterArray++;
                        }


                        for (int j = 0; j < levelConfigs[i].tracks.Length; j++)
                        {
                            var track = levelConfigs[i].tracks[j];
                            var instance = Instantiate(instanceTrack, TrackContainer.transform);
                            instance.GetComponent<TextMeshProUGUI>().text = track;
                        }
                    }
                }
            }
        }

        public void DeleteAllChilds(GameObject gameObject)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }
    }
}