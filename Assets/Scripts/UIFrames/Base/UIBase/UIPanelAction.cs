using UnityEngine;
using System;
using System.Collections;

public class UIPanelAction
{
    public static void RightMoveIn(GameObject obj, float changeTime, Action updateAction = null, Action completeAction = null)
    {
        var rectTransform = obj.GetComponent<RectTransform>();
        var originAPos = rectTransform.anchoredPosition;
        iTween.ValueTo(obj, iTween.Hash
            (
                "name", "RightMoveIn",
                "from", 1.0f,
                "to", 0.0f,
                "time", changeTime,
                "easeType", iTween.EaseType.easeInOutExpo,
                "islocal", true,
                "onupdate", (Action<float>)delegate(float value)
                {
                    rectTransform.anchoredPosition = new Vector2(
                        originAPos.x + value * rectTransform.rect.width, originAPos.y);

                    if (updateAction != null)
                    {
                        updateAction();
                    }
                },
                "oncomplete", (Action)delegate()
                {
                    if (completeAction != null)
                    {
                        completeAction();
                    }
                }
            ));
    }

    public static void RightMoveOut(GameObject obj, float changeTime, Action updateAction = null, Action completeAction = null)
    {
        var rectTransform = obj.GetComponent<RectTransform>();
        var originAPos = rectTransform.anchoredPosition;
        iTween.ValueTo(obj, iTween.Hash
            (
                "name", "RightMoveOut",
                "from", 1.0f,
                "to", 0.0f,
                "time", changeTime,
                "easeType", iTween.EaseType.easeInOutExpo,
                "islocal", true,
                "onupdate", (Action<float>)delegate(float value)
                {
                    rectTransform.anchoredPosition = new Vector2(
                        originAPos.x - value * rectTransform.rect.width, originAPos.y);

                    if (updateAction != null)
                    {
                        updateAction();
                    }
                },
                "oncomplete", (Action)delegate()
                {
                    if (completeAction != null)
                    {
                        completeAction();
                    }
                }
            ));
    }

    public static void LeftMoveIn(GameObject obj, float changeTime, Action updateAction = null, Action completeAction = null)
    {
        var rectTransform = obj.GetComponent<RectTransform>();
        var originAPos = rectTransform.anchoredPosition;
        iTween.ValueTo(obj, iTween.Hash
                       (
            "name", "LeftMoveIn",
            "from", 0.0f,
            "to", 1.0f,
            "time", changeTime,
            "easeType", iTween.EaseType.easeInOutExpo,
            "islocal", true,
            "onupdate", (Action<float>)delegate(float value)
            {
                rectTransform.anchoredPosition = new Vector2(
                    originAPos.x + value * rectTransform.rect.width, originAPos.y);

                if (updateAction != null)
                {
                    updateAction();
                }
            },
        "oncomplete", (Action)delegate()
        {
            if (completeAction != null)
            {
                completeAction();
            }
        }
        ));
    }

    public static void LeftMoveOut(GameObject obj, float changeTime, Action updateAction = null, Action completeAction = null)
    {
        var rectTransform = obj.GetComponent<RectTransform>();
        var originAPos = rectTransform.anchoredPosition;
        iTween.ValueTo(obj, iTween.Hash
                       (
            "name", "LeftMoveOut",
            "from", 0.0f,
            "to", 1.0f,
            "time", changeTime,
            "easeType", iTween.EaseType.easeInOutExpo,
            "islocal", true,
            "onupdate", (Action<float>)delegate(float value)
            {
                rectTransform.anchoredPosition = new Vector2(
                    originAPos.x - value * rectTransform.rect.width, originAPos.y);

                if (updateAction != null)
                {
                    updateAction();
                }
            },
        "oncomplete", (Action)delegate()
        {
            if (completeAction != null)
            {
                completeAction();
            }
        }
        ));
    }

    public static void BottomMoveIn(GameObject obj, float changeTime, Action updateAction = null, Action completeAction = null)
    {
        var rectTransform = obj.GetComponent<RectTransform>();
        var originAPos = rectTransform.anchoredPosition;
        iTween.ValueTo(obj, iTween.Hash
                       (
            "name", "BottomMoveIn",
            "from", 0.0f,
            "to", 1.5f,
            "time", changeTime,
            "easeType", iTween.EaseType.easeInOutExpo,
            "islocal", true,
            "onupdate", (Action<float>)delegate(float value)
            {
                rectTransform.anchoredPosition = new Vector2(originAPos.x,
                                                             originAPos.y + value * rectTransform.rect.height);

                if (updateAction != null)
                {
                    updateAction();
                }
            },
        "oncomplete", (Action)delegate()
        {
            if (completeAction != null)
            {
                completeAction();
            }
        }
        ));
    }

    public static void BottomMoveOut(GameObject obj, float changeTime, Action updateAction = null, Action completeAction = null)
    {
        var rectTransform = obj.GetComponent<RectTransform>();
        var originAPos = rectTransform.anchoredPosition;
        iTween.ValueTo(obj, iTween.Hash
                       (
            "name", "BottomMoveOut",
            "from", 0.0f,
            "to", 1.5f,
            "time", changeTime,
            "easeType", iTween.EaseType.easeInOutExpo,
            "islocal", true,
            "onupdate", (Action<float>)delegate(float value)
            {
                rectTransform.anchoredPosition = new Vector2(originAPos.x,
                                                             originAPos.y - value * rectTransform.rect.height);

                if (updateAction != null)
                {
                    updateAction();
                }
            },
        "oncomplete", (Action)delegate()
        {
            if (completeAction != null)
            {
                completeAction();
            }
        }
        ));
    }

    public static void AlphaFadeIn(GameObject obj, float changeTime, Action updateAction = null, Action completeAction = null)
    {
        if (obj.GetComponent<CanvasGroup>() == null)
        {
            obj.AddComponent<CanvasGroup>();
        }
        iTween.ValueTo(obj, iTween.Hash
                       (
            "name", "AlphaFadeIn",
            "from", 0.0f,
            "to", 1.0f,
            "time", changeTime,
            "easeType", iTween.EaseType.easeInOutExpo,
            "islocal", true,
            "onupdate", (Action<float>)delegate(float value)
            {
                obj.GetComponent<CanvasGroup>().alpha = value;
                if (updateAction != null)
                {
                    updateAction();
                }
            },
        "oncomplete", (Action)delegate()
        {
            if (completeAction != null)
            {
                completeAction();
            }
        }
        ));

    }

    public static void AlphaFadeOut(GameObject obj, float changeTime, Action updateAction = null, Action completeAction = null)
    {
        if (obj.GetComponent<CanvasGroup>() == null)
        {
            obj.AddComponent<CanvasGroup>();
        }
        iTween.ValueTo(obj, iTween.Hash
                       (
            "name", "AlphaFadeOut",
            "from", 1.0f,
            "to", 0f,
            "time", changeTime,
            "easeType", iTween.EaseType.easeInOutExpo,
            "islocal", true,
            "onupdate", (Action<float>)delegate(float value)
            {
                obj.GetComponent<CanvasGroup>().alpha = value;
                if (updateAction != null)
                {
                    updateAction();
                }
            },
        "oncomplete", (Action)delegate()
        {
            if (completeAction != null)
            {
                completeAction();
            }
        }
        ));

    }

    public static void ScaleFadeIn(GameObject obj, float changeTime, Action updateAction = null, Action completeAction = null)
    {
        obj.SetActive(true);
        obj.transform.localScale = new Vector3(1, 1, 1);
        iTween.ScaleFrom(obj, iTween.Hash
                         (
            "scale", Vector3.zero,
            "name", "ScaleFadeIn",
            "time", changeTime,
            "easeType", iTween.EaseType.easeInOutCirc,
            "islocal", true,
            "onupdate", (Action)delegate()
            {
                if (updateAction != null)
                {
                    updateAction();
                }
            },
        "oncomplete", (Action)delegate()
        {
            if (completeAction != null)
            {
                completeAction();
            }
        }

        ));
    }

    public static void ScaleFadeOut(GameObject obj, float changeTime, Action updateAction = null, Action completeAction = null)
    {
        iTween.ScaleTo(obj, iTween.Hash
                       (
            "name", "ScaleFadeOut",
            "scale", Vector3.zero,
            "time", changeTime,
            "easeType", iTween.EaseType.easeInOutCirc,
            "islocal", true,
            "onupdate", (Action)delegate()
            {
                if (updateAction != null)
                {
                    updateAction();
                }
            },
        "oncomplete", (Action)delegate()
        {
            if (completeAction != null)
            {
                completeAction();
            }
        }

        ));
    }
}
