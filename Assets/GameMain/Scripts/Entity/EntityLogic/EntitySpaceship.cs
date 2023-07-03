using UnityGameFramework.Runtime;
using UnityEngine;
using GameFramework.Fsm;

namespace ETLG
{
    public class EntitySpaceship : EntityLogicEx
    {
        private IFsm<EntitySpaceship> m_Fsm = null;

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

            // m_Fsm = GameEntry.Fsm.CreateFsm("SpaceshipFsm", this, new ElectronicWarfare(), new FireWall(), new Medicalsupport());

            // Debug.Log(EntityDataSpaceship.SpaceshipData.EntityId);
            // Debug.Log(EntityDataSpaceship.SpaceshipData.Firepower);
            // Debug.Log(EntityDataSpaceship.SpaceshipData.Agility);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            // GameEntry.Fsm.DestroyFsm<EntitySpaceship>("SpaceshipFsm");
            m_Fsm = null;
        }
    }
}
