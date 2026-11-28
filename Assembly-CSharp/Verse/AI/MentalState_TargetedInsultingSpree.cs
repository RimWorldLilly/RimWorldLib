using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000862 RID: 2146
	public class MentalState_TargetedInsultingSpree : MentalState_InsultingSpree
	{
		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x06004355 RID: 17237 RVA: 0x001A8E0F File Offset: 0x001A700F
		public override string InspectLine
		{
			get
			{
				return string.Format(this.def.baseInspectLine, this.target.LabelShort);
			}
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06004356 RID: 17238 RVA: 0x001A8E2C File Offset: 0x001A702C
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return this.insultedTargetAtLeastOnce;
			}
		}

		// Token: 0x06004357 RID: 17239 RVA: 0x001A8E34 File Offset: 0x001A7034
		public override void MentalStateTick(int delta)
		{
			if (this.target != null && (!this.target.Spawned || !this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) || !this.target.Awake()))
			{
				Pawn target = this.target;
				if (!this.TryFindNewTarget())
				{
					base.RecoverFromState();
					return;
				}
				Messages.Message("MessageTargetedInsultingSpreeChangedTarget".Translate(this.pawn.LabelShort, target.Label, this.target.Label, this.pawn.Named("PAWN"), target.Named("OLDTARGET"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NegativeEvent, true);
				base.MentalStateTick(delta);
				return;
			}
			else
			{
				if (this.target == null || !InsultingSpreeMentalStateUtility.CanChaseAndInsult(this.pawn, this.target, false, false))
				{
					base.RecoverFromState();
					return;
				}
				base.MentalStateTick(delta);
				return;
			}
		}

		// Token: 0x06004358 RID: 17240 RVA: 0x001A8F5E File Offset: 0x001A715E
		public override void PreStart()
		{
			base.PreStart();
			this.TryFindNewTarget();
		}

		// Token: 0x06004359 RID: 17241 RVA: 0x001A8F6D File Offset: 0x001A716D
		private bool TryFindNewTarget()
		{
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(this.pawn, MentalState_TargetedInsultingSpree.candidates, false);
			bool result = MentalState_TargetedInsultingSpree.candidates.TryRandomElement(out this.target);
			MentalState_TargetedInsultingSpree.candidates.Clear();
			return result;
		}

		// Token: 0x0600435A RID: 17242 RVA: 0x001A8F9C File Offset: 0x001A719C
		public override void PostEnd()
		{
			base.PostEnd();
			if (this.target != null && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageNoLongerOnTargetedInsultingSpree".Translate(this.pawn.LabelShort, this.target.Label, this.pawn.Named("PAWN"), this.target.Named("TARGET")), this.pawn, MessageTypeDefOf.SituationResolved, true);
			}
		}

		// Token: 0x0600435B RID: 17243 RVA: 0x001A902C File Offset: 0x001A722C
		public override TaggedString GetBeginLetterText()
		{
			if (this.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.");
				return "";
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored, this.target.NameShortColored, this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true).Resolve().CapitalizeFirst();
		}

		// Token: 0x04002BFA RID: 11258
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
