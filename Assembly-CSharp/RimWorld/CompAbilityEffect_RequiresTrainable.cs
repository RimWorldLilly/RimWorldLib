using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001439 RID: 5177
	public class CompAbilityEffect_RequiresTrainable : CompAbilityEffect
	{
		// Token: 0x170015D5 RID: 5589
		// (get) Token: 0x06007F95 RID: 32661 RVA: 0x002D8C7D File Offset: 0x002D6E7D
		public new CompProperties_AbilityRequiresTrainable Props
		{
			get
			{
				return (CompProperties_AbilityRequiresTrainable)this.props;
			}
		}

		// Token: 0x170015D6 RID: 5590
		// (get) Token: 0x06007F96 RID: 32662 RVA: 0x002D8C8C File Offset: 0x002D6E8C
		private bool HasLearnedTrainable
		{
			get
			{
				Pawn pawn = this.parent.pawn;
				bool? flag;
				if (pawn == null)
				{
					flag = null;
				}
				else
				{
					Pawn_TrainingTracker training = pawn.training;
					flag = ((training != null) ? new bool?(training.HasLearned(this.Props.trainableDef)) : null);
				}
				bool? flag2 = flag;
				return flag2.GetValueOrDefault();
			}
		}

		// Token: 0x170015D7 RID: 5591
		// (get) Token: 0x06007F97 RID: 32663 RVA: 0x002D8CE4 File Offset: 0x002D6EE4
		public override bool ShouldHideGizmo
		{
			get
			{
				return !this.HasLearnedTrainable;
			}
		}

		// Token: 0x06007F98 RID: 32664 RVA: 0x002D8CEF File Offset: 0x002D6EEF
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return !this.HasLearnedTrainable && this.Props.aiCanCastWithoutTrainable;
		}
	}
}
