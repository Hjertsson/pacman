using SFML.Graphics;
using SFML.System;


namespace Pacman;

public sealed class Ghost: Actor
{
    private bool preyMode = false;
    private float animationTimer;
    private float preyModeTimer;
    private int bonusPoints = 200;
    private readonly IntRect redGhostBase = new IntRect(36, 0, 18, 18);
    private readonly IntRect redGhostMove = new IntRect(54, 0, 18, 18);
    private readonly IntRect blueGhostBase = new IntRect(36, 18, 18, 18);
    private readonly IntRect blueGhostMove = new IntRect(54, 18, 18, 18);
    public override void Create(Scene scene)
    {
        Direction = -1;
        Speed = 100.0f;
        Moving = true;
        base.Create(scene);
        sprite.TextureRect = redGhostBase;
        scene.Events.CandyEaten += OnCandyEaten;
    }

    private void OnCandyEaten(Scene scene, int amount)
    {
        preyModeTimer = 5.0f;
        preyMode = true;
    }
    protected override void CollideWith(Scene scene, Entity e)
    {
        if (!Collided) // För att inte kunna kollidera med samma spöke igen innan en ny "Prey mode" aktiveras igen
        {
            if (e is Pacman)
            {
                if (preyModeTimer <= 0.0f)
                {
                    if (!e.Immortal)
                    {
                        scene.Events.PublishLoseHealth(1);
                        e.Immortal = true;
                    }
                }
                if (preyModeTimer > 0.0f)
                {
                    Collided = true;
                    CollisionTimer = 0;
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
                if (preyMode)
                {
                    sprite.TextureRect = blueGhostBase;
                }
                else
                {
                    sprite.TextureRect = redGhostBase;
                }

                break;
            case > 0.2f and < 0.4f:
                if (preyMode)
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
            if ((i+2) % 4 == Direction) continue;
            if (IsFree(scene, i)) validMoves.Add(i);
        }
        
        int r = new Random().Next(0, validMoves.Count);
        return validMoves[r];
    }

    public override void Update(Scene scene, float dt)
    {
        base.Update(scene, dt);
        animationTimer += dt;
        preyModeTimer = MathF.Max(preyModeTimer - dt, 0.0f);
        if (preyModeTimer <= 0)
        {
            preyMode = false;
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