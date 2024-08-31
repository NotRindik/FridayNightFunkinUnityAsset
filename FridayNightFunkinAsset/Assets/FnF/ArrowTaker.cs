using FridayNightFunkin.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FridayNightFunkin
{
    public class ArrowTaker : MonoBehaviour
    {
        [SerializeField] private ArrowSide arrowSide;

        [SerializeField] private float arrowDetectRadius = 0.8f;

        private FnfInput inputActions;
        private Animator animator;
        internal bool isHold;

        public Arrow arrow;

        internal float distance;

        private float arrowDetectRadiusCalcualted;

        private ScoreManager scoreManager => ScoreManager.instance;
        private LevelSettings levelSettings => LevelSettings.instance;

        private void OnEnable()
        {
            inputActions = InputManager.inputActions;
            GameStateManager.instance.OnGameStateChanged += OnGameStateChange;
            GetInputFromSide(arrowSide).performed += arrow => OnArrowPressed();
            GetInputFromSide(arrowSide).canceled += arrow => OnArrowUnPressed();
        }

        private void OnDisable()
        {
            GetInputFromSide(arrowSide).performed -= arrow => OnArrowPressed();
            GetInputFromSide(arrowSide).canceled -= arrow => OnArrowUnPressed();
        }

        private void Update()
        {
            arrowDetectRadiusCalcualted = arrowDetectRadius * (Camera.main.orthographicSize / 5);

            //if (isHold && arrow != null)
            //{
            //    if (arrow.distanceCount != 0 && arrow.isActiveAndEnabled)
            //    {
            //        ArrowMask.instance.ActivateMask((int)arrowSide);
            //        arrow.TakeLongArrow(distance, isHold);
            //    }
            //}
            //else
            //{
            //    ArrowMask.instance.DisActivateMask((int)arrowSide);
            //}
        }

        private void OnArrowPressed()
        {
            animator.CrossFade("Pressed", 0);
            Collider2D[] overlapCircle = Physics2D.OverlapCircleAll(transform.position, arrowDetectRadiusCalcualted, levelSettings.arrowLayer);
            if (overlapCircle.Length != 0)
            {
                if (overlapCircle[0].TryGetComponent(out Arrow arrow))
                {
                    isHold = true;
                    this.arrow = arrow;
                    if (arrowSide == arrow.arrowSide)
                    {
                        ActivateSplash(arrowSide);
                        distance = Vector2.Distance(transform.position, arrow.transform.position);
                        if (arrow.distanceCount == 0)
                        {
                            int accuracy = scoreManager.ÑalculateAccuracy(distance);
                            scoreManager.ÑalculateTotalAccuracy(scoreManager.accuracyList);
                            float scoreByAccuracy = (levelSettings.addMaxScore * ((float)accuracy / 100) + scoreManager.combo);

                            scoreManager.AddScore((uint)(Mathf.Floor(scoreByAccuracy)));
                            scoreManager.AddCombo();

                            FNFUIElement.instance.UpdateUI();
                            scoreManager.AddValueToSlider(levelSettings.playerForce);

                            arrow.TakeArrow(isHold);
                            arrow = null;
                        }
                    }
                }
            }
            else
            {
                animator.CrossFade("NoArrowPress", 0);
            }
        }
        private void OnGameStateChange(GameState currentState)
        {
            if (currentState == GameState.Paused)
            {   
                GetInputFromSide(arrowSide).performed -= arrow => OnArrowPressed();
                GetInputFromSide(arrowSide).canceled -= arrow => OnArrowUnPressed();
            }
            else
            {
                GetInputFromSide(arrowSide).performed += arrow => OnArrowPressed();
                GetInputFromSide(arrowSide).canceled += arrow => OnArrowUnPressed();
            }
        }
        private void ActivateSplash(ArrowSide arrowSide)
        {
            Animator animator = LevelSettings.instance.splashAnim[(int)arrowSide];
            animator.Play("Splash");
        }
        private void OnArrowUnPressed()
        {
            animator.CrossFade("Idle", 0);
            isHold = false;
        }

        private void OnDrawGizmos()
        {
            arrowDetectRadiusCalcualted = arrowDetectRadius * (Camera.main.orthographicSize / 5);
            Gizmos.DrawWireSphere(transform.position,arrowDetectRadiusCalcualted);
        }

        private InputAction GetInputFromSide(ArrowSide arrowSide)
        {
            switch (arrowSide)
            {
                case ArrowSide.LeftArrow:
                    return inputActions.PlayableArrow.Left;
                case ArrowSide.UpArrow:
                    return inputActions.PlayableArrow.Up;
                case ArrowSide.DownArrow:
                    return inputActions.PlayableArrow.Down;
                case ArrowSide.RightArrow:
                    return inputActions.PlayableArrow.Right;
                default:
                    Debug.LogError($"{arrowSide} this arrow is not taken into account, if you added a new one, then please add its code here");
                    return null;
            }
        }

        private void OnDestroy()
        {
            GameStateManager.instance.OnGameStateChanged -= OnGameStateChange;
        }
    }
}