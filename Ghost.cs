using SFML.Graphics;
using SFML.System;


namespace Pacman;

public sealed class Ghost: Actor
{
    //private bool collided = false;
    //private float collisionTimer;

    private bool frozen = false;
    
    private float animationTimer;
    private float frozenTimer;
    private int bonusPoints = 200;
    private IntRect redGhostBase = new IntRect(36, 0, 18, 18);
    private IntRect redGhostMove = new IntRect(54, 0, 18, 18);
    private IntRect blueGhostBase = new IntRect(36, 18, 18, 18);
    private IntRect blueGhostMove = new IntRect(54, 18, 18, 18);
    public override void Create(Scene scene)
    {
        direction = -1;
        speed = 100.0f;
        moving = true;
        base.Create(scene);
        sprite.TextureRect = redGhostBase;
        scene.Events.CandyEaten += OnCandyEaten;
    }

    private void OnCandyEaten(Scene scene, int amount)
    {
        frozenTimer = 5.0f;
        frozen = true;
    }
    protected override void CollideWith(Scene scene, Entity e)
    {
        if (!collided)
        {
            if (e is Pacman)
            {
                if (frozenTimer <= 0.0f)
                {
                    if (!e.immortal)
                    {
                        scene.Events.PublishLoseHealth(1);
                        e.immortal = true;
                    }
                }
                if (frozenTimer > 0.0f)
                {
                    collided = true;
                    collisionTimer = 0;
                    scene.Events.PublishGainScore(1000 + bonusPoints);
                    bonusPoints *= 2;
                    Reset();
                }
                
            }
        }
    }

    private void Animation()
    {
        switch (animationTimer)
        {
            case < 0.2f:
                if (frozen)
                {
                    sprite.TextureRect = blueGhostBase;
                }
                else
                {
                    sprite.TextureRect = redGhostBase;
                }

                break;
            case > 0.2f and < 0.4f:
                if (frozen)
                {
                    sprite.TextureRect = blueGhostMove;
                }
                else
                {
                    sprite.TextureRect = redGhostMove;
                }
                break;
            case > 0.4f:
                animationTimer = 0;
                break;
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

    public override void Update(Scene scene, float dt)
    {
        base.Update(scene, dt);
        animationTimer += dt;
        frozenTimer = MathF.Max(frozenTimer - dt, 0.0f);
        if (frozenTimer <= 0)
        {
            frozen = false;
        }
        Animation();
    }
    
    public override FloatRect Bounds
    {
        get
        {
            var bounds = base.Bounds;
            bounds.Left += 1;
            bounds.Width -= 1;
            bounds.Top += 1;
            bounds.Height -= 1;
            return bounds;
        } 
        
    }
}