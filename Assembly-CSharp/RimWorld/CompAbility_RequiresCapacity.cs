using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001471 RID: 5233
	public class CompAbility_RequiresCapacity : AbilityComp
	{
		// Token: 0x17001605 RID: 5637
		// (get) Token: 0x0600806E RID: 32878 RVA: 0x002DCCB6 File Offset: 0x002DAEB6
		public CompProperties_AbilityRequiresCapacity Props
		{
			get
			{
				return (CompProperties_AbilityRequiresCapacity)this.props;
			}
		}

		// Token: 0x0600806F RID: 32879 RVA: 0x002DCCC4 File Offset: 0x002DAEC4
		public override bool GizmoDisabled(out string reason)
		{
			if (!this.parent.pawn.health.capacities.CapableOf(this.Props.capacity))
			{
				reason = "AbilityDisabledNoCapacity".Translate(this.parent.pawn, this.Props.capacity.label);
				return true;
			}
			reason = null;
			return false;
		}
	}
}
