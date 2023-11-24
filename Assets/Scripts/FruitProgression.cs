using System;
using UnityEngine;
using UnityEngine.UI;

public class FruitProgression : MonoBehaviour
{
    public static FruitProgression Instance;

    public event EventHandler OnFruitCombined;

    [SerializeField] private Transform[] fruitProgression;
    [SerializeField] private Fruit maxFruitSizeForSpawning;
    [SerializeField] private Image nextFruitImage;
    [SerializeField] private Transform combinationUIParent;
    [SerializeField] private Transform imagePrefab;

    [Header("Developer Setting")]
    [SerializeField] private bool generateFixedFruit;
    [SerializeField] private Fruit fruitToGenerate;

    private int id;
    private Transform nextFruit;

    public int ID
    {
        get
        {
            id++;
            return id;
        }

        private set
        {
            id = value;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("You have another instance of the Fruitprogression manager");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        AssignNextFruit();
        AddImagesToCombinationUI();
    }

    public bool TryCombineFruitAtPosition(Transform original, Transform contact, int fruitIndex)
    {
        if (original == null || contact == null)
        {
            return false;
        }

        Vector2 midPoint =
            new Vector2((original.position.x + contact.position.x) / 2,
            (original.position.y + contact.position.y) / 2);

        Transform fruitTransform = Instantiate(fruitProgression[fruitIndex], midPoint, Quaternion.identity);

        BaseFruit newFruit = fruitTransform.GetComponent<BaseFruit>();

        newFruit.IsHeld = false;

        newFruit.ID = ID;

        OnFruitCombined?.Invoke(this, EventArgs.Empty);

        return true;
    }

    public Transform GetRandomFruit()
    {
        Transform fruitToGive = nextFruit;

        AssignNextFruit();
         
        return fruitToGive;
    }

    private void AssignNextFruit()
    {
        int index;

        if (generateFixedFruit)
        {
            index = (int)fruitToGenerate;
        } else
        {
            index = UnityEngine.Random.Range(0, (int)maxFruitSizeForSpawning + 1);
        }        

        nextFruit = fruitProgression[index];

        UpdateFruitImage();
    }

    private void UpdateFruitImage()
    {
        nextFruitImage.sprite = nextFruit.GetComponent<BaseFruit>().GetFruitSprite();
    }

    private void AddImagesToCombinationUI()
    {
        for (int i = fruitProgression.Length - 1; i >= 0; i--)
        {
            Image image = Instantiate(imagePrefab, combinationUIParent).GetComponent<Image>();
            image.sprite = fruitProgression[i].GetComponent<BaseFruit>().GetFruitSprite();
        }
    }
}
