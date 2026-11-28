using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200088A RID: 2186
	public class MentalState_TargetedTantrum : MentalState_Tantrum
	{
		// Token: 0x06004415 RID: 17429 RVA: 0x001AB448 File Offset: 0x001A9648
		public override void MentalStateTick(int delta)
		{
			if (this.target == null || this.target.Destroyed)
			{
				base.RecoverFromState();
				return;
			}
			if (this.target.Spawned && this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				base.MentalStateTick(delta);
				return;
			}
			Thing target = this.target;
			if (!this.TryFindNewTarget())
			{
				base.RecoverFromState();
				return;
			}
			Messages.Message("MessageTargetedTantrumChangedTarget".Translate(this.pawn.LabelShort, target.Label, this.target.Label, this.pawn.Named("PAWN"), target.Named("OLDTARGET"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NegativeEvent, true);
			base.MentalStateTick(delta);
		}

		// Token: 0x06004416 RID: 17430 RVA: 0x001AB552 File Offset: 0x001A9752
		public override void PreStart()
		{
			base.PreStart();
			this.TryFindNewTarget();
		}

		// Token: 0x06004417 RID: 17431 RVA: 0x001AB564 File Offset: 0x001A9764
		private bool TryFindNewTarget()
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, MentalState_TargetedTantrum.tmpThings, null, 300, 40);
			bool result = MentalState_TargetedTantrum.tmpThings.TryRandomElementByWeight((Thing x) => x.MarketValue * (float)x.stackCount, out this.target);
			MentalState_TargetedTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x06004418 RID: 17432 RVA: 0x001AB5D0 File Offset: 0x001A97D0
		public override TaggedString GetBeginLetterText()
		{
			if (this.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.");
				return "";
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored, this.target.Label, this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true).Resolve().CapitalizeFirst();
		}

		// Token: 0x04002C39 RID: 11321
		public const int MinMarketValue = 300;

		// Token: 0x04002C3A RID: 11322
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
