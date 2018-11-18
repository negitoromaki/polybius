using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connect4Player : MonoBehaviour
{
    public GameObject redPuck;
    public GameObject yellowPuck;
    private GameObject currentPuck;

    public Connect4Controller gameController;
    private bool instantiated = false;

    private int placePoint = 3; // starts out at position 3

    public void Update()
    {
        if (gameController.GameOver())
        {
            return;
        }

        if (gameController.getTurn() == 0 && !instantiated)
        {
            currentPuck = (GameObject)Instantiate(redPuck, new Vector3(0.1f, 46.5f, 0), Quaternion.identity);
            instantiated = true;
        }
        else if (gameController.getTurn() == 1 && !instantiated)
        {
            currentPuck = (GameObject)Instantiate(yellowPuck, new Vector3(0.1f, 46.5f, 0), Quaternion.identity);
            instantiated = true;
        }
    }

    public void moveLeft()
    {
        if (!instantiated || gameController.GameOver())
        {
            return;
        }

        if (placePoint <= 0)
        {
            placePoint = 0;
            return;
        }

        // move puck left
        placePoint--;
        currentPuck.transform.position -= new Vector3(8.7f, 0, 0);
    }

    public void moveRight()
    {
        if (!instantiated || gameController.GameOver())
        {
            return;
        }

        if (placePoint >= 6)
        {
            placePoint = 6;
            return;
        }

        // move puck right
        placePoint++;
        currentPuck.transform.position += new Vector3(8.7f, 0, 0);
    }

    public void changeInstantiated()
    {
        Destroy(currentPuck);
        placePoint = 3;
        instantiated = false;
    }

    public void PlaceChip()
    {
        gameController.placeChip(placePoint);
    }
}
