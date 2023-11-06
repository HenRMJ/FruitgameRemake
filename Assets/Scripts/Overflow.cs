using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Overflow : MonoBehaviour
{
    private const float TIME_TO_FAIL = 2f;

    private float timer;
    private int fruitInFailArea;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BaseFruit fruit))
        {
            if (fruit.IsHeld) return;
            fruitInFailArea++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BaseFruit fruit))
        {
            if (fruit.IsHeld) return;

            fruitInFailArea--;
        }
    }

    private void Update()
    {
        FailPlayer();
    }

    private void FailPlayer()
    {
        if (fruitInFailArea > 0)
        {
            timer += Time.deltaTime;
            if (timer > TIME_TO_FAIL)
            {
                ScoreManager.Instance.LostGame();
            }
        }
        else
        {
            timer = 0;
        }
    }
}
