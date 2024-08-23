using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FridayNightFunkin.CHARACTERS;
using UnityEditor;
using DIALOGUE;

namespace FridayNightFunkin
{
    [ExecuteAlways]
    public class LevelSettings : MonoBehaviour
    {
        public float BPM = 120f;

        public float BPS;

        public float playerForce = 2;

        public float missForce = 2;

        public float enemyForce = 0;

        public Character_Fnf_PlayAble player;

        public PlayerInputManager playerInput;

        public static LevelSettings instance { get; private set; }

        public Image[] arrowsPlayer;

        public Image[] arrowsEnemy;

        public float arrowDetectRadius;

        internal float arrowDetectRadiusCalcualted;


        public LayerMask arrowLayer;

        public Animator[] splashAnim;

        public float chartSpeed;

        public Camera cam;

        public List<Arrow> arrowsList = new List<Arrow>();

        public List<Vector3> arrowsPlayerPos { private set; get; } = new List<Vector3>();
        public List<Vector3> arrowsEnemyPos { private set; get; } = new List<Vector3>();

        [SerializeField] public uint addMaxScore;

        public bool IsArrowPositionSaved;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                InitializeAction();
            }
        }
        private void Update()
        {
            arrowDetectRadiusCalcualted = arrowDetectRadius * (Camera.main.orthographicSize/ 5);

            if (arrowsPlayerPos.Count == 0 || arrowsEnemyPos.Count == 0)
            {
                arrowsEnemyPos = new List<Vector3>();
                arrowsPlayerPos = new List<Vector3>();
                GetPositionFromList(arrowsPlayer, arrowsPlayerPos);
                GetPositionFromList(arrowsEnemy, arrowsEnemyPos);
                IsArrowPositionSaved = true;
            }

            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (instance == null)
                {
                    instance = this;
                    InitializeAction();
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

        public void InitializeAction()
        {
            BPS = BPM / 60;
        }
    }
}