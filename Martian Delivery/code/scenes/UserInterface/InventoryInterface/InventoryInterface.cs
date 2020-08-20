using Godot;
using System;

namespace MartianDelivery
{
	public class InventoryInterface : HBoxContainer

	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{

		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }

		private void NewItem(string item)
		{
			InventoryItem itemNode = (InventoryItem)ResourceLoader.Load<PackedScene>("res://code/scenes/UserInterface/InventoryInterface/IventoryItem/InventoryItem.tscn").Instance();			
			itemNode.Image.Texture = ResourceLoader.Load<Texture>("res://assets/models/food/isometric/" + item + ".png");
			itemNode.Label.Text = "1";
			AddChild(itemNode);
			itemNode.Name = item;
		}

		private void IncreaseItem(string item)
		{
			Label label = GetNode<InventoryItem>(item).Label;
			label.Text = (int.Parse(label.Text) + 1).ToString();
		}

		private void DecreaseItem(string item)
		{
			Label label = GetNode<InventoryItem>(item).Label;
			int count = int.Parse(label.Text) - 1;
			if (count > 0)
			{
				label.Text = count.ToString();
			}
			else
			{
				Node node = GetNode<InventoryItem>(item);
				RemoveChild(node);
				node.QueueFree();
			}
		}

		public void AddItem(string item)
		{
			if (GetNode(item) == null)
			{
				NewItem(item);
			}
			else
			{
				IncreaseItem(item);
			}
		}

		public void RemoveItem(string item)
		{
			if (GetNode(item) != null)
			{
				DecreaseItem(item);
			}
		}
	}
}
