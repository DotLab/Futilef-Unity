using System;
using UnityEngine;

public class BBanana : FSprite {
	private float _rotationSpeed;
	private float _speedY;
	
	public BBanana() : base("Banana") {
		_rotationSpeed = RXRandom.Range(-360.0f, 360.0f);	
		_speedY = RXRandom.Range(-0.1f, -0.5f);	

		ListenForUpdate(HandleUpdate);
	}
	
	public void HandleUpdate() {
		_speedY -= 0.013f;
		
		rotation += _rotationSpeed * Time.deltaTime;
		y += _speedY;	
	}
}


