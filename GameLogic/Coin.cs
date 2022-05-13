// This script controls the coin collectables. It is responsible for detecting collision
// with the player and reporting it to the game manager. Additionally, since the coin
// is a part of the level it will need to register itself with the game manager

using UnityEngine;

public class Coin : MonoBehaviour
{
	int playerLayer;	//The layer the player game object is on


	void Start()
	{
		//Get the integer representation of the "Player" layer
		playerLayer = LayerMask.NameToLayer("Player");

        //Register this orb with the game manager
		GameManager.RegisterCoin(this);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		//If the collided object isn't on the Player layer, exit.
		if (collision.gameObject.layer != playerLayer)
			return;


        //TODO: make sound
		
		//Tell the game manager that this coin was collected
		GameManager.PlayerGrabbedCoin(this);

		//Deactivate this coin to hide it and prevent further collection
		gameObject.SetActive(false);
	}
}
