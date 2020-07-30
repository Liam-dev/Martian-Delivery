using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MartianDelivery
{
	public class Alien : StaticBody, ISelectable
	{
		[Export] public string TooltipDescription { get; set; }

		protected Area DetectionArea { get { return GetNode<Area>("DetectionArea"); } }
		protected MeshInstance Head { get { return GetNode("CollisionShape").GetNode("Body").GetNode("Neck").GetNode<MeshInstance>("Head"); } }
		protected Spatial[] Eyes { get { return new Spatial[] { Head.GetNode<Spatial>("LEye"), Head.GetNode<Spatial>("REye") }; } }

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{

		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(float delta)
		{
			List<Player> players = new List<Player>();
			foreach (PhysicsBody body in DetectionArea.GetOverlappingBodies())
			{
				if (body is Player player)
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
	}
}
