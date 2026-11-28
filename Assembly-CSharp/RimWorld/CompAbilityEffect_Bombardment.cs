using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013DB RID: 5083
	public class CompAbilityEffect_Bombardment : CompAbilityEffect_WithDest
	{
		// Token: 0x170015A2 RID: 5538
		// (get) Token: 0x06007E72 RID: 32370 RVA: 0x002D2EF0 File Offset: 0x002D10F0
		public new CompProperties_Bombardment Props
		{
			get
			{
				return (CompProperties_Bombardment)this.props;
			}
		}

		// Token: 0x06007E73 RID: 32371 RVA: 0x002D2F00 File Offset: 0x002D1100
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Bombardment bombardment = (Bombardment)GenSpawn.Spawn(ThingDefOf.Bombardment, target.Cell, this.parent.pawn.Map, WipeMode.Vanish);
			bombardment.duration = this.Props.durationTicks;
			bombardment.instigator = this.parent.pawn;
			bombardment.StartStrike();
		}
	}
}
