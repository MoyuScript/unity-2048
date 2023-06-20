using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class UI : MonoBehaviour
{
    private static UIDocument _uiDocument;
    public static UIDocument uiDocument {
        get {
            if (_uiDocument == null) {
                _uiDocument = GameObject.Find("UIDocument").GetComponent<UIDocument>();
            }
            return _uiDocument;
        }
    }
    void Awake()
    {
        GlobalState.instance.scoreChangedEvent.AddListener(OnScoreChanged);
        GlobalState.instance.gameOverChangedEvent.AddListener(OnGameOverChanged);

        Button repoLink = uiDocument.rootVisualElement.Q<Button>("RepoLink");
        repoLink.clicked += () => {
            Application.OpenURL("https://github.com/MoyuScript/unity-2048");
        };

        Label versionLabel = uiDocument.rootVisualElement.Q<Label>("Version");
        versionLabel.text = $"版本号：{Application.version}";
    }

    private void OnScoreChanged(int score)
    {
        Label scoreLabel = uiDocument.rootVisualElement.Q<Label>("ScoreValue");
        scoreLabel.text = score.ToString();

        Label gameOverScoreLabel = uiDocument.rootVisualElement.Q<Label>("GameOverScore");
        gameOverScoreLabel.text = score.ToString();
    }

    private void ShowGameOver()
    {
        VisualElement gameOverBox = uiDocument.rootVisualElement.Q<VisualElement>("GameOverBox");
        gameOverBox.style.display = DisplayStyle.Flex;
        gameOverBox.style.opacity = 1;
    }
    private IEnumerator HideGameOver()
    {
        VisualElement gameOverBox = uiDocument.rootVisualElement.Q<VisualElement>("GameOverBox");
        gameOverBox.style.opacity = 0;
        yield return new WaitForSeconds(0.3f);
        gameOverBox.style.display = DisplayStyle.None;
    }
    private void OnGameOverChanged(bool gameOver)
    {
        if (gameOver)
        {
            ShowGameOver();
        }
        else
        {
            StartCoroutine(HideGameOver());
        }
    }
}
