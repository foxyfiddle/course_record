using Godot;
using System;

public partial class DiscController : CharacterBody2D
{
	public Vector2 Direction;
	public float Speed = 500f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Velocity = Direction * Speed;
		MoveAndSlide();
	}
}
