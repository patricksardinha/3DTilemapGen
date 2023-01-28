using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Displayer : MonoBehaviour
{

    void Update()
    {
        
    }

    public void HelloDisplayer(GameObject layer)
    {
        StartCoroutine(WaitAndDisplayLine(layer));
    }

    private IEnumerator WaitAndDisplayLine(GameObject layer)
    {
        foreach (Transform tile in layer.GetComponentsInChildren<Transform>(true))
        {
            yield return new WaitForSeconds(0.05f);
            tile.gameObject.SetActive(true);
        }
    }

    // TODO: FIX
    private IEnumerator WaitAndDisplayRandom(GameObject layer)
    {
        Transform[] inactiveGo = layer.GetComponentsInChildren<Transform>(true);
        List<GameObject> listInactiveGo = new List<GameObject>();

        foreach (Transform go in inactiveGo)
        {
            listInactiveGo.Add(go.gameObject);
        }

        foreach (Transform tile in layer.GetComponentsInChildren<Transform>(true))
        {
            yield return new WaitForSeconds(0.05f);
            tile.gameObject.SetActive(true);
        }
    }
}
