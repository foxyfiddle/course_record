using Godot;
using System;

public partial class BasketArea2d : Area2D
{
	void _on_body_entered(Node body)
	{
		GD.Print("BasketArea2d: Body entered: " + body.Name);
	}
}
