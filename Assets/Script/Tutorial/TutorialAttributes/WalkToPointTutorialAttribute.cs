using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TutorialAttribute/WalkToPoint")]
public class WalkToPointTutorialAttribute : TutorialAttribute
{
    [SerializeField]
    private string _prefabName = "WayPointCollection";

    private List<GameObject> _targetWaypoints = new List<GameObject>();

    // Update is called once per frame
    public override void CheckingObjective()
    {
        _targetWaypoints.RemoveAll(item => item == null);
        _clear = _targetWaypoints.Count <= 0;
    }

    public override void SetBegin()
    {
        base.SetBegin();
        GameObject prefab = LoadResource();

        if (prefab != null)
        {
            // Instantiate the prefab at the specified position and rotation
            Vector3 spawnPosition = new Vector3(0, 0, 0); // Change this to your desired position
            Quaternion spawnRotation = Quaternion.identity; // Change this to your desired rotation
            GameObject spawnPrefab = Instantiate(prefab, spawnPosition, spawnRotation);
            _targetWaypoints.Clear();

            for (int i = 0; i < spawnPrefab.transform.childCount; i++)
            {
                _targetWaypoints.Add(spawnPrefab.transform.GetChild(i).gameObject);
            }

        }
        else
        {
            Debug.LogError("Prefab not found in Resources folder");
        }
    }

    protected virtual GameObject LoadResource() 
    {
        return Resources.Load<GameObject>(_prefabName); ;
    }
}
