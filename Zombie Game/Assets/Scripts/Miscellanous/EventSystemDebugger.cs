using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemDebugger : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        EventSystem[] systems = Resources.FindObjectsOfTypeAll<EventSystem>();
        if (systems.Length > 1)
        {
            Debug.LogWarning("Multiple EventSystems detected!");

            foreach (var sys in systems)
            {
                Debug.Log("EventSystem: " + sys.name + " (active: " + sys.gameObject.activeInHierarchy + ")");
            }
        }
    }
}
