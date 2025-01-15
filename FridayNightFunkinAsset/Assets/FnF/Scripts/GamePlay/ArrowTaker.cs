using FridayNightFunkin.Editor.TimeLineEditor;
using System;
using UnityEngine;
namespace FridayNightFunkin.GamePlay
{
    public abstract class ArrowTaker : MonoBehaviour
    {
        [SerializeField] protected ArrowSide arrowSide;
        public abstract RoadSide roadSide { get;}

        [SerializeField] protected float arrowDetectRadius = 0.8f;

        public Action<ArrowSide> OnArrowTake;

        public Action<ArrowSide> OnArrowUnTake;

        protected Animator animator;

        protected float arrowDetectRadiusCalcualted;

        protected ChartPlayBack chartPlayBack => ChartPlayBack.Instance;
        protected virtual void OnEnable()
        {
            GameStateManager.instance.OnGameStateChanged += OnGameStateChange;
        }
        protected void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            arrowDetectRadiusCalcualted = arrowDetectRadius * (Camera.main.orthographicSize / 5);
        }

        protected virtual void OnGameStateChange(GameState currentState)
        {
            if (currentState == GameState.Paused)
            {
                animator.speed = 0;
            }
            else
            {
                animator.speed = 1;
            }
        }
        protected void ActivateSplash(ArrowSide arrowSide)
        {
            //Animator animator = LevelSettings.instance.splashAnim[(int)arrowSide]; кароче сплэши добавь
            //animator.Play("Splash");
        }


        protected virtual void OnDrawGizmos()
        {
            DrawDetectRadius(Color.green);
        }

        protected void DrawDetectRadius(Color color)
        {
            arrowDetectRadiusCalcualted = arrowDetectRadius * (Camera.main.orthographicSize / 5);
            Gizmos.color = color;
            Gizmos.DrawWireSphere(transform.position, arrowDetectRadiusCalcualted);
        }

        protected virtual void OnDestroy()
        {
            GameStateManager.instance.OnGameStateChanged -= OnGameStateChange;
        }
    }
}