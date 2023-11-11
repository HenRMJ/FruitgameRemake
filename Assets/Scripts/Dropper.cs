using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dropper : MonoBehaviour
{
    public event EventHandler fruitDropped;

    private PlayerControls playerControls;

    [SerializeField] private float speed, leftLimit, rightLimit;
    [SerializeField] private Transform fruitInstatiationSlot;
    [SerializeField] private BoxCollider2D leftCollider, rightCollider;

    private Bounds leftBound, rightBound;
    private Transform fruitHeld;
    private BaseFruit fruit;

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerControls.Player.Drop.performed += OnDrop;
    }

    private void Start()
    {
        leftBound = leftCollider.bounds;
        rightBound = rightCollider.bounds;

        InstantiateNewFruit();
    }

    // Update is called once per frame
    void Update()
    {
        if (ScoreManager.Instance.HasLost()) return;

        float moveAmount = playerControls.Player.Move.ReadValue<float>();
        
        if (moveAmount > 0 && transform.position.x >= rightLimit ||
            moveAmount < 0 && transform.position.x <= leftLimit)
        {
            return;
        }

        transform.position = new Vector2(transform.position.x + moveAmount * Time.deltaTime * speed, transform.position.y);
    }

    public void OnEnable()
    {
        playerControls.Enable();
    }

    public void OnDisable()
    {
        playerControls.Disable();
    }

    public void OnDrop(InputAction.CallbackContext callbackContext)
    {
        if (ScoreManager.Instance.HasLost()) return;
        DropFruit();
    }

    private void DropFruit()
    {
        fruitHeld.parent = null;
        fruit.IsHeld = false;
        InstantiateNewFruit();
        SoundManager.Instance.PlaySound("Drop");
        fruitDropped?.Invoke(this, EventArgs.Empty);
    }

    private void InstantiateNewFruit()
    {
        Transform fruitTransform = Instantiate(FruitProgression.Instance.GetRandomFruit(), fruitInstatiationSlot);
        fruitHeld = fruitTransform;

        fruitTransform.position = fruitInstatiationSlot.position;

        BaseFruit fruit = fruitTransform.GetComponent<BaseFruit>();
        this.fruit = fruit;
        fruit.IsHeld = true;
        fruit.ID = FruitProgression.Instance.ID;

        SetNewLimit();
    }

    private void SetNewLimit()
    {
        Bounds fruitBound = fruitHeld.GetComponent<CircleCollider2D>().bounds;

        float leftWall = leftBound.center.x + leftBound.extents.x;
        float rightWall = rightBound.center.x - rightBound.extents.x;

        leftLimit = leftWall + fruitBound.extents.x;
        rightLimit = rightWall - fruitBound.extents.x;

        if (transform.position.x < leftLimit)
        {
            transform.position = new Vector2(leftLimit, transform.position.y);
        }

        if (transform.position.x > rightLimit)
        {
            transform.position = new Vector2(rightLimit, transform.position.y);
        }
    }
}
