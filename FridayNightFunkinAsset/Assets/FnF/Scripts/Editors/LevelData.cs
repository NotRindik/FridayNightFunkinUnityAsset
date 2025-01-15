using FridayNightFunkin.CHARACTERS;
using FridayNightFunkin.GamePlay;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public enum IconProgressStatus{
    Losing, Mid, Winning
}


public class LevelData : ScriptableObject
{

    [SerializeField] public uint addMaxScore = 20;
    [SerializeField] public uint addMaxScoreInLongArrow = 10;

    public Dictionary<CharacterSide, List<Arrow>> arrows = new Dictionary<CharacterSide, List<Arrow>>();
    public LayerMask arrowLayer;

    public LevelStage[] stage;

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
public unsafe class LevelStage
{
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

    public GameObject[] backgroundObjectList;

    [SerializeField] public float BPS => BPM / 60;

    [SerializeField] public float playerForce = 2;

    [SerializeField] public float missForce = 2;

    [SerializeField] public float enemyForce = 0;

    [SerializeField] public float _chartSpeed = 4;

    public Action OnSpeedChanges;
    public float chartSpeed
    {
        get => _chartSpeed;
        set
        {
            if (Mathf.Abs(_chartSpeed - value) > Mathf.Epsilon) 
            {
                _chartSpeed = value;
                OnSpeedChanges?.Invoke();
            }
        }
    }

    [SerializeField] public Character_Fnf_PlayAble[] playerPrefab;
    [SerializeField] public Character_Fnf_Girlfriend[] girlFriendPrefab;
    [SerializeField] public Character_Fnf_Enemy[] enemyPrefab;

    [SerializeField] public Transform[] playerPos;
    [SerializeField] public Transform[] girlPos;   
    [SerializeField] public Transform[] enemyPos;

    [SerializeField] public TimelineAsset[] chartVariants;

    public Dictionary<IconProgressStatus, Sprite> playerIcon = new Dictionary<IconProgressStatus, Sprite>();
    public Dictionary<IconProgressStatus, Sprite> enemyIcon = new Dictionary<IconProgressStatus, Sprite>();

    [SerializeField] public GameObject[] backGroundPrefab;

    public LevelStage()
    {
       
    }

    public float GetPlayerForce()
    {
        return playerForce;
    }

    public float GetMissForce()
    {
        return missForce;
    }

    public int GetCharacterLenth(CharacterSide characterSide)
    {
        switch (characterSide)
        {
            case CharacterSide.Player:
                return playerPrefab.Length;
            case CharacterSide.Enemy:
                return enemyPrefab.Length;
            default:
                Debug.LogError($"'{characterSide}' Character Prefab doesn't exist");
                return 0;
        }
    }

    public Ñharacter_FNF GetCharacterPrefab(CharacterSide characterSide, int index)
    {
        switch (characterSide)
        {
            case CharacterSide.Player:
                return playerPrefab[index];
            case CharacterSide.Enemy:
                return enemyPrefab[index];
            default:
                Debug.LogError($"'{characterSide}' Character Prefab doesn't exist");
                return null;
        }
    }
    public Transform GetCharacterPos(CharacterSide characterSide, int index)
    {
        switch (characterSide)
        {
            case CharacterSide.Player:
                return playerPos[index];
            case CharacterSide.Enemy:
                return enemyPos[index];
            default:
                Debug.LogError($"'{characterSide}' Character Transform doesn't exist");
                return null;
        }
    }

    public float GetEnemyForce()
    {
        return enemyForce;
    }
}