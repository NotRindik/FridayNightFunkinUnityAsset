using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FridayNightFunkin.CHARACTERS;
using UnityEditor;
using UnityEngine.Timeline;

namespace FridayNightFunkin
{
    [ExecuteAlways]
    public class LevelSettings : MonoBehaviour
    {
        public static LevelSettings instance 
        {
            get; private set;
        }

        public LevelStage[] stage;

        public int stageIndex;

        public Image[] arrowsPlayer;

        public Image[] arrowsEnemy;

        public LayerMask arrowLayer;

        public Animator[] splashAnim;

        public float chartSpeed;

        public Camera cam;

        public List<Arrow> arrowsList = new List<Arrow>();

        public List<Vector3> arrowsPlayerPos { private set; get; } = new List<Vector3>();
        public List<Vector3> arrowsEnemyPos { private set; get; } = new List<Vector3>();

        [SerializeField] public uint addMaxScore;
        [SerializeField] public uint addMaxScoreInLongArrow;

        public bool IsArrowPositionSaved;

        public Character_Fnf_PlayAble currentPlayer { get; private set; }
        public Character_Fnf_Girlfriend currentGirlFriend { get; private set; }
        public Character_Fnf_Enemy currentEnemy { get; private set; }

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
            stageIndex = PlayerPrefs.GetInt($"{SceneManager.GetActiveScene().name}Stage");
            if (stage.Length != 0) stage[stageIndex].CalculateBPS();
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
            }
        }
        public void SpawnCharacters()
        {
            if (stage[stageIndex].GetCharacterPrefab(CharacterSide.Player))
            {
                currentPlayer = (Character_Fnf_PlayAble)Instantiate(stage[stageIndex].GetCharacterPrefab(CharacterSide.Player), stage[stageIndex].GetCharacterPos(CharacterSide.Player).position, Quaternion.identity);
                currentPlayer.transform.SetParent(stage[stageIndex].GetCharacterPos(CharacterSide.Player));
            }
            if (stage[stageIndex].GetCharacterPrefab(CharacterSide.Enemy))
            {
                currentEnemy = (Character_Fnf_Enemy)Instantiate(stage[stageIndex].GetCharacterPrefab(CharacterSide.Enemy), stage[stageIndex].GetCharacterPos(CharacterSide.Enemy).position, Quaternion.identity);
                currentEnemy.transform.SetParent(stage[stageIndex].GetCharacterPos(CharacterSide.Enemy));
            }
            if (stage[stageIndex].GetCharacterPrefab(CharacterSide.Gf))
            {
                currentGirlFriend = (Character_Fnf_Girlfriend)Instantiate(stage[stageIndex].GetCharacterPrefab(CharacterSide.Gf), stage[stageIndex].GetCharacterPos(CharacterSide.Gf).position, Quaternion.identity);
                currentGirlFriend.transform.SetParent(stage[stageIndex].GetCharacterPos(CharacterSide.Gf));
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

        [SerializeField] private Character_Fnf_PlayAble playerPrefab;
        [SerializeField] private Character_Fnf_Girlfriend girlFriendPrefab;
        [SerializeField] private Character_Fnf_Enemy enemyPrefab;

        [SerializeField] private Transform playerPos;
        [SerializeField] private Transform girlPos;
        [SerializeField] private Transform enemyPos;

        [SerializeField] public TimelineAsset[] chartVariants;


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

        public Ñharacter_FNF GetCharacterPrefab(CharacterSide characterSide)
        {
            switch (characterSide)
            {
                case CharacterSide.Player:
                    return playerPrefab;
                case CharacterSide.Enemy:
                    return enemyPrefab;
                case CharacterSide.Gf:
                    return girlFriendPrefab;
                default:
                    Debug.LogError($"'{characterSide}' Character Prefab doesn't exist");
                    return null;
            }
        }
        public Transform GetCharacterPos(CharacterSide characterSide)
        {
            switch (characterSide)
            {
                case CharacterSide.Player:
                    return playerPos;
                case CharacterSide.Enemy:
                    return enemyPos;
                case CharacterSide.Gf:
                    return girlPos;
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
