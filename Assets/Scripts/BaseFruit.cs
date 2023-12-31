using UnityEngine;
using System;

public class BaseFruit : MonoBehaviour
{
    public static EventHandler OnFruitCombined;

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
            EndlessMode(baseFruit, collision);

            if (fruit == Fruit.Pumpkin) return;
            if (combined) return;

            if (baseFruit.GetFruitType() == fruit)
            {
                if (ID <= baseFruit.ID) return;

                if (combined = FruitProgression.Instance.TryCombineFruitAtPosition(transform, collision.transform, (int)fruit + 1))
                {
                    ScoreManager.Instance.IncreaseScore(((int)fruit + 1) * 2);
                    OnFruitCombined?.Invoke(this, EventArgs.Empty);
                    Destroy(gameObject);
                    Destroy(collision.gameObject);
                } 
            }
        }
    }

    private void EndlessMode(BaseFruit baseFruit, Collision2D collision)
    {
        if (SaveManager.Instance.Mode == GameMode.Endless)
        {
            if (fruit != Fruit.Pumpkin) return;

            if (combined) return;

            if (baseFruit.GetFruitType() == fruit)
            {
                if (ID <= baseFruit.ID) return;

                OnFruitCombined?.Invoke(this, EventArgs.Empty);
                foreach (BaseFruit fruit in FindObjectsOfType<BaseFruit>())
                {
                    if (fruit.isHeld)
                    {
                        continue;
                    }

                    Destroy(fruit.gameObject);
                }
            }
        }
    }

    public Sprite GetFruitSprite() => fruitUISprite;
    public Fruit GetFruitType() => fruit;
}
