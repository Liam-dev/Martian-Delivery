using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MartianDelivery
{
	class Alien : StaticBody, ISelectable
	{
		[Export] public string MartianName { get; set; }
		[Export] public Color Colour
		{
			get
			{
				if (Body.MaterialOverride != null)
				{
					return ((SpatialMaterial)Body.MaterialOverride).AlbedoColor;

				}
				else
				{
					return new Color(95, 12, 107, 255);
				}
			}

			set
			{				
				SpatialMaterial material = (SpatialMaterial)ResourceLoader.Load<SpatialMaterial>("res://code/scenes/Alien/AlienMaterial.tres").Duplicate();
				material.AlbedoColor = value;
				Body.MaterialOverride = material;
			}
		}

		[Export(PropertyHint.MultilineText)] public string TooltipDescription { get; set; }

		protected Area DetectionArea { get { return GetNode<Area>("DetectionArea"); } }
		protected MeshInstance Head { get { return GetNode("CollisionShape").GetNode("Body").GetNode("Neck").GetNode<MeshInstance>("Head"); } }
		protected Spatial[] Eyes { get { return new Spatial[] { Head.GetNode<Spatial>("LEye"), Head.GetNode<Spatial>("REye") }; } }
		protected AudioStreamPlayer3D AudioPlayer { get { return (AudioStreamPlayer3D)GetNode("AudioPlayer"); } }
		protected Position3D Hand { get { return GetNode("CollisionShape").GetNode<Position3D>("Hand"); } }
		protected MeshInstance Body { get { return GetNode("CollisionShape").GetNode<MeshInstance>("Body"); } }

		private int orderNumber;
		private Order order;
		private bool orderComplete;
		private int talkCount;
		private bool talkedTo = false;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			order = new Order(MartianName, orderNumber);
			TooltipDescription = "Talk to " + MartianName;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(float delta)
		{
			List<Player> players = new List<Player>();
			foreach (PhysicsBody body in DetectionArea.GetOverlappingBodies())
			{
				if (body is Player player && player.Visible)
				{
					players.Add(player);
				}
			}

			// Orders playes by distance from alien
			players = players.OrderBy(p => p.GlobalTransform.origin - GlobalTransform.origin).ToList();

			if (players.Count > 0)
			{
				Player closest = players[0];

				foreach (Spatial eye in Eyes)
				{
					eye.LookAt(closest.GlobalTransform.origin, Vector3.Up);
					eye.Rotate(Vector3.Up, Mathf.Pi);
					eye.Rotation = new Vector3(-eye.Rotation.x, eye.Rotation.y, eye.Rotation.z);
				}
			}
		}

		public void Interact(Player player)
		{
			if (orderComplete)
			{
				TooltipDescription = "Hello there, I have no orders for you to bring me at the moment...";

				if (talkCount == 0)
				{
					talkCount = 0;
					Talk();
					player.Watch(this, 3);
				}
			}
			else
			{
				if (!talkedTo || !IsOrderCompleted(player))
				{
					InitialConversation(player);
				}
				else if (IsOrderCompleted(player))
				{
					CompletedConversation(player);
				}
			}				
		}

		private void InitialConversation(Player player)
		{
			talkedTo = true;
			TooltipDescription = order.initialConversation;

			if (talkCount == 0)
			{
				talkCount = 0;
				Talk();
				player.Watch(this, 4);
			}
		}

		private void CompletedConversation(Player player)
		{
			orderComplete = true;
			player.AddPoints(order.reward);
			TooltipDescription = order.completedConversation;
			DisplayFood();

			foreach (string item in order.items)
			{
				player.GiveItem(item);
			}		

			if (talkCount == 0)
			{
				talkCount = 0;
				Talk();
				player.Watch(this, 4);
			}
		}

		private bool IsOrderCompleted(Player player)
		{
			return player.HasItems(order.items);
		}

		private void DisplayFood()
		{
			Random random = new Random();
			string item = order.items[random.Next(order.items.Count)];

			PizzaBox box = (PizzaBox)ResourceLoader.Load<PackedScene>("res://code/scenes/PizzaBox/PizzaBox.tscn").Instance();
			box.RemoveChild(GetNode("SelectionArea"));
			Spatial foodItem = (Spatial)ResourceLoader.Load<PackedScene>("res://assets/models/food/" + item + ".dae").Instance();
			box.AddFood(foodItem, item);
			Hand.AddChild(box);
			foodItem.Show();
			box.AnimationPlayer.Play("Open");
		}

		private void Talk()
		{			
			Random random = new Random();
			int n = random.Next(1, 4);
			AudioPlayer.Stream = ResourceLoader.Load<AudioStream>("res://assets/audio/sound_effects/alien_sounds/alien" + n.ToString() + ".ogg");
			AudioPlayer.Play();
			talkCount++;
		}

		private void _on_AudioPlayer_finished()
		{
			if (talkCount < TooltipDescription.Length / 16)
			{
				Talk();
			}
			else
			{
				talkCount = 0;
				TooltipDescription = "Talk to " + MartianName;
			}
		}
	}
}
