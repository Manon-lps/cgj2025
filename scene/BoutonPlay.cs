using Godot;

public partial class BoutonPlay : Button
{
	public override void _Ready()
	{
		this.Pressed += OnButtonPressed;
	}

	private void OnButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://scene/Game.tscn");
	}
}
