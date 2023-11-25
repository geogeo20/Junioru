using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] private TMP_Text rankLabel;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private TMP_Text messageLabel;
    [SerializeField] private TMP_Text scoreLabel;

    public void Init(int rank, string name, int score)
    {
        rankLabel.text = string.Format("#{0}", rank + 1);
        nameLabel.text = name;
        scoreLabel.text = score.ToString();
    }
    
    public void SetMessage(string message)
    {
        messageLabel.text = message;
    }
}