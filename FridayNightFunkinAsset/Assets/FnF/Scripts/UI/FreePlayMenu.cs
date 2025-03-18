using System;
using System.Collections.Generic;
using FnF.Scripts;
using FnF.Scripts.UI;
using FridayNightFunkin.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FreePlayMenu : MenuBehaviour,IChangeDifficulty
{
    public GameObject container;
    public GameObject freePlayLevelButtonPrefab;

    private List<FreePlayButton> _buttons = new List<FreePlayButton>();

    [SerializeField] private TextMeshProUGUI personalBestText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private DifficultLevel difficult;

    private int difficultIndex = 1;
    private GameObject lastSelectedGameObject;
    public void Init()
    {
        foreach (var level in LevelManager.levels.Values)
        {
            for (int i = 0; i < level.stage.Length; i++)
            {
                var instance = Instantiate(freePlayLevelButtonPrefab,container.transform);
                instance.transform.SetParent(container.transform);
                var freePlay = instance.GetComponent<FreePlayButton>();
                _buttons.Add(freePlay);
                freePlay.Init(level,i);
                instance.name = level.stage[i].name;
                var textField = instance.GetComponentInChildren<TextMeshProUGUI>();
                var image = instance.GetComponentInChildren<Image>();
                textField.text = level.stage[i].name;
                image.sprite = level.stage[i].icon;   
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
            difficultIndex++;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            difficultIndex--;
        }

        if (2 < difficultIndex)
        {
            difficultIndex = 0;
        }
        else if (difficultIndex < 0)
        {
            difficultIndex = 2;
        }

        if ((int)difficult != difficultIndex)
        {
            difficult = (DifficultLevel)difficultIndex;
            PlayerPrefs.SetInt("Difficult", difficultIndex);
            difficultyText.text = difficult.ToString();
            AudioManager.instance.PlaySoundEffect($"{FilePaths.resources_sfx}scrollMenu");
        }
    }
    private void ChangeInformationByLevel()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            if (_buttons[i].gameObject == EventSystem.current.currentSelectedGameObject && lastSelectedGameObject != EventSystem.current.currentSelectedGameObject)
            {
                var button = _buttons[i].GetComponent<FreePlayButton>();
                lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
                personalBestText.text = $"PERSONAL BEST: {PlayerPrefs.GetInt($"{button.LevelData.stage[button.LevelData.selectedStageIndex].name}{ScoreManager.STAGE_PERSONAL_RECORD_PREFIX}")}";
            }
        }
    }
    public void SetSelectedOnFreePlayButton()
    {
        if (_buttons.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(_buttons[0].gameObject);
            var button =_buttons[0].GetComponent<Button>();
            var collor = button.colors;
            collor.colorMultiplier = 1;
            button.colors = collor;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
