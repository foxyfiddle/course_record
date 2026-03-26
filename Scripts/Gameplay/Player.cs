using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public PackedScene DiscScene;

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && 
			mouseEvent.Pressed &&
			mouseEvent.ButtonIndex == MouseButton.Left)
		{
			ThrowDisc();
		}
	}

	private void ThrowDisc()
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		Vector2 direction = (mousePosition - GlobalPosition).Normalized();

		var discInstance = DiscScene.Instantiate<DiscController>();

		discInstance.GlobalPosition = GlobalPosition;
		discInstance.Direction = direction;
		discInstance.Speed = 500f; // Set the speed for the disc

		GetParent().AddChild(discInstance);
		discInstance.Launch(direction, (mousePosition - GlobalPosition).Length());
	}
}
