using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static SFML.Window.Keyboard.Key;

namespace Pacman;

public sealed class Pacman : Actor
{
    public override void Create(Scene scene)
    {
        speed = 100.0f;
        base.Create(scene);
        sprite.TextureRect = new IntRect(0, 0, 18, 18);
        

        scene.LoseHealth += OnLoseHealth;
    }
    
    private void OnLoseHealth(Scene scene, int amount)
    {
        Reset();
    }

    public override void Destroy(Scene scene)
    {
        base.Destroy(scene);
        scene.LoseHealth -= OnLoseHealth;
    }
    
    protected override int PickDirection(Scene scene)
    {
        int dir = direction;
        if (Keyboard.IsKeyPressed(Right))
        {
            dir = 0;
            moving = true;
        }
        else if (Keyboard.IsKeyPressed(Up))
        {
            dir = 1;
            moving = true;
        }
        else if (Keyboard.IsKeyPressed(Left))
        {
            dir = 2;
            moving = true;
        }
        else if (Keyboard.IsKeyPressed(Down))
        {
            dir = 3;
            moving = true;
        }
        else
        {
            moving = false;
        }

        if (IsFree(scene, dir)) return dir;
        if (!IsFree(scene, direction)) moving = false;
        return direction;
    }
}