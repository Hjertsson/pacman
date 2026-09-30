namespace Pacman;
public delegate void ValueChangedEvent(Scene scene, int value);
public class EventManager
{
    private int scoreGained;
    private int healthLost;
    private int candyStatus;
    public event ValueChangedEvent GainScore;
    public event ValueChangedEvent LoseHealth;
    public event ValueChangedEvent CandyEaten;
    
    public void PublishGainScore(int amount) => scoreGained += amount;
    public void PublishLoseHealth(int amount) => healthLost += amount;
    public void PublishCandyEaten(int amount) => candyStatus += amount;

    public void CheckEvent(Scene scene)
    {
        if (scoreGained != 0)
        {
            GainScore?.Invoke(scene, scoreGained);
            scoreGained = 0;
        }
        if (healthLost != 0)
        {
            LoseHealth?.Invoke(scene, healthLost);
            healthLost = 0;
        }
        if (candyStatus != 0)
        {
            CandyEaten?.Invoke(scene, candyStatus);
            candyStatus = 0;
        }
    }
}