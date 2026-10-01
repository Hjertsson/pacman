using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Pacman;

public class GUI : Entity
{
    private Text scoreText;
    private Text highScoreText;
    private int maxHealth;
    private int currentHealth;
    private int currentScore;
    private int highScore;
    
    public GUI() : base("pacman")
    {
        scoreText = new Text();
        highScoreText = new Text();
        sprite.TextureRect = new IntRect(72, 36, 18, 18);
        maxHealth = 4;
    }

    public override void Create(Scene scene)
    {
        scoreText.Font = scene.Assets.LoadFont("pixel-font");
        scoreText.CharacterSize = 150;
        scoreText.DisplayedString = "Score";
        scoreText.Scale = new Vector2f(0.1f, 0.1f);
        scoreText.FillColor = Color.Black;
        highScoreText.Font = scene.Assets.LoadFont("pixel-font");
        highScoreText.CharacterSize = 150;
        highScoreText.DisplayedString = "Score";
        highScoreText.Scale = new Vector2f(0.1f, 0.1f);
        highScoreText.FillColor = Color.Red;
        currentHealth = maxHealth;
        base.Create(scene);

        scene.Events.LoseHealth += OnLoseHealth;
        scene.Events.GainScore += OnScoreGain;
    }

    private void OnScoreGain(Scene scene, int amount)
    {
        currentScore += amount;
        if (!scene.FindByType<Coin>(out _))
        {
            DontDestroyOnLoad = true;
            scene.Loader.Reload();
        }
    }
    private void OnLoseHealth(Scene scene, int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            DontDestroyOnLoad = false;
            string score = HighScore();
            Program.paused = true;
            ShowHighScore(score);
            currentScore = 0;
            scene.Loader.Reload();
        }
        
    }
    private string HighScore()
    {
        string fileMain = "../../../assets/HighScore.txt";
        string fileBin = "assets/HighScore.txt";
        string score = $"{currentScore}";
        string currentHighScore = File.ReadAllText(fileBin);
        
        if (File.Exists(fileBin))
        {
            if (int.Parse(currentHighScore) < currentScore)
            {
                File.WriteAllText(fileBin, score, Encoding.UTF8);
            }
        }
        string newHighScore = File.ReadAllText(fileBin);
        File.WriteAllText(fileMain, newHighScore, Encoding.UTF8);
        return newHighScore;
    }

    private void ShowHighScore(string score)
    {
        highScoreText.DisplayedString = $"HIGH SCORE: {score}\nPress Enter";
        highScoreText.Position = new Vector2f(300 - highScoreText.GetGlobalBounds().Width, 396);
    }
    public override void Destroy(Scene scene)
    {
        base.Destroy(scene);
        scene.Events.LoseHealth -= OnLoseHealth;
        scene.Events.GainScore -= OnScoreGain;
    }

    public override void Render(RenderTarget target)
    {
        sprite.Position = new Vector2f(36, 396);
        for (int i = 0; i < maxHealth; i++)
        {
            sprite.TextureRect = i < currentHealth
                ? new IntRect(72, 36, 18, 18)
                : new IntRect(72, 0, 18, 18);

            base.Render(target);
            sprite.Position += new Vector2f(18, 0);
        }
        scoreText.DisplayedString = $"Score: {currentScore}";
        scoreText.Position = new Vector2f(414 - scoreText.GetGlobalBounds().Width, 396);
        target.Draw(scoreText);
        if (Program.paused)
        {
            target.Draw(highScoreText);
        }
    }
}