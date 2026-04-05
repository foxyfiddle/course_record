using Godot;
using System;

public partial class DiscController : CharacterBody2D
{
	// variables for disc movement
	private Vector2 direction = Vector2.Zero;
	public float speed = 0f;
	private float remainingDistance = 0f;
	private float totalDistance = 0f;
	private bool isLaunched = false;
	public bool isStationary = true;

	// variables for disc animation
	private AnimatedSprite2D _animatedSprite;

	// variable creating camera instance
	private CameraController _camera;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{

		if (!isLaunched)
			return;

		float frameDistance = speed * (float)delta;
		float flightLeftRatio = remainingDistance / totalDistance;

		// If this frame would overshoot, clamp to the remaining distance
		if (frameDistance > remainingDistance)
		{
			Velocity = direction * (remainingDistance / (float)delta);
			MoveAndSlide();

			// Stop movement
			Velocity = Vector2.Zero;
			isStationary = true;
			isLaunched = false;
			remainingDistance = 0f;
			_camera.StopFollowDisc();
			_camera.FollowPlayer();

			return;
		}

		Velocity = direction * speed;

		_animatedSprite.SpeedScale = flightLeftRatio; // Slow down animation as it approaches the target
		MoveAndSlide();

		remainingDistance -= frameDistance;
	}

	public void Launch(Vector2 launchDirection, float launchDistance, float launchSpeed)
	{
		direction = launchDirection.Normalized();
		remainingDistance = launchDistance;
		totalDistance  = launchDistance;
		speed = launchSpeed;
		isLaunched = true;
		isStationary = false;

		StartSpin();
	}

	// animation logic
	public void StartSpin()
	{
		_animatedSprite.Play("spin");
	}

	public void SetCamera(CameraController camera)
	{
		_camera = camera;
	}
}
