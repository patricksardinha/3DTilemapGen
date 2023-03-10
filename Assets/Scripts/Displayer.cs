using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Displayer : MonoBehaviour
{
    // TODO: fix for iteration on grid's childs and layers' childs 
    private GameObject[] arrayLayers;
    private string[] infoArrayLayers;

    [SerializeField] private float tileAnimationSpeed;
    private float offSetParameter = 1.0f;

    private bool updateForCleaning = false;

    void Update()
    {
        // TODO: check for all layers
        if (arrayLayers != null && infoArrayLayers != null)
        {
            int currentLayerUpdating = 0;
            for (int i = 0; i < arrayLayers.Length; i++)
            {
                int layerLevel = i;
                Debug.Log("LAYER: " + currentLayerUpdating + "------> " + infoArrayLayers[i]);
                if (i == currentLayerUpdating && infoArrayLayers[i] == "updating")
                {
                    bool isLayerUpdated;
                    bool updateForPainting;
                    (isLayerUpdated, updateForPainting) = CheckForUpdate(arrayLayers[i], layerLevel);

                    Debug.Log("ufp: " + updateForPainting + " -- ilu: " + isLayerUpdated);
                    // TODO: Fix delay for second layer?
                    if (updateForPainting)
                    {
                        if (!isLayerUpdated)
                        {
                            foreach (Transform tile in arrayLayers[i].GetComponentsInChildren<Transform>())
                            {
                                if (tile.transform.position.y != layerLevel)
                                {
                                    tile.transform.position = Vector3.MoveTowards(tile.transform.position, new Vector3(tile.transform.position.x, layerLevel, tile.transform.position.z), (1/tileAnimationSpeed) * Time.deltaTime);
                                }
                            }
                        }
                        else
                        {
                            infoArrayLayers[i] = "updated";
                            currentLayerUpdating++;
                        }
                    }
                    else
                    {
                        infoArrayLayers[i] = "updated";
                        currentLayerUpdating++;
                    }
                    /*
                    else if (updateForCleaning)
                    {
                        Debug.Log("ok");
                    }
                    */
                }
                else
                {
                    currentLayerUpdating++;
                }
            }
        }
    }

    private (bool, bool) CheckForUpdate(GameObject layer, int layerLevel)
    {
        // TODO: Get object list out of update method
        bool updated = true;
        bool updateForP = false;
        if (layer != null) {
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
                    updateForP = true;
                    // TODO: shortcut calculs
                    // return updated;
                }
            }
        }
        Debug.Log("updated?" + updated);
        return (updated, updateForP);
    }

    /// <summary>
    /// Start the displayer true for a layer.
    /// </summary>
    /// <param name="layer">The current layer.</param>
    public IEnumerator HelloDisplayer(GameObject[] layers)
    {
        bool activeFlag = true;
        //yield return StartCoroutine(WaitAndDisplayInline(layers[0], 0.05f, activeFlag));
        arrayLayers = layers;
        infoArrayLayers = new string[arrayLayers.Length] ;

        Debug.Log("hello ---> " + infoArrayLayers.Length);
        // Displayer was called so infoArrayLayers is created.
        // Both array have the same size.
        if (arrayLayers != null)
        {
            for (int i = 0; i < arrayLayers.Length; i++)
            {
                infoArrayLayers[i] = "noUpdate";
            }

            infoArrayLayers[0] = "updating";
            for (int i = 0; i < arrayLayers.Length; i++) {
                if (i == arrayLayers.Length - 2)
                {
                    yield return new WaitForSeconds(0.1f);
                    infoArrayLayers[arrayLayers.Length - 1] = "updating";
                }
                
                yield return StartCoroutine(WaitAndDisplayRandom(arrayLayers[i], 0.05f, activeFlag));
                Debug.Log("hellodisplayer() is done: " + arrayLayers[i].name);

            }
        }
    }

    /// <summary>
    /// Start the displayer false for a layer.
    /// </summary>
    /// <param name="layer">The current layer.</param>
    public IEnumerator ByeDisplayer(GameObject layer)
    {
        bool activeFlag = false;
        //yield return StartCoroutine(WaitAndDisplayInline(layer[0], 0.05f, activeFlag));

        yield return StartCoroutine(WaitAndDisplayRandom(layer, 0.05f, activeFlag));
        Debug.Log("byedisplayer() is done?");
    }

    private void GetActiveGoInLayer(GameObject layer)
    {

    }

    private List<GameObject> GetAllGoInLayer(GameObject layer)
    {
        Transform[] allGoInLayer = layer.GetComponentsInChildren<Transform>(true);
        List<GameObject> listAllGoInLayer = new List<GameObject>();

        foreach (Transform go in allGoInLayer)
        {
            if (!go.CompareTag("layer"))
                listAllGoInLayer.Add(go.gameObject);
        }

        return listAllGoInLayer;
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
        
        List<GameObject> listGoInLayer = GetAllGoInLayer(layer);
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
