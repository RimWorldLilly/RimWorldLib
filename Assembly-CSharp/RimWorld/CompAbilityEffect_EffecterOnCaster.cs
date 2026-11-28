using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013F4 RID: 5108
	public class CompAbilityEffect_EffecterOnCaster : CompAbilityEffect
	{
		// Token: 0x170015AF RID: 5551
		// (get) Token: 0x06007EC1 RID: 32449 RVA: 0x002D4D84 File Offset: 0x002D2F84
		public new CompProperties_AbilityEffecterOnCaster Props
		{
			get
			{
				return (CompProperties_AbilityEffecterOnCaster)this.props;
			}
		}

		// Token: 0x06007EC2 RID: 32450 RVA: 0x002D4D94 File Offset: 0x002D2F94
		public override void PostApplied(List<LocalTargetInfo> targets, Map map)
		{
			base.PostApplied(targets, map);
			Effecter effecter = this.Props.effecterDef.Spawn(this.parent.pawn, map, this.Props.scale);
			if (this.Props.maintainTicks > 0)
			{
				map.effecterMaintainer.AddEffecterToMaintain(effecter, new TargetInfo(this.parent.pawn), this.parent.pawn, this.Props.maintainTicks);
				return;
			}
			effecter.Cleanup();
		}
	}
}
