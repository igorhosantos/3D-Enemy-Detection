using System;
using EnemyDetection.Behaviours;
using EnemyDetection.DataStructures;
using UnityEngine;
using UnityEngine.Events;

namespace EnemyDetection.Controllers
{
    public class PlayerController : ControllerMonoBehaviour<PlayerInteractor, IPlayerOutput>
    {

        [SerializeField] private PlayerMovementScriptableObject playerMovement;
        [SerializeField] private FindNearestNeighbour findNearestNeighbour;
        [SerializeField] private LineRenderer lineRenderer;

        private bool startMovement;
        private KdTree<PlayerController> players;
        private int hits;

        public UnityAction<PlayerData> UpdateData;

        public void Initiate(KdTree<PlayerController> players)
        {
            this.players = players;
            interactor.SetInitialPosition(playerMovement.PlayerMovement);
            hits = 0;
            startMovement = true;
        }

        private void FixedUpdate()
        {
            if (!startMovement)
            {
                return;
            }

            interactor.UpdateMovement(playerMovement.PlayerMovement.speedMovement,
                transform.localPosition,
                playerMovement.PlayerMovement.distance);

            //Calculate distance only if we have more than one player
            if (players.Count > 1)
            {
                var nearestNeighbour = findNearestNeighbour.GetNext(players);
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, nearestNeighbour.transform.position);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.GetComponent<PlayerController>())
            {
                // collide with another player, update hit
                hits++;
                UpdateData?.Invoke(new PlayerData(name, hits));
            }
        }
    }
}
