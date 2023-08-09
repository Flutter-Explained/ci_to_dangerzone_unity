using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VehicleSelectionScript : MonoBehaviour
{
    public GameObject[] vehicles;
    public int selectedVehicle;
    public float rotationSpeed = 0.5f;

    void Update()
    {
        vehicles[selectedVehicle].transform.Rotate(100 * Time.deltaTime * -Vector3.forward);

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            NextVehicle();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            PrevVehicle();
        }

        if (Input.GetKeyDown(KeyCode.Return)) StartGame(); 
    }

    public void PrevVehicle()
    {
        vehicles[selectedVehicle].SetActive(false);
        selectedVehicle--;
        if (selectedVehicle < 0)
        {
            selectedVehicle += vehicles.Length;
        }
        vehicles[selectedVehicle].SetActive(true);
    }

    public void NextVehicle()
    {
        vehicles[selectedVehicle].SetActive(false);
        selectedVehicle = (selectedVehicle + 1) % vehicles.Length;
        vehicles[selectedVehicle].SetActive(true);
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("selectedVehicle", selectedVehicle);
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
