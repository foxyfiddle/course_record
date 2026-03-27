using Godot;
using System;

public partial class BasketArea2d : Area2D
{
	void _on_body_entered(Node body)
	{
		if (body is DiscController disc)
		{
			GD.Print(body.Name + " entered the basket area!");
		}
	}
}
