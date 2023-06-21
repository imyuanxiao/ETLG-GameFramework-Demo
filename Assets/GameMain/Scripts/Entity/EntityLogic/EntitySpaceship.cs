using UnityGameFramework.Runtime;
using UnityEngine;

namespace ETLG
{
    public class EntitySpaceship : EntityLogicEx
    {
        public EntityDataSpaceship EntityDataSpaceship
        {
            get;
            private set;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            EntityDataSpaceship = userData as EntityDataSpaceship;

            if (EntityDataSpaceship == null)
            {
                Log.Error("Entity spaceship '{0}' entity data invaild.", Id);
                return;
            }

            // Debug.Log(EntityDataSpaceship.SpaceshipData.EntityId);
            // Debug.Log(EntityDataSpaceship.SpaceshipData.Firepower);
            // Debug.Log(EntityDataSpaceship.SpaceshipData.Agility);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }
    }
}
