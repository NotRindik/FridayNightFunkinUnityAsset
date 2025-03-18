using System;
using FridayNightFunkin.CHARACTERS;
using FridayNightFunkin.GamePlay;
using System.Collections.Generic;
using FnF.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

public enum IconProgressStatus{
    Losing, Mid, Winning
}


public class LevelData : ScriptableObject
{

    [SerializeField] public uint addMaxScore = 20;
    [SerializeField] public uint addMaxScoreInLongArrow = 10;
    public int selectedStageIndex { get; set; }
    public int selectedChartVar { get; set; }

    public Dictionary<CharacterSide, List<Arrow>> arrows = new Dictionary<CharacterSide, List<Arrow>>();
    public LayerMask arrowLayer;

    public LevelStage[] stage;

    private void OnValidate()
    {
        LevelManager.RegisterLevel(this.name,this);
    }
    public void AddArrow(CharacterSide side, Arrow arrow)
    {
        if (!arrows.ContainsKey(side))
        {
            arrows[side] = new List<Arrow>();
        }
        arrows[side].Add(arrow);
    }

    public List<Arrow> GetArrows(CharacterSide side)
    {
        if (arrows.ContainsKey(side))
        {
            return arrows[side];
        }
        return new List<Arrow>();
    }
}

[System.Serializable]
public class LevelStage
{
    [SerializeField] public string name;
    [SerializeField] public Sprite icon;
    [SerializeField] private float bpm;

    public float BPM
    {
        get
        {
            return bpm;
        }
        set 
        {
            bpm = Mathf.Max(0, value);
        }
    }

    public GameObject[] mapGameObjects;

    public float BPS => BPM / 60;

    [SerializeField] public float playerForce = 2;

    [SerializeField] public float missForce = 2;

    [SerializeField] public float enemyForce = 0;

    [SerializeField] public float chartSpeed = 4;
    [SerializeField] public float chartSpawnDistance = 10;
    
    [SerializeField] public float startHealth = 0;
    [SerializeField] public float maxHealth = 100;
    [SerializeField] public float minHealth = -100;

    public float ChartSpeed
    {
        get => chartSpeed;
        set
        {
            if (Mathf.Abs(chartSpeed - value) > Mathf.Epsilon) 
            {
                chartSpeed = value;
            }
        }
    }

    [SerializeField] public Character_Fnf_PlayAble[] playerPrefab;
    [SerializeField] public Character_Fnf_Girlfriend[] girlFriendPrefab;
    [SerializeField] public Character_Fnf_Enemy[] enemyPrefab;

    [SerializeField] public TimelineAsset[] chartVariants;

    public Dictionary<IconProgressStatus, Sprite> playerIcon = new Dictionary<IconProgressStatus, Sprite>();
    public Dictionary<IconProgressStatus, Sprite> enemyIcon = new Dictionary<IconProgressStatus, Sprite>();

    [SerializeField] public GameObject[] backGroundPrefab;

    public float GetPlayerForce()
    {
        return playerForce;
    }

    public float GetMissForce()
    {
        return missForce;
    }

    public int GetCharacterLength(CharacterSide characterSide)
    {
        switch (characterSide)
        {
            case CharacterSide.Player:
                return playerPrefab.Length;
            case CharacterSide.Enemy:
                return enemyPrefab.Length;
            case CharacterSide.Gf:
                return girlFriendPrefab.Length;
            default:
                Debug.LogError($"'{characterSide}' Character Prefab doesn't exist");
                return 0;
        }
    }

    public Character_FNF GetCharacterPrefab(CharacterSide characterSide, int index)
    {
        switch (characterSide)
        {
            case CharacterSide.Player:
                return playerPrefab[index];
            case CharacterSide.Enemy:
                return enemyPrefab[index];
            case CharacterSide.Gf:
                return girlFriendPrefab[index];
            default:
                Debug.LogError($"'{characterSide}' Character Prefab doesn't exist");
                return null;
        }
    }

    public float GetEnemyForce()
    {
        return enemyForce;
    }
}