using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    [SerializeField]
    private GameObject gameOfLife, cave;
    [SerializeField]
    private bool isGameOfLife = true;
    [SerializeField]
    private Text text;
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.C)) {
            isGameOfLife = !isGameOfLife;
        }
		if(isGameOfLife) {
            gameOfLife.SetActive(true);
            cave.SetActive(false);
            text.text = "Conway's Game Of Life";
        }
        else {
            gameOfLife.SetActive(false);
            cave.SetActive(true);
            text.text = "Procedural Cave Generation";
        }
	}
}
