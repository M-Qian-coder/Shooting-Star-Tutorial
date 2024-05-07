using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class Enemy : Character
{
    [SerializeField]int scorePoint = 100;
    [SerializeField]int deathBonus = 3;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.Die();
            Die();
        }
    }
    public override void Die()
    {
        ScoreManager.Instance.AddScore(scorePoint);
        PlayerEnergy.Instance.Obtain(deathBonus);
        EnemyManager.Instance.RemoveFromList(gameObject);
        
        base.Die();
    }
}
