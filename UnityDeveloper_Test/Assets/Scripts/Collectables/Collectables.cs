
using GravityGuy.UI;
using GravityGuy.Player;
using UnityEngine;

public class Collectables : MonoBehaviour,ICollectable
{
    public UImanager manager;
    public void OnCollect()
    {
        Debug.Log("Collectable collected");
        Destroy(this.gameObject);
        manager.Collect();
    }
}
