using System.Data.SqlTypes;
using SFML.Graphics;

namespace Pacman;

public sealed class Wall : Entity
{
    public Wall() : base ("pacman"){}

    public override bool Solid => true;

    public override void Create(Scene scene)
    {
        base.Create(scene);
        sprite.TextureRect = new IntRect(54, 54, 18, 18);
    }

    public override void Update(Scene scene, float dt) {}
}