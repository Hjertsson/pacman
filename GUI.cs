using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Pacman;

public sealed class GUI : Entity
{
    private readonly Text scoreText;
    private readonly Text highScoreText;
    private int maxHealth;
    private int currentHealth;
    private int currentScore;

    private int savedHealth = 4;
    
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
        currentHealth = savedHealth;
        base.Create(scene);

        scene.Events.LoseHealth += OnLoseHealth; // Prenumererar på Eventen
        scene.Events.GainScore += OnScoreGain;
    }

    private void OnScoreGain(Scene scene, int amount)
    {
        currentScore += amount;
        if (!scene.FindByType<Coin>(out _)) // Om det inte finns fler Coin objekt, återställ alla objekt som inte har DontDestroyOnLoad = true;
        {
            savedHealth = currentHealth;
            DontDestroyOnLoad = true;
            scene.Loader.Reload();
            scene.Events.GainScore -= OnScoreGain;
        }
    }
    private void OnLoseHealth(Scene scene, int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            DontDestroyOnLoad = false;
            string score = HighScore(); // HighScore() returnerar värdet i HighScore filen
            Program.paused = true; // Flaggar paused vilket pausar alla objekt tills paused är satt till false
            ShowHighScore(score); // Skriver ut score värdet på skärmen
            currentScore = 0;
            savedHealth = maxHealth;
            scene.Loader.Reload();
        }
        
    }
    private string HighScore()
    {
        string fileMain = "../../../assets/HighScore.txt"; // Läser in Highscore i Assets
        string fileBin = "assets/HighScore.txt"; // Läser in Highscore i bin/debug/net10
        string score = $"{currentScore}";
        string currentHighScore = File.ReadAllText(fileBin); //Läser av nuvarande värdet i filen "fileBin"
        
        if (int.Parse(currentHighScore) < currentScore) // Konverterar till int och jämför med poängen i nuvarande omgång
        {
            File.WriteAllText(fileBin, score, Encoding.UTF8); // Om det är högre så skriver vi över med det nya värdet
        }
       
        string newHighScore = File.ReadAllText(fileBin); //Värdet i fileBin läses av och sparas som en string NewHighScore
        File.WriteAllText(fileMain, newHighScore, Encoding.UTF8); //Skriver newHighScore i Assets filen för att spara mellan omstart av spelet
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