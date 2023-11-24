using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dropper : MonoBehaviour
{
    public event EventHandler fruitDropped;

    private PlayerControls playerControls;

    [SerializeField] private float speed;
    [SerializeField] private Transform fruitInstatiationSlot;
    [SerializeField] private BoxCollider2D leftWall, rightWall;


    private Bounds leftBound, rightBound;
    private float leftLimit, rightLimit;
    private Transform fruitHeld;
    private BaseFruit fruit;
    private FMODUnity.StudioEventEmitter cloudMove;

    private void Awake()
    {

        playerControls = new PlayerControls();

        playerControls.Player.Drop.performed += OnDrop;

        playerControls.Player.Move.started += OnMoveStarted;

        playerControls.Player.Move.canceled += OnMoveCanceled;

        cloudMove = GetComponent<FMODUnity.StudioEventEmitter>();
    }


    private void Start()
    {
        leftBound = leftWall.bounds;
        rightBound = rightWall.bounds;
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
        playerControls.Player.Drop.performed -= OnDrop;

        playerControls.Player.Move.started -= OnMoveStarted;

        playerControls.Player.Move.canceled -= OnMoveCanceled;

        playerControls.Disable();

    }

    public void OnDrop(InputAction.CallbackContext callbackContext)
    {
        if (ScoreManager.Instance.HasLost()) return;
        DropFruit();
    }

    public void OnMoveStarted(InputAction.CallbackContext callbackContext)
    {
        if (ScoreManager.Instance.HasLost()) return;
        cloudMove.Play();
    }

    public void OnMoveCanceled(InputAction.CallbackContext callbackContext)
    {
        if (ScoreManager.Instance.HasLost()) return;
        cloudMove.Stop();
    }

    private void DropFruit()
    {
        fruitHeld.parent = null;
        fruit.IsHeld = false;
        InstantiateNewFruit();
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
        CircleCollider2D collider = fruitHeld.GetComponent<CircleCollider2D>();

        collider.enabled = true;
        Bounds fruitBound = collider.bounds;
        collider.enabled = false;

        float staticLeftLimit = leftBound.extents.x + leftBound.center.x;
        float staticRightLimit = rightBound.center.x - rightBound.extents.x;

        leftLimit = staticLeftLimit + fruitBound.extents.x;
        rightLimit = staticRightLimit - fruitBound.extents.x;

        if (transform.position.x > rightLimit)
        {
            transform.position = new Vector2(rightLimit, transform.position.y);
        }

        if (transform.position.x < leftLimit)
        {
            transform.position = new Vector2(leftLimit, transform.position.y);
        }
    }
}
