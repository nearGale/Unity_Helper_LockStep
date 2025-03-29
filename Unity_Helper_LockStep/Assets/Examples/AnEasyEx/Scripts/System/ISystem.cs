using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.EX_A
{
    public interface ISystem
    {
        void Start();
        void Update();
        void LogicUpdate();
    }

    public interface IClientSystem : ISystem 
    {
        void OnClientConnect();
        void OnClientDisconnect();
    }

    public interface IServerSystem : ISystem
    {
        void OnStartServer();
        void OnStopServer();
    }
}