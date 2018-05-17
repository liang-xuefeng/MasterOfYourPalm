using UnityEngine;
using System;

public class UILoader
{
    public static readonly string uiPath = "";//前置公公路径

    public static T LoadUI<T>(string path, string bundleName = "") where T : UIPanel
    {
        path = uiPath + path;
        if (bundleName.Length == 0)
            bundleName = StaticAttributes.MODULE_UI;

        //GameObject uiGameObject = BundleHelper.Instance.Load<GameObject>(bundleName, path);
        GameObject uiGameObject = Resources.Load<GameObject>(path);

        if (uiGameObject == null)
        {
            Debug.LogError("UILoader->LoadUI->Panle加载失败 path=" + path);
            return default(T);
        }

        Vector2 lastTransformPosition = uiGameObject.GetComponent<RectTransform>().anchoredPosition;
        string uiName = uiGameObject.name;

        uiGameObject = MonoBehaviour.Instantiate(uiGameObject) as GameObject;

        uiGameObject.name = uiName;
        T component = uiGameObject.GetComponent<T>();
        if (component == null)
        {
            Debug.LogError("UILoader->LoadUI->Panle加载成功，但找不到component path=" + path + "    component=" + typeof(T));
        }

        UI.Instance.addChild(component, UI.UILayerType.NOTHING);

        uiGameObject.GetComponent<RectTransform>().anchoredPosition = lastTransformPosition;
        uiGameObject.GetComponent<RectTransform>().localScale = Vector3.one;

        float disX = uiGameObject.GetComponent<RectTransform>().anchorMax.x - uiGameObject.GetComponent<RectTransform>().anchorMin.x;
        float disY = uiGameObject.GetComponent<RectTransform>().anchorMax.y - uiGameObject.GetComponent<RectTransform>().anchorMin.y;
        Vector2 sizeDelta = uiGameObject.GetComponent<RectTransform>().sizeDelta;
        if (disX == 1)
            sizeDelta.x = 0;
        if (disY == 1)
            sizeDelta.y = 0;
        uiGameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;

        return component;
    }

    public static void LoadUI<T>(string path, Action<T> callback) where T : UIPanel
    {
        LoadUI<T>(path, StaticAttributes.MODULE_UI, callback);
    }

    public static void LoadUI<K>(string path, string bundleName, Action<K> callback) where K : UIPanel
    {
        path = uiPath + path;
        if (bundleName.Length == 0)
        {
            bundleName = StaticAttributes.MODULE_UI;
        }       
    }


    public static GameObject LoadUIEffect(string path)
    {
        path = uiPath + path;
        GameObject effect = Resources.Load(path, typeof(GameObject)) as GameObject;
        if (effect == null)
            return null;
        effect = MonoBehaviour.Instantiate(effect) as GameObject;
        return effect;
    }
}
