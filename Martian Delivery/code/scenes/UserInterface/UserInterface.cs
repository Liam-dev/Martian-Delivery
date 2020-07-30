using Godot;
using System;

namespace MartianDelivery
{
	public class UserInterface : Control
	{
		protected ColorRect Tooltip { get { return GetNode<ColorRect>("Tooltip"); } }

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Tooltip.Hide();
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }

		public void ShowTooltip(string text)
		{
			Tooltip.GetNode("CenterContainer").GetNode<Label>("Label").Text = text;
			Tooltip.Show();
		}

		public void HideTooltip()
		{
			Tooltip.Hide();
		}
	}
}
