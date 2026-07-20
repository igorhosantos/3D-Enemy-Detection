using EnemyDetection.Controllers;
using EnemyDetection.DataStructures;
using UnityEngine;

namespace EnemyDetection.Behaviours
{
    public class FindNearestNeighbour : MonoBehaviour
    {
        public PlayerController GetNext(KdTree<PlayerController> players)
        {
            return players.FindClosest(transform.position);
        }
    }
}
