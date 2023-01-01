using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarrierWindow : MonoBehaviour
{
    private Text barrierText;
    private void Awake()
    {

        barrierText = transform.Find("barrierText").GetComponent<Text>();
    }

    private void Update()
    {

        barrierText.text = GameHandler.GetHaveBarrier().ToString();
    }
}
