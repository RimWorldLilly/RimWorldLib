using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001455 RID: 5205
	public class CompAbilityEffect_StartTrial : CompAbilityEffect_StartRitualOnPawn
	{
		// Token: 0x170015E9 RID: 5609
		// (get) Token: 0x06007FF5 RID: 32757 RVA: 0x002DA4B6 File Offset: 0x002D86B6
		public new CompProperties_AbilityStartTrial Props
		{
			get
			{
				return (CompProperties_AbilityStartTrial)this.props;
			}
		}

		// Token: 0x06007FF6 RID: 32758 RVA: 0x002DA4C4 File Offset: 0x002D86C4
		protected override Precept_Ritual RitualForTarget(Pawn pawn)
		{
			PreceptDef preceptDef = pawn.InMentalState ? this.Props.ritualDefForMentalState : (pawn.IsPrisonerOfColony ? this.Props.ritualDefForPrisoner : null);
			if (preceptDef != null)
			{
				for (int i = 0; i < this.parent.pawn.Ideo.PreceptsListForReading.Count; i++)
				{
					if (this.parent.pawn.Ideo.PreceptsListForReading[i].def == preceptDef)
					{
						return (Precept_Ritual)this.parent.pawn.Ideo.PreceptsListForReading[i];
					}
				}
			}
			return base.RitualForTarget(pawn);
		}

		// Token: 0x06007FF7 RID: 32759 RVA: 0x002DA570 File Offset: 0x002D8770
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && AbilityUtility.ValidateCanWalk(pawn, throwMessages, this.parent) && AbilityUtility.ValidateNotGuilty(pawn, throwMessages, this.parent) && base.Valid(target, throwMessages);
		}
	}
}
