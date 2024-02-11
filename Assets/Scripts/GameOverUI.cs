using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    [SerializeField]
    private TMP_Text headerLabel;

    [SerializeField]
    private TMP_Text scoreLabel;

    [SerializeField]
    private TMP_Text newHighScoreLabel;

    [SerializeField]
    private TMP_Text playAgainLabel;

    [SerializeField]
    private Button playAgainButton;

    [SerializeField]
    private SceneReference sceneToLoad;
    
    private void Awake() {
        UIUtils.SetAlphaColor(headerLabel, /* alpha= */ 0f);
        UIUtils.SetAlphaColor(scoreLabel, /* alpha= */ 0f);
        UIUtils.SetAlphaColor(newHighScoreLabel, /* alpha= */ 0f);
        UIUtils.SetAlphaColor(playAgainLabel, /* alpha= */ 0f);
    }

    private void OnEnable() {
        playAgainButton.onClick.AddListener(PlayAgain);
    }

    private void OnDisable() {
        playAgainButton.onClick.RemoveListener(PlayAgain);
    }

    private void Start() {
        var scoreTable = ScoreTable.Instance;
        scoreLabel.text = scoreTable.LastScore.ToString();
        newHighScoreLabel.gameObject.SetActive(scoreTable.LastScore == scoreTable.HighScore);
        Appear();
    }

    private void PlayAgain()
    {
        Disappear(LoadGameScene);
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(sceneToLoad.ScenePath);
    }

    private void Appear(Action completion = null) {
        StartCoroutine(UIUtils.FadeInOverSeconds(headerLabel, /* duration= */ 0.5f, () => {
            StartCoroutine(UIUtils.FadeInOverSeconds(scoreLabel, /* duration= */ 0.5f));
            StartCoroutine(UIUtils.FadeInOverSeconds(newHighScoreLabel, /* duration= */ 0.5f, () => {
                StartCoroutine(UIUtils.FadeInOverSeconds(playAgainLabel, /* duration= */ 0.5f, completion));
            }));
        }));
    }

    private void Disappear(Action completion = null) {
        StartCoroutine(UIUtils.FadeOutOverSeconds(headerLabel, /* duration= */ 0.5f));
        StartCoroutine(UIUtils.FadeOutOverSeconds(scoreLabel, /* duration= */ 0.5f));
        StartCoroutine(UIUtils.FadeOutOverSeconds(newHighScoreLabel, /* duration= */ 0.5f));
        StartCoroutine(UIUtils.FadeOutOverSeconds(playAgainLabel, /* duration= */ 0.5f, completion));
    }
}