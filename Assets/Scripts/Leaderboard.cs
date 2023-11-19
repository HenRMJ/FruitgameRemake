using UnityEngine;
using TMPro;
using Dan.Main;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI labelText;
    [SerializeField] private Image leaderboardPanel;
    [SerializeField] private Transform leadboardEntryPrefab;
    [SerializeField] private RectTransform contentParent;
    [SerializeField] private bool autoLoadBoard;
    [SerializeField] private GameMode mode;
    [SerializeField] private TMP_FontAsset greenFont, yellowFont, redFont;
    [SerializeField] private Color greenFontColor, yellowFontColor, redFontColor, green, yellow, red;

    private string PUBLIC_CLASSIC_KEY = Keys.Classic;
    private string PUBLIC_ENDLESS_KEY = Keys.Endless;
    private string PUBLIC_QUICK_KEY = Keys.Quick;

    private void Start()
    {
        if (autoLoadBoard)
        {
            AutoLoadLeaderboard();
        }
        else
        {
            LoadSpecificLeadboard(mode);
        }     
    }

    public void AutoLoadLeaderboard()
    {
        LoadSpecificLeadboard(SaveManager.Instance.Mode);
    }

    public void LoadSpecificLeadboard(GameMode gameMode)
    {
        string key = string.Empty;
        Color fontColor = new Color();

        switch (gameMode)
        {
            case GameMode.Classic:
                key = PUBLIC_CLASSIC_KEY;
                labelText.font = redFont;
                labelText.text = "Classic";
                labelText.color = redFontColor;
                fontColor = redFontColor;
                leaderboardPanel.color = red;
                break;
            case GameMode.Endless:
                key = PUBLIC_ENDLESS_KEY;
                labelText.font = yellowFont;
                labelText.text = "Endless";
                labelText.color = yellowFontColor;
                fontColor = yellowFontColor;
                leaderboardPanel.color = yellow;
                break;
            case GameMode.Quick:
                key = PUBLIC_QUICK_KEY;
                labelText.font = greenFont;
                labelText.text = "Quick Play";
                labelText.color = greenFontColor;
                fontColor = greenFontColor;
                leaderboardPanel.color = green;
                break;

        }

        LeaderboardCreator.GetLeaderboard(key, ((msg) =>
        {
            float prefabSize = leadboardEntryPrefab.GetComponent<RectTransform>().rect.size.y + 16f;

            contentParent.sizeDelta = new Vector2(contentParent.rect.width, prefabSize * msg.Length);

            for (int i = 0; i < msg.Length; i++)
            {
                Transform leaderboardItem = Instantiate(leadboardEntryPrefab, contentParent);

                TextMeshProUGUI rank = leaderboardItem.GetChild(0).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI username = leaderboardItem.GetChild(1).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI score = leaderboardItem.GetChild(2).GetComponent<TextMeshProUGUI>();

                rank.text = msg[i].Rank.ToString();
                username.text = msg[i].Username;
                score.text = msg[i].Score.ToString();

                rank.color = fontColor;
                username.color = fontColor;
                score.color = fontColor;
            }
        }));
    }

    public void SetLeaderboardEntry()
    {
        if (inputField.text == string.Empty) return;

        GameMode gameMode;

        if (autoLoadBoard)
        {
            gameMode = SaveManager.Instance.Mode;
        }
        else
        {
            gameMode = mode;
        }

        string key = string.Empty;

        switch (gameMode)
        {
            case GameMode.Classic:
                key = PUBLIC_CLASSIC_KEY;
                break;
            case GameMode.Endless:
                key = PUBLIC_ENDLESS_KEY;
                break;
            case GameMode.Quick:
                key = PUBLIC_QUICK_KEY;
                break;

        }

        LeaderboardCreator.UploadNewEntry(key, inputField.text, SaveManager.Instance.GetHighScore(gameMode), ((msg) =>
        {
            ClearLeaderboard();
            LoadSpecificLeadboard(gameMode);
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
