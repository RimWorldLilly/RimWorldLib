using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001420 RID: 5152
	public class CompAbilityEffect_MustBeCapableOf : CompAbilityEffect
	{
		// Token: 0x170015C7 RID: 5575
		// (get) Token: 0x06007F4D RID: 32589 RVA: 0x002D78AF File Offset: 0x002D5AAF
		public new CompProperties_AbilityMustBeCapableOf Props
		{
			get
			{
				return (CompProperties_AbilityMustBeCapableOf)this.props;
			}
		}

		// Token: 0x06007F4E RID: 32590 RVA: 0x002D78BC File Offset: 0x002D5ABC
		public override bool GizmoDisabled(out string reason)
		{
			foreach (WorkTags workTags in this.Props.workTags.GetAllSelectedItems<WorkTags>())
			{
				if (this.parent.pawn.WorkTagIsDisabled(workTags))
				{
					reason = "AbilityDisabled_IncapableOfWorkTag".Translate(this.parent.pawn.Named("PAWN"), workTags.LabelTranslated());
					return true;
				}
			}
			reason = null;
			return false;
		}
	}
}
