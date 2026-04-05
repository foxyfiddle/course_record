using System;
using Godot;

public partial class CameraController : Camera2D
{
	public bool followingDisc;
	public bool followingPlayer;
	private DiscController _targetDisc;
	private Player _player;

	public override void _Ready()
	{
		_player = GetNode<Player>("../Player");


	}

	public override void _Process(double delta)
	{
		if (followingDisc)
		{
			GlobalPosition = GlobalPosition.MoveToward(_targetDisc.GlobalPosition, 
			_targetDisc.speed * (float)delta);
		}

		if (followingPlayer)
		{
			GlobalPosition = GlobalPosition.MoveToward(_player.GlobalPosition, 
			_player.MaxSpeed * (float)delta);
		}
	}

	public void FollowDisc(DiscController disc)
{
	_targetDisc = disc;
	followingDisc = true;
	followingPlayer = false;
}

	public void StopFollowDisc()
	{
		followingDisc = false;
	}

	public void FollowPlayer()
{
	followingPlayer = true;
	followingDisc = false;
}
}
