using Godot;
using System;

public partial class BasketArea2d : Area2D
{
	void _on_body_entered(Node body)
	{
		if (body is DiscController disc)
		{
			disc.QueueFree();
			GD.Print("Goal!");
		}
	}
}
