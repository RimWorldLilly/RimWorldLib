using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200087E RID: 2174
	public class MentalState_IdeoChange : MentalState
	{
		// Token: 0x060043D7 RID: 17367 RVA: 0x001AA8FC File Offset: 0x001A8AFC
		public override void PreStart()
		{
			base.PreStart();
			this.oldIdeo = this.pawn.Ideo;
			this.oldRole = this.oldIdeo.GetRole(this.pawn);
			this.newIdeo = Find.IdeoManager.IdeosListForReading.RandomElementByWeight((Ideo x) => IdeoUtility.IdeoChangeToWeight(this.pawn, x));
			if (this.pawn.ideo.IdeoConversionAttempt(0.5f, this.newIdeo, false))
			{
				this.changedIdeo = true;
			}
			this.newCertainty = this.pawn.ideo.Certainty;
		}

		// Token: 0x060043D8 RID: 17368 RVA: 0x001AA994 File Offset: 0x001A8B94
		public override TaggedString GetBeginLetterText()
		{
			TaggedString taggedString = this.def.beginLetter.Formatted(this.pawn.LabelShort, this.pawn.Named("PAWN"), this.pawn.Ideo.Named("NEWIDEO"), this.oldIdeo.Named("OLDIDEO")).CapitalizeFirst() + "\n\n";
			if (this.changedIdeo)
			{
				taggedString += "LetterIdeoChangeConverted".Translate(this.pawn.Named("PAWN"), this.newIdeo.Named("NEWIDEO"), this.oldIdeo.Named("OLDIDEO")).CapitalizeFirst();
			}
			else
			{
				taggedString += "LetterIdeoChangeNotConverted".Translate(this.pawn.Named("PAWN"), this.oldIdeo.Named("OLDIDEO"), this.newCertainty.ToStringPercent().Named("NEWCERTAINTY")).CapitalizeFirst();
			}
			MentalStateDef wanderToOwnRoomStateOrFallback = MentalStateUtility.GetWanderToOwnRoomStateOrFallback(this.pawn);
			if (wanderToOwnRoomStateOrFallback == MentalStateDefOf.Wander_OwnRoom)
			{
				taggedString += "\n\n" + "LetterIdeoChangedWanderOwnRoom".Translate(this.pawn.Named("PAWN"));
			}
			else if (wanderToOwnRoomStateOrFallback == MentalStateDefOf.Wander_Sad)
			{
				taggedString += "\n\n" + "LetterIdeoChangedSadWander".Translate(this.pawn.Named("PAWN"));
			}
			if (this.changedIdeo && this.oldRole != null)
			{
				taggedString += "\n\n" + "LetterRoleLostLetterIdeoChangedPostfix".Translate(this.pawn.Named("PAWN"), this.oldRole.Named("ROLE"), this.oldIdeo.Named("OLDIDEO"));
			}
			return taggedString;
		}

		// Token: 0x060043D9 RID: 17369 RVA: 0x001AAB7A File Offset: 0x001A8D7A
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			MentalStateUtility.TryTransitionToWanderOwnRoom(this);
		}

		// Token: 0x060043DA RID: 17370 RVA: 0x001AAB8C File Offset: 0x001A8D8C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Ideo>(ref this.oldIdeo, "oldIdeo", false);
			Scribe_References.Look<Ideo>(ref this.newIdeo, "newIdeo", false);
			Scribe_References.Look<Precept_Role>(ref this.oldRole, "oldRole", false);
			Scribe_Values.Look<bool>(ref this.changedIdeo, "changedIdeo", false, false);
			Scribe_Values.Look<float>(ref this.newCertainty, "newCertainty", 0f, false);
		}

		// Token: 0x04002C23 RID: 11299
		private Ideo oldIdeo;

		// Token: 0x04002C24 RID: 11300
		private Ideo newIdeo;

		// Token: 0x04002C25 RID: 11301
		private Precept_Role oldRole;

		// Token: 0x04002C26 RID: 11302
		private bool changedIdeo;

		// Token: 0x04002C27 RID: 11303
		private float newCertainty;

		// Token: 0x04002C28 RID: 11304
		private const float ConversionCertaintyReduction = 0.5f;
	}
}
