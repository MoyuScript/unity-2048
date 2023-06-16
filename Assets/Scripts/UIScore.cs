using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class UIScore : MonoBehaviour
{
    void Awake()
    {
        GlobalState.instance.scoreChangedEvent.AddListener(OnScoreChanged);
        GlobalState.instance.gameOverChangedEvent.AddListener(OnGameOverChanged);

        OnScoreChanged(GlobalState.instance.score);
        OnGameOverChanged(GlobalState.instance.gameOver);
    }

    private void OnScoreChanged(int score)
    {
        Label scoreLabel = Ui.UiDocument.rootVisualElement.Q<Label>("ScoreValue");
        scoreLabel.text = score.ToString();

        Label gameOverScoreLabel = Ui.UiDocument.rootVisualElement.Q<Label>("GameOverScore");
        gameOverScoreLabel.text = score.ToString();
    }

    private void ShowGameOver()
    {
        VisualElement gameOverBox = Ui.UiDocument.rootVisualElement.Q<VisualElement>("GameOverBox");
        gameOverBox.style.display = DisplayStyle.Flex;
        gameOverBox.style.opacity = 1;
    }
    private IEnumerator HideGameOver()
    {
        VisualElement gameOverBox = Ui.UiDocument.rootVisualElement.Q<VisualElement>("GameOverBox");
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
