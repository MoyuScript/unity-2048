using UnityEngine;
using UnityEngine.Events;
class GlobalState
{
    private const string SAVE_FILE_NAME = "save.json";
    private static GlobalState _instance;
    public static GlobalState instance {
        get
        {
            if (_instance == null)
            {
                _instance = new GlobalState();
            }
            return _instance;
        }
    }
    public GlobalState()
    {
        if (_instance != null)
        {
            Debug.LogError("GlobalState is a singleton!");
        }
        _instance = this;
    }
    public UnityEvent globalStateChangedEvent = new();
    public UnityEvent<int> scoreChangedEvent = new();
    private int _score = 0;
    public int score
    {
        get { return _score; }
        set
        {
            _score = value;
            scoreChangedEvent.Invoke(_score);
            globalStateChangedEvent.Invoke();
        }
    }

    public UnityEvent<bool> gameOverChangedEvent = new();
    private bool _gameOver = false;
    public bool gameOver
    {
        get { return _gameOver; }
        set
        {
            _gameOver = value;
            globalStateChangedEvent.Invoke();
            gameOverChangedEvent.Invoke(_gameOver);
            globalStateChangedEvent.Invoke();
        }
    }
}