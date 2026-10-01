using SFML.Graphics;

namespace Pacman;

public sealed class Candy : Entity
{
    public Candy() : base("pacman")
    {
        sprite.TextureRect = new IntRect(54, 36, 18, 18);
    }

    protected override void CollideWith(Scene scene, Entity e)
    {
        if (e is Pacman)
        {
            scene.Events.PublishCandyEaten(1);
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