using SFML.System;
using System.Linq;
using SFML.Graphics;

namespace Pacman;

public class Actor : Entity
{
    private bool wasAligned;
    protected float speed;
    protected int direction;
    protected bool moving;
    protected Vector2f originalPosition;
    protected float originalSpeed;

    protected Actor() : base ("pacman")
    {
        
    }

    protected void Reset()
    {
        wasAligned = false;
        Position = originalPosition;
        speed = originalSpeed;
    }

    public override void Create(Scene scene)
    {
        base.Create(scene);
        Reset();
    }

    protected bool IsAligned =>
        (int)MathF.Floor(Position.X) % 18 == 0 &&
        (int)MathF.Floor(Position.Y) % 18 == 0;

    protected bool IsFree(Scene scene, int dir) //TODO: Steg 23
    {
        Vector2f at = Position + new Vector2f(9, 9);
        at += 18 * ToVector(dir);
        FloatRect rect = new FloatRect(at.X, at.Y, 1, 1);
        return !scene.FindByType(rect).Any(e => e.Solid);
    }
}