using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomMapGenerator : MonoBehaviour
{
    public string[] availableMapNames = 
		{ "The U-Turn", "The Asteroid Party", "The Zig Zag", "The Dark" };
	[SerializeField] public int currentMapIndex = 0;

	public static RandomMapGenerator instance;

	// Start is called before the first frame update
	void Start()
    {
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShuffleMapOrder(string[] array)
    {
		//Using Fisher-Yates Shuffle - gives O(n) time complexity
		for (int i = array.Length - 1; i > 0; i--)
		{
			// Randomize a number between 0 and i (so that the range decreases each time)
			int rnd = Random.Range(0, i);

			// Save the value of the current i, otherwise it'll overright when we swap the values
			string temp = array[i];

			// Swap the new and old values
			array[i] = array[rnd];
			array[rnd] = temp;
		}

		// Print to check array
		for (int i = 0; i < array.Length; i++)
		{
			Debug.Log(array[i]);
		}
	}

	public void StartGame()
	{
		currentMapIndex = 0;
		ShuffleMapOrder(availableMapNames);
		LoadNextMap();
	}

	public void LoadNextMap()
	{
		if(currentMapIndex < availableMapNames.Length)
		{
			SceneManager.LoadScene(availableMapNames[currentMapIndex]);
			currentMapIndex++;
		}
		else
		{
			if(GameManager.instance.playerPoints <= 7)
			{
				// Load final map
				SceneManager.LoadScene("The Finish Line");
			}
			else
			{
				LoadMainMenu();
			}
		}
	}

	public void LoadMainMenu()
	{
		// Load Main menu and reset
		SceneManager.LoadScene("Main Menu");

		GameManager.instance.player = null;
		GameManager.instance.enemy1 = null;
		GameManager.instance.enemy2 = null;
		GameManager.instance.enemy3 = null;
		GameManager.instance.playerPoints = 0;
		GameManager.instance.GameState = GameManager.State.Preparing;
		DialogueManager.instance.soCloseToWinningPanel.SetActive(false);
	}

	public string GetMapName()
	{
		if(currentMapIndex == availableMapNames.Length)
		{
			return "The Finish Line";
		}
		else
		{
			return availableMapNames[currentMapIndex];
		}
	}
}
