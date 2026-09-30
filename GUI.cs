using SFML.Graphics;
using SFML.System;

namespace Pacman;

public class GUI : Entity
{
    private Text scoreText;
    private readonly int maxHealth;
    private int currentHealth;
    private int currentScore;
    
    public GUI() : base("pacman")
    {
        scoreText = new Text();
        sprite.TextureRect = new IntRect(72, 36, 18, 18);
        maxHealth = 4;
    }

    public override void Create(Scene scene)
    {
        scoreText.Font = scene.Assets.LoadFont("pixel-font");
        scoreText.CharacterSize = 200;
        scoreText.Scale = new Vector2f(0.1f, 0.1f);
        scoreText.FillColor = Color.Black;
        scoreText.DisplayedString = "Score";
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
            currentScore = 0;
            scene.Loader.Reload();
        }
        
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
    }
}