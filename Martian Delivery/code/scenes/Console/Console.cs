using Godot;
using System;
using System.Collections.Generic;

namespace MartianDelivery
{
	public class Console : StaticBody
	{
		Conveyor Conveyor { get { return GetNode<Conveyor>("Conveyor"); } }
		ConsoleInterface ConsoleInterface { get { return GetNode("Viewport").GetNode<ConsoleInterface>("ConsoleInterface"); } }
		Position3D ItemDropPoint { get { return GetNode<Position3D>("ItemDropPoint"); } }
		AnimationPlayer AnimationPlayer { get { return ItemDropPoint.GetNode<AnimationPlayer>("AnimationPlayer"); } }

		private PizzaBox box;
		private int selectedItem = 0;

		public static string FoodPath = "res://assets/models/food/";

		public static readonly List<string> Items = new List<string>()
		{
			"pizza", "waffle", "pancakes", "donut"
		};

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			ConsoleInterface.Display(Items[selectedItem]);
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }

		private void _on_LeftArrowButton_Pressed()
		{
			selectedItem--;
			if (selectedItem < 0)
			{
				selectedItem = Items.Count - 1;
			}

			ConsoleInterface.Display(Items[selectedItem]);
		}

		private void _on_RightArrowButton_Pressed()
		{
			selectedItem++;
			if (selectedItem >= Items.Count)
			{
				selectedItem = 0;
			}

			ConsoleInterface.Display(Items[selectedItem]);
		}

		private void _on_SelectButton_Pressed()
		{
			if(box == null || box.Collected)
			{
				ConsoleInterface.Select();
				DropItem(Items[selectedItem]);
				Conveyor.Moving = true;
			}		
		}

		private void DropItem(string item)
		{
			box = (PizzaBox)ResourceLoader.Load<PackedScene>("res://code/scenes/PizzaBox/PizzaBox.tscn").Instance();
			Spatial foodItem = (Spatial)ResourceLoader.Load<PackedScene>(FoodPath + item + ".dae").Instance();
			box.AddFood(foodItem, item);
			ItemDropPoint.AddChild(box);
			AnimationPlayer.Play("ItemDrop");
		}

		private void _on_AnimationPlayer_animation_finished(string anim_name)
		{
			if (anim_name == "ItemDrop")
			{
				AnimationPlayer.Play("ItemMove");
			}

			if (anim_name == "ItemMove")
			{
				box.ShowFood();
				box.Interactable = true;
				box.AnimationPlayer.Play("Open");
			}
		}
	}
}
