
using GravityGuy.Player;
using GravityGuy.UI;
using UnityEngine;

namespace GravityGuy.Env
{
    public class GameOverZone : MonoBehaviour
    {
        public UImanager manager;
        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.GetComponent<PlayerView>() != null)
            {
                Debug.Log("Game Over");
                manager.GameOverUI();
            }
            
        }
    }

}
