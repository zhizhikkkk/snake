using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using System;
using Unity.VisualScripting;
using Random = UnityEngine.Random;


public class LevelGrid 
{
    private Vector2Int foodGridPosition;
    private GameObject foodGameObject;
    private GameObject spikesGameObject;
    private Vector2Int spikesGridPosition;
    private GameObject shieldGameObject;
    private Vector2Int shieldGridPosition;
    private int countOfDeaths;
    private int width;
    private int height;
    private Snake snake;

    
    public Vector2Int getSpikesGridPosition()
    {
        return spikesGridPosition;
    }
    public LevelGrid(int width,int height)
    {
        this.width = width;
        this.height = height;   
        

        
    }

    public int getCountOfDeath()
    {
        return countOfDeaths;
    }
    
    public void Setup(Snake snake)
    {
        
        this.snake = snake;
        SpawnFood();
        SpawnSpikes();
        SpawnShield();
        
    }
    private void SpawnFood()
    {
        
        do
        {
            foodGridPosition = new Vector2Int(Random.Range(15, 15 + width), Random.Range(15, 15 + height));
        } while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition)!=-1);
        

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
        
    }
    private void SpawnSpikes()
    { 
        do
        {
            spikesGridPosition = new Vector2Int(Random.Range(15, 15 + width), Random.Range(15, 15 + height));
            
        } while (snake.GetFullSnakeGridPositionList().IndexOf(spikesGridPosition) != -1);


        spikesGameObject = new GameObject("Spikes", typeof(SpriteRenderer));
        spikesGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.spikesSprite;
        spikesGameObject.transform.position = new Vector3(spikesGridPosition.x, spikesGridPosition.y);
    }
    private void SpawnShield()
    {

        do
        {

            shieldGridPosition = new Vector2Int(Random.Range(15, 15 + width), Random.Range(15, 15 + height));

        } while (snake.GetFullSnakeGridPositionList().IndexOf(shieldGridPosition) != -1);


        shieldGameObject = new GameObject("Shield", typeof(SpriteRenderer));
        shieldGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.shieldSprite;
        shieldGameObject.transform.position = new Vector3(shieldGridPosition.x, shieldGridPosition.y);
    }

    public bool TrySnakeEatFood(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == foodGridPosition)
        {
            GameObject.Destroy(foodGameObject);
            SpawnFood();
            GameHandler.AddScore();
            CMDebug.TextPopupMouse("Snake Ate Food");
            return true;
        }
        else return false;
    }

    public bool TrySnakeHitSpikes(Vector2Int snakeGridPosition)
    {
        
        if (snakeGridPosition == spikesGridPosition)
        {
            if (GameHandler.GetHaveBarrier())
            {
                GameObject.Destroy(spikesGameObject);
                SpawnSpikes();
                GameHandler.setHaveBarrier(false);
                return false;
                
            }
            GameObject.Destroy(spikesGameObject);
            SpawnSpikes();
            GameHandler.MinusScore();
            CMDebug.TextPopupMouse("Snake Ate Spikes");
            countOfDeaths++;
            return true;
        }
        else return false;
    }
    public bool TrySnakeHasShield(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == shieldGridPosition)
        {
            GameObject.Destroy(shieldGameObject);
            SpawnShield();
            return true;
        }
        else return false;
    }

    public Vector2Int ValidateGradePosition(Vector2Int gridPosition)
    {
        if (gridPosition.x < 15)
        {
            gridPosition.x=width+15;
        }

        if (gridPosition.x > 35)
        {
            gridPosition.x = 15;
        }

        if (gridPosition.y < 15)
        {
            gridPosition.y=height+15;
        }

        if (gridPosition.y > 35)
        {
            gridPosition.y = 15;
        }

        return gridPosition;
    }
}
