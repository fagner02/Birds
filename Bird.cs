using Godot;
using System;

public class Bird : Polygon2D
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
	public override void _Process(float delta)
	{
		return;
		var screenSize = GetViewportRect().Size;
		var x = (rand.NextDouble() > 0.5 ? -1 : 1) * (float)rand.NextDouble() * 5;
		var y = (rand.NextDouble() > 0.5 ? -1 : 1) * (float)rand.NextDouble() * 5;

		velocity.x += x;
		velocity.y += y;

		if (Position.x + velocity.x < 0 || Position.x + velocity.x > screenSize.x)
		{
			velocity.x *= -1;
		}
		if (Position.y + velocity.y < 0 || Position.y + velocity.y > screenSize.y)
		{
			velocity.y *= -1;
		}

		var hip = Math.Sqrt(velocity.x * velocity.x + velocity.y * velocity.y);
		var angle = (float)Math.Acos(velocity.x / hip);
		Translate(velocity * 10 * delta);
		Rotation = velocity.y < 0 ? -angle : angle;
	}
}
