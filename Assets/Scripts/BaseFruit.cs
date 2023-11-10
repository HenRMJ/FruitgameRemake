using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFruit : MonoBehaviour
{
    [SerializeField] private Fruit fruit;
    [SerializeField] private Sprite fruitUISprite;

    public int ID;
    private bool combined;
    private bool isHeld;

    public bool IsHeld
    {
        get
        {
            return isHeld;
        }
        set
        {
            isHeld = value;

            if (!value)
            {
                GetComponent<CircleCollider2D>().enabled = true;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BaseFruit baseFruit))
        {
            if (fruit == Fruit.Eleven) return;
            if (combined) return;

            if (baseFruit.GetFruitType() == fruit)
            {
                if (ID <= baseFruit.ID) return;

                if (combined = FruitProgression.Instance.TryCombineFruitAtPosition(transform, collision.transform, (int)fruit + 1))
                {
                    ScoreManager.Instance.IncreaseScore(((int)fruit + 1) * 2);
                    Destroy(gameObject);
                    Destroy(collision.gameObject);
                }
                
            }
        }
    }

    public Sprite GetFruitSprite() => fruitUISprite;
    public Fruit GetFruitType() => fruit;
}