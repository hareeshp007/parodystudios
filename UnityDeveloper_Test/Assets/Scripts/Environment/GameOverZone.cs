
using GravityGuy.Player;
using GravityGuy.UI;
using UnityEngine;

namespace GravityGuy.Env
{
    public class GameOverZone : MonoBehaviour
    {
        public UImanager manager;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PlayerView>() != null)
            {
                Debug.Log("Game Over");
                manager.GameOverUI();
            }
        }
    }

}
