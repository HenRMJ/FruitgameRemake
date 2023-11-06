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

    private Transform fruitHeld;
    private BaseFruit fruit;

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerControls.Player.Drop.performed += OnDrop;
    }

    private void Start()
    {
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

        Debug.Log($"Dropper Fruit Name: {fruit.name}, Dropper Fruit ID: {fruit.ID}");
    }
}
