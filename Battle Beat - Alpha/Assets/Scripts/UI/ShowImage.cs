﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowImage : MonoBehaviour
{
    [SerializeField]
    Image image;
    [SerializeField]
    float interval = 0.2f;
    float displayTime = 1f;
    bool isEnd = false;
    int index;
    Coroutine coroutine=null;
    List<Sprite> sprites=new List<Sprite>();
    public static ShowImage _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void ShowImages(string[] textureNames,float time = 0.5f,float time2=0.1f)
    {
        displayTime = time;
        interval = time2;
        index = 0;
        sprites.Clear();
        Load(textureNames);
        coroutine = StartCoroutine(ShowImageMain());
    }

    public bool IsEnd()
    {
        return isEnd;
    }
    IEnumerator ShowImageMain()
    {
        while (index / 2 < sprites.Count)
        {
            float time = 0;
            if (index % 2 == 0)
            {
                time = displayTime;    
                image.rectTransform.sizeDelta = new Vector2(sprites[index / 2].rect.width, sprites[index / 2].rect.height);
                image.sprite = sprites[index / 2];
                image.gameObject.SetActive(true);
            }
            else
            {
                time = interval;
                image.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(time);
            index++;
        }
        isEnd = true;
    }

    private void Load(string[] textureName)
    {
        foreach(string str in textureName)
        {
            Texture2D texture = Resources.Load("Images/" + str) as Texture2D;
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
            sprites.Add(sprite);
        }
    }
}