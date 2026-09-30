using SFML.Graphics;

namespace Pacman;

public class Candy : Entity
{
    public Candy() : base("pacman")
    {
        sprite.TextureRect = new IntRect(54, 36, 18, 18);
    }   
}