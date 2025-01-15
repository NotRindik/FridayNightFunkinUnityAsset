//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using FridayNightFunkin.CHARACTERS;
//using UnityEngine.Timeline;
//using FridayNightFunkin.Editor.TimeLineEditor;
//using FridayNightFunkin.GamePlay;

//namespace FridayNightFunkin.Settings
//{
//    [ExecuteAlways]
//    public class LevelSettings : MonoBehaviour
//    {
//        public static LevelSettings instance;

//        public LevelStage[] stage;

//        public ChartPlayBack container;

//        public int stageIndex;

//        public Image[] arrowsPlayer;

//        public Image[] arrowsEnemy;

//        public LayerMask arrowLayer;

//        public Animator[] splashAnim;

//        public Camera cam;

//        public List<Arrow> arrowsList = new List<Arrow>();

//        public List<Vector3> arrowsPlayerPos { private set; get; } = new List<Vector3>();
//        public List<Vector3> arrowsEnemyPos { private set; get; } = new List<Vector3>();

//        [SerializeField] public uint addMaxScore;
//        [SerializeField] public uint addMaxScoreInLongArrow;

//        public bool IsArrowPositionSaved;

//        public List<Character_Fnf_PlayAble> currentPlayer { get; private set; }
//        public List<Character_Fnf_Girlfriend> currentGirlFriend { get; private set; }
//        public List<Character_Fnf_Enemy> currentEnemy { get; private set; }

//        public delegate void OnSpeedChanged();

//        public event OnSpeedChanged OnSpeedChanges;

//        public PlayerDeath playerDeath;

//        private void OnEnable()
//        {
//            if (instance == null)
//            {
//                instance = this;
//            }
//        }

//        private void Awake()
//        {
//            if (instance == null)
//            {
//                instance = this;
//            }
//            if (arrowsPlayerPos.Count == 0 || arrowsEnemyPos.Count == 0)
//            {
//                arrowsEnemyPos = new List<Vector3>();
//                arrowsPlayerPos = new List<Vector3>();
//                GetPositionFromList(arrowsPlayer, arrowsPlayerPos);
//                GetPositionFromList(arrowsEnemy, arrowsEnemyPos);
//                IsArrowPositionSaved = true;
//            }
//        }

//        public void ActivePlayer(int index)
//        {
//            currentPlayer[index].Active();
//        }
//        public void DisactivePlayer(int index)
//        {
//            currentPlayer[index].Disactive();
//        }
//        public void ActiveEnemy(int index)
//        {
//            currentEnemy[index].Active();
//        }
//        public void DisactiveEnemy(int index)
//        {
//            currentEnemy[index].Disactive();
//        }

//        private void Update()
//        {
//            if (arrowsPlayerPos.Count == 0 || arrowsEnemyPos.Count == 0)
//            {
//                arrowsEnemyPos = new List<Vector3>();
//                arrowsPlayerPos = new List<Vector3>();
//                GetPositionFromList(arrowsPlayer, arrowsPlayerPos);
//                GetPositionFromList(arrowsEnemy, arrowsEnemyPos);
//                IsArrowPositionSaved = true;
//            }

//            if (!Application.isPlaying)
//            {
//                if (instance == null)
//                {
//                    instance = this;
//                }
//            }
//        }
//        public void SpawnCharacters()
//        {
//            currentPlayer = new List<Character_Fnf_PlayAble>();
//            currentEnemy = new List<Character_Fnf_Enemy>();
//            currentGirlFriend = new List<Character_Fnf_Girlfriend>();
//            for (int i = 0; i < stage[stageIndex].GetCharacterLenth(CharacterSide.Player); i++)
//            {
//                if (stage[stageIndex].GetCharacterPrefab(CharacterSide.Player, i))
//                {
//                    currentPlayer.Add((Character_Fnf_PlayAble)Instantiate(stage[stageIndex].GetCharacterPrefab(CharacterSide.Player, i), stage[stageIndex].GetCharacterPos(CharacterSide.Player, i).position, Quaternion.identity));
//                    currentPlayer[i].transform.SetParent(stage[stageIndex].GetCharacterPos(CharacterSide.Player, i));
//                }
//            }
//            for (int i = 0; i < stage[stageIndex].GetCharacterLenth(CharacterSide.Enemy); i++)
//            {
//                if (stage[stageIndex].GetCharacterPrefab(CharacterSide.Enemy, i))
//                {
//                    currentEnemy.Add((Character_Fnf_Enemy)Instantiate(stage[stageIndex].GetCharacterPrefab(CharacterSide.Enemy, i), stage[stageIndex].GetCharacterPos(CharacterSide.Enemy, i).position, Quaternion.identity));
//                    currentEnemy[i].transform.SetParent(stage[stageIndex].GetCharacterPos(CharacterSide.Enemy, i));
//                }
//            }
//            for (int i = 0; i < stage[stageIndex].GetCharacterLenth(CharacterSide.Gf); i++)
//            {
//                if (stage[stageIndex].GetCharacterPrefab(CharacterSide.Gf, i))
//                {
//                    currentGirlFriend.Add((Character_Fnf_Girlfriend)Instantiate(stage[stageIndex].GetCharacterPrefab(CharacterSide.Gf, i), stage[stageIndex].GetCharacterPos(CharacterSide.Gf, i).position, Quaternion.identity));
//                    currentGirlFriend[i].transform.SetParent(stage[stageIndex].GetCharacterPos(CharacterSide.Gf, i));
//                }
//            }
//        }
//        private void GetPositionFromList(Image[] arrow, List<Vector3> arrowList)
//        {
//            foreach (var item in arrow)
//            {
//                arrowList.Add(item.transform.position);
//            }
//        }

//        public void SetStage(int index)
//        {
//            PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().name}Stage", index);
//            stageIndex = index;
//        }
//    }
//}
