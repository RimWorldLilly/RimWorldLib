using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001427 RID: 5159
	public class CompAbilityEffect_PowerBeam : CompAbilityEffect_WithDest
	{
		// Token: 0x170015CA RID: 5578
		// (get) Token: 0x06007F64 RID: 32612 RVA: 0x002D81CE File Offset: 0x002D63CE
		public new CompProperties_PowerBeam Props
		{
			get
			{
				return (CompProperties_PowerBeam)this.props;
			}
		}

		// Token: 0x06007F65 RID: 32613 RVA: 0x002D81DC File Offset: 0x002D63DC
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			PowerBeam powerBeam = (PowerBeam)GenSpawn.Spawn(ThingDefOf.PowerBeam, target.Cell, this.parent.pawn.Map, WipeMode.Vanish);
			powerBeam.duration = this.Props.durationTicks;
			powerBeam.instigator = this.parent.pawn;
			powerBeam.StartStrike();
		}
	}
}
