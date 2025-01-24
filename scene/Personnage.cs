using Godot;
using System;

public partial class Personnage : Node2D
{
	[Export]
	public int speed {get; set;} = 500;
	
	[Export]
	public int health_point {get; set;} = 100;
	
	[Export]
	public int damage {get; set;} = 25;
	
	private CharacterBody2D _characterBody;
	
	private AnimatedSprite2D _animatedSprite;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_characterBody = GetNode<CharacterBody2D>("CharacterBody2D");
		_animatedSprite = GetNode<AnimatedSprite2D>("CharacterBody2D/AnimatedSprite2D");
		_animatedSprite.Animation = "walkBoss";
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//GD.Print("position perso:"+_characterBody.GlobalPosition);
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
		
		_characterBody.ZIndex = (int) velocity.Y;
		
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

		// Appliquer le déplacement avec MoveAndSlide
		_characterBody.Velocity = velocity; // Velocity est une propriété du CharacterBody2D
		_characterBody.MoveAndSlide(); // MoveAndSlide prend en compte les collisions
	}
}
