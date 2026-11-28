using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001FEF RID: 8175
	public class Verb_PowerBeam : Verb_CastBase
	{
		// Token: 0x0600CF41 RID: 53057 RVA: 0x004C56D4 File Offset: 0x004C38D4
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			PowerBeam powerBeam = (PowerBeam)GenSpawn.Spawn(ThingDefOf.PowerBeam, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
			powerBeam.duration = 600;
			powerBeam.instigator = this.caster;
			powerBeam.weaponDef = ((base.EquipmentSource != null) ? base.EquipmentSource.def : null);
			powerBeam.StartStrike();
			CompApparelReloadable reloadableCompSource = base.ReloadableCompSource;
			if (reloadableCompSource != null)
			{
				reloadableCompSource.UsedOnce();
			}
			return true;
		}

		// Token: 0x0600CF42 RID: 53058 RVA: 0x004C577D File Offset: 0x004C397D
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 15f;
		}

		// Token: 0x040083C5 RID: 33733
		private const int DurationTicks = 600;
	}
}
