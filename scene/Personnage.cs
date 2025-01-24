using Godot;
using System;

public partial class Personnage : Node2D
{
	[Export]
	public int speed {get; set;} = 500;

	private Vector2 screensize;
	
	private AnimatedSprite2D _animatedSprite;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		screensize = GetViewportRect().Size;
		_animatedSprite = GetNode<AnimatedSprite2D>("CharacterBody2D/AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 velocity = Vector2.Zero;

		// Gérer les entrées utilisateur
		if (Input.IsActionPressed("walk_right"))
		{
			velocity.X += 1;
			_animatedSprite.FlipH = false;
		}
		if (Input.IsActionPressed("walk_left"))
		{
			velocity.X -= 1;
			_animatedSprite.FlipH = true;
		}
		if (Input.IsActionPressed("walk_down"))
		{
			velocity.Y += 1;
		}
		if (Input.IsActionPressed("walk_up"))
		{
			velocity.Y -= 1;
		}

		// Normaliser la vitesse pour éviter d'aller plus vite en diagonale
		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * speed;
			_animatedSprite.Play();
		}
		else
		{
			_animatedSprite.Stop();
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
