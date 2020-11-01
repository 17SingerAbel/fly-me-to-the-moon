using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class HandPresence : MonoBehaviour
{
    // Start is called before the first frame update
    public bool showController;
    public InputDeviceCharacteristics controllerCharacteristics;
    public List<GameObject> controllerPrefabs;
    public GameObject handPrefabs;
    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHand;
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        InputDevices.GetDevices(devices);
        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            Debug.Log("get target device");
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.Log("Did not find corresponding controller model");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }

            spawnedHand = Instantiate(handPrefabs, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (showController)
        {
            spawnedHand.SetActive(false);
            spawnedController.SetActive(true);
        }
        else
        {
            spawnedHand.SetActive(true);
            spawnedController.SetActive(false);
        }
        // targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        // if (primaryButtonValue)
        // {
        //     Debug.Log("Pressing Primary Button");
        // }
        // targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        // if (triggerValue > 0.1f)
        // {
        //     Debug.Log("Trigger pressed" + triggerValue);
        // }

        // targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisVlue);
        // if (primary2DAxisVlue != Vector2.zero)
        // {
        //     Debug.Log("Primary Touchpad" + primary2DAxisVlue);
        // }
    }
}
