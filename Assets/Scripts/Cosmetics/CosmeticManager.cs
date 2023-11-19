using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticManager : MonoBehaviour
{
    [SerializeField] private List<CosmeticData> cosmetics = new List<CosmeticData>();

    private Dictionary<string, Transform> cosmeticDic = new Dictionary<string, Transform>();

    private void ConvertListToDictionary()
    {
        foreach (CosmeticData cosmetic in cosmetics)
        {
            cosmeticDic.Add(cosmetic.cosmeticID, cosmetic.prefab);
        }
    }
}
