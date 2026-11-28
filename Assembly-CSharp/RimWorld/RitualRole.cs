using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001706 RID: 5894
	public abstract class RitualRole : ILordJobRole, IExposable, ILoadReferenceable
	{
		// Token: 0x1700194D RID: 6477
		// (get) Token: 0x0600935B RID: 37723 RVA: 0x0035CDFF File Offset: 0x0035AFFF
		public virtual bool Animal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700194E RID: 6478
		// (get) Token: 0x0600935C RID: 37724 RVA: 0x0035CE02 File Offset: 0x0035B002
		public int MaxCount
		{
			get
			{
				return this.maxCount;
			}
		}

		// Token: 0x1700194F RID: 6479
		// (get) Token: 0x0600935D RID: 37725 RVA: 0x0035CE0A File Offset: 0x0035B00A
		public int MinCount
		{
			get
			{
				return (this.required > false) ? 1 : 0;
			}
		}

		// Token: 0x17001950 RID: 6480
		// (get) Token: 0x0600935E RID: 37726 RVA: 0x0035CE15 File Offset: 0x0035B015
		public TaggedString Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17001951 RID: 6481
		// (get) Token: 0x0600935F RID: 37727 RVA: 0x0035CE22 File Offset: 0x0035B022
		public TaggedString LabelCap
		{
			get
			{
				return this.label.CapitalizeFirst();
			}
		}

		// Token: 0x17001952 RID: 6482
		// (get) Token: 0x06009360 RID: 37728 RVA: 0x0035CE34 File Offset: 0x0035B034
		public TaggedString CategoryLabel
		{
			get
			{
				return this.categoryLabel;
			}
		}

		// Token: 0x17001953 RID: 6483
		// (get) Token: 0x06009361 RID: 37729 RVA: 0x0035CE41 File Offset: 0x0035B041
		public TaggedString CategoryLabelCap
		{
			get
			{
				return (this.categoryLabel ?? this.label).CapitalizeFirst();
			}
		}

		// Token: 0x06009362 RID: 37730 RVA: 0x0035CE60 File Offset: 0x0035B060
		public RitualRole()
		{
			if (Find.UniqueIDsManager != null && this.loadID == -1)
			{
				this.loadID = Find.UniqueIDsManager.GetNextRitualRoleID();
			}
		}

		// Token: 0x06009363 RID: 37731 RVA: 0x0035CEC0 File Offset: 0x0035B0C0
		public virtual bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			if (!this.AppliesIfChild(p, out reason, skipReason))
			{
				return false;
			}
			Ideo ideo = p.Ideo;
			Precept_Role role = (ideo != null) ? ideo.GetRole(p) : null;
			Precept_Ritual ritual2;
			if ((ritual2 = ((ritual != null) ? ritual.Ritual : null)) == null)
			{
				ritual2 = (((assignments != null) ? assignments.Ritual : null) ?? precept);
			}
			return this.AppliesToRole(role, out reason, ritual2, p, skipReason);
		}

		// Token: 0x06009364 RID: 37732 RVA: 0x0035CF20 File Offset: 0x0035B120
		public Precept_Role FindInstance(Ideo ideo)
		{
			foreach (Precept_Role precept_Role in ideo.RolesListForReading)
			{
				if (precept_Role.def == this.precept)
				{
					return precept_Role;
				}
			}
			return null;
		}

		// Token: 0x06009365 RID: 37733 RVA: 0x0035CF84 File Offset: 0x0035B184
		protected bool AppliesIfChild(Pawn p, out string reason, bool skipReason = false)
		{
			reason = null;
			if (ModsConfig.BiotechActive && !this.allowChild && p.DevelopmentalStage.Juvenile())
			{
				if (!skipReason)
				{
					reason = (this.customChildDisallowMessage ?? "MessageRitualRoleCannotBeChild".Translate(this.Label).CapitalizeFirst());
				}
				return false;
			}
			return true;
		}

		// Token: 0x06009366 RID: 37734
		public abstract bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn pawn = null, bool skipReason = false);

		// Token: 0x06009367 RID: 37735 RVA: 0x0035CFE4 File Offset: 0x0035B1E4
		protected virtual int PawnDesirability(Pawn pawn)
		{
			if (this.precept == null)
			{
				return 0;
			}
			Ideo ideo = pawn.Ideo;
			PreceptDef preceptDef;
			if (ideo == null)
			{
				preceptDef = null;
			}
			else
			{
				Precept_Role role = ideo.GetRole(pawn);
				preceptDef = ((role != null) ? role.def : null);
			}
			if (preceptDef == this.precept)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06009368 RID: 37736 RVA: 0x0035D01A File Offset: 0x0035B21A
		public virtual void OrderByDesirability(List<Pawn> pawns)
		{
			pawns.Sort((Pawn lhs, Pawn rhs) => this.PawnDesirability(rhs).CompareTo(this.PawnDesirability(lhs)));
		}

		// Token: 0x06009369 RID: 37737 RVA: 0x0035D02E File Offset: 0x0035B22E
		public virtual string ExtraInfoForDialog(IEnumerable<Pawn> selected)
		{
			return null;
		}

		// Token: 0x0600936A RID: 37738 RVA: 0x0035D034 File Offset: 0x0035B234
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<string>(ref this.missingDesc, "missingDesc", null, false);
			Scribe_Values.Look<string>(ref this.customChildDisallowMessage, "customChildDisallowMessage", null, false);
			Scribe_Values.Look<string>(ref this.id, "id", null, false);
			Scribe_Values.Look<int>(ref this.maxCount, "maxCount", 0, false);
			Scribe_Values.Look<bool>(ref this.required, "required", false, false);
			Scribe_Values.Look<bool>(ref this.substitutable, "substitutable", false, false);
			Scribe_Values.Look<bool>(ref this.countsAsParticipant, "countsAsParticipant", true, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", -1, false);
			Scribe_Values.Look<bool>(ref this.defaultForSelectedColonist, "defaultForSelectedColonist", false, false);
			Scribe_Defs.Look<PreceptDef>(ref this.precept, "precept");
			Scribe_Values.Look<string>(ref this.mergeId, "mergeId", null, false);
			Scribe_Values.Look<bool>(ref this.allowOtherIdeos, "allowOtherIdeos", false, false);
			Scribe_Values.Look<bool>(ref this.allowDowned, "allowDowned", false, false);
			Scribe_Values.Look<bool>(ref this.allowKeepLayingDown, "allowKeepLayingDown", false, false);
			Scribe_Values.Look<bool>(ref this.allowChild, "allowChild", false, false);
			Scribe_Values.Look<bool>(ref this.endJobOnRitualCleanup, "endJobOnRitualCleanup", false, false);
			Scribe_Values.Look<bool>(ref this.mustBeAbleToReachTarget, "mustBeAbleToReachTarget", false, false);
			Scribe_Values.Look<bool>(ref this.removeFromAssignmentsOnDeath, "removeFromAssignmentsOnDeath", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.loadID == -1)
			{
				this.loadID = Find.UniqueIDsManager.GetNextRitualRoleID();
			}
		}

		// Token: 0x0600936B RID: 37739 RVA: 0x0035D1B6 File Offset: 0x0035B3B6
		public string GetUniqueLoadID()
		{
			return "RitualRole_" + this.loadID.ToString();
		}

		// Token: 0x0400591F RID: 22815
		[MustTranslate]
		protected string label;

		// Token: 0x04005920 RID: 22816
		[MustTranslate]
		protected string categoryLabel;

		// Token: 0x04005921 RID: 22817
		[MustTranslate]
		public string missingDesc;

		// Token: 0x04005922 RID: 22818
		[MustTranslate]
		public string noCandidatesGizmoDesc;

		// Token: 0x04005923 RID: 22819
		[MustTranslate]
		public string customChildDisallowMessage;

		// Token: 0x04005924 RID: 22820
		[NoTranslate]
		public string id;

		// Token: 0x04005925 RID: 22821
		public PreceptDef precept;

		// Token: 0x04005926 RID: 22822
		public int maxCount;

		// Token: 0x04005927 RID: 22823
		[NoTranslate]
		public string mergeId;

		// Token: 0x04005928 RID: 22824
		public bool required;

		// Token: 0x04005929 RID: 22825
		public bool substitutable;

		// Token: 0x0400592A RID: 22826
		public bool ignoreBleeding;

		// Token: 0x0400592B RID: 22827
		public bool countsAsParticipant = true;

		// Token: 0x0400592C RID: 22828
		public bool addToLord = true;

		// Token: 0x0400592D RID: 22829
		public bool allowNonAggroMentalState;

		// Token: 0x0400592E RID: 22830
		public bool defaultForSelectedColonist;

		// Token: 0x0400592F RID: 22831
		public bool allowOtherIdeos;

		// Token: 0x04005930 RID: 22832
		public bool allowDowned;

		// Token: 0x04005931 RID: 22833
		public bool allowKeepLayingDown;

		// Token: 0x04005932 RID: 22834
		public bool allowChild = true;

		// Token: 0x04005933 RID: 22835
		public bool allowBaby;

		// Token: 0x04005934 RID: 22836
		public bool removeFromAssignmentsOnDeath = true;

		// Token: 0x04005935 RID: 22837
		public bool endJobOnRitualCleanup = true;

		// Token: 0x04005936 RID: 22838
		public bool mustBeAbleToReachTarget;

		// Token: 0x04005937 RID: 22839
		public bool blocksSocialInteraction;

		// Token: 0x04005938 RID: 22840
		private int loadID = -1;
	}
}
