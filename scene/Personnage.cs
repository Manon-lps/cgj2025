using Godot;
using System;
using System.Collections.Generic;

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
	
	private List<Node> enemies = new List<Node>();
	
	private bool canAttack = true;
	private float attackCooldown = 1f; // Temps en secondes
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_characterBody = GetNode<CharacterBody2D>("CharacterBody2D");
		_animatedSprite = GetNode<AnimatedSprite2D>("CharacterBody2D/AnimatedSprite2D");
		_animatedSprite.Animation = "walkBoss";
		
		//Gestion des combats
		Area2D area_attack = _characterBody.GetNode<Area2D>("attack_hitbox");
		area_attack.AreaEntered += EnnemyWithinRange;
		area_attack.AreaExited += EnnemyQuitRange;
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

		if (Input.IsActionJustPressed("attack"))
		{
			GD.Print("Joueur attaque !");
			PerformAttack();
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

	public void TakeDamage(int damage)
	{
		//recevoir une attaque
		GD.Print("Le joueur se fait attaquer.");
		health_point = health_point - damage;
		GD.Print("Hp restants : " + health_point);
		if (health_point <= 0)
		{
			Died();
		}
	}

	public void EnnemyWithinRange(Node body)
	{
		GD.Print("EnnemyWithinRange");
		if (body is Area2D hitbox)
		{
			Node node = hitbox.GetParent().GetParent();
			if (node is Mob mob)
			{
				enemies.Add(mob);
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

	private void EnnemyQuitRange(Node body)
	{
		GD.Print("EnnemyQuitRange");
		if (body is Area2D hitbox)
		{
			Node node = hitbox.GetParent().GetParent();
			if (node is Mob mob)
			{
				enemies.Add(mob);
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
		GD.Print("Le joueur est mort !");
		QueueFree();
		// renvoyer au menu
	}
	
	private async void PerformAttack()
	{
		if (!canAttack) return;
		canAttack = false;
		foreach (var enemy in enemies)
		{
			if (enemy is Mob mob)
			{
				mob.TakeDamage(damage);
			}
			else
			{
				GD.Print("C'est pas un mob, bizarre !");
			}
		}
		// Attendre avant de pouvoir attaquer à nouveau
		await ToSignal(GetTree().CreateTimer(attackCooldown), "timeout");
		canAttack = true;
	}
}
