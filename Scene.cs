using Godot;
using System;
using System.Collections.Generic;

public partial class Scene : Node3D
{
	private List<Node2D> birds = new List<Node2D>();
	private List<double> directions = new List<double>();
	private List<double> nudges = new List<double>();
	private readonly Random rand = new Random();

	public override void _Ready()
	{
		for (int i = 0; i < 10; i++)
		{
			var bird = (Node2D)GD.Load<PackedScene>("res://Bird.tscn").Instantiate();
			AddChild(bird);
			birds.Add(bird);
			directions.Add(0);
			nudges.Add(0);
		}
	}

	public override void _Process(double delta)
	{
		// return;
		delta *= 100 * 5;
		var last = 0.0;
		for (int i = 0; i < birds.Count; i++)
		{
			var screenSize = GetViewport().GetVisibleRect().Size;
			var limit = (nudges[(nudges.Count + i - 1) / nudges.Count] + 2) / 4;
			nudges[i] += (rand.NextDouble() < limit ? 1 : -1) * rand.NextDouble() * 1.5;
			if (nudges[i] > 1)
			{
				nudges[i] = 1;
			}
			if (nudges[i] < -1)
			{
				nudges[i] = -1;
			}

			var update = true;
			directions[i] += nudges[i] * Math.Abs(nudges[i]) / 20;
			directions[i] = directions[i] % (Math.PI * 2);
			var y = (float)Math.Sin(directions[i]);
			var x = (float)Math.Cos(directions[i]);
			if (birds[i].Position.X + x < 0 || birds[i].Position.X + x > screenSize.X)
			{
				directions[i] = directions[i] < Math.PI ? Math.PI - directions[i] : Math.PI * 3 - directions[i];
				birds[i].Position = new Vector2(screenSize.X - birds[i].Position.X, birds[i].Position.Y);
				update = false;
			}
			if (birds[i].Position.Y + y < 0 || birds[i].Position.Y + y > screenSize.Y)
			{
				directions[i] = Math.PI * 2 - directions[i];
				birds[i].Position = new Vector2(birds[i].Position.X, screenSize.Y - birds[i].Position.Y);
				update = false;
			}


			directions[i] = directions[i] % (Math.PI * 2);

			if (!update)
			{
				y *= 0;
				x *= 0;
			}

			birds[i].Translate(new Vector2(x, y) * (float)delta);

			var rotation = birds[i].Rotation;
			if (rotation < directions[i] - 0.1 || rotation > directions[i] + 0.1)
			{
				birds[i].Rotate((float)delta * (rotation > directions[i] ? -.01f : 0.01f));
				((Node2D)birds[i].GetChild(0)).Rotation = ((float)delta * (rotation > directions[i] ? -0.26f : 0.26f));
				((Node2D)birds[i].GetChild(1)).Rotation = ((float)delta * (rotation > directions[i] ? -0.26f : 0.26f));
			}
		}
	}
}
