using FridayNightFunkin.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FridayNightFunkin.CHARACTERS
{
    public class Character_Fnf_Enemy : Ñharacter_FNF
    {
        public float EnemyArrowTakeHeight;
        private List<Arrow> LastArrowReference = new List<Arrow>();
        private Collider2D[] overlapCircle;
        public LevelSettings levelSettings;
        private float lockTill;
        private float lockTillCharacter;
        private Arrow currentArrow;


        [SerializeField] private float arrowDetectRadius = 0.8f;

        private float arrowDetectRadiusCalcualted;

        private void Start()
        {
            levelSettings = LevelSettings.instance;
        }

        public void Update()
        {
            arrowDetectRadiusCalcualted = arrowDetectRadius * (Camera.main.orthographicSize / 5);
            BotPlay(levelSettings.arrowsEnemy);
        }

        private void OnDrawGizmos()
        {
            arrowDetectRadiusCalcualted = arrowDetectRadius * (Camera.main.orthographicSize / 5);
            for (int i = 0; i < levelSettings.arrowsEnemy.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(new Vector3(levelSettings.arrowsEnemy[i].transform.position.x, levelSettings.arrowsEnemy[i].transform.position.y + EnemyArrowTakeHeight * (Camera.main.orthographicSize/5), levelSettings.arrowsEnemy[i].transform.position.z),arrowDetectRadiusCalcualted);
            }
        }

        private void BotPlay(Image[] arrows)
        {
            for (int i = 0; i < arrows.Length; i++)
            {
                Animator animator = levelSettings.arrowsEnemy[i].gameObject.GetComponent<Animator>();
                overlapCircle = Physics2D.OverlapCircleAll(new Vector3(arrows[i].transform.position.x, levelSettings.arrowsEnemy[i].transform.position.y + EnemyArrowTakeHeight, arrows[i].transform.position.z), arrowDetectRadiusCalcualted, levelSettings.arrowLayer);

                for (int j = 0; j < overlapCircle.Length; j++)
                {
                    if (overlapCircle[j].TryGetComponent(out Arrow arrow))
                    {
                        currentArrow = arrow;
                        if (arrow.distanceCount == 0)
                        {
                            arrow.isWork = false;
                            ScoreManager.instance.ReduceValueToSliderEnemy(levelSettings.enemyForce);
                            arrow.gameObject.SetActive(false);
                        }
                        else if (!LastArrowReference.Contains(arrow))
                        {
                            ArrowMask.instance.ActivateMask(i,CharacterSide.Enemy);
                            arrow.spriteRendererOfArrow.color = new Color(arrow.spriteRendererOfArrow.color.r, arrow.spriteRendererOfArrow.color.g, arrow.spriteRendererOfArrow.color.b, 0f);
                            LastArrowReference.Add(arrow);
                        }
                        lockTillCharacter = 0;
                        currentAnimationState = ReturnNote(i);
                        PlayNote(i);
                        animator.CrossFade(ARROW_PRESSED, 0f);
                    }
                }

                if (LastArrowReference.Count != 0)
                {
                    for (int j = 0; j < LastArrowReference.Count; j++)
                    {
                        if (LastArrowReference[j] != currentArrow)
                        {
                            //LastArrowReference[j].TakeLongArrow(true, LastArrowReference);
                            animator.CrossFade(LockAnimation("Idle", 0.1f), 0.5f);
                            base.animator.CrossFade(LockAnimationCharacter(IDLE, 0.8f), 0f);
                        }
                    }
                }

                if (overlapCircle.Length == 0)
                {
                    animator.CrossFade(LockAnimation("Idle", 0.1f), 0.5f);
                    base.animator.CrossFade(LockAnimationCharacter(IDLE, 0.8f), 0f);
                }

            }
        }

        private string LockAnimation(string animationName, float time)
        {
            lockTill += Time.deltaTime;
            if (lockTill > time)
            {
                lockTill = 0;
                return animationName;
            }
            return currentAnimationState;
        }
        private string LockAnimationCharacter(string animationName, float time)
        {
            lockTillCharacter += Time.deltaTime;
            if (lockTillCharacter > time)
            {
                return animationName;
            }
            return currentAnimationState;
        }
    }
}