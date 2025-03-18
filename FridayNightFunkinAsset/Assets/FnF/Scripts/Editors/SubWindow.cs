/*using FridayNightFunkin.Editor.TimeLineEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SubWindow
{
    private Rect subwindowRect;
    private bool isResizing = false;
    private Vector2 lastMousePosition;
    private ResizeDirection currentResizeDirection = ResizeDirection.None;
    private Vector2 scroll;

    private Dictionary<ResizeDirection, Rect> resizeRects = new Dictionary<ResizeDirection, Rect>();
    private Rect transformRect;

    private bool isDragging = false;
    private Vector2 dragStartMousePosition;
    private Vector2 dragStartWindowPosition;

    private string windowName = "";

    public SubWindow(Rect initialRect,string windowName)
    {
        subwindowRect = initialRect;
        float borderSize = 5f;
        resizeRects.Clear();
        resizeRects[ResizeDirection.Top] = new Rect(subwindowRect.x, subwindowRect.y - borderSize, subwindowRect.width, borderSize);
        resizeRects[ResizeDirection.Bottom] = new Rect(subwindowRect.x, subwindowRect.yMax, subwindowRect.width, borderSize);
        resizeRects[ResizeDirection.Left] = new Rect(subwindowRect.x - borderSize, subwindowRect.y, borderSize, subwindowRect.height);
        resizeRects[ResizeDirection.Right] = new Rect(subwindowRect.xMax, subwindowRect.y, borderSize, subwindowRect.height);

        resizeRects[ResizeDirection.TopLeft] = new Rect(subwindowRect.x - borderSize, subwindowRect.y - borderSize, borderSize, borderSize);
        resizeRects[ResizeDirection.TopRight] = new Rect(subwindowRect.xMax, subwindowRect.y - borderSize, borderSize, borderSize);
        resizeRects[ResizeDirection.BottomLeft] = new Rect(subwindowRect.x - borderSize, subwindowRect.yMax, borderSize, borderSize);
        resizeRects[ResizeDirection.BottomRight] = new Rect(subwindowRect.xMax, subwindowRect.yMax, borderSize, borderSize);
        this.windowName = windowName;
    }

    public void BeginSubWindow()
    {
        GUI.depth = -10;
        UpdateResizing();
        GUILayout.BeginArea(subwindowRect);
        DrawHeader();
        scroll = EditorGUILayout.BeginScrollView(scroll);
    }

    public void EndSubWindow() 
    {
        EditorGUILayout.EndScrollView();
        GUILayout.EndArea();
        GUI.depth = 0;
    }

    private void UpdateResizing()
    {
        float borderSize = 5f;

        resizeRects[ResizeDirection.Top] = new Rect(subwindowRect.x, subwindowRect.y - borderSize, subwindowRect.width, borderSize);
        resizeRects[ResizeDirection.Bottom] = new Rect(subwindowRect.x, subwindowRect.yMax, subwindowRect.width, borderSize);
        resizeRects[ResizeDirection.Left] = new Rect(subwindowRect.x - borderSize, subwindowRect.y, borderSize, subwindowRect.height);
        resizeRects[ResizeDirection.Right] = new Rect(subwindowRect.xMax, subwindowRect.y, borderSize, subwindowRect.height);

        resizeRects[ResizeDirection.TopLeft] = new Rect(subwindowRect.x - borderSize, subwindowRect.y - borderSize, borderSize, borderSize);
        resizeRects[ResizeDirection.TopRight] = new Rect(subwindowRect.xMax, subwindowRect.y - borderSize, borderSize, borderSize);
        resizeRects[ResizeDirection.BottomLeft] = new Rect(subwindowRect.x - borderSize, subwindowRect.yMax, borderSize, borderSize);
        resizeRects[ResizeDirection.BottomRight] = new Rect(subwindowRect.xMax, subwindowRect.yMax, borderSize, borderSize);

        foreach (Rect rect in resizeRects.Values)
        {
            GUI.DrawTexture(rect, Texture2D.grayTexture);
        }

        foreach (var kvp in resizeRects)
        {
            HandleMouseInput(kvp.Value, kvp.Key);
        }

        if (isResizing)
        {
            if (Event.current.type == EventType.MouseDrag)
            {
                Vector2 delta = Event.current.mousePosition - lastMousePosition;

                switch (currentResizeDirection)
                {
                    case ResizeDirection.Top:
                        subwindowRect.y += delta.y;
                        subwindowRect.height -= delta.y;
                        break;
                    case ResizeDirection.Bottom:
                        subwindowRect.height += delta.y;
                        break;
                    case ResizeDirection.Left:
                        subwindowRect.x += delta.x;
                        subwindowRect.width -= delta.x;
                        break;
                    case ResizeDirection.Right:
                        subwindowRect.width += delta.x;
                        break;
                    case ResizeDirection.TopLeft:
                        subwindowRect.x += delta.x;
                        subwindowRect.y += delta.y;
                        subwindowRect.width -= delta.x;
                        subwindowRect.height -= delta.y;
                        break;
                    case ResizeDirection.TopRight:
                        subwindowRect.y += delta.y;
                        subwindowRect.width += delta.x;
                        subwindowRect.height -= delta.y;
                        break;
                    case ResizeDirection.BottomLeft:
                        subwindowRect.x += delta.x;
                        subwindowRect.height += delta.y;
                        subwindowRect.width -= delta.x;
                        break;
                    case ResizeDirection.BottomRight:
                        subwindowRect.width += delta.x;
                        subwindowRect.height += delta.y;
                        break;
                }

                lastMousePosition = Event.current.mousePosition;

                if (subwindowRect.width < 100) subwindowRect.width = 100;
                if (subwindowRect.height < 50) subwindowRect.height = 50;

                Event.current.Use();
            }
        }

        if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && isResizing)
        {
            isResizing = false;

            Event.current.Use();
        }
    }

    private bool HandleMouseInput(Rect handleRect, ResizeDirection cursorDirection)
    {
        if (handleRect.Contains(Event.current.mousePosition))
        {
            EditorGUIUtility.AddCursorRect(handleRect, GetCursorType(cursorDirection));
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                StartResizing(cursorDirection);
                Event.current.Use();
                return true;
            }
        }
        return false;
    }
    private MouseCursor GetCursorType(ResizeDirection cursorDirection)
    {
        switch (cursorDirection)
        {
            case ResizeDirection.Top:
            case ResizeDirection.Bottom:
                return MouseCursor.ResizeVertical;
            case ResizeDirection.Left:
            case ResizeDirection.Right:
                return MouseCursor.ResizeHorizontal;
            case ResizeDirection.TopLeft:
            case ResizeDirection.BottomRight:
                return MouseCursor.ResizeUpLeft;
            case ResizeDirection.TopRight:
            case ResizeDirection.BottomLeft:
                return MouseCursor.ResizeUpRight;
            default:
                return MouseCursor.Arrow;
        }
    }

    private void DrawHeader()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        float headerHeight = 18f;

        Rect headerRect = new Rect(0, 0, subwindowRect.width, headerHeight);
        GUILayout.Label(windowName, EditorStyles.boldLabel);
        DragSubWindow(headerRect);
        GUILayout.EndHorizontal();
    }

    private void DragSubWindow(Rect headerRect)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && headerRect.Contains(e.mousePosition) && !isDragging)
        {
            isDragging = true;
            dragStartMousePosition = e.mousePosition;
            e.Use();
        }

        if (isDragging)
        {
            if (e.type == EventType.MouseDrag)
            {
                Vector2 delta = e.mousePosition - dragStartMousePosition;
                subwindowRect.x += delta.x;
                subwindowRect.y += delta.y;
                e.Use();
            }
            else if (e.type == EventType.MouseUp)
            {
                isDragging = false;
                e.Use();
            }
        }
    }

    private void StartResizing(ResizeDirection cursorDirection)
    {
        isResizing = true;
        currentResizeDirection = cursorDirection;
        lastMousePosition = Event.current.mousePosition;
    }
}

enum ResizeDirection
{
    None, Top, Bottom, Left, Right, TopLeft, TopRight, BottomLeft, BottomRight
}*/