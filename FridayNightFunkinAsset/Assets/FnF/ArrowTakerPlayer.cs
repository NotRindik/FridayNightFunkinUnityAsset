using FridayNightFunkin.UI;
using System.Collections;
using System.Collections.Generic;
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
            animator.CrossFade("Pressed", 0);
            Collider2D[] overlapCircle = Physics2D.OverlapCircleAll(transform.position, arrowDetectRadiusCalcualted, levelSettings.arrowLayer);
            if (overlapCircle.Length != 0)
            {
                if (overlapCircle[0].TryGetComponent(out Arrow arrow))
                {
                    if (arrowSide == arrow.arrowSide)
                    {
                        isHold = true;
                        ActivateSplash(arrowSide);
                        distanceFromArrowToTaker = Vector2.Distance(transform.position, arrow.transform.position);

                        int accuracy = scoreManager.ÑalculateAccuracy(distanceFromArrowToTaker);
                        scoreManager.ÑalculateTotalAccuracy(scoreManager.accuracyList);
                        float scoreByAccuracy = (levelSettings.addMaxScore * ((float)accuracy / 100) + scoreManager.combo);

                        scoreManager.AddScore((uint)(Mathf.Floor(scoreByAccuracy)));
                        scoreManager.AddCombo();
                        OnArrowTake?.Invoke(arrowSide);

                        FNFUIElement.instance.UpdateUI();
                        scoreManager.AddValueToSlider(levelSettings.playerForce);
                        ArrowMask.instance.ActivateMask((int)arrowSide);
                        arrow.TakeArrow(isHold);
                    }
                }
            }
            else
            {
                animator.CrossFade("NoArrowPress", 0);
            }
        }
        private void OnArrowUnPressed(InputAction.CallbackContext context)
        {
            animator.CrossFade("Idle", 0);
            ArrowMask.instance.DisActivateMask((int)arrowSide);
            if (isHold)
            {
                isHold = false;
                OnArrowUnTake?.Invoke(arrowSide);
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