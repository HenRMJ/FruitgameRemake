using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faces : MonoBehaviour
{
    [SerializeField] private SpriteRenderer faceImage;

    [Header("Face Settings")]
    [SerializeField] private float distanceToChangeFace;
    [SerializeField] private Sprite waitingFace, idleFace, closeFace;

    private BaseFruit myFruit;
    private Fruit fruitType;

    private void Awake()
    {
        myFruit = GetComponent<BaseFruit>();
        fruitType = myFruit.GetFruitType();
        faceImage.sprite = waitingFace;
    }

    private void Update()
    {
        if (myFruit.IsHeld) return;
        
        faceImage.sprite = idleFace;

        foreach(BaseFruit fruit in FindObjectsOfType<BaseFruit>())
        {
            if (fruit.GetFruitType() != fruitType) continue;
            if (fruit == myFruit) continue;

            if (Vector2.Distance(fruit.transform.position, transform.position) <= distanceToChangeFace)
            {
                faceImage.sprite = closeFace;
                break;
            }
        }
    }
}
