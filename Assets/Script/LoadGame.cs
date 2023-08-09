using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    public GameObject[] vehicles;
    // Start is called before the first frame update
    void Start()
    {
        var selectedVehicle = PlayerPrefs.GetInt("selectedVehicle", 0);
        vehicles[selectedVehicle].SetActive(true);
    }
}
