using Godot;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public partial class Player : CharacterBody2D
{
	// variables for disc throwing
	[Export] public PackedScene DiscScene;
	[Export] private float chargeRate = 300f;
	[Export] private float MaxPower = 400f;
	[Export] private float MinPower = 75f;
	[Export] private float discSpeed = 500f;

	private bool isCharging = false;
	private bool hasDisc = true;
	private float currectLaunchPower = 0f;

	// variables for player movement
	[Export] public float MaxSpeed = 200f;
	[Export] private float Acceleration = 500f;
	[Export] private float Friction = 600f;
	private Vector2 _inputDirection = Vector2.Zero;
	private bool canMove = false;

	// camera node
	private CameraController _camera;

	// 
	public override void _Ready()
	{
		_camera = GetNode<CameraController>("../Camera2D");

	}

	// player input handling
	public override void _Input(InputEvent @event)
{
	if (@event is not InputEventMouseButton mouseEvent)
		return;

	if (mouseEvent.ButtonIndex != MouseButton.Left)
		return;

	if (mouseEvent.Pressed)
		if (hasDisc)
			StartCharging();
		else
			return;
	else
		ReleaseThrow();
}

	// player pickup handling
	void _on_area_2d_body_entered(Node body)
	{
		PickupDisc(body);
	}

	// player movement handling
	public override void _PhysicsProcess(double delta)
	{
		ReadInput();
		ApplyMovement((float)delta);

		MoveAndSlide();
	}

	// player movement logic
	private void ReadInput()
	{
		_inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
	}

	private void ApplyMovement(float delta)
	{
		if (canMove && _inputDirection != Vector2.Zero)
		{
			Vector2 targetVelocity = _inputDirection * MaxSpeed;
			Velocity = Velocity.MoveToward(targetVelocity, Acceleration * delta);
		}
		else
		{
			Velocity = Velocity.MoveToward(Vector2.Zero, Friction * delta);
		}
	}

	// charging logic
	public override void _Process(double delta)
	{
		if (!isCharging)
			return;
		
		currectLaunchPower += chargeRate * (float)delta;
		currectLaunchPower = Mathf.Clamp(currectLaunchPower, MinPower, MaxPower);
	}

// player throwing logic
	private void StartCharging()
	{
		isCharging = true;
		currectLaunchPower = MinPower;
	}

	private void ReleaseThrow()
	{
		if (!isCharging)
			return;

		isCharging = false;
		hasDisc = false;
		GameManager.Instance.AddStroke();
		canMove = true;
		ThrowDisc(currectLaunchPower);
	}

	private void ThrowDisc(float launchPower)
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		Vector2 direction = (mousePosition - GlobalPosition).Normalized();

		var discInstance = DiscScene.Instantiate<DiscController>();
		discInstance.GlobalPosition = GlobalPosition;
		discInstance.SetCamera(_camera);
		
		GetParent().AddChild(discInstance);

		discInstance.Launch(direction, launchPower, discSpeed);
		_camera.FollowDisc(discInstance);
	}

	// player pickup logic
	void PickupDisc(Node body)
	{
		if (body is DiscController disc 
			&& !hasDisc
			&& disc.isStationary == true)
		{
			hasDisc = true;
			canMove = false;
			disc.QueueFree();
		}
	}
}
