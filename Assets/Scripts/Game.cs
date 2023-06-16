using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    public GameObject tilePrefab;
    private TileMapController _tileMap;

    private void NewGame()
    {
        _tileMap?.Destroy();
        _tileMap = new(tilePrefab);
        _tileMap.GenerateNewTile();
        _tileMap.GenerateNewTile();
        GlobalState.instance.score = 0;
        GlobalState.instance.gameOver = false;
        _tileMap.Print();
    }
    void Start()
    {
        NewGame();
        Ui.UiDocument.rootVisualElement.Q<Button>("ButtonNewGame").clicked += NewGame;
        Ui.UiDocument.rootVisualElement.Q<Button>("GameOverRestartButton").clicked += NewGame;
    }

    void Update()
    {
        if (!GlobalState.instance.gameOver)
        {
            bool moved = false;
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                moved = _tileMap.Move(Constants.Direction.Up);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                moved = _tileMap.Move(Constants.Direction.Down);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                moved = _tileMap.Move(Constants.Direction.Left);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                moved = _tileMap.Move(Constants.Direction.Right);
            }

            if (moved)
            {
                _tileMap.GenerateNewTile();
                _tileMap.Print();
                if (_tileMap.IsGameOver())
                {
                    GlobalState.instance.gameOver = true;
                }
            }
        }
    }

    void FixedUpdate()
    {
    }
}
