using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000880 RID: 2176
	public class MentalState_MurderousRage : MentalState
	{
		// Token: 0x060043E0 RID: 17376 RVA: 0x001AAC6C File Offset: 0x001A8E6C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
		}

		// Token: 0x060043E1 RID: 17377 RVA: 0x001AAC85 File Offset: 0x001A8E85
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x060043E2 RID: 17378 RVA: 0x001AAC88 File Offset: 0x001A8E88
		public override void PreStart()
		{
			base.PreStart();
			this.TryFindNewTarget();
		}

		// Token: 0x060043E3 RID: 17379 RVA: 0x001AAC98 File Offset: 0x001A8E98
		public override void MentalStateTick(int delta)
		{
			base.MentalStateTick(delta);
			if (this.target != null && this.target.Dead)
			{
				base.RecoverFromState();
			}
			if (this.pawn.IsHashIntervalTick(120, delta) && !this.IsTargetStillValidAndReachable())
			{
				if (!this.TryFindNewTarget())
				{
					base.RecoverFromState();
					return;
				}
				Messages.Message("MessageMurderousRageChangedTarget".Translate(this.pawn.NameShortColored, this.target.Label, this.pawn.Named("PAWN"), this.target.Named("TARGET")).Resolve().AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NegativeEvent, true);
				base.MentalStateTick(delta);
			}
		}

		// Token: 0x060043E4 RID: 17380 RVA: 0x001AAD74 File Offset: 0x001A8F74
		public override TaggedString GetBeginLetterText()
		{
			if (this.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.");
				return "";
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored, this.target.NameShortColored, this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true).Resolve().CapitalizeFirst();
		}

		// Token: 0x060043E5 RID: 17381 RVA: 0x001AAE14 File Offset: 0x001A9014
		private bool TryFindNewTarget()
		{
			this.target = MurderousRageMentalStateUtility.FindPawnToKill(this.pawn);
			return this.target != null;
		}

		// Token: 0x060043E6 RID: 17382 RVA: 0x001AAE30 File Offset: 0x001A9030
		public bool IsTargetStillValidAndReachable()
		{
			return this.target != null && this.target.SpawnedParentOrMe != null && (!(this.target.SpawnedParentOrMe is Pawn) || this.target.SpawnedParentOrMe == this.target) && this.pawn.CanReach(this.target.SpawnedParentOrMe, PathEndMode.Touch, Danger.Deadly, true, false, TraverseMode.ByPawn);
		}

		// Token: 0x04002C2A RID: 11306
		public Pawn target;

		// Token: 0x04002C2B RID: 11307
		private const int NoLongerValidTargetCheckInterval = 120;
	}
}
