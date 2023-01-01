using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{

    public static GameAssets i;

    private void Awake()//«апускаетс€ единыжды при запуске игры
    {
        i = this;//инстаншиейт головы
        //вроде, когда создали public Sprite snakeHeadSprite; мы присваиваем инстансу эту голову
    }

    public Sprite snakeHeadSprite;
    public Sprite foodSprite;
    public Sprite snakeBodySprite;
    public Sprite spikesSprite;
    public Sprite shieldSprite;
}
