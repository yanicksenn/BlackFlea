using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour {

    [SerializeField]
    private TMP_Text headerLabel;
    [SerializeField]
    private TMP_Text subtitleLabel;

    [SerializeField]
    private TMP_Text playLabel;

    [SerializeField]
    private Button playButton;

    [SerializeField]
    private SceneReference sceneToLoad;
    
    private void Awake() {
        UIUtils.SetAlphaColor(headerLabel, /* alpha= */ 0f);
        UIUtils.SetAlphaColor(subtitleLabel, /* alpha= */ 0f);
        UIUtils.SetAlphaColor(playLabel, /* alpha= */ 0f);
    }

    private void OnEnable() {
        playButton.onClick.AddListener(Play);
    }

    private void OnDisable() {
        playButton.onClick.RemoveListener(Play);
    }

    private void Start() {
        Appear();
    }

    private void Play()
    {
        Disappear(LoadGameScene);
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(sceneToLoad.ScenePath);
    }

    private void Appear(Action completion = null) {
        StartCoroutine(UIUtils.FadeInOverSeconds(headerLabel, /* duration= */ 0.5f, () => {
            StartCoroutine(UIUtils.FadeInOverSeconds(subtitleLabel, /* duration= */ 0.5f, () => {
                StartCoroutine(UIUtils.FadeInOverSeconds(playLabel, /* duration= */ 0.5f, completion));
            }));
        }));
    }

    private void Disappear(Action completion = null) {
        StartCoroutine(UIUtils.FadeOutOverSeconds(headerLabel, /* duration= */ 0.5f));
        StartCoroutine(UIUtils.FadeOutOverSeconds(subtitleLabel, /* duration= */ 0.5f));
        StartCoroutine(UIUtils.FadeOutOverSeconds(playLabel, /* duration= */ 0.5f, completion));
    }
}