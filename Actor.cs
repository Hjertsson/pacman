using System.Data;
using SFML.System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using SFML.Graphics;

namespace Pacman;

public abstract class Actor : Entity
{
    protected bool Collided;
    protected float CollisionTimer; // Timer för kollision mellan två olika typer av Actor objekt
    
    private bool wasAligned;
    protected float Speed;
    protected int Direction;
    protected bool Moving;
    private Vector2f originalPosition;
    private float originalSpeed;
    
    protected Actor() : base ("pacman") {}

    protected void Reset()
    {
        wasAligned = false;
        Position = originalPosition;
        Speed = originalSpeed;
    }

    public override void Create(Scene scene)
    {
        base.Create(scene);
        originalPosition = Position;
        originalSpeed = Speed;
        Reset();
    }
    private void TimeOut()
    {
        if (Collided) 
        {
            Speed = 0;
            sprite.Color = new Color(255, 255, 255, 100);
            if (CollisionTimer >= 1f)
            {
                sprite.Color = new Color(255, 255, 255);
                Speed = originalSpeed;
                Collided = false;
                CollisionTimer = 0;
            }
        }
    }
    protected bool IsAligned =>
        (int)MathF.Floor(Position.X) % 18 == 0 &&
        (int)MathF.Floor(Position.Y) % 18 == 0;

    protected bool IsFree(Scene scene, int dir)
    {
        Vector2f at = Position + new Vector2f(9, 9);
        at += 18 * ToVector(dir);
        FloatRect rect = new FloatRect(at.X, at.Y, 1, 1);
        return !scene.FindIntersects(rect).Any(e => e.Solid);
    }

    protected static Vector2f ToVector(int dir)
    {
        switch (dir) // Konverterar int till en vektor, Kontrollerar vilket håll ett Actor objekt rör sig
        {
            case 0: return new Vector2f(1, 0);
            case 1: return new Vector2f(0, -1);
            case 2: return new Vector2f(-1, 0);
            case 3: return new Vector2f(0, 1);
            default: return new Vector2f(0, 0);
        }
    }

    protected virtual int PickDirection(Scene scene)
    {
        return 0;
    }

    public override void Update(Scene scene, float dt)
    {
        CollisionTimer += dt;
        base.Update(scene, dt);
        if (IsAligned)
        {
            if (!wasAligned)
            {
                if (!Immortal)
                {
                    Direction = PickDirection(scene);
                }
                else
                {
                    Direction = -1;
                }
            }
            if (Moving)
            {
                wasAligned = true;
            }
        }
        else
        {
            wasAligned = false;
        }   
        
        if(!Moving) return;
        Position += ToVector(Direction) * (Speed * dt);
        Position = MathF.Floor(Position.X)switch
        {
            < 0 => new Vector2f(432, Position.Y),
            > 432 => new Vector2f(0, Position.Y),
            _ => Position
        };
        TimeOut();
    }
   
}