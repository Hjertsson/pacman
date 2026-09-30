using SFML.Graphics;
using SFML.System;

namespace Pacman;

public sealed class Ghost: Actor
{
    public override void Create(Scene scene)
    {
        direction = -1;
        speed = 100.0f;
        moving = true;
        base.Create(scene);
        sprite.TextureRect = new IntRect(36, 0, 18, 18);
    }

    protected override void CollideWith(Scene scene, Entity e)
    {
        if (e is Pacman)
        {
            scene.PublishLoseHealth(1);
            Reset();
        }
    }

    protected override int PickDirection(Scene scene)
    {
        List<int> validMoves = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            if ((i+2) % 4 == direction) continue;
            if (IsFree(scene, i)) validMoves.Add(i);
        }
        
        int r = new Random().Next(0, validMoves.Count);
        return validMoves[r];
    }
}