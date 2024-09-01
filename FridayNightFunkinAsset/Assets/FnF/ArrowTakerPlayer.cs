using FridayNightFunkin.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FridayNightFunkin
{
    public class ArrowTakerPlayer : ArrowTaker
    {
        private FnfInput inputActions;
        private ScoreManager scoreManager => ScoreManager.instance;
        private float distanceFromArrowToTaker;
        private bool isHold;

        protected override void OnEnable()
        {
            base.OnEnable();
            inputActions = InputManager.inputActions;
            GetInputFromSide(arrowSide).performed += arrow => OnArrowPressed();
            GetInputFromSide(arrowSide).canceled += arrow => OnArrowUnPressed();
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
                    if (arrowSide == arrow.arrowSide)
                    {
                        ActivateSplash(arrowSide);
                        distanceFromArrowToTaker = Vector2.Distance(transform.position, arrow.transform.position);
                        if (arrow.distanceCount == 0)
                        {
                            int accuracy = scoreManager.ÑalculateAccuracy(distanceFromArrowToTaker);
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
        private void OnArrowUnPressed()
        {
            animator.CrossFade("Idle", 0);
            isHold = false;
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