using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.EX_A
{
    [AddComponentMenu("")]
    public class EX_A_NetworkMgr : NetworkManager
    {
        #region server func
        /// <summary>
        /// Called on the server when a client adds a new player with NetworkClient.AddPlayer.
        /// <para>The default implementation for this function creates a new player object from the playerPrefab.</para>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            //EX_A_Server.Instance.
        }

        /// <summary>
        /// Called on the server when a client disconnects.
        /// <para>This is called on the Server when a Client disconnects from the Server. Use an override to decide what should happen when a disconnection is detected.</para>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            //Player.ResetPlayerNumbers();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            GameFacade.isServer = true;

            foreach (var system in GameFacade.serverSystems)
            {
                system.OnStartServer();
            }
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            GameFacade.isServer = false;

            foreach (var system in GameFacade.serverSystems)
            {
                system.OnStopServer();
            }
        }

        #endregion


        #region client func
        public override void OnStartClient()
        {
        }

        /// <summary>
        /// 客户端处调用，当连接到服务器时
        /// </summary>
        public override void OnClientConnect()
        {
            base.OnClientConnect();

            foreach (var system in GameFacade.clientSystems)
            {
                system.OnClientConnect();
            }
        }

        /// <summary>
        /// 客户端处调用，当连接断开时
        /// </summary>
        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();

            foreach (var system in GameFacade.clientSystems)
            {
                system.OnClientDisconnect();
            }
        }

        #endregion
    }
}