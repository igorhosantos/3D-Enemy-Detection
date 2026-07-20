using EnemyDetection.DataStructures;
using UnityEngine;
using Utils.ObjectPooling;


namespace EnemyDetection.Controllers
{
    public class PlayerSpawnerController : ControllerMonoBehaviour<PlayerInteractor, IPlayerOutput>, IObserver<int>
    {

        [SerializeField] private ObjectPoolingMonoBehaviour polling;
        [SerializeField] private HudController hudController;
        [SerializeField] private ScorePanelController scorePanelController;


        private readonly KdTree<PlayerController> players = new KdTree<PlayerController>();

        void Start()
        {
            hudController.Initiate(this);
        }

        private void SpawnPlayers(int totalPlayers)
        {
            for (var i = 0; i < totalPlayers; i++)
            {
                var player = polling.GetObjectInThePool().GetComponent<PlayerController>();
                players.Add(player);
                player.name = "Player" + i;
                player.UpdateData = MakeScore;
            }

            //after create, they need to know the list of active players
            for (var i = 0; i < totalPlayers; i++)
            {
                players[i].Initiate(players);
            }
        }

        public void OnNotify(int item)
        {
            DeletePlayers();
            SpawnPlayers(item);
        }

        private void DeletePlayers()
        {
            foreach (var t in players)
            {
                polling.ReturnObjectInThePool(t.gameObject);
            }

            players.Clear();
            players.UpdatePositions();
        }

        private void FixedUpdate()
        {
            players.UpdatePositions();
        }

        private void MakeScore(PlayerData playerData)
        {
            scorePanelController.MakeScore(playerData);
        }
    }
}
