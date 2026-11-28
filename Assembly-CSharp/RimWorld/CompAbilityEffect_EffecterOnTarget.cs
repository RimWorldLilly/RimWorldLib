using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013F6 RID: 5110
	public class CompAbilityEffect_EffecterOnTarget : CompAbilityEffect
	{
		// Token: 0x170015B0 RID: 5552
		// (get) Token: 0x06007EC5 RID: 32453 RVA: 0x002D4E4F File Offset: 0x002D304F
		public new CompProperties_AbilityEffecterOnTarget Props
		{
			get
			{
				return (CompProperties_AbilityEffecterOnTarget)this.props;
			}
		}

		// Token: 0x06007EC6 RID: 32454 RVA: 0x002D4E5C File Offset: 0x002D305C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Effecter effecter;
			if (target.HasThing)
			{
				effecter = this.Props.effecterDef.Spawn(target.Thing, this.parent.pawn.Map, this.Props.scale);
			}
			else
			{
				effecter = this.Props.effecterDef.Spawn(target.Cell, this.parent.pawn.Map, this.Props.scale);
			}
			if (this.Props.maintainForTicks > 0)
			{
				this.parent.AddEffecterToMaintain(effecter, target.Cell, this.Props.maintainForTicks, null);
				return;
			}
			effecter.Cleanup();
		}
	}
}
