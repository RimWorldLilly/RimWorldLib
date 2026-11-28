using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A50 RID: 6736
	public class BroadshieldPack : Apparel
	{
		// Token: 0x0600A8D0 RID: 43216 RVA: 0x003BEFBC File Offset: 0x003BD1BC
		public override void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
			base.Notify_BulletImpactNearby(impactData);
			Pawn wearer = base.Wearer;
			if (wearer == null || wearer.Dead)
			{
				return;
			}
			if (!impactData.bullet.DamageDef.isExplosive && CompProjectileInterceptor.InterceptsProjectile(BroadshieldPack.BroadshieldProjectileInterceptorProperties(), impactData.bullet) && impactData.bullet.Launcher != null && impactData.bullet.Launcher.HostileTo(base.Wearer) && !wearer.IsColonist && wearer.Spawned && !this.NearbyActiveBroadshield())
			{
				Verb_DeployBroadshield.Deploy(this.TryGetComp<CompApparelReloadable>());
			}
		}

		// Token: 0x0600A8D1 RID: 43217 RVA: 0x003BF050 File Offset: 0x003BD250
		private bool NearbyActiveBroadshield()
		{
			float radius = BroadshieldPack.BroadshieldProjectileInterceptorProperties().radius;
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(base.PositionHeld, base.MapHeld, radius, true))
			{
				CompProjectileInterceptor compProjectileInterceptor = thing.TryGetComp<CompProjectileInterceptor>();
				if (thing.def == ThingDefOf.BroadshieldProjector && compProjectileInterceptor != null && compProjectileInterceptor.Active)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A8D2 RID: 43218 RVA: 0x003BF0D4 File Offset: 0x003BD2D4
		private static CompProperties_ProjectileInterceptor BroadshieldProjectileInterceptorProperties()
		{
			return ThingDefOf.BroadshieldProjector.GetCompProperties<CompProperties_ProjectileInterceptor>();
		}
	}
}
