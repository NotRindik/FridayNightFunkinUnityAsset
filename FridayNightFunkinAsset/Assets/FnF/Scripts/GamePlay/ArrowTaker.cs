using FridayNightFunkin.Editor.TimeLineEditor;
using System;
using FnF.Scripts.Extensions;
using UnityEngine;
namespace FridayNightFunkin.GamePlay
{
    public abstract class ArrowTaker : MonoBehaviour
    {
        [SerializeField]public ArrowSide arrowSide;
        public abstract RoadSide RoadSide { get;}

        [SerializeField] protected float arrowDetectRadius = 0.8f;

        public Action<ArrowSide> OnArrowTake;

        public Action<ArrowSide> OnArrowUnTake;

        protected Animator Animator;

        protected float ArrowDetectRadiusCalcualted;

        public ChartPlayBack chartPlayBack;

        protected Camera Camera;
        
        public Animator splashAnim;

#if  UNITY_EDITOR
        
        private void OnValidate()
        {
            if(chartPlayBack == null)
            {
                chartPlayBack = FindAnyObjectByType<ChartPlayBack>();
            }
        }
        
#endif
        protected void OnEnable()
        {
            GameStateManager.instance.OnGameStateChanged += OnGameStateChange;
        }
        protected void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
            chartPlayBack = G.Instance.Get<ChartPlayBack>();
            Camera = Camera.main;
        }

        private void Update()
        {
            ArrowDetectRadiusCalcualted = arrowDetectRadius * (Camera.main.orthographicSize / 5);
        }

        protected virtual void OnGameStateChange(GameState currentState)
        {
            if (currentState == GameState.Paused)
            {
                Animator.speed = 0;
            }
            else
            {
                Animator.speed = 1;
            }
        }
        protected void ActivateSplash(ArrowSide arrowSide)
        { 
            splashAnim.Play("Splash");
        }


        protected virtual void OnDrawGizmos()
        {
            DrawDetectRadius(Color.green);
        }

        protected void DrawDetectRadius(Color color)
        {
            ArrowDetectRadiusCalcualted = arrowDetectRadius * (Camera.main.orthographicSize / 5);
            Gizmos.color = color;
            Gizmos.DrawWireSphere(transform.position, ArrowDetectRadiusCalcualted);
        }

        protected virtual void OnDestroy()
        {
            GameStateManager.instance.OnGameStateChanged -= OnGameStateChange;
        }
    }
}