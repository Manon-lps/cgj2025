using Godot;
using System;

public partial class Mob : Node2D
{
	[Export]
	public int speed {get; set;} = 50;

	private Vector2 screensize;
	private Node2D personnage;
	private AnimatedSprite2D _animatedSprite;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		personnage = GetParent().GetNodeOrNull<Node2D>("Player");	
		screensize = GetViewportRect().Size;
		_animatedSprite = GetNode<AnimatedSprite2D>("mobBody/AnimatedSprite2D");
		
		if (personnage == null)
			GD.Print("WARNING: Mob.cs -> player n'as pas pu être trouvé!");
		if (_animatedSprite == null)
			GD.Print("WARNING: Mob.cs -> mob.cs -> l'animation n'as pas pu être trouvée!");

		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{		
		
		Vector2 playerPosition = personnage.GetNodeOrNull<CharacterBody2D>("CharacterBody2D").GlobalPosition;
		Vector2 mobPosition = GlobalPosition;
		Vector2 velocity = Vector2.Zero;
		
		//GD.Print("position joueur trouvée:"+playerPosition.ToString());

		if (playerPosition.X > mobPosition.X){
			velocity.X += 1; // Le joueur est à droite
			_animatedSprite.FlipH = false;
		}
			
		else if (playerPosition.X < mobPosition.X){
			velocity.X -=1; // Le joueur est à gauche
			_animatedSprite.FlipH = true;
			}

		if (playerPosition.Y > mobPosition.Y)
			velocity.Y += 1; // Le joueur est en bas
			
		else if (playerPosition.Y < mobPosition.Y)
			velocity.Y -=1; // Le joueur est en haut
			


		// Normaliser la vitesse pour éviter d'aller plus vite en diagonale
		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * speed;
			_animatedSprite.Play();
		}
		else
			_animatedSprite.Stop();
		

		// Mettre à jour la position du personnage
		Position += velocity * (float)delta;

		// Garder le personnage dans les limites de l'écran
		Position = new Vector2(
			Mathf.Clamp(Position.X, 0, screensize.X),
			Mathf.Clamp(Position.Y, 0, screensize.Y)
		);
	}
}
