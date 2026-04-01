using Godot;

public partial class GameManager : Node
{
	// Global variables and references

	public static GameManager Instance { get; private set; }
	private int _strokes = 0;

	public override void _Ready()
	{
		Instance = this;
	}

	public void AddStroke()
	{
		_strokes++;
		GD.Print("Stroke added! Total strokes: " + _strokes);
	}
}
