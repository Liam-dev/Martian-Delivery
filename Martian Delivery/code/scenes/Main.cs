using Godot;
using System;

namespace MarsMission
{
    public class Main : Spatial
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

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
            else if (Input.IsActionJustPressed("perspective_first"))
            {
                ((Player)GetNode("Player")).Camera.MakeCurrent();
            }
            else if (Input.IsActionJustPressed("perspective_third"))
            {              
                ((Camera)GetNode("Camera")).MakeCurrent();
            }
        }
    }
}

