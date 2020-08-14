using Godot;
using System;

namespace MartianDelivery
{
	public class UserInterface : Control
	{
		public Tooltip Tooltip { get { return GetNode<Tooltip>("Tooltip"); } }

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
