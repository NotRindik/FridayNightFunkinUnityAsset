using FridayNightFunkin;
using UnityEngine.SceneManagement;
using UnityEngine;
using FridayNightFunkin.Editor.TimeLineEditor;

public class LevelInitializator : MonoBehaviour
{
    private LevelSettings levelSettings;
    [SerializeField]private bool isTestStage;
    [SerializeField]private int testingStage;
    [SerializeField] private ÑhartContainer container;
    public void Start()
    {
        levelSettings = LevelSettings.instance;

        if(!isTestStage) 
            levelSettings.SetStage(PlayerPrefs.GetInt($"{SceneManager.GetActiveScene().name}Stage"));
        else
        {
            levelSettings.SetStage(testingStage);
        }

        levelSettings.stage[levelSettings.stageIndex].CalculateBPS();
        levelSettings.SpawnCharacters();
        container.ReloadChart();
    }
}
