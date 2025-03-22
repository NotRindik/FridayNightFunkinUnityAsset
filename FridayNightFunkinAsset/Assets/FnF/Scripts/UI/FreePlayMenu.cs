using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using FnF.Scripts;
using FnF.Scripts.UI;
using FridayNightFunkin.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FreePlayMenu : MenuBehaviour,IChangeDifficulty
{
    public GameObject container;
    public GameObject freePlayLevelButtonPrefab;

    public List<FreePlayButton> buttons = new List<FreePlayButton>();

    [SerializeField] private TextMeshProUGUI personalBestText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private DifficultLevel difficult;

    private int _difficultIndex = 1;
    private GameObject _lastSelectedGameObject;

    public List<LevelData> levels = new List<LevelData>();

    public void Init()
    {
        difficultyText.text = difficult.ToString();
        
        foreach (var level in levels)
        {
            for (int i = 0; i < level.stage.Length; i++)
            {
                if (!level.stage[i].ignoreInFreePlay)
                {
                    var instance = Instantiate(freePlayLevelButtonPrefab,container.transform);
                    instance.transform.SetParent(container.transform);
                    var freePlay = instance.GetComponent<FreePlayButton>();
                    buttons.Add(freePlay);
                    freePlay.Init(level,i);
                    instance.name = level.stage[i].name;
                    var textField = instance.GetComponentInChildren<TextMeshProUGUI>();
                    var image = instance.GetComponentInChildren<Image>();
                    textField.text = level.stage[i].name;
                    image.sprite = level.stage[i].icon;      
                }
            }
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
            _difficultIndex++;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _difficultIndex--;
        }

        if (2 < _difficultIndex)
        {
            _difficultIndex = 0;
        }
        else if (_difficultIndex < 0)
        {
            _difficultIndex = 2;
        }

        if ((int)difficult != _difficultIndex)
        {
            difficult = (DifficultLevel)_difficultIndex;
            PlayerPrefs.SetInt(LevelManager.DIFFICULTY_PLAYERPREFS_NAME, _difficultIndex);
            difficultyText.text = difficult.ToString();
            AudioManager.instance.PlaySoundEffect($"{FilePaths.resources_sfx}scrollMenu");
        }
    }
    private void ChangeInformationByLevel()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].gameObject == EventSystem.current.currentSelectedGameObject && _lastSelectedGameObject != EventSystem.current.currentSelectedGameObject)
            {
                var button = buttons[i].GetComponent<FreePlayButton>();
                _lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
                personalBestText.text = $"PERSONAL BEST: {PlayerPrefs.GetInt($"{button.LevelData.stage[button.LevelData.selectedStageIndex].name}{ScoreManager.STAGE_PERSONAL_RECORD_PREFIX}")}";
                print($"{button.LevelData.stage[button.LevelData.selectedStageIndex].name}{ScoreManager.STAGE_PERSONAL_RECORD_PREFIX}");
            }
        }
    }
    public void SetSelectedOnFreePlayButton()
    {
        if (buttons.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
