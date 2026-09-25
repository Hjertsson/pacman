using System.Runtime.Loader;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace Pacman;

public class SceneLoader
{
    private readonly Dictionary<char, Func<Entity>> loaders;
    private string currentScene = "", nextScene = "";

    public SceneLoader()
    {
        loaders = new Dictionary<char, Func<Entity>>
        {
            { '#', () => new Wall() }
        };
    }

    public void HandleSceneLoad(Scene scene)
    {
        if (nextScene == "") return;
        scene.Clear();
        string file = "assets/maze.txt";
        int row = 0;
        int col = 0;
        foreach (string line in File.ReadLines(file, Encoding.UTF8))
        {
            foreach (char c in line)
            {
                switch (c)
                {
                    case '#':
                        Wall wall = new Wall();
                        wall.Position = new Vector2f(col * 18, row * 18);
                        scene.Spawn(wall);
                        break;
                }

                col++;
            }

            col = 0;
            row++;
        }

        currentScene = nextScene;
        nextScene = "";
    }

    private bool Create(char symbol, out Entity created)
    {
        if (loaders.TryGetValue(symbol, out Func<Entity> loader))
        {
            created = loader();
            return true;
        }

        created = null;
        return false;
    }

    public void Load(string scene) => nextScene = scene;
    public void Reload() => nextScene = currentScene;
}