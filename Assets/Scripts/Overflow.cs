using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Overflow : MonoBehaviour
{
    [SerializeField] private SpriteRenderer overflowLine;
    [SerializeField] private float detectionDistance;

    private const float TIME_TO_FAIL = 2f;

    private Bounds bounds = new Bounds();

    private float timer;
    private int fruitInFailArea;

    private void Awake()
    {
        bounds = GetComponent<BoxCollider2D>().bounds;
    }

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
        CheckDistnaceToFruit();
    }

    private void FailPlayer()
    {
        if (fruitInFailArea > 0)
        {
            timer += Time.deltaTime;

            float greenBlueValues = (TIME_TO_FAIL - timer) / TIME_TO_FAIL;

            overflowLine.color = new Color(1, greenBlueValues, greenBlueValues, overflowLine.color.a);

            if (timer > TIME_TO_FAIL)
            {
                ScoreManager.Instance.LostGame();
            }
        }
        else
        {
            timer = 0;
            overflowLine.color = new Color(1, 1, 1, overflowLine.color.a);
        }
    }

    private void CheckDistnaceToFruit()
    {
        Vector2 bottomOverlowPosition = new Vector2(bounds.center.x, bounds.center.y - bounds.extents.y);
        float closestPosition = Mathf.Infinity;

        foreach (BaseFruit fruit in FindObjectsOfType<BaseFruit>())
        {
            if (fruit.IsHeld) continue;

            Bounds fruitBounds = fruit.gameObject.GetComponent<CircleCollider2D>().bounds;

            Vector2 transformedTopFruitPosition = new Vector2(bottomOverlowPosition.x, fruitBounds.center.y + fruitBounds.extents.y);

            float distanceToFruit = Vector2.Distance(bottomOverlowPosition, transformedTopFruitPosition);

            if (detectionDistance <= distanceToFruit) continue;
            if (closestPosition < distanceToFruit) continue;
            
            closestPosition = distanceToFruit;
            
            // Change Visibility of overflow line
            if (closestPosition == Mathf.Infinity)
            {
                overflowLine.color = new Color(1, 1, 1, 0); 
            }
            else
            {
                float alphaLevel = (detectionDistance - closestPosition) / detectionDistance;

                overflowLine.color = new Color(overflowLine.color.r, overflowLine.color.g, overflowLine.color.b, alphaLevel);
            }
        }
    }
}
