using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class GameHandler : MonoBehaviour
{
    private LevelGrid levelGrid;
    [SerializeField] private Snake snake;

    private static GameHandler instance;
    private static int score;
    private static bool haveBarrier;


    private void Awake()
    {
        instance=this;
        InitializeStatic();
    }
    private void Start()
    {
        //создаст геймобджект
        // GameObject snakeHeadGameObject = new GameObject();

        // SpriteRenderer snakeSpriteRenderer = snakeHeadGameObject.AddComponent<SpriteRenderer>();
        //мы тут говорим какой именно объект присваиваем снэйкспрайтрендереру
        //snakeSpriteRenderer.sprite = GameAssets.i.snakeHeadSprite;


        /*int number = 0;
        FunctionPeriodic.Create(() =>
        {
            CMDebug.TextPopupMouse("Ding!" + number);

            number++;
        },.3f);*/

        
        levelGrid = new LevelGrid(20,20);

        snake.Setup(levelGrid);
        levelGrid.Setup(snake);

        CMDebug.ButtonUI(Vector2.zero, "Reload Scene", () =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });
    }


    private static void InitializeStatic()
    {
        score = 0;
    }
    public static int GetScore()
    {
        return score;
    }
    
    public static void AddScore()
    {
        score += 1;
    }
    public static void MinusScore()
    {
        score -= 1;
    }
    public static bool GetHaveBarrier()
    {
        return haveBarrier;

    }
    public static void setHaveBarrier(bool haveB)
    {
        haveBarrier = haveB;
    }

}
