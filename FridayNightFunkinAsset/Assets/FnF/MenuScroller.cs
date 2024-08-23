using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ScrollFactor
{
    ButtonChanging,
    Mathematical
}

public class MenuScroller : MonoBehaviour
{
    public float scrollOffset = 1f;
    private float MaxDistanceBetweenButton = 1;
    public float speed = 0.2f;
    public float stopSlideDistance = 0.0001f;

    private RectTransform contentContainer;

    private float startPosScroll;

    private Button[] buttons;

    private GameObject lastSelectedGameObject;

    private bool isMoving;

    public Coroutine Scrolling;

    private float[] scrollPos;

    private void Awake()
    {
        contentContainer = GetComponent<RectTransform>();
        startPosScroll = contentContainer.anchoredPosition.y;
        buttons = contentContainer.GetComponentsInChildren<Button>(false);
        scrollPos = new float[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            scrollPos[i] = startPosScroll + scrollOffset * i;
        }
    }
    private void Start()
    {
        lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        if (!lastSelectedGameObject)
        {
            lastSelectedGameObject = EventSystem.current.firstSelectedGameObject;
        }
    }

    public void Update()
    {
        GetSpaceBetweenButtons();
       GameObject currentGameObject = EventSystem.current.currentSelectedGameObject;
        if (lastSelectedGameObject != currentGameObject && !Input.GetKeyDown(KeyCode.Return) && (lastSelectedGameObject))
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if(buttons[i].gameObject == currentGameObject)
                {
                    var distance = Vector2.Distance(new Vector2(0,lastSelectedGameObject.transform.position.y), new Vector2(0, currentGameObject.transform.position.y));
                    if (distance < 0.1f)
                        return;
                    else if (distance < MaxDistanceBetweenButton)
                        StartMove(contentContainer.anchoredPosition.y, scrollPos[i], speed, contentContainer);
                    else
                        StartMove(contentContainer.anchoredPosition.y, scrollPos[i], speed * 4, contentContainer);
                }
            }
            lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        }
    }

    private void GetSpaceBetweenButtons()
    {
        if (buttons.Length > 2 && MaxDistanceBetweenButton <= 1)
        {
            MaxDistanceBetweenButton += Vector2.Distance(buttons[1].transform.position, buttons[0].transform.position);
        }
    }

    public void StartMove(float currentValue, float targetValue, float adder, RectTransform rectTransform = null)
    {
        if (isMoving && Scrolling != null)
        {
            StopCoroutine(Scrolling);
        }
        Scrolling = StartCoroutine(MoveTowardEnumerator(currentValue,targetValue,adder,rectTransform));
    }

    public void StartMove(float currentValue, float targetValue, float adder, Transform transform = null)
    {
        if (isMoving && Scrolling != null)
        {
            StopCoroutine(Scrolling);
        }
        Scrolling = StartCoroutine(MoveTowardEnumerator(currentValue, targetValue, adder, transform));
    }
    public IEnumerator MoveTowardEnumerator(float currentValue, float targetValue, float initialAdder, RectTransform rectTransform = null)
    {
        isMoving = true;
        float k = (initialAdder / Mathf.Pow(targetValue - currentValue, 2)) * 0.6f;
        float adder = initialAdder;
        while (Mathf.Abs(currentValue - targetValue) > 0.01f)
        {
            yield return new WaitForFixedUpdate();
            if(adder > stopSlideDistance)
                adder = k * Mathf.Pow(targetValue - currentValue, 2);

            if (currentValue < targetValue)
            {
                currentValue += adder;
                if (currentValue > targetValue) currentValue = targetValue;
            }
            else
            {
                currentValue -= adder;
                if (currentValue < targetValue) currentValue = targetValue;
            }

            if (rectTransform)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, currentValue);
            }
        }
        isMoving = false;
    }

    public IEnumerator MoveTowardEnumerator(float currentValue, float targetValue, float adder, Transform transform = null)
    {
        adder = Mathf.Abs(adder);
        while (currentValue != targetValue)
        {
            if (currentValue < targetValue)
            {
                yield return new WaitForFixedUpdate();
                currentValue += adder;
                if (currentValue > targetValue) currentValue = targetValue;
                if (transform)
                {
                    yield return new WaitForFixedUpdate();
                    transform.position = new Vector2(0f, currentValue);
                }
            }
            if (currentValue > targetValue)
            {
                yield return new WaitForFixedUpdate();
                currentValue -= adder;
                if (currentValue < targetValue) currentValue = targetValue;
                if (transform)
                {
                    yield return new WaitForFixedUpdate();
                    transform.position = new Vector2(0f, currentValue);
                }
            }
        }
    }
}
