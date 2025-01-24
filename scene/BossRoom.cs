using Godot;
using System;

public partial class BossRoom : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Charger la scène du personnage dynamiquement
		var personnageScene = GD.Load<PackedScene>("res://scene/Personnage.tscn");
		if (personnageScene != null)
		{
			var personnageInstance = personnageScene.Instantiate();
			// Vérifier que l'instance est un Node2D (ou dérivé)
			if (personnageInstance is Node2D personnageNode)
			{
				// Définir la position de spawn du joueur
				personnageNode.Position = new Vector2(664, 360); // Par exemple, X=200, Y=150
			}
			AddChild(personnageInstance);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
