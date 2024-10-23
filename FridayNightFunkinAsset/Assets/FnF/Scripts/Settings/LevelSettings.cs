using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FridayNightFunkin.CHARACTERS;
using UnityEngine.Timeline;
using FridayNightFunkin.Editor.TimeLineEditor;
using FridayNightFunkin.GamePlay;

namespace FridayNightFunkin.Settings
{
    [ExecuteAlways]
    public class LevelSettings : MonoBehaviour
    {
        public static LevelSettings instance 
        {
            get; private set;
        }

        public LevelStage[] stage;

        public ChartContainer container;

        public int stageIndex;

        public Image[] arrowsPlayer;

        public Image[] arrowsEnemy;

        public LayerMask arrowLayer;

        public Animator[] splashAnim;

        public Camera cam;

        public List<Arrow> arrowsList = new List<Arrow>();

        public List<Vector3> arrowsPlayerPos { private set; get; } = new List<Vector3>();
        public List<Vector3> arrowsEnemyPos { private set; get; } = new List<Vector3>();

        [SerializeField] public uint addMaxScore;
        [SerializeField] public uint addMaxScoreInLongArrow;

        public bool IsArrowPositionSaved;

        public List<Character_Fnf_PlayAble> currentPlayer { get; private set; }
        public List<Character_Fnf_Girlfriend> currentGirlFriend { get; private set; }
        public List<Character_Fnf_Enemy> currentEnemy { get; private set; }

        public delegate void OnSpeedChanged();

        public event OnSpeedChanged OnSpeedChanges;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            if (arrowsPlayerPos.Count == 0 || arrowsEnemyPos.Count == 0)
            {
                arrowsEnemyPos = new List<Vector3>();
                arrowsPlayerPos = new List<Vector3>();
                GetPositionFromList(arrowsPlayer, arrowsPlayerPos);
                GetPositionFromList(arrowsEnemy, arrowsEnemyPos);
                IsArrowPositionSaved = true;
            }
        }
        public bool IsSub(OnSpeedChanged method)
        {
            if (OnSpeedChanges == null) return false;

            foreach (var d in OnSpeedChanges.GetInvocationList())
            {
                if (d.Method == method.Method && d.Target == method.Target)
                {
                    return true;
                }
            }
            return false;
        }

        public void ActivePlayer(int index)
        {
            currentPlayer[index].Active();
        }
        public void DisactivePlayer(int index)
        {
            currentPlayer[index].Disactive();
        }
        public void ActiveEnemy(int index)
        {
            currentEnemy[index].Active();
        }
        public void DisactiveEnemy(int index)
        {
            currentEnemy[index].Disactive();
        }

        private void Update()
        {
            if (arrowsPlayerPos.Count == 0 || arrowsEnemyPos.Count == 0)
            {
                arrowsEnemyPos = new List<Vector3>();
                arrowsPlayerPos = new List<Vector3>();
                GetPositionFromList(arrowsPlayer, arrowsPlayerPos);
                GetPositionFromList(arrowsEnemy, arrowsEnemyPos);
                IsArrowPositionSaved = true;
            }

            if (!Application.isPlaying)
            {
                if (instance == null)
                {
                    instance = this;
                }

                foreach (var item in stage)
                {
                    if(item.chartSpeed != item.currentChartSpeed)
                    {
                        item.currentChartSpeed = item.chartSpeed;
                        OnSpeedChanges?.Invoke();
                        break;
                    }
                }
            }
        }
        public void SpawnCharacters()
        {
            currentPlayer = new List<Character_Fnf_PlayAble>();
            currentEnemy = new List<Character_Fnf_Enemy>();
            currentGirlFriend = new List<Character_Fnf_Girlfriend>();
            for (int i = 0; i < stage[stageIndex].GetCharacterLenth(CharacterSide.Player); i++)
            {
                if (stage[stageIndex].GetCharacterPrefab(CharacterSide.Player,i))
                {
                    currentPlayer.Add((Character_Fnf_PlayAble)Instantiate(stage[stageIndex].GetCharacterPrefab(CharacterSide.Player,i), stage[stageIndex].GetCharacterPos(CharacterSide.Player, i).position, Quaternion.identity));
                    currentPlayer[i].transform.SetParent(stage[stageIndex].GetCharacterPos(CharacterSide.Player,i));
                }
            }
            for (int i = 0; i < stage[stageIndex].GetCharacterLenth(CharacterSide.Enemy); i++)
            {
                if (stage[stageIndex].GetCharacterPrefab(CharacterSide.Enemy, i))
                {
                    currentEnemy.Add((Character_Fnf_Enemy)Instantiate(stage[stageIndex].GetCharacterPrefab(CharacterSide.Enemy, i), stage[stageIndex].GetCharacterPos(CharacterSide.Enemy,i).position, Quaternion.identity));
                    currentEnemy[i].transform.SetParent(stage[stageIndex].GetCharacterPos(CharacterSide.Enemy, i));
                }
            }
            for (int i = 0; i < stage[stageIndex].GetCharacterLenth(CharacterSide.Gf); i++)
            {
                if (stage[stageIndex].GetCharacterPrefab(CharacterSide.Gf, i))
                {
                    currentGirlFriend.Add((Character_Fnf_Girlfriend)Instantiate(stage[stageIndex].GetCharacterPrefab(CharacterSide.Gf, i), stage[stageIndex].GetCharacterPos(CharacterSide.Gf, i).position, Quaternion.identity));
                    currentGirlFriend[i].transform.SetParent(stage[stageIndex].GetCharacterPos(CharacterSide.Gf, i));
                }
            }
        }
        private void GetPositionFromList(Image[] arrow, List<Vector3> arrowList)
        {
            foreach (var item in arrow)
            {
                arrowList.Add(item.transform.position);
            }
        }

        public void SetStage(int index)
        {
            PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().name}Stage", index);
            stageIndex = index;
        }
    }


    [System.Serializable]
    public class LevelStage
    {
        [SerializeField] private float BPM = 120f;

        [SerializeField]  private float BPS;

        [SerializeField] private float playerForce = 2;

        [SerializeField] private float missForce = 2;

        [SerializeField] private float enemyForce = 0;

        [SerializeField] public float chartSpeed = 4;
        internal float currentChartSpeed;

        [SerializeField] private Character_Fnf_PlayAble[] playerPrefab;
        [SerializeField] private Character_Fnf_Girlfriend[] girlFriendPrefab;
        [SerializeField] private Character_Fnf_Enemy[] enemyPrefab;

        [SerializeField] private Transform[] playerPos;
        [SerializeField] private Transform[] girlPos;
        [SerializeField] private Transform[] enemyPos;

        [SerializeField] public TimelineAsset[] chartVariants;

        public Sprite[] playerIcon;
        public Sprite[] enemyIcon;


        public void CalculateBPS()
        {
            BPS = BPM / 60;
        }

        public float GetGeneralBPM()
        {
            return BPM;
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
                case CharacterSide.Gf:
                    return girlFriendPrefab.Length;
                default:
                    Debug.LogError($"'{characterSide}' Character Prefab doesn't exist");
                    return 0;
            }
        }

        public Ñharacter_FNF GetCharacterPrefab(CharacterSide characterSide,int index)
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
        public Transform GetCharacterPos(CharacterSide characterSide,int index)
        {
            switch (characterSide)
            {
                case CharacterSide.Player:
                    return playerPos[index];
                case CharacterSide.Enemy:
                    return enemyPos[index];
                case CharacterSide.Gf:
                    return girlPos[index];
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
}
