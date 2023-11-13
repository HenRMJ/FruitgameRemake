using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Transform leadboardPrefab;
    [SerializeField] private RectTransform contentParent;

    private const string PUBLIC_LEADERBOARD_KEY = "54efd0143bae947553c51d294c8c6d755ae1f1c5abd783a58f83a3cab713ec04";

    private void Start()
    {
        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(PUBLIC_LEADERBOARD_KEY, ((msg) =>
        {
            float prefabSize = leadboardPrefab.GetComponent<RectTransform>().rect.size.y;

            contentParent.sizeDelta = new Vector2(contentParent.rect.width, prefabSize * msg.Length);

            for (int i = 0; i < msg.Length; i++)
            {
                Transform leaderboardItem = Instantiate(leadboardPrefab, contentParent);

                leaderboardItem.GetChild(0).GetComponent<TextMeshProUGUI>().text = msg[i].Rank.ToString();
                leaderboardItem.GetChild(1).GetComponent<TextMeshProUGUI>().text = msg[i].Username;
                leaderboardItem.GetChild(2).GetComponent<TextMeshProUGUI>().text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderboardEntry()
    {
        if (inputField.text == string.Empty) return;

        LeaderboardCreator.UploadNewEntry(PUBLIC_LEADERBOARD_KEY, inputField.text, SaveManager.Instance.GetHighScore(), ((msg) =>
        {
            ClearLeaderboard();
            GetLeaderboard();
        }));
    }

    private void ClearLeaderboard()
    {
        foreach(Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
    }
}
