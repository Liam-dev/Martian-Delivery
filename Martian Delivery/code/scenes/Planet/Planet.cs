using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace MartianDelivery
{
	class Planet : Spatial
	{

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(float delta)
		{
			if (Input.IsActionJustPressed("ui_accept"))
			{
				GD.Print("Enter key pressed");
			}
			else if (Input.IsActionJustPressed("ui_cancel"))
			{
				GetTree().Quit();
			}
		}
	}
}
