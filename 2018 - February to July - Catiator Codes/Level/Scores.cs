using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Scores : MonoBehaviour
{
    [SerializeField] private GameObject prefabScore;
    private List<Score> scores = new List<Score>();
    private Score winner;
    private Transform winnerScore;

    [SerializeField] private Sprite ranger, berserker;

    private bool tie = false;
    public bool HasTie
    {
        get { return tie; }
        private set { tie = value; }
    }

    public void SetScores(Transform parentCharacter)
    {
        Transform hasWon = parentCharacter;

        foreach (Transform child in parentCharacter)
        {
            string player = "";
            string character = "";
            int kills = 0;
            Color clr = Color.white;

            BasicMovement movement = child.GetComponent<BasicMovement>();
            if(movement != null)
            {
                movement.canMove = false;
                movement.endgame = true;

                player = movement.player;
                character = movement.character;
                clr = movement.clr;
            }

            PlayerManager manager = child.GetComponent<PlayerManager>();
            if (manager != null)
            {
                kills = manager.HasKilled;
            }
            
            Score s = new Score(player, character, 0, kills, clr, null);

            Animator anim = movement.CharacterAnimator;
            Debug.Log(anim);
            if (anim != null)
            {
                anim.SetBool("Game", false);
                anim.SetBool("Running", false);
            }

            if (winner == null || winner.kills < s.kills)
            {
                winner = s;
                hasWon = child;
                HasTie = false;
                if (anim != null) anim.SetBool("Dance", true);
            }
            else if (winner.kills == s.kills)
            {
                HasTie = true;
                if (anim != null) anim.SetBool("Dance", true);
            }

            s.characterPrefab = child.gameObject;
            scores.Add(s);
        }

        if (!HasTie)
            EndLevelCamera.cam.SetDestination(hasWon);
        else
            EndLevelCamera.cam.NoMove();
    }

    public string ShowScores(Transform winnerPosition, Transform loserPosition)
    {
        foreach (Transform child in winnerPosition)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Transform child in loserPosition)
        {
            GameObject.Destroy(child.gameObject);
        }

        scores = scores.OrderByDescending(x => x.kills).ToList();

        string title = "";
        int nr = 0;
        Score last = new Score("", "", 0, 0, Color.white, null);

        foreach(Score s in scores)
        {
            bool haswon = (nr == 0 && !HasTie);

            Transform parent = loserPosition;
            if (haswon) parent = winnerPosition;

            Transform scoreObj = Instantiate(prefabScore, parent).transform;
            ScoreBells bells = scoreObj.GetComponent<ScoreBells>();
            if(bells != null)
            {
                bells.SetBackground(s.clr);
                bells.SetBells(s.kills);

                if (nr == 0)
                {
                    last = s;
                    nr++;
                }
                else if (last.kills != s.kills)
                    nr++;

                bells.SetPosition(nr);
                bells.SetPlayer(haswon, s.characterPrefab);
            }
        }

        if (HasTie) title += "It has become a tie!";
        else title += scores[0].player + " player wins!";

        return title;
    }

    public class Score
    {
        public string player;
        public string character;
        public int deaths;
        public int kills;
        public Color clr;
        public GameObject characterPrefab;

        public Score(string _player, string _character, int _deaths, int _kills, Color _clr, GameObject prefab)
        {
            player = _player;
            character = _character;
            deaths = _deaths;
            kills = _kills;
            clr = _clr;
            
            characterPrefab = prefab;
        }
    }

}



