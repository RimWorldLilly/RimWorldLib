using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001FE2 RID: 8162
	public class Verb_Bombardment : Verb_CastBase
	{
		// Token: 0x0600CF02 RID: 52994 RVA: 0x004C4248 File Offset: 0x004C2448
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			Bombardment bombardment = (Bombardment)GenSpawn.Spawn(ThingDefOf.Bombardment, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
			bombardment.duration = 540;
			bombardment.instigator = this.caster;
			bombardment.weaponDef = ((base.EquipmentSource != null) ? base.EquipmentSource.def : null);
			CompApparelReloadable reloadableCompSource = base.ReloadableCompSource;
			if (reloadableCompSource != null)
			{
				reloadableCompSource.UsedOnce();
			}
			return true;
		}

		// Token: 0x0600CF03 RID: 52995 RVA: 0x004C42EB File Offset: 0x004C24EB
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 23f;
		}

		// Token: 0x040083BF RID: 33727
		public const int DurationTicks = 540;
	}
}
