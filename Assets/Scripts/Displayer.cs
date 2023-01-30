using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Displayer : MonoBehaviour
{
    // TODO: fix for iteration on grid's childs and layers' childs 
    public GameObject layer;
    public int layerLevel = 0;

    private float offSetParameter = 1.0f;

    private bool updateForPainting = false;
    private bool updateForCleaning = false;

    void Update()
    {
        // TODO: check for all layers
        bool isLayerUpdated = CheckForUpdate(layer, layerLevel);
        if (updateForPainting)
        {
            if (isLayerUpdated)
            {
                updateForPainting = false;
            }
            else
            {
                foreach (Transform tile in layer.GetComponentsInChildren<Transform>())
                {
                    if (tile.transform.position.y != 0)
                    {
                        tile.transform.position = Vector3.MoveTowards(tile.transform.position, new Vector3(tile.transform.position.x, 0, tile.transform.position.z), 0.5f * Time.deltaTime);
                    }
                }
            }
        } 
        /*
        else if (updateForCleaning)
        {
            Debug.Log("ok");
        }
        */
    }

    private bool CheckForUpdate(GameObject layer, int layerLevel)
    {
        // TODO: Get object list out of update method
        bool updated = true;
        Transform[] inactiveGoInLayer = layer.GetComponentsInChildren<Transform>(true);
        List<GameObject> listGoInLayer = new List<GameObject>();

        foreach (Transform go in inactiveGoInLayer)
        {
            if (!go.CompareTag("layer"))
                listGoInLayer.Add(go.gameObject);
        }

        foreach (GameObject go in listGoInLayer)
        {
            if (go.transform.position.y != layerLevel)
            {
                updated = false;
                updateForPainting = true;
                // TODO: shortcut calculs
                // return updated;
            }
        }
        Debug.Log("updated?" + updated);
        return updated;
    }

    /// <summary>
    /// Start the displayer true for a layer.
    /// </summary>
    /// <param name="layer">The current layer.</param>
    public IEnumerator HelloDisplayer(GameObject layer)
    {
        bool activeFlag = true;
        //yield return StartCoroutine(WaitAndDisplayInline(layer, 0.05f, activeFlag));
        yield return StartCoroutine(WaitAndDisplayRandom(layer, 0.05f, activeFlag));
        Debug.Log("hellodisplayer() is done?");
    }

    /// <summary>
    /// Start the displayer false for a layer.
    /// </summary>
    /// <param name="layer">The current layer.</param>
    public IEnumerator ByeDisplayer(GameObject layer)
    {
        bool activeFlag = false;
        //yield return StartCoroutine(WaitAndDisplayInline(layer, 0.05f, activeFlag));
        yield return StartCoroutine(WaitAndDisplayRandom(layer, 0.05f, activeFlag));
        Debug.Log("byedisplayer() is done?");
    }

    /// <summary>
    /// Display line by line the tiles of a layer with a short delay between each tile.
    /// </summary>
    /// <param name="layer">The current layer.</param>
    /// <param name="displayDelay">The delay to display.</param>
    /// <param name="activeFlag">The flag to display or hide a tile.</param>
    /// <returns>Yield with delay.</returns>
    private IEnumerator WaitAndDisplayInline(GameObject layer, float displayDelay, bool activeFlag)
    {
        foreach (Transform tile in layer.GetComponentsInChildren<Transform>(true))
        {
            if (!tile.CompareTag("layer")) { 
                if (activeFlag)
                {
                    yield return new WaitForSeconds(displayDelay);
                    tile.gameObject.SetActive(activeFlag);
                }
                else
                {
                    yield return new WaitForSeconds(displayDelay);
                    tile.gameObject.SetActive(activeFlag);
                }
            }
        }
    }

    /// <summary>
    /// Display randomly the tiles of a layer with a short delay between each tile.
    /// </summary>
    /// <param name="layer">The current layer.</param>
    /// <param name="displayDelay">The delay to display.</param>
    /// <param name="activeFlag">The flag to display or hide a tile.</param>
    /// <returns>Yield with delay.</returns>
    private IEnumerator WaitAndDisplayRandom(GameObject layer, float displayDelay, bool activeFlag)
    {
        // IMPORTANT TODO: check if ground gameobject is taken
        // if yes remove it
        Transform[] inactiveGoInLayer = layer.GetComponentsInChildren<Transform>(true);
        List<GameObject> listGoInLayer = new List<GameObject>();

        foreach (Transform go in inactiveGoInLayer)
        {
            if (!go.CompareTag("layer"))
                listGoInLayer.Add(go.gameObject);
        }

        List<GameObject> listGoShuffledInLayer = ShuffleGameobjects(listGoInLayer);


        foreach (GameObject tile in listGoShuffledInLayer)
        {
            if (activeFlag)
            {
                yield return new WaitForSeconds(displayDelay);
                tile.SetActive(activeFlag);
            }
            else
            {
                yield return new WaitForSeconds(displayDelay);
                tile.SetActive(activeFlag);
            }
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
