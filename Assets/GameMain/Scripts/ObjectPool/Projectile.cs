using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;

namespace ETLG
{
    public class Projectile : MonoBehaviour
    {
        [HideInInspector] public float destoryTime;
        [HideInInspector] public float flyingSpeed;
        [HideInInspector] public Vector3 flyingDirection;
        [HideInInspector] public int damage;
        protected Rigidbody rb;

        private void Awake() 
        {
            rb = GetComponent<Rigidbody>();
        }

        protected virtual void OnEnable() 
        {
            GameEntry.Event.Subscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Subscribe(GamePauseEventArgs.EventId, OnGamePause);
            GameEntry.Event.Subscribe(BattleWinEventArgs.EventId, OnBattleWin);
        }

        private void OnBattleWin(object sender, GameEventArgs e)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        private void OnGamePause(object sender, GameEventArgs e)
        {

        }

        private void OnPlayerDead(object sender, GameEventArgs e)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        protected IEnumerator ReturnToPoolAfterTime()
        {
            float elapsedTime = 0f;
            while (elapsedTime < destoryTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        private void OnDisable() 
        {
            StopAllCoroutines();
            GameEntry.Event.Unsubscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Unsubscribe(GamePauseEventArgs.EventId, OnGamePause);
            GameEntry.Event.Unsubscribe(BattleWinEventArgs.EventId, OnBattleWin);
        }
    }
}
