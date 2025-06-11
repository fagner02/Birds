using System;
using System.Collections.Generic;
using Godot;

namespace Birds;

public partial class Scene : Node3D
{
	private readonly List<Node2D> birds = [];
	private readonly List<double> directions = [];
	private readonly List<double> nudges = [];
	private readonly Random rand = new();
	private readonly double speed = 1;

	public override void _Ready()
	{

		Vector2 screenSize = GetViewport().GetVisibleRect().Size;
		PackedScene scene = GD.Load<PackedScene>("res://Bird.tscn");
		for (int i = 0; i < 100; i++)
		{
			Node2D bird = (Node2D)scene.Instantiate();
			AddChild(bird);
			birds.Add(bird);
			bird.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2);
			directions.Add(rand.NextDouble() * Math.PI * 2.0);
			nudges.Add(rand.NextDouble() * Math.PI * 2.0);
		}
	}

	public override void _Process(double delta)
	{
		Vector2 mouse = GetViewport().GetMousePosition();
		Vector2 screenSize = GetViewport().GetVisibleRect().Size;
		for (int i = 0; i < birds.Count; i++)
		{
			Vector2 pos = birds[i].GlobalPosition;
			double angle = (pos - mouse).Angle();
			//double angle = rand.NextDouble() * Math.PI * 2;
			double diff = Math.Abs(angle - directions[i]);
			double diff2 = (Math.PI * 2) - diff;

			double nudge;
			if (diff2 < diff)
				nudge = -diff2;
			else
				nudge = diff;

			if (angle > directions[i])
				nudge *= -1;

			nudges[i] += nudge * 5 * delta * speed;
			if (nudges[i] > 1)
				nudges[i] = 1;
			if (nudges[i] < -1)
				nudges[i] = -1;


			directions[i] += nudges[i] * delta * 10 * speed;
			if (Math.Abs(directions[i]) > Math.PI * 2)
				directions[i] = directions[i] * -1 % Math.PI * 2;

			float y = (float)Math.Sin(directions[i]);
			float x = (float)Math.Cos(directions[i]);

			if (birds[i].Position.X + x < 0 || birds[i].Position.X + x > screenSize.X)
			{
				birds[i].Position = new Vector2(
					screenSize.X - Math.Clamp(birds[i].Position.X, 0, screenSize.X),
					birds[i].Position.Y
				);
			}
			if (birds[i].Position.Y + y < 0 || birds[i].Position.Y + y > screenSize.Y)
			{
				birds[i].Position = new Vector2(
					birds[i].Position.X,
					screenSize.Y - Math.Clamp(birds[i].Position.Y, 0, screenSize.Y)
				);
			}

			birds[i].Translate(new Vector2(x, y) * (float)(delta * 500.0 * speed));

			birds[i].Rotation = (float)directions[i];
			Node2D child = (Node2D)birds[i].GetChild(0);
			child.Rotation = (float)(0.35 * -nudges[i]);
			child = (Node2D)child.GetChild(0);
			child.Rotation = (float)(0.35 * -nudges[i]);
		}
	}
}
