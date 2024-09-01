using FridayNightFunkin.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FridayNightFunkin
{
    public abstract class ArrowTaker : MonoBehaviour
    {
        [SerializeField] protected ArrowSide arrowSide;

        [SerializeField] protected float arrowDetectRadius = 0.8f;
        protected LevelSettings levelSettings => LevelSettings.instance;

        protected Animator animator;


        protected float arrowDetectRadiusCalcualted;

        protected virtual void OnEnable()
        {
            GameStateManager.instance.OnGameStateChanged += OnGameStateChange;
        }
        private void Awake()
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
            Animator animator = LevelSettings.instance.splashAnim[(int)arrowSide];
            animator.Play("Splash");
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

        private void OnDestroy()
        {
            GameStateManager.instance.OnGameStateChanged -= OnGameStateChange;
        }
    }
}