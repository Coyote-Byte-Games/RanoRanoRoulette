using UnityEngine;

internal interface IEnemy
{
    public int GetDamage();
    public float GetKB();
    public void OnCollisionEnter2D(Collision2D other);
    void TakeDamage(int v);
}