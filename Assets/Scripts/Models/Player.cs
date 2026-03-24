using System;

namespace models
{
    [Serializable]
    public class Player
    {
        public int id;
        public string nick;
        public int score;

        // Constructor que inicializa el jugador con nick y puntuación
        public Player (string _nick, int _score)
        {
            this.nick = _nick;
            this.score = _score;
        }
    }
}