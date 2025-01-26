using Godot;
using System;
using System.Dynamic;

public partial class Gun : Node2D
{

	[Export]
	PackedScene bullet;

	[Export]
	Marker2D bulletGenPoint;

	[Export]
	AudioStreamPlayer2D audioStream;

	public AudioStreamMP3 shootSound {get; set;}

    public override void _Ready()
    {
		if(shootSound != null)
      		audioStream.Stream = shootSound;
    }

    public void Shoot(Vector2 shootDir){
		if(audioStream.Stream == null && shootSound != null)
			audioStream.Stream = shootSound;
		if(audioStream.Stream != null)
			audioStream.Play();

		Node2D bulletInstance = (Node2D)bullet.Instantiate();

		bulletInstance.Position = bulletGenPoint.GlobalPosition;
		((Bullet)bulletInstance).ChangeDir(shootDir);

		GetTree().Root.GetNode("/root/World").AddChild(bulletInstance);
	}

}
