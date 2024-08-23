using FridayNightFunkin;
using FridayNightFunkin.CHARACTERS;
using FridayNightFunkin.UI;
using History;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DIALOGUE
{
    public class PlayerInputManager : MonoBehaviour
    {
        private PlayerInput input;

        private List<(InputAction action, Action<InputAction.CallbackContext> command)> actions = new List<(InputAction action, Action<InputAction.CallbackContext> command)>();

        public static PlayerInputManager instance;

        private FnfInput inputActions;

        public Character_Fnf_PlayAble character;

        private ScoreManager scoreManager => ScoreManager.instance;

        private Animator animator;

        internal bool isHold;

        public Arrow arrow;

        public bool[] whichButtonPressed;

        internal float distance;
        public LevelSettings levelSettings;

        private float lockIdle;

        void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            inputActions = new FnfInput();
            input = GetComponent<PlayerInput>();
        }
        private void Start()
        {
            InitializeActions();
        }

        private void FixedUpdate()
        {
            lockIdle -= Time.deltaTime;
        }

        private void InitializeActions()
        {
            actions.Add((input.actions["Next"], OnNext));
            actions.Add((input.actions["HistoryBack"], OnHistoryBack));
            actions.Add((input.actions["HistoryForward"], OnHistoryForward));
            actions.Add((input.actions["HistoryLogs"], OnHistoryToggleLog));


            levelSettings = LevelSettings.instance;
            levelSettings.playerInput = this;

            inputActions.PlayableArrow.Left.performed += left => PressArrow(levelSettings.arrowsPlayer[0], ArrowSide.LeftArrow, 0);
            inputActions.PlayableArrow.Left.canceled += stopleft => StopArrow(levelSettings.arrowsPlayer[0], 0);

            inputActions.PlayableArrow.Up.performed += up => PressArrow(levelSettings.arrowsPlayer[2], ArrowSide.UpArrow, 2);
            inputActions.PlayableArrow.Up.canceled += stopleft => StopArrow(levelSettings.arrowsPlayer[2], 2);

            inputActions.PlayableArrow.Down.performed += down => PressArrow(levelSettings.arrowsPlayer[1], ArrowSide.DownArrow, 1);
            inputActions.PlayableArrow.Down.canceled += stopleft => StopArrow(levelSettings.arrowsPlayer[1], 1);

            inputActions.PlayableArrow.Right.performed += right => PressArrow(levelSettings.arrowsPlayer[3], ArrowSide.RightArrow, 3);
            inputActions.PlayableArrow.Right.canceled += stopleft => StopArrow(levelSettings.arrowsPlayer[3], 3);
        }
        private void Update()
        {

            for (int i = 0; i < whichButtonPressed.Length; i++)
            {
                if (isHold && arrow != null)
                {
                    if (arrow.distanceCount != 0 && arrow.isActiveAndEnabled)
                    {
                        if (whichButtonPressed[i])
                        {
                            MaskManager.instance.ActivateMask(i);
                        }
                        arrow.TakeLongArrow(distance, isHold);
                    }
                }
                else
                {
                    if (!whichButtonPressed[i])
                    {
                        MaskManager.instance.DisActivateMask(i);
                    }
                }
            }

        }
        private void ActivateSplash(ArrowSide arrowSide)
        {
            Animator animator = LevelSettings.instance.splashAnim[(int)arrowSide];
            animator.Play("Splash");
        }
        public void PressArrow(Image arrowButton, ArrowSide arrowSide, int arrowIndex)
        {
            animator = arrowButton.GetComponent<Animator>();
            Collider2D[] overlapCircle = Physics2D.OverlapCircleAll(arrowButton.transform.position, levelSettings.arrowDetectRadiusCalcualted, levelSettings.arrowLayer);

            if (overlapCircle.Length != 0)
            {
                if (overlapCircle[0].TryGetComponent(out Arrow arrow))
                {
                    isHold = true;
                    whichButtonPressed[arrowIndex] = true;
                    this.arrow = arrow;
                    if (arrowSide == arrow.arrowSide)
                    {
                        ActivateSplash(arrowSide);
                        PlayNote(arrowSide);
                        distance = Vector2.Distance(arrowButton.transform.position, arrow.transform.position);
                        animator.CrossFade("Pressed", 0);
                        if (arrow.distanceCount == 0)
                        {
                            int accuracy = scoreManager.ÑalculateAccuracy(distance);
                            scoreManager.ÑalculateTotalAccuracy(scoreManager.accuracyList);
                            float scoreByAccuracy = (levelSettings.addMaxScore * ((float)accuracy / 100) + scoreManager.combo);
                            scoreManager.AddScore((uint)(Mathf.Floor(scoreByAccuracy)));
                            scoreManager.AddCombo();
                            FNFUIElement.instance.UpdateUI();
                            scoreManager.AddValueToSlider(levelSettings.playerForce);
                            arrow.TakeArrow();
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

        private void OnDrawGizmos()
        {
            if (LevelSettings.instance != null)
            {
                for (int i = 0; i < LevelSettings.instance.arrowsPlayer.Length; i++)
                {
                    Gizmos.DrawWireSphere(LevelSettings.instance.arrowsPlayer[i].transform.position, LevelSettings.instance.arrowDetectRadiusCalcualted);
                }
            }
        }
        public void StopArrow(Image arrow, int arrowIndex)
        {
            animator = arrow.GetComponent<Animator>();
            isHold = false;
            animator.CrossFade("Idle", 0);
            whichButtonPressed[arrowIndex] = false;
        }

        public void SetIdleAnim(float time)
        {
            lockIdle = time;
        }

        private bool IsAllBoolsTrue(bool[] bools)
        {
            bool isAllButtonPressed = false;
            for (int i = 0; i < bools.Length; i++)
            {
                if (whichButtonPressed[i])
                    return true;
                else
                    isAllButtonPressed = false;
            }

            return isAllButtonPressed;
        }
        private void OnEnable()
        {
            inputActions.Enable();
            foreach (var inputActions in actions)
            {
                inputActions.action.performed += inputActions.command;
            }
        }

        private void OnDisable()
        {
            inputActions.Disable();
            foreach (var inputActions in actions)
            {
                inputActions.action.performed -= inputActions.command;
            }
        }

        public void OnNext(InputAction.CallbackContext c)
        {
            DialogueSystem.instance.OnUserPromt_Next();
        }

        public void OnHistoryBack(InputAction.CallbackContext c)
        {
            HistoryManager.instance.GoBack();
        }

        public void OnHistoryForward(InputAction.CallbackContext c)
        {
            HistoryManager.instance.GoForward();
        }

        public void OnHistoryToggleLog(InputAction.CallbackContext c)
        {
            var logs = HistoryManager.instance.logManager;
            if (!logs.isOpen)
                logs.Open();
            else
                logs.Close();
        }

        public void PlayNote(ArrowSide arrowSide)
        {
            character.PlayNote(arrowSide);
        }

        public ArrowSide GetArrowSideFromId(int id)
        {
            switch (id)
            {
                case (int)ArrowSide.DownArrow:
                    return ArrowSide.DownArrow;
                case (int)ArrowSide.LeftArrow:
                    return ArrowSide.LeftArrow;
                case (int)ArrowSide.RightArrow:
                    return ArrowSide.RightArrow;
                case (int)ArrowSide.UpArrow:
                    return ArrowSide.UpArrow;

            }

            return ArrowSide.DownArrow;
        }

    }
}