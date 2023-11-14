using UnityEngine;
using TMPro;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Transform leadboardPrefab;
    [SerializeField] private RectTransform contentParent;
    [SerializeField] private bool autoLoadBoard;
    [SerializeField] private GameMode mode;

    private string PUBLIC_CLASSIC_KEY = Keys.Classic;
    private string PUBLIC_ENDLESS_KEY = Keys.Endless;

    private void Start()
    {
        if (!autoLoadBoard) return;

        AutoLoadLeaderboard();
    }

    public void AutoLoadLeaderboard()
    {
        string key = string.Empty;

        key = SaveManager.Instance.Mode == GameMode.Classic ? PUBLIC_CLASSIC_KEY : PUBLIC_ENDLESS_KEY;

        LeaderboardCreator.GetLeaderboard(key, ((msg) =>
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

        string key = string.Empty;

        key = SaveManager.Instance.Mode == GameMode.Classic ? PUBLIC_CLASSIC_KEY : PUBLIC_ENDLESS_KEY;

        LeaderboardCreator.UploadNewEntry(key, inputField.text, SaveManager.Instance.GetHighScore(SaveManager.Instance.Mode), ((msg) =>
        {
            ClearLeaderboard();
            AutoLoadLeaderboard();
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
