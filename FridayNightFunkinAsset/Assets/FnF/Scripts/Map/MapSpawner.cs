using System;
using System.Collections;
using System.Collections.Generic;
using FnF.Scripts.Extensions;
using FnF.Scripts.Map;
using FridayNightFunkin.Editor.TimeLineEditor;
using UnityEngine;

public class MapSpawner : MonoBehaviour,IService
{
    private GameObject[] _maps;
    public Action OnSpawnMapEnd;
    private ChartPlayBack _chartPlayBack;

    public MapData mapData = new MapData();
    public void Init(ChartPlayBack chartPlayBack)
    {
        _maps = chartPlayBack.levelData.stage[ChartPlayBack.CurrentStageIndex].mapGameObjects;
        _chartPlayBack = chartPlayBack;
        StartCoroutine(SpawnMap());
    }

    public IEnumerator SpawnMap()
    {
        foreach (var map in _maps)
        {
            var inst = Instantiate(map, Vector2.zero, Quaternion.identity);
            yield return null;

            yield return StartCoroutine(ProcessObject(inst.transform));
        }
    
        OnSpawnMapEnd?.Invoke();
    }
    
    private IEnumerator ProcessObject(Transform obj)
    {
        string baseName = obj.name;
        string uniqueName = baseName;
        int objCloneIndex = 0;
    
        while (mapData.MapObjects.ContainsKey(uniqueName))
        {
            objCloneIndex++;
            uniqueName = $"{baseName}_Clone_{objCloneIndex}";
            obj.name = uniqueName;
        }
        
        mapData.MapObjects.Add(uniqueName, obj.gameObject);
        
        if (obj.TryGetComponent(out SpawnPoint spawnPoint))
        {
            switch (spawnPoint.characterSide)
            {
                case CharacterSide.Player:
                    mapData.playerPos.Add(spawnPoint.transform);
                    break;
                case CharacterSide.Enemy:
                    mapData.enemyPos.Add(spawnPoint.transform);
                    break;
                case CharacterSide.Gf:
                    mapData.gfPos.Add(spawnPoint.transform);
                    break;
                default:
                    Debug.Log("No Character side to add spawn point, if it additional character side just add it here");
                    break;
            }
        }
        
        for (int i = 0; i < obj.childCount; i++)
        {
            yield return StartCoroutine(ProcessObject(obj.GetChild(i)));
        }
        
        if (Time.frameCount % 10 == 0)
            yield return null;
    }
    public Transform GetCharacterPos(CharacterSide characterSide, int index)
    {
        switch (characterSide)
        {
            case CharacterSide.Player:
                return mapData.playerPos[index];
            case CharacterSide.Enemy:
                return mapData.enemyPos[index];
            case CharacterSide.Gf:
                return mapData.gfPos[index];
            default:
                Debug.LogError($"'{characterSide}' Character Transform doesn't exist");
                return null;
        }
    }
    public class MapData
    {
        public List<Transform> playerPos = new List<Transform>();
        public List<Transform> gfPos = new List<Transform>();   
        public List<Transform> enemyPos = new List<Transform>();
        public Dictionary<string, GameObject> MapObjects = new Dictionary<string, GameObject>();
    }
}
