using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    public UnityEvent onAnimationEnd;
    public string AnimationName;

    public float scaleMultiplierInSelecting = 1f;

    private Animator animator;
    private float timeToEndAnimation;
    private bool isAnimationStart;

    private float startXScale;
    private float startYScale;

    private Button button;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
        startXScale = transform.localScale.x;
        startYScale = transform.localScale.y;
        button = GetComponent<Button>();
    }
    private void Update()
    {
        if(scaleMultiplierInSelecting != 1 && EventSystem.current.currentSelectedGameObject == gameObject)
        {
            transform.localScale = new Vector3(startXScale * scaleMultiplierInSelecting, startYScale * scaleMultiplierInSelecting,1);
        }
        else
        {
            transform.localScale = new Vector3(startXScale ,startYScale,1);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationName) && !isAnimationStart)
        {
            timeToEndAnimation = animator.GetCurrentAnimatorStateInfo(0).length;
            button.interactable = false;
            EventSystem.current.SetSelectedGameObject(gameObject);
            isAnimationStart = true;
        }

        if(timeToEndAnimation <= 0 && isAnimationStart)
        {
            isAnimationStart = false;
            button.interactable = true;
            onAnimationEnd?.Invoke();
        }
    }
    
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private void FixedUpdate()
    {
        timeToEndAnimation -= Time.fixedDeltaTime;
    }

    public void PlayAnimation(string animation)
    {
        animator.Play(animation,0);
    }
    public void PlayAudioClip(AudioClip audioClip)
    {
        AudioManager.instance.PlaySoundEffect(audioClip);
    }

    public void PlayMusic(AudioClip audioClip)
    {
        AudioManager.instance.PlayTrack(audioClip);
    }
    public void StopMusic(AudioClip audioClip)
    {
        string audioName = audioClip.name;
        AudioManager.instance.StopTrack(audioName);
    }
    public void StopMusic(int channel)
    {
        AudioManager.instance.StopTrack(channel);
    }
    public void StopMusic(string audioName)
    {
        AudioManager.instance.StopTrack(audioName);
    }

}