using FnF.Scripts.Extensions;
using FnF.Scripts.Settings;
using FridayNightFunkin.Calculations;
using FridayNightFunkin.Editor.TimeLineEditor;
using FridayNightFunkin.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FridayNightFunkin.GamePlay
{
    public class ArrowTakerPlayer : ArrowTaker
    {
        private FnfInput InputActions => InputManager.InputActions;
        private StatisticManager StatisticManager => G.Instance.Get<StatisticManager>();
        private float _distanceFromArrowToTaker;
        public bool IsHold { get; private set; }

        internal Arrow _lastArrow;

        private bool _isPause;
        public override RoadSide RoadSide => RoadSide.Player;

        public CharacterSpawner characterSpawner;
        protected override void Start()
        {
            base.Start();
            InputActions.Enable();
            GetInputFromSide(arrowSide).performed += OnArrowPressed;
            GetInputFromSide(arrowSide).canceled += OnArrowUnPressed;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            GetInputFromSide(arrowSide).performed -=  OnArrowPressed;
            GetInputFromSide(arrowSide).canceled -=  OnArrowUnPressed;
            InputActions.Disable();
        }
        private void OnArrowPressed(InputAction.CallbackContext context)
        {
            if (_isPause)
                return;
            Animator.CrossFade("Pressed", 0);
            Collider2D[] overlapCircle = Physics2D.OverlapCircleAll(transform.position, ArrowDetectRadiusCalcualted, chartPlayBack.arrowsLayer);
            if (overlapCircle.Length != 0)
            {
                if (overlapCircle[^1 ].TryGetComponent(out Arrow arrow))
                {
                    if (arrowSide == arrow.arrowSide)
                    {
                        IsHold = true;
                        Vector2 x = Camera.WorldToScreenPoint(transform.position);
                        Vector2 y = Camera.WorldToScreenPoint(arrow.transform.position);
                        _distanceFromArrowToTaker = Vector2.Distance(Camera.WorldToScreenPoint(transform.position), Camera.WorldToScreenPoint(arrow.transform.position));

                        int accuracy = StatisticManager.CalculateAccuracy(_distanceFromArrowToTaker);
                        StatisticManager.CalculateTotalAccuracy(StatisticManager.accuracyList);
                        if (accuracy == 100)
                        {
                            ActivateSplash(arrowSide);   
                        }
                        float scoreByAccuracy = (chartPlayBack.levelData.addMaxScore * ((float)accuracy / 100) + StatisticManager.combo);

                        StatisticManager.AddScore((uint)(Mathf.Floor(scoreByAccuracy)));
                        StatisticManager.AddCombo();
                        OnArrowTake?.Invoke(arrowSide);
                        _lastArrow = arrow;
                        G.Instance.Get<HealthBar>().ModifyValue(chartPlayBack.levelData.stage[ChartPlayBack.CurrentStageIndex].GetPlayerForce());
                        ArrowMask.instance.ActivateMask((int)arrowSide);
                        arrow.TakeArrow(IsHold);
                    }
                }
            }
            else
            {
                Animator.CrossFade("NoArrowPress", 0);
                if(G.Instance.Get<SettingsManager>().activeGameSettings.GhostTapping == 0) 
                { 
                    foreach (var currentPlayer in characterSpawner.currentPlayer)
                    {
                        if(currentPlayer.isActive)
                            currentPlayer.PlayMissAnimation(arrowSide);
                    }
                    G.Instance.Get<HealthBar>().ModifyValue(-chartPlayBack.levelData.stage[ChartPlayBack.CurrentStageIndex].GetMissForce());
                    StatisticManager.AddMiss();
                    StatisticManager.CalculateAccuracy(500);
                    AudioManager.instance.PlaySoundEffect($"{FilePaths.resources_sfx}missnote{Random.Range(1, 4)}");
                    StatisticManager.CalculateTotalAccuracy(StatisticManager.accuracyList);
                    StatisticManager.ResetCombo();
                }
            }
        }
        private void OnArrowUnPressed(InputAction.CallbackContext context)
        {
            Animator.CrossFade("Idle", 0);
            ArrowMask.instance.DisActivateMask((int)arrowSide);
            if (IsHold)
            {
                IsHold = false;
                _lastArrow.SetArrowHolding(IsHold);
                _lastArrow = null;
                OnArrowUnTake?.Invoke(arrowSide);
            }
        }
        protected override void OnGameStateChange(GameState gameState)
        {
            base.OnGameStateChange(gameState);
            if (gameState == GameState.GamePlay)
            {
                _isPause = false;
            }
            else
            {
                _isPause = true;
            }
        }


        private InputAction GetInputFromSide(ArrowSide arrowSide)
        {
            switch (arrowSide)
            {
                case ArrowSide.LeftArrow:
                    return InputActions.PlayableArrow.Left;
                case ArrowSide.UpArrow:
                    return InputActions.PlayableArrow.Up;
                case ArrowSide.DownArrow:
                    return InputActions.PlayableArrow.Down;
                case ArrowSide.RightArrow:
                    return InputActions.PlayableArrow.Right;
                default:
                    Debug.LogError($"{arrowSide} this arrow is not taken into account, if you added a new one, then please add its code here");
                    return null;
            }
        }
    }
}