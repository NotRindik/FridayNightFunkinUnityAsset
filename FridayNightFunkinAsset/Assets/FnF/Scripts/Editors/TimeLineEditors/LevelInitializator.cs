using UnityEngine;
using System.Collections.Generic;
using FridayNightFunkin.CHARACTERS;
namespace FridayNightFunkin.Editor.TimeLineEditor
{
    public class LevelInitializator : MonoBehaviour
    {
        [SerializeField] private bool isTestStage;
        [SerializeField] private int testingStage;
        [SerializeField] private ChartPlayBack chartPlayback;

        public List<Character_Fnf_PlayAble> currentPlayer { get; private set; }
        public List<Character_Fnf_Girlfriend> currentGirlFriend { get; private set; }
        public List<Character_Fnf_Enemy> currentEnemy { get; private set; }
        public void Start()
        {
            SpawnCharacters();
            chartPlayback.ReloadChart();
        }

        public void SpawnCharacters()
        {
            currentPlayer = new List<Character_Fnf_PlayAble>();
            currentEnemy = new List<Character_Fnf_Enemy>();
            currentGirlFriend = new List<Character_Fnf_Girlfriend>();
            for (int i = 0; i < chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetCharacterLenth(CharacterSide.Player); i++)
            {
                if (chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetCharacterPrefab(CharacterSide.Player, i))
                {
                    currentPlayer.Add((Character_Fnf_PlayAble)Instantiate(chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetCharacterPrefab(CharacterSide.Player, i), chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetCharacterPos(CharacterSide.Player, i).position, Quaternion.identity));
                    currentPlayer[i].transform.SetParent(chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetCharacterPos(CharacterSide.Player, i));
                    currentPlayer[i].chartPlayBack = chartPlayback;
                }
            }
            for (int i = 0; i < chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetCharacterLenth(CharacterSide.Enemy); i++)
            {
                if (chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetCharacterPrefab(CharacterSide.Enemy, i))
                {
                    currentEnemy.Add((Character_Fnf_Enemy)Instantiate(chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetCharacterPrefab(CharacterSide.Enemy, i), chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetCharacterPos(CharacterSide.Enemy, i).position, Quaternion.identity));
                    currentEnemy[i].transform.SetParent(chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetCharacterPos(CharacterSide.Enemy, i));
                    currentEnemy[i].chartPlayBack = chartPlayback;
                }
            }
            for (int i = 0; i < chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetCharacterLenth(CharacterSide.Gf); i++)
            {
                if (chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetCharacterPrefab(CharacterSide.Gf, i))
                {
                    currentGirlFriend.Add((Character_Fnf_Girlfriend)Instantiate(chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetCharacterPrefab(CharacterSide.Gf, i), chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetCharacterPos(CharacterSide.Gf, i).position, Quaternion.identity));
                    currentGirlFriend[i].transform.SetParent(chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetCharacterPos(CharacterSide.Gf, i));
                    currentGirlFriend[i].chartPlayBack = chartPlayback;
                }
            }
        }
    }
}