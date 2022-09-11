using Godot;
using System;
using System.Collections.Generic;

public class Scene : Spatial
{
    private List<Node2D> birds = new List<Node2D>();
    private List<double> directions = new List<double>();
    private List<double> nudges = new List<double>();
    private readonly Random rand = new Random();

    public override void _Ready()
    {
        for (int i = 0; i < 1; i++)
        {
            var bird = (Node2D)GD.Load<PackedScene>("res://Bird.tscn").Instance();
            AddChild(bird);
            birds.Add(bird);
            directions.Add(0);
            nudges.Add(0);
        }
    }

    public override void _Process(float delta)
    {
        delta *= 100 * 5;
        for (int i = 0; i < birds.Count; i++)
        {
            var screenSize = GetViewport().Size;
            nudges[i] += (rand.NextDouble() > 0.5 ? 1 : -1) * rand.NextDouble() * 1.5;
            if (nudges[i] > 1)
            {
                nudges[i] = 1;
            }
            if (nudges[i] < -1)
            {
                nudges[i] = -1;
            }

            var update = true;
            directions[i] += (nudges[i] * (((1 - Math.Abs(nudges[i])))));
            directions[i] = directions[i] % (Math.PI * 2);
            if (birds[i].Position.x - 1 < 1 || birds[i].Position.x + 1 > screenSize.x - 1)
            {
                directions[i] = directions[i] < Math.PI ? Math.PI - directions[i] : Math.PI * 3 - directions[i];
                update = false;
            }
            if (birds[i].Position.y - 1 < 1 || birds[i].Position.y + 1 > screenSize.y - 1)
            {
                directions[i] = Math.PI * 2 - directions[i];
                update = false;
            }


            directions[i] = directions[i] % (Math.PI * 2);
            var y = (float)Math.Sin(directions[i]);
            var x = (float)Math.Cos(directions[i]);
            if (!update)
            {
                // y *= 0;
                // x *= 0;
            }

            birds[i].Translate(new Vector2(x, y) * delta);

            var rotation = birds[i].Rotation;
            if (rotation < directions[i] - 0.1 || rotation > directions[i] + 0.1)
            {
                birds[i].Rotate(delta * (rotation > directions[i] ? -.01f : 0.01f));
                // ((Node2D)birds[i].GetChild(0)).Rotation = (delta * (rotation > directions[i] ? -0.26f : 0.26f));
                // ((Node2D)birds[i].GetChild(1)).Rotation = (delta * (rotation > directions[i] ? -0.26f : 0.26f));
            }
        }
    }
}
