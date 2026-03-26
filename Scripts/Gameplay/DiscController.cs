using Godot;
using System;

public partial class DiscController : CharacterBody2D
{
	private Vector2 direction = Vector2.Zero;
	private float Speed = 0f;
	private float remainingDistance = 0f;
	private bool isLaunched = false;

	public void Launch(Vector2 launchDirection, float launchDistance, float launchSpeed)
	{
		direction = launchDirection.Normalized();
		remainingDistance = launchDistance;
		Speed = launchSpeed;
		isLaunched = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{

		if (!isLaunched)
			return;

		float frameDistance = Speed * (float)delta;

		// If this frame would overshoot, clamp to the remaining distance
		if (frameDistance > remainingDistance)
		{
			Velocity = direction * (remainingDistance / (float)delta);
			MoveAndSlide();

			// Stop movement
			Velocity = Vector2.Zero;
			isLaunched = false;
			remainingDistance = 0f;
			return;
		}

		Velocity = direction * Speed;
		MoveAndSlide();

		remainingDistance -= frameDistance;
	}
}
