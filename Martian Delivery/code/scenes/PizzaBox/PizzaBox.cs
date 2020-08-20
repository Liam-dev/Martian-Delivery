using Godot;
using System;

namespace MartianDelivery
{
	public class PizzaBox : StaticBody, ISelectable
	{
		[Export(PropertyHint.MultilineText)] private string description = "This is a box containing ";

		public string TooltipDescription { get { return description + foodName; } set { } }

		public AnimationPlayer AnimationPlayer { get { return GetNode<AnimationPlayer>("AnimationPlayer"); } }

		private Player collector;

		private string foodName;

		public bool Interactable = false;
		public bool Collected = false;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{

		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }

		public void Interact(Player player)
		{
			if (Interactable)
			{
				AnimationPlayer.Play("Close");
				collector = player;
			}
		}

		private void _on_AnimationPlayer_animation_finished(string anim_name)
		{
			if (anim_name == "Close")
			{
				collector.ReceiveItem(foodName);
				Collected = true;
				QueueFree();
			}
		}

		public void AddFood(Spatial foodItem, string name)
		{
			foodItem.Name = "foodItem";
			foodItem.Hide();
			foodName = name;
			AddChild(foodItem);
		}

		public void ShowFood()
		{
			if (GetNode("foodItem") != null)
			{
				((Spatial)GetNode("foodItem")).Show();
			}
		}
	}
}
