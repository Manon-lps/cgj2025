using Godot;
using System;

public partial class BossRoom : Node2D
{
	private Node2D _personnage;
	private Node2D _boss;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Charger la scène du personnage dynamiquement
		var noeudparent = GetParent();
		_personnage = noeudparent.GetNodeOrNull<Node2D>("Player");
		_boss = noeudparent.GetNodeOrNull<Node2D>("Mob");
		if (_personnage != null && _boss != null)
		{
			// Positionner le joueur dans la BossRoom
			_personnage.Position = new Vector2(664, 360); // Par exemple, X=664, Y=360
			_boss.Position = new Vector2(604, 300);
		}else {
			// Afficher une erreur si "Player" est introuvable
			GD.PrintErr("Erreur : Le nœud 'Player' et/ou 'Mob' est introuvable !");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
