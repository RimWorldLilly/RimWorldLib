using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B44 RID: 6980
	public class FlameThrower : Projectile
	{
		// Token: 0x0600B344 RID: 45892 RVA: 0x003F2E68 File Offset: 0x003F1068
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			IntVec3 position = base.Position;
			Map map = base.Map;
			base.Impact(hitThing, blockedByShield);
			Find.BattleLog.Add(new BattleLogEntry_RangedImpact(this.launcher, hitThing, this.intendedTarget.Thing, this.equipmentDef, this.def, this.targetCoverDef));
			if (hitThing != null)
			{
				Pawn pawn = hitThing as Pawn;
				if (pawn == null || Rand.Chance(FireUtility.ChanceToAttachFireFromEvent(pawn)))
				{
					hitThing.TryAttachFire(FlameThrower.FireSizeRange.RandomInRange, this.launcher);
				}
			}
			FireUtility.TryStartFireIn(position, map, FlameThrower.FireSizeRange.RandomInRange, this.launcher, null);
			if (Rand.Chance(FlameThrower.ChanceToMakeFilth))
			{
				FilthMaker.TryMakeFilth(position, map, ThingDefOf.Filth_Fuel, 1, FilthSourceFlags.None, true);
			}
		}

		// Token: 0x0400676F RID: 26479
		private static FloatRange FireSizeRange = new FloatRange(0.55f, 0.85f);

		// Token: 0x04006770 RID: 26480
		private static float ChanceToMakeFilth = 0.25f;
	}
}
