using Godot;
using System;

public partial class Bird : Polygon2D
{
	private Vector2 velocity = new(0, 0);
	private readonly Random rand = new();

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}
