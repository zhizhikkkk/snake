using System.Collections;
using System.Collections.Generic;
using CodeMonkey;
using CodeMonkey.Utils;
using UnityEngine;

public class GameOverWindow : MonoBehaviour
{
    private void Awake()
    {
        transform.Find("retryBtn").GetComponent<Button_UI>().ClickFunc=()=>
        {
            Loader.Load(Loader.Scene.GameScene);
        };
    }
}
