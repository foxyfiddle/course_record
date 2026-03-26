using Godot;
using System;

public partial class DiscController : CharacterBody2D
{
	public Vector2 Direction;
	public float Speed = 500f;

	private float remainingDistance = 0f;
	private bool isLaunched = false;

	public void Launch(Vector2 direction, float distance)
	{
		Direction = direction;
		remainingDistance = distance;
		isLaunched = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{

		if (!isLaunched)
			return;

		float moveAmount = Speed * (float)delta;

		// If this frame would overshoot, clamp to the remaining distance
		if (moveAmount > remainingDistance)
		{
			Velocity = Direction * (remainingDistance / (float)delta);
			MoveAndSlide();

			// Stop movement
			Velocity = Vector2.Zero;
			isLaunched = false;
			remainingDistance = 0f;
			return;
		}

		Velocity = Direction * Speed;
		MoveAndSlide();

		remainingDistance -= moveAmount;
	}
}
