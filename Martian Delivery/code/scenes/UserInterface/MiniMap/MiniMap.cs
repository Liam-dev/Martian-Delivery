using Godot;
using System;
using System.Collections.Generic;

namespace MartianDelivery
{
	public class MiniMap : MarginContainer
	{
		[Export] private float magnification = 10f;
		private Vector2 scale;

		public MarginContainer GridContainer { get { return GetNode<MarginContainer>("GridContainer"); } }
		public TextureRect Grid { get { return GridContainer.GetNode<TextureRect>("Grid"); } }
		public Sprite PlayerMarker { get { return Grid.GetNode<Sprite>("PlayerMarker"); } }
		public Sprite HomeMarker { get { return Grid.GetNode<Sprite>("HomeMarker"); } }
		public Sprite AlienMarker { get { return Grid.GetNode<Sprite>("AlienMarker"); } }
		public Sprite PlayerShipMarker { get { return Grid.GetNode<Sprite>("PlayerShipMarker"); } }

		private Player Player { get { return GetParent().GetParent<Player>(); } }

		private Dictionary<Spatial, Sprite> markers = new Dictionary<Spatial, Sprite>();
		private Dictionary<string, Sprite> icons;

		private PlayerShip vehicle;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			PlayerMarker.Position = Grid.RectSize / 2;

			icons = new Dictionary<string, Sprite>()
			{
				{ "Alien", AlienMarker }, { "PlayerShip", PlayerShipMarker }, {"PizzaPlace", HomeMarker}
			};

			Godot.Collections.Array mapObjects = GetTree().GetNodesInGroup("MiniMapObjects");

			foreach (Spatial mapObject in mapObjects)
			{
				Sprite marker = (Sprite)icons[mapObject.GetType().Name].Duplicate();
				Grid.AddChild(marker);
				marker.Show();
				markers.Add(mapObject, marker);				
			}
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(float delta)
		{
			if (Player.driving)
			{
				magnification = 3;
				
				if (markers.ContainsKey(Player.vehicle))
				{
					markers[Player.vehicle].Hide();
					vehicle = Player.vehicle;
				}
			}
			else
			{
				magnification = 10;
				if (vehicle != null && markers.ContainsKey(vehicle))
				{
					markers[vehicle].Show();
					vehicle = null;
				}
			}

			scale = Grid.RectSize / (GetViewportRect().Size / magnification);

			PlayerMarker.Rotation = -Player.PlaneRotation;

			Vector2 playerPosition = new Vector2(Player.GlobalTransform.origin.x, Player.GlobalTransform.origin.z);

			foreach (KeyValuePair<Spatial, Sprite> pair in markers)
			{
				Vector2 objectPosition = new Vector2(pair.Key.GlobalTransform.origin.x, pair.Key.GlobalTransform.origin.z);

				Vector2 markerPosition = (objectPosition - playerPosition) * scale + Grid.RectSize / 2;

				if (Grid.GetRect().HasPoint(markerPosition + Grid.RectPosition))
				{					
					pair.Value.Scale = new Vector2(1, 1);
				}
				else
				{
					pair.Value.Scale = new Vector2(0.75f, 0.75f);
				}

				markerPosition.x = Mathf.Clamp(markerPosition.x, 0, Grid.RectSize.x);
				markerPosition.y = Mathf.Clamp(markerPosition.y, 0, Grid.RectSize.y);

				pair.Value.Position = markerPosition;
			}

		}
	}
}
