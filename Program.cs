using System.Text;
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
        Scene scene = new Scene();
        scene.Loader.Load("maze");
        using (RenderWindow window = new RenderWindow(
                   new VideoMode(SCREEN_WIDTH, SCREEN_HEIGHT), "Pacman"))
        {  
            window.Closed += (o, e) => window.Close();
            window.SetView(new View(new FloatRect(18,0,414,450)));

            Clock clock = new Clock();
            while (window.IsOpen)
            {
                float dt = clock.Restart().AsSeconds();
                if (dt > 0.1f) dt = 0.1f;
                window.DispatchEvents();
                //TODO UPDATES
                scene.UpdateAll(dt);
                window.Clear();
                // TODO DRAWING
                scene.RenderAll(window);
                window.Display();
            }
        }
    }
}