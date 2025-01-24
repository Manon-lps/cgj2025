using Godot;
using System;

public partial class Mob : Node2D
{
	[Export]
	public int speed {get; set;} = 50;

	private Vector2 screensize;
	private Node2D personnage;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		screensize = GetViewportRect().Size;
		personnage = GetNode<Node2D>("../Player");	
		
		if (personnage == null)
		{
			GD.Print("Warning: Player character node not found!");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{		
		Vector2 playerPosition = personnage.GlobalPosition;
		Vector2 mobPosition = GlobalPosition;
		Vector2 velocity = Vector2.Zero;

		if (playerPosition.X > mobPosition.X)
			velocity.X += 1; // Le joueur est à droite
			
		else if (playerPosition.X < mobPosition.X)
			velocity.X -=1; // Le joueur est à gauche
			

		if (playerPosition.Y > mobPosition.Y)
			velocity.Y += 1; // Le joueur est en bas
			
		else if (playerPosition.Y < mobPosition.Y)
			velocity.Y -=1; // Le joueur est en haut
			


		// Normaliser la vitesse pour éviter d'aller plus vite en diagonale
		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * speed;
		}

		// Mettre à jour la position du personnage
		Position += velocity * (float)delta;

		// Garder le personnage dans les limites de l'écran
		Position = new Vector2(
			Mathf.Clamp(Position.X, 0, screensize.X),
			Mathf.Clamp(Position.Y, 0, screensize.Y)
		);
	}
}
