using UnityEngine;
using System.Collections.Generic;
using FnF.Scripts;
using FnF.Scripts.Extensions;
using FridayNightFunkin.CHARACTERS;
namespace FridayNightFunkin.Editor.TimeLineEditor
{
    public class CharacterSpawner : MonoBehaviour,IService
    {
        [SerializeField] private bool isTestStage;
        [SerializeField] private int testingStage;
        private ChartPlayBack _chartPlayback;

        public List<Character_Fnf_PlayAble> currentPlayer { get; private set; }
        public List<Character_Fnf_Girlfriend> currentGirlFriend { get; private set; }
        public List<Character_Fnf_Enemy> currentEnemy { get; private set; }

        private MapSpawner _mapSpawner;
        public void Init(ChartPlayBack chartPlayBack,MapSpawner mapSpawner)
        {
            if (isTestStage)
            {
                PlayerPrefs.SetInt(LevelSaveConst.STAGE_PLAYERPREFS_NAME, testingStage);
            }
            _mapSpawner = mapSpawner;
            _chartPlayback = chartPlayBack;
            
            currentPlayer = new List<Character_Fnf_PlayAble>();
            currentEnemy = new List<Character_Fnf_Enemy>();
            currentGirlFriend = new List<Character_Fnf_Girlfriend>();
        }
        public void SpawnCharacters()
        {
            for (int i = 0; i < _chartPlayback.levelData.stage[ChartPlayBack.CurrentStageIndex].GetCharacterLength(CharacterSide.Player); i++)
            {
                if (_chartPlayback.levelData.stage[ChartPlayBack.CurrentStageIndex].GetCharacterPrefab(CharacterSide.Player, i))
                {
                    currentPlayer.Add((Character_Fnf_PlayAble)Instantiate(_chartPlayback.levelData.stage[ChartPlayBack.CurrentStageIndex].GetCharacterPrefab(CharacterSide.Player, i), _mapSpawner.GetCharacterPos(CharacterSide.Player, i).position, Quaternion.identity));
                    currentPlayer[i].transform.SetParent(_mapSpawner.GetCharacterPos(CharacterSide.Player, i));
                    currentPlayer[i].chartPlayBack = _chartPlayback;
                    currentPlayer[i].Init();
                }
            }
            for (int i = 0; i < _chartPlayback.levelData.stage[ChartPlayBack.CurrentStageIndex].GetCharacterLength(CharacterSide.Enemy); i++)
            {
                if (_chartPlayback.levelData.stage[ChartPlayBack.CurrentStageIndex].GetCharacterPrefab(CharacterSide.Enemy, i))
                {
                    currentEnemy.Add((Character_Fnf_Enemy)Instantiate(_chartPlayback.levelData.stage[ChartPlayBack.CurrentStageIndex].GetCharacterPrefab(CharacterSide.Enemy, i), _mapSpawner.GetCharacterPos(CharacterSide.Enemy, i).position, Quaternion.identity));
                    currentEnemy[i].transform.SetParent(_mapSpawner.GetCharacterPos(CharacterSide.Enemy, i));
                    currentEnemy[i].chartPlayBack = _chartPlayback;
                    currentEnemy[i].Init();
                }
            }
            for (int i = 0; i < _chartPlayback.levelData.stage[ChartPlayBack.CurrentStageIndex].GetCharacterLength(CharacterSide.Gf); i++)
            {
                if (_chartPlayback.levelData.stage[ChartPlayBack.CurrentStageIndex].GetCharacterPrefab(CharacterSide.Gf, i))
                {
                    currentGirlFriend.Add((Character_Fnf_Girlfriend)Instantiate(_chartPlayback.levelData.stage[ChartPlayBack.CurrentStageIndex].GetCharacterPrefab(CharacterSide.Gf, i), _mapSpawner.GetCharacterPos(CharacterSide.Gf, i).position, Quaternion.identity));
                    currentGirlFriend[i].transform.SetParent(_mapSpawner.GetCharacterPos(CharacterSide.Gf, i));
                    currentGirlFriend[i].chartPlayBack = _chartPlayback;
                    currentGirlFriend[i].Init();
                }
            }
        }
    }
}