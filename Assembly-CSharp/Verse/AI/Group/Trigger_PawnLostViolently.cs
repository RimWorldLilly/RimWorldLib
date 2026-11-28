using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200097B RID: 2427
	public class Trigger_PawnLostViolently : Trigger
	{
		// Token: 0x060049A0 RID: 18848 RVA: 0x001CAECE File Offset: 0x001C90CE
		public Trigger_PawnLostViolently(bool allowRoofCollapse = true)
		{
			this.allowRoofCollapse = allowRoofCollapse;
		}

		// Token: 0x060049A1 RID: 18849 RVA: 0x001CAEDD File Offset: 0x001C90DD
		public Trigger_PawnLostViolently(bool allowRoofCollapse, Faction ignoreDamageFromFaction)
		{
			this.allowRoofCollapse = allowRoofCollapse;
			this.ignoreDamageFromFaction = ignoreDamageFromFaction;
		}

		// Token: 0x060049A2 RID: 18850 RVA: 0x001CAEF4 File Offset: 0x001C90F4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnLost)
			{
				if (signal.dinfo.Instigator != null && this.ignoreDamageFromFaction != null && signal.dinfo.Instigator.Faction == this.ignoreDamageFromFaction)
				{
					return false;
				}
				if (signal.condition == PawnLostCondition.MadePrisoner)
				{
					return true;
				}
				if ((signal.condition == PawnLostCondition.Incapped || signal.condition == PawnLostCondition.Killed) && (signal.dinfo.Category != DamageInfo.SourceCategory.Collapse || this.allowRoofCollapse))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04002F41 RID: 12097
		public bool allowRoofCollapse;

		// Token: 0x04002F42 RID: 12098
		public Faction ignoreDamageFromFaction;
	}
}
