using FridayNightFunkin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelInitializator : MonoBehaviour
{
    private LevelSettings levelSettings;
    public void Start()
    {
        levelSettings = LevelSettings.instance;

        levelSettings.stageIndex = PlayerPrefs.GetInt($"{SceneManager.GetActiveScene().name}Stage");
        levelSettings.stage[levelSettings.stageIndex].CalculateBPS();
        levelSettings.SpawnCharacters();
    }
}
