using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cosemetic", menuName = "Cosmetic")]
public class CosmeticData : ScriptableObject
{
    public string cosmeticID;
    public Transform prefab;
}
