using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004B9 RID: 1209
	public class Hediff_Mechlink : HediffWithComps
	{
		// Token: 0x0600218E RID: 8590 RVA: 0x000C74C0 File Offset: 0x000C56C0
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (!ModLister.CheckBiotech("Mechlink"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			PawnComponentsUtility.AddAndRemoveDynamicComponents(this.pawn, false);
			if (this.pawn.Spawned)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelMechlinkInstalled".Translate() + ": " + this.pawn.LabelShortCap, "LetterMechlinkInstalled".Translate(this.pawn.Named("PAWN")), LetterDefOf.PositiveEvent, this.pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x000C7569 File Offset: 0x000C5769
		public override void PostRemoved()
		{
			base.PostRemoved();
			Pawn_MechanitorTracker mechanitor = this.pawn.mechanitor;
			if (mechanitor == null)
			{
				return;
			}
			mechanitor.Notify_MechlinkRemoved();
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x000C7586 File Offset: 0x000C5786
		public override void PostTickInterval(int delta)
		{
			base.PostTickInterval(delta);
			if (this.pawn.Spawned && this.pawn.IsHashIntervalTick(300, delta))
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.Mechanitors, OpportunityType.Important);
			}
		}

		// Token: 0x04001978 RID: 6520
		private const int LearningOpportunityCheckInterval = 300;
	}
}
