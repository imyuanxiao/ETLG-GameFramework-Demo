using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public interface IDamageable
    {
        public void TakeDamage(int damage);
        public bool IsDead();
    }
}
