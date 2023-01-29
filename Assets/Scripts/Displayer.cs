using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Displayer : MonoBehaviour
{

    void Update()
    {
        
    }

    /// <summary>
    /// Start the displayer for a layer.
    /// </summary>
    /// <param name="layer">The current layer.</param>
    public void HelloDisplayer(GameObject layer)
    {
        //StartCoroutine(WaitAndDisplayInline(layer, 0.05f));
        StartCoroutine(WaitAndDisplayRandom(layer, 0.05f));
    }

    /// <summary>
    /// Display line by line the tiles of a layer with a short delay between each tile.
    /// </summary>
    /// <param name="layer">The current layer.</param>
    /// <param name="displayDelay">The delay to display.</param>
    /// <returns>Yield with delay.</returns>
    private IEnumerator WaitAndDisplayInline(GameObject layer, float displayDelay)
    {
        foreach (Transform tile in layer.GetComponentsInChildren<Transform>(true))
        {
            yield return new WaitForSeconds(displayDelay);
            tile.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Display randomly the tiles of a layer with a short delay between each tile.
    /// </summary>
    /// <param name="layer">The current layer.</param>
    /// <param name="displayDelay">The delay to display.</param>
    /// <returns>Yield with delay.</returns>
    private IEnumerator WaitAndDisplayRandom(GameObject layer, float displayDelay)
    {
        Transform[] inactiveGoInLayer = layer.GetComponentsInChildren<Transform>(true);
        List<GameObject> listGoInLayer = new List<GameObject>();

        foreach (Transform go in inactiveGoInLayer)
        {
            listGoInLayer.Add(go.gameObject);
        }

        List<GameObject> listGoShuffledInLayer = ShuffleGameobjects(listGoInLayer);

        foreach (GameObject tile in listGoShuffledInLayer)
        {
            yield return new WaitForSeconds(displayDelay);
            tile.SetActive(true);
        }
    }

    /// <summary>
    /// Shuffle a list of gameobjects.
    /// </summary>
    /// <param name="listGo">The initial list of gameobjects.</param>
    /// <returns>The list randomly shuffled.</returns>
    private List<GameObject> ShuffleGameobjects(List<GameObject> listGo)
    {
        for (int i = 0; i < listGo.Count; i++)
        {
            GameObject temp = listGo[i];
            int randomIndex = Random.Range(i, listGo.Count);
            listGo[i] = listGo[randomIndex];
            listGo[randomIndex] = temp;
        }
        return listGo;
    }
}
