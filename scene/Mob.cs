using Godot;
using System;
using System.Collections.Generic;

public partial class Mob : Node2D
{
	[Export]
	public int speed {get; set;} = 50;
	
	[Export]
	public int health_point {get; set;} = 50;
	
	[Export]
	public int damage {get; set;} = 20;

	private Vector2 screensize;
	private Node2D personnage;
	private AnimatedSprite2D _animatedSprite;
	private CharacterBody2D _characterBody;
	
	private List<Node> enemies = new List<Node>();
	private bool canAttack = true;
	private float attackCooldown = 5f; // Temps en secondes
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		personnage = GetParent().GetNodeOrNull<Node2D>("Player");	
		screensize = GetViewportRect().Size;
		_animatedSprite = GetNode<AnimatedSprite2D>("mobBody/AnimatedSprite2D");
		_characterBody = GetNode<CharacterBody2D>("mobBody");
		
		Area2D area_attack = _characterBody.GetNode<Area2D>("attack_hitbox");
		area_attack.AreaEntered += EnnemyWithinRange;
		area_attack.AreaExited += EnnemyQuitRange;
		
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
		
		if (enemies.Count != 0)
		{
			PerformAttack();
		}

		// Appliquer le déplacement avec MoveAndSlide
		_characterBody.Velocity = velocity; // Velocity est une propriété du CharacterBody2D
		_characterBody.MoveAndSlide(); // MoveAndSlide prend en compte les collisions
	}

	public void TakeDamage(int damage)
	{
		//recevoir une attaque
		GD.Print("Mob reçoit une attaque !");
		health_point = health_point - damage;
		GD.Print("Hp restants : " + health_point);
		if (health_point <= 0)
		{
			Died();
		}
	}

	public void EnnemyWithinRange(Node body)
	{
		if (body is Area2D hitbox)
		{
			Node node = hitbox.GetParent().GetParent();
			if (node is Personnage player)
			{
				enemies.Add(player);
			}
			else
			{
				GD.Print("C'est pas un mob, bizarre !?");
			}
		}
		else
		{
			GD.Print("Ya un problème chef !");
		}
	}

	public void EnnemyQuitRange(Node body)
	{
		if (body is Area2D hitbox)
		{
			Node node = hitbox.GetParent().GetParent();
			if (node is Personnage player)
			{
				enemies.Remove(player);
			}
			else
			{
				GD.Print("C'est pas un mob, bizarre !?");
			}
		}
		else
		{
			GD.Print("Ya un problème chef !");
		}
	}
	
	private void Died()
	{
		//jouer une animation de mort
		GD.Print("L'ennemi est mort !");
		QueueFree();
		// renvoyer au menu
	}
	
	private async void PerformAttack()
	{
		if (!canAttack) return;
		canAttack = false;
		foreach (var enemy in enemies)
		{
			if (enemy is Personnage player)
			{
				player.TakeDamage(damage);
			}
			else
			{
				GD.Print("C'est pas un player, bizarre !");
			}
		}
		// Attendre avant de pouvoir attaquer à nouveau
		await ToSignal(GetTree().CreateTimer(attackCooldown), "timeout");
		canAttack = true;
	}
}
