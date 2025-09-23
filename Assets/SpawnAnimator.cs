using UnityEngine;

public class SpawnAnimator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Humanoid 3D Model")]
    public GameObject humanoidModel;

    void Start()
    {
        if (humanoidModel == null)
        {
            Debug.Log("Humanoid model does not have value");
        }

        GameObject instantiatedModel = Instantiate(humanoidModel);
        instantiatedModel.name = "Instantiated Humanoid 3D Model";
    }
}
