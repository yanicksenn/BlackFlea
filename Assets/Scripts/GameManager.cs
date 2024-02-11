using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Optional<GameManager> Instance { get; private set; } = Optional<GameManager>.OfEmpty();

    private bool isRunning;
    private int currentScore;

    [SerializeField]
    private TMP_Text scoreLabel;

    [SerializeField]
    private float leftBound;
    public float LeftBound => leftBound;

    [SerializeField]
    private float rightBound;
    public float RightBound => rightBound;

    [SerializeField]
    private HiveMind hiveMind;

    [SerializeField]
    private SceneReference gameOverScene;

    [SerializeField]
    private float gameOverDelay;

    [SerializeField, Space]
    private UnityEvent onGameStartEvent = new();
    public UnityEvent OnGameStartEvent => onGameStartEvent;

    [SerializeField]
    private UnityEvent onScoreChangeEvent = new();
    public UnityEvent OnScoreChangeEvent => onScoreChangeEvent;

    [SerializeField]
    private UnityEvent onGameOverEvent = new();
    public UnityEvent OnGameOverEvent => onGameOverEvent;

    private void Awake()
    {
        if (Instance.IsPresent && Instance.Get() != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = Optional<GameManager>.Of(this);
        }
    }

    private void Start()
    {
        isRunning = true;
        SetScore(0);
        OnGameStartEvent.Invoke();
    }

    private void OnDestroy() {
        if (Instance.Get() == this) {
            Instance = Optional<GameManager>.OfEmpty();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 1f);
        var left = new Vector3(LeftBound, 0, 0);
        var right = new Vector3(RightBound, 0, 0);
        Gizmos.DrawLine(left + Vector3.up * 3, left + Vector3.down * 3);
        Gizmos.DrawLine(right + Vector3.up * 3, right + Vector3.down * 3);
    }

    public void SetScore(int newScore)
    {
        currentScore = newScore;
        scoreLabel.text = currentScore.ToString();
        OnScoreChangeEvent.Invoke();
    }

    public void AddScore(int newScore)
    {
        SetScore(currentScore + newScore);
    }

    public void GameOver()
    {
        if (!isRunning) return;

        isRunning = false;
        hiveMind.SetSiege();
        ScoreTable.Instance.Submit(currentScore);
        OnGameOverEvent.Invoke();

        StartCoroutine(UnityUtils.DoAfterSeconds(gameOverDelay, () =>
        {
            SceneManager.LoadScene(gameOverScene.ScenePath);
        }));
    }
}