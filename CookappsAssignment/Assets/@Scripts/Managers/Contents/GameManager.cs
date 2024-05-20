using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using UnityEngine;
using static Define;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class GameManager
{
    public int StageLevel { get; set; } = 1;
    public int EnemyKillCount { get; set; } = 0;
    public bool IsGameOver { get; set; } = false;

    public void CheckGameOver()
    {
        bool gameOver = true;
        foreach (var hero in Managers.Object.Heroes)
        {
            if (hero.IsValid())
            {
                gameOver = false;
                break;
            }
        }

        if (gameOver)
        {
            IsGameOver = true;
            Managers.UI.ShowToast("Stage Failed...");
        }
    }
}