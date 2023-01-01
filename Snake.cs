using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using CodeMonkey;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Snake : MonoBehaviour
{
    private enum Direction
    {
        Left,Right,Up,Down
    }

    private enum State
    {
        Alive,Dead
    }

    private State state;

    private Direction gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
    
    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }
    private void Awake()
    {
        gridPosition = new Vector2Int(25, 25); //дали начальное значение позиции змейки
        gridMoveTimerMax = 1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;//по дефолту змейка будет двигаться направо

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;

        snakeBodyPartList = new List<SnakeBodyPart>();
        state=State.Alive;
    }
    private void Update()
    {
        if (levelGrid.getCountOfDeath() == 3) state = State.Dead;
        if (GameHandler.GetScore() < 0) state = State.Dead;
        switch (state)
        { 
            case State.Alive:
                HandleInput();
                HandleGridMovement();
                break;
            case State.Dead:
                break;
        }
        
        
        
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (gridMoveDirection != Direction.Down)
            {
                gridMoveDirection = Direction.Up;
            }

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (gridMoveDirection!= Direction.Up)
            {
                gridMoveDirection = Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (gridMoveDirection !=Direction.Left)
            {
                gridMoveDirection = Direction.Right;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (gridMoveDirection != Direction.Right)
            {
                gridMoveDirection = Direction.Left;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection != Direction.Down)
            {
                gridMoveDirection = Direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection != Direction.Up)
            {
                gridMoveDirection = Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection != Direction.Right)
            {
                gridMoveDirection = Direction.Left;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection != Direction.Left)
            {
                gridMoveDirection = Direction.Right;
            }
        }
    }
    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime * 5;


        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;
            SnakeMovePosition previousSnakeMovePosition = null;
            if (snakeMovePositionList.Count > 0)
            {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition,gridPosition, gridMoveDirection);

            snakeMovePositionList.Insert(0,snakeMovePosition);
            Vector2Int gridMoveDirectionVector;
            switch (gridMoveDirection)
            { 
            default:
                case Direction.Right: 
                    gridMoveDirectionVector = new Vector2Int(1, 0);
                    break;
                case Direction.Left:
                    gridMoveDirectionVector = new Vector2Int(-1, 0);
                    break;
                case Direction.Up:
                    gridMoveDirectionVector = new Vector2Int(0, 1);
                    break;
                case Direction.Down:
                    gridMoveDirectionVector = new Vector2Int(0, -1);
                    break;
            }

            gridPosition += gridMoveDirectionVector;
            gridPosition=levelGrid.ValidateGradePosition(gridPosition);
            bool snakeHasShield = levelGrid.TrySnakeHasShield(gridPosition);

            if (snakeHasShield)
            {
                GameHandler.setHaveBarrier(true);
            }
            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);

            if (snakeAteFood)
            {
                //Snake ate food,grow body
                snakeBodySize++;
                CreateSnakeBody();
                UpdateSnakeBodyParts();
            }

            bool snakeAteSpikes = levelGrid.TrySnakeHitSpikes(gridPosition);
            if (snakeAteSpikes)
            {
                
                
                snakeBodySize--;
                DeleteOneSnakeBody();
                UpdateSnakeBodyParts();
                
                
            }
            
             /*if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }*/


           /* for (int i = 0; i < snakeMovePositionList.Count; i++)
            {
                Vector2Int snakeMovePosition = snakeMovePositionList[i];
                World_Sprite worldSprite=World_Sprite.Create(new Vector3(snakeMovePosition.x,snakeMovePosition.y),Vector3.one*1f,Color.white);
                FunctionTimer.Create(worldSprite.DestroySelf, gridMoveTimerMax);
            }*/
           foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
           {
               Vector2Int snakeBodyPartGridPosition= snakeBodyPart.GetGridPosition();
               if (gridPosition == snakeBodyPartGridPosition)
               {
                   //GAME OVER
                   CMDebug.TextPopup("DEAD", transform.position);
                   state = State.Dead;
               }
                 

           }
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0,0,GetAngleFromVector(gridMoveDirectionVector)-90);
            UpdateSnakeBodyParts();
            

        }
        
    }

    private void CreateSnakeBody()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }
    private void DeleteOneSnakeBody()
    {
        
        snakeBodyPartList.RemoveAt(snakeBodyPartList.Count-1);
        

    }
    
    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
        }
    }
    
    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }
    //return the full list of positions occupied by tha snake :head +body
    public List<Vector2Int> GetFullSnakeGridPositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>(){gridPosition};
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        
        return gridPositionList;
    }

    private class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;
        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);
            float angle;
            switch (snakeMovePosition.GetDirection())
            {
            default:
                case Direction.Up:
                switch (snakeMovePosition.GetPreviousDirection())
                {
                    default:
                        angle = 0;
                        break;
                    case Direction.Left:
                        angle = 45;
                        break;
                    case Direction.Right:
                        angle = -45;
                        break;
                }

                break;
                    
                case Direction.Down:
                switch (snakeMovePosition.GetPreviousDirection())
                {
                    default:
                        angle = 180;
                        break;
                    case Direction.Left:
                        angle = -45;
                        break;
                    case Direction.Right:
                        angle = 45;
                        break;
                }

                break;
            case Direction.Left:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 270;
                            break;
                        case Direction.Down:
                            angle = -45;
                            break;
                        case Direction.Up:
                            angle = 45;
                            break;
                    }

                    break;
                case Direction.Right://currently going to the right
                    
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default: 
                            angle = 90;
                            break;
                        case Direction.Down://previously was going down
                            angle = 45;
                            break;
                        case Direction.Up:
                            angle = -45;
                            break;
                    }

                    break;
                
            }
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.GetGridPosition();
        }
    }
    //handles on move position from the snake
    private class SnakeMovePosition
    {
        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition,Vector2Int gridPosition, Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition=gridPosition;
            this.direction=direction;
        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }

        public Direction GetDirection()
        {
            return direction;
        }

        public Direction GetPreviousDirection()
        {
            if (previousSnakeMovePosition == null)
            {
                return Direction.Right;
            }
            else
            {
                return previousSnakeMovePosition.direction;
            }
        }
    }
}
