using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B40 RID: 6976
	public class Beam : Bullet
	{
		// Token: 0x1700206D RID: 8301
		// (get) Token: 0x0600B339 RID: 45881 RVA: 0x003F295C File Offset: 0x003F0B5C
		public override Vector3 ExactPosition
		{
			get
			{
				return this.destination + Vector3.up * this.def.Altitude;
			}
		}

		// Token: 0x0600B33A RID: 45882 RVA: 0x003F2980 File Offset: 0x003F0B80
		public override void Launch(Thing launcher, Vector3 origin, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, bool preventFriendlyFire = false, Thing equipment = null, ThingDef targetCoverDef = null)
		{
			base.Launch(launcher, origin, usedTarget, intendedTarget, hitFlags, preventFriendlyFire, equipment, targetCoverDef);
			Vector3 offsetA = (this.ExactPosition - launcher.Position.ToVector3Shifted()).Yto0().normalized * this.def.projectile.beamStartOffset;
			if (this.def.projectile.beamMoteDef != null)
			{
				MoteMaker.MakeInteractionOverlay(this.def.projectile.beamMoteDef, launcher, usedTarget.ToTargetInfo(base.Map), offsetA, Vector3.zero);
			}
			this.ImpactSomething();
		}
	}
}
