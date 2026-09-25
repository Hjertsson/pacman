using SFML.Window;
using SFML.System;
using SFML.Graphics;
namespace Pacman;

class Program
{
    public const int SCREEN_WIDTH = 800;
    public const int SCREEN_HEIGHT = 600;
    
    static void Main(string[] args)
    {

        using (RenderWindow window = new RenderWindow(
                   new VideoMode(SCREEN_WIDTH, SCREEN_HEIGHT), "Pacman"))
        {  
            window.Closed += (o, e) => window.Close();
            window.SetView(new View(
                new Vector2f(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2),
                new Vector2f(SCREEN_WIDTH, SCREEN_HEIGHT)));

            Clock clock = new Clock();
            while (window.IsOpen)
            {
                float dt = clock.Restart().AsSeconds();
                window.DispatchEvents();
                //TODO UPDATES

                window.Clear();
                // TODO DRAWING

                window.Display();
            }
        }
    }
}