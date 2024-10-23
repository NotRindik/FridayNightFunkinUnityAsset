using FridayNightFunkin.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FridayNightFunkin
{
    public class ArrowTakerPlayer : ArrowTaker
    {
        private FnfInput inputActions => InputManager.inputActions;
        private ScoreManager scoreManager => ScoreManager.instance;
        private float distanceFromArrowToTaker;
        public bool isHold { get; private set; }

        public delegate void OnArrowTakeHandler(ArrowSide arrow);
        public event OnArrowTakeHandler OnArrowTake;

        public delegate void OnArrowUnTakeHandler(ArrowSide arrow);
        public event OnArrowUnTakeHandler OnArrowUnTake;

        private Arrow LastArrow;

        private bool isPause;

        protected void Start()
        {
            inputActions.Enable();
            GetInputFromSide(arrowSide).performed += OnArrowPressed;
            GetInputFromSide(arrowSide).canceled += OnArrowUnPressed;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            GetInputFromSide(arrowSide).performed -=  OnArrowPressed;
            GetInputFromSide(arrowSide).canceled -=  OnArrowUnPressed;
            inputActions.Disable();
        }
        private void OnArrowPressed(InputAction.CallbackContext context)
        {
            if (isPause)
                return;
            animator.CrossFade("Pressed", 0);
            Collider2D[] overlapCircle = Physics2D.OverlapCircleAll(transform.position, arrowDetectRadiusCalcualted, levelSettings.arrowLayer);
            if (overlapCircle.Length != 0)
            {
                if (overlapCircle[overlapCircle.Length - 1 ].TryGetComponent(out Arrow arrow))
                {
                    if (arrowSide == arrow.arrowSide)
                    {
                        isHold = true;
                        ActivateSplash(arrowSide);
                        Vector2 x = Camera.main.WorldToScreenPoint(transform.position);
                        Vector2 y = Camera.main.WorldToScreenPoint(arrow.transform.position);
                        distanceFromArrowToTaker = Vector2.Distance(Camera.main.WorldToScreenPoint(transform.position), Camera.main.WorldToScreenPoint(arrow.transform.position));

                        int accuracy = scoreManager.ÑalculateAccuracy(distanceFromArrowToTaker);
                        scoreManager.ÑalculateTotalAccuracy(scoreManager.accuracyList);
                        float scoreByAccuracy = (levelSettings.addMaxScore * ((float)accuracy / 100) + scoreManager.combo);

                        scoreManager.AddScore((uint)(Mathf.Floor(scoreByAccuracy)));
                        scoreManager.AddCombo();
                        OnArrowTake?.Invoke(arrowSide);
                        LastArrow = arrow;
                        FNFUIElement.instance.UpdateUI();
                        scoreManager.AddValueToSlider(levelSettings.stage[levelSettings.stageIndex].GetPlayerForce());
                        ArrowMask.instance.ActivateMask((int)arrowSide);
                        arrow.TakeArrow(isHold);
                    }
                }
            }
            else
            {
                animator.CrossFade("NoArrowPress", 0);
                if(ServiceLocator.instance.Get<ChangesByGameSettings>().ghostTapping == 1) 
                { 
                    foreach (var currentPlayer in levelSettings.currentPlayer)
                    {
                        if(currentPlayer.isActive)
                            currentPlayer.PlayMissAnimation(arrowSide);
                    }
                    scoreManager.ReduceValueToSlider(levelSettings.stage[levelSettings.stageIndex].GetMissForce());
                    scoreManager.AddMiss();
                    scoreManager.ÑalculateAccuracy(500);
                    AudioManager.instance.PlaySoundEffect($"{FilePaths.resources_sfx}missnote{Random.Range(1, 4)}");
                    scoreManager.ÑalculateTotalAccuracy(scoreManager.accuracyList);
                    scoreManager.ResetCombo();
                }
            }
        }
        private void OnArrowUnPressed(InputAction.CallbackContext context)
        {
            animator.CrossFade("Idle", 0);
            ArrowMask.instance.DisActivateMask((int)arrowSide);
            if (isHold)
            {
                isHold = false;
                LastArrow.SetArrowHolding(isHold);
                LastArrow = null;
                OnArrowUnTake?.Invoke(arrowSide);
            }
        }
        protected override void OnGameStateChange(GameState gameState)
        {
            base.OnGameStateChange(gameState);
            if (gameState == GameState.GamePlay)
            {
                isPause = false;
            }
            else
            {
                isPause = true;
            }
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
    }
}