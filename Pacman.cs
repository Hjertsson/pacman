using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static SFML.Window.Keyboard.Key;

namespace Pacman;

public sealed class Pacman : Actor
{
    private int dir;
    private int animationFrame;
    private float animationTimer;
    
    public override void Create(Scene scene)
    {
        speed = 100.0f;
        base.Create(scene);
        sprite.TextureRect = new IntRect(0, 0, 18, 18);
        scene.Events.LoseHealth += OnLoseHealth;
    }
    
    private void OnLoseHealth(Scene scene, int amount)
    {
        Reset();
    }

    public override void Destroy(Scene scene)
    {
        base.Destroy(scene);
        scene.Events.LoseHealth -= OnLoseHealth;
    }

    public override void Update(Scene scene, float dt)
    {
        base.Update(scene, dt);
        animationTimer += dt;
    }

    private void Animation(int dir)
    { 
        if (moving)
        {
            switch (animationTimer)
            {
                case < 0.2f:
                    sprite.TextureRect = new IntRect(18, dir * 18, 18, 18);
                    break;
                case < 0.4f and > 0.2f:
                    sprite.TextureRect = new IntRect(0, dir * 18, 18, 18);
                    break;
                case > 0.4f:
                    animationTimer = 0;
                    break;
            }
        }
        else
        {
            sprite.TextureRect = new IntRect(0, dir * 18, 18, 18);
        }

    }
    protected override int PickDirection(Scene scene)
    {
        dir = direction;
        if (Keyboard.IsKeyPressed(Right))
        {
            dir = 0;
        }
        else if (Keyboard.IsKeyPressed(Up))
        {
            dir = 1;
        }
        else if (Keyboard.IsKeyPressed(Left))
        {
            dir = 2;
        }
        else if (Keyboard.IsKeyPressed(Down))
        {
            dir = 3;
        }

        if (Keyboard.IsKeyPressed(Right) ||
            Keyboard.IsKeyPressed(Up) ||
            Keyboard.IsKeyPressed(Left) ||
            Keyboard.IsKeyPressed(Down))
        {
            Animation(dir);
            moving = true;
        }
        else
        {
            moving = false;
        }


        if (IsFree(scene, dir)) { return dir; }

        if (!IsFree(scene, direction))
        { moving = false; }
        return direction;
    }
}