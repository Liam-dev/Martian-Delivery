using Godot;
using System;

namespace MartianDelivery
{
	public class ConsoleInterface : Control
	{
		public Label Label { get { return GetNode("VSplitContainer").GetNode("NameContainer").GetNode<Label>("Label"); } }
		public TextureRect Image { get { return GetNode<TextureRect>("Image"); } }

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{

		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }

		public void Display(string item)
		{
			Label.Text = item;
			Image.Texture = ResourceLoader.Load<Texture>(Console.FoodPath + "isometric/" + item + ".png");
		}

		public void Select()
		{

		}
	}
}
