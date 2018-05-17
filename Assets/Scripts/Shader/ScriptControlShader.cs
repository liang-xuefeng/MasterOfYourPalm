using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptControlShader : MonoBehaviour
{
    public Image m_Image;

    public Texture2D m_Texture2D ;

	void Start ()
	{
        m_Image.sprite = Sprite.Create(m_Texture2D, new Rect(0, 0, m_Texture2D.width, m_Texture2D.height), Vector2.one);

        //m_Texture2D = (Texture2D)m_Image.mainTexture;
	}

    void Update()
    {
        
        //Debug.Log(m_Texture2D.GetPixel(0, 0));
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 50, 30), "11"))
        {
            for (int i = 0; i < 225 * 2; i++)
            {
                for (int j = 0; j < 171 * 2; j++)
                {
                    m_Texture2D.SetPixel(i, j, Color.red);
                }
            }
            m_Texture2D.Apply();
        }
        if (GUI.Button(new Rect(10, 30, 50, 30), "22"))
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 500; j++)
                {
                    m_Texture2D.SetPixel(i, j, Color.white);
                }
            }
            m_Texture2D.Apply();
        }
    }
}
