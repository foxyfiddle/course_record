using Godot;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public partial class Player : CharacterBody2D
{
	[Export] public PackedScene DiscScene;
	[Export] private float chargeRate = 300f;
	[Export] private float MaxPower = 400f;
	[Export] private float MinPower = 75f;
	[Export] private float discSpeed = 500f;

	private bool isCharging = false;
	private float currectLaunchPower = 0f;

	public override void _Input(InputEvent @event)
{
	if (@event is not InputEventMouseButton mouseEvent)
		return;

	if (mouseEvent.ButtonIndex != MouseButton.Left)
		return;

	if (mouseEvent.Pressed)
		StartCharging();
	else
		ReleaseThrow();
}

	public override void _Process(double delta)
	{
		if (!isCharging)
			return;
		
		currectLaunchPower += chargeRate * (float)delta;
		currectLaunchPower = Mathf.Clamp(currectLaunchPower, MinPower, MaxPower);

		GD.Print($"Charging... Current Power: {currectLaunchPower}");
	}

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
		ThrowDisc(currectLaunchPower);
	}

	private void ThrowDisc(float launchPower)
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		Vector2 direction = (mousePosition - GlobalPosition).Normalized();

		var discInstance = DiscScene.Instantiate<DiscController>();
		discInstance.GlobalPosition = GlobalPosition;
		
		GetParent().AddChild(discInstance);

		discInstance.Launch(direction, launchPower, discSpeed);
	}
}
