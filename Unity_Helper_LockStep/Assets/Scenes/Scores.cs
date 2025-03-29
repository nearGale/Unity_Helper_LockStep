using UnityEngine;
using Mirror;

public class Scores : MonoBehaviour
{
    public struct ScoreMessage : NetworkMessage
    {
        public int score;
        public Vector3 scorePos;
        public int lives;
    }

    public void SendScore(int score, Vector3 scorePos, int lives)
    {
        ScoreMessage msg = new ScoreMessage()
        {
            score = score,
            scorePos = scorePos,
            lives = lives
        };

        NetworkServer.SendToAll(msg);
    }

    public void SetupClient()
    {
        NetworkClient.RegisterHandler<ScoreMessage>(OnScore);
        NetworkClient.Connect("localhost");
    }

    public void OnScore(ScoreMessage msg)
    {
        Debug.Log("OnScoreMessage " + msg.score);
    }
}