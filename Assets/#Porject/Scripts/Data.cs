using System;
using System.Collections;
using System.Collections.Generic;


public class Data {
    public static Action<int> ScoreUpdate;
    private static int _score = 0;
    public static int Score {
        get { return _score; }
        set {
            _score = value;
            ScoreUpdate?.Invoke(Score);
        }
    }

    public static Action<bool> difficultyUpdate;
    private static bool _isHard = false;
    public static bool IsHard {
        get { return _isHard; }
        set {
            _isHard = value;
            difficultyUpdate?.Invoke(IsHard);
        }
    }

    public static Action<bool> gameOverUpdate;
    private static bool _gameOver = true;
    public static bool GameOver {
        get { return _gameOver; }
        set {
            _gameOver = value;
            gameOverUpdate?.Invoke(GameOver);
        }
    }

    public static float FallSpeed { get; set; } = 10;
}
