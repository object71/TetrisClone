using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    private int score = 0;
    private int lines = 0;
    private int level = 1;

    public int linesPerLevel = 5;
    public Text linesText;
    public Text levelText;
    public Text scoreText;

    internal LevelUpEvent levelUpEvent = new LevelUpEvent();

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnLinesCleared(int n)
    {
        switch (n)
        {
            case 1:
                score += 40 * level;
                break;
            case 2:
                score += 100 * level;
                break;
            case 3:
                score += 300 * level;
                break;
            case 4:
                score += 1200 * level;
                break;
        }

        lines -= n;

        if (lines <= 0)
        {
            LevelUp();
        }

        UpdateUIText();
    }

    public void Reset()
    {
        level = 1;
        lines = linesPerLevel * level;

        UpdateUIText();
    }

    private void UpdateUIText()
    {
        if (linesText)
        {
            linesText.text = lines.ToString();
        }

        if (levelText)
        {
            levelText.text = level.ToString();
        }

        if (scoreText)
        {
            scoreText.text = score.ToString().PadLeft(5, '0');
        }
    }

    private void LevelUp()
    {
        level++;
        lines += linesPerLevel * level;

        levelUpEvent.Invoke(level);
    }
}
