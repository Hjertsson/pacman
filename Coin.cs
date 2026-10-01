using SFML.Graphics;

namespace Pacman;

public sealed class Coin : Entity
{
    public Coin() : base("pacman")
    {
        sprite.TextureRect = new IntRect(36, 36, 18, 18);
    }

    protected override void CollideWith(Scene scene, Entity e)
    {
        if (e is Pacman)
        {
            scene.Events.PublishGainScore(100);
            Dead = true;
        }
    }
    
    
    public override FloatRect Bounds
    {
        get
        {
            var bounds = base.Bounds;
            bounds.Left += 3;
            bounds.Width -= 3;
            bounds.Top += 3;
            bounds.Height -= 3;
            return bounds;
        } 
        
    }
}