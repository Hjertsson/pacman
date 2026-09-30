using SFML.Graphics;

namespace Pacman;

public delegate void ValueChangedEvent(Scene scene, int value);

public sealed class Scene
{
    private List<Entity> entities;
    public readonly SceneLoader Loader;
    public readonly AssetManager Assets;
    private int scoreGained;
    private int healthLost;

    public event ValueChangedEvent GainScore;
    public event ValueChangedEvent LoseHealth;

    public Scene()
    {
        entities = new List<Entity>();
        Loader = new SceneLoader();
        Assets = new AssetManager();
    }
    public void PublishGainScore(int amount) => scoreGained += amount;
    public void PublishLoseHealth(int amount) => healthLost += amount;

    public void Spawn(Entity entity)
    {
        entities.Add(entity);
        entity.Create(this);
    }

    public void Clear()
    {
        for (int i = entities.Count - 1; i >= 0; i--)
        {
            Entity entity = entities[i];
            entities.RemoveAt(i);
            entity.Destroy(this);
        }
    }
    public bool FindByType<T>(out T found) where T : Entity
    {
        foreach (var entity in entities)
        {
            if (!entity.Dead && entity is T typed)
            {
                found = typed;
                return true;
            }
        }

        found = default(T);
        return false;
    }

    public IEnumerable<Entity> FindIntersects(FloatRect bounds)
    {
        int lastEntity = entities.Count - 1;

        for (int i = lastEntity; i >= 0; i--)
        {
            Entity entity = entities[i];
            if(entity.Dead) continue;
            if (entity.Bounds.Intersects(bounds))
            {
                yield return entity;
            }
        }
    }

    public void UpdateAll(float dt)
    {
        Loader.HandleSceneLoad(this);
        for (int i = entities.Count - 1; i >= 0; i--)
        {
            Entity entity = entities[i];
            entity.Update(this, dt);
        }

        for (int i = 0; i < entities.Count;)
        {
            Entity entity = entities[i];
            if (entity.Dead) entities.RemoveAt(i);
            else i++;
        }

        if (scoreGained != 0)
        {
            GainScore?.Invoke(this, scoreGained);
            scoreGained = 0;
        }
        if (healthLost != 0)
        {
            LoseHealth?.Invoke(this, healthLost);
            healthLost = 0;
        }
    }
    
    public void RenderAll(RenderTarget target)
    {
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].Render(target);
        }
    }


}