using Godot;
using System;

public partial class Bird : Polygon2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private Vector2 velocity = new Vector2(0, 0);
	private Random rand = new Random();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		return;
		var screenSize = GetViewportRect().Size;
		var x = (rand.NextDouble() > 0.5 ? -1 : 1) * (float)rand.NextDouble() * 5;
		var y = (rand.NextDouble() > 0.5 ? -1 : 1) * (float)rand.NextDouble() * 5;

		velocity.X += x;
		velocity.Y += y;

		if (Position.X + velocity.X < 0 || Position.X + velocity.X > screenSize.X)
		{
			velocity.X *= -1;
		}
		if (Position.Y + velocity.Y < 0 || Position.Y + velocity.Y > screenSize.Y)
		{
			velocity.Y *= -1;
		}

		var hip = Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
		var angle = (float)Math.Acos(velocity.X / hip);
		Translate(velocity * 10f * (float)delta);
		Rotation = velocity.Y < 0 ? -angle : angle;
	}
}
