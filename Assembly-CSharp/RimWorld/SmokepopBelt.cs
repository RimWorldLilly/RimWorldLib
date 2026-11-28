using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B5B RID: 7003
	public class SmokepopBelt : Apparel
	{
		// Token: 0x0600B3D9 RID: 46041 RVA: 0x003F6730 File Offset: 0x003F4930
		public override void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
			base.Notify_BulletImpactNearby(impactData);
			Pawn wearer = base.Wearer;
			if (wearer == null || wearer.Dead)
			{
				return;
			}
			if (!impactData.bullet.DamageDef.isExplosive && impactData.bullet.Launcher is Building_Turret && impactData.bullet.Launcher != null && impactData.bullet.Launcher.HostileTo(base.Wearer) && !wearer.IsColonist && wearer.Spawned && !wearer.Position.AnyGas(wearer.Map, GasType.BlindSmoke))
			{
				Verb_SmokePop.Pop(this.TryGetComp<CompApparelReloadable>());
			}
		}

		// Token: 0x0600B3DA RID: 46042 RVA: 0x003F67CF File Offset: 0x003F49CF
		public override float GetSpecialApparelScoreOffset()
		{
			return this.GetStatValue(StatDefOf.PackRadius, true, -1) * this.ApparelScorePerBeltRadius;
		}

		// Token: 0x040067B4 RID: 26548
		private float ApparelScorePerBeltRadius = 0.046f;
	}
}
