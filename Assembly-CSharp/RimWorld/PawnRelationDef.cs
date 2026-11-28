using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FBA RID: 4026
	public class PawnRelationDef : Def
	{
		// Token: 0x17001187 RID: 4487
		// (get) Token: 0x06006649 RID: 26185 RVA: 0x002456C7 File Offset: 0x002438C7
		public PawnRelationWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnRelationWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x0600664A RID: 26186 RVA: 0x002456F9 File Offset: 0x002438F9
		public string GetGenderSpecificLabel(Pawn pawn)
		{
			if (pawn.gender == Gender.Female && !this.labelFemale.NullOrEmpty())
			{
				return this.labelFemale;
			}
			return this.label;
		}

		// Token: 0x0600664B RID: 26187 RVA: 0x0024571E File Offset: 0x0024391E
		public string GetGenderSpecificLabelCap(Pawn pawn)
		{
			return this.GetGenderSpecificLabel(pawn).CapitalizeFirst();
		}

		// Token: 0x0600664C RID: 26188 RVA: 0x0024572C File Offset: 0x0024392C
		public ThoughtDef GetGenderSpecificDiedThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.diedThoughtFemale != null)
			{
				return this.diedThoughtFemale;
			}
			return this.diedThought;
		}

		// Token: 0x0600664D RID: 26189 RVA: 0x0024574C File Offset: 0x0024394C
		public ThoughtDef GetGenderSpecificLostThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.diedThoughtFemale != null)
			{
				return this.lostThoughtFemale;
			}
			return this.lostThought;
		}

		// Token: 0x0600664E RID: 26190 RVA: 0x0024576C File Offset: 0x0024396C
		public ThoughtDef GetGenderSpecificKilledThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.killedThoughtFemale != null)
			{
				return this.killedThoughtFemale;
			}
			return this.killedThought;
		}

		// Token: 0x0600664F RID: 26191 RVA: 0x0024578C File Offset: 0x0024398C
		public ThoughtDef GetGenderSpecificThought(Pawn pawn, PawnDiedOrDownedThoughtsKind thoughtsKind)
		{
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Lost)
			{
				return this.GetGenderSpecificLostThought(pawn);
			}
			return this.GetGenderSpecificDiedThought(pawn);
		}

		// Token: 0x06006650 RID: 26192 RVA: 0x002457A1 File Offset: 0x002439A1
		public override IEnumerable<string> ConfigErrors()
		{
			PawnRelationDef.<ConfigErrors>d__29 <ConfigErrors>d__ = new PawnRelationDef.<ConfigErrors>d__29(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003EA0 RID: 16032
		public Type workerClass = typeof(PawnRelationWorker);

		// Token: 0x04003EA1 RID: 16033
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04003EA2 RID: 16034
		public float importance;

		// Token: 0x04003EA3 RID: 16035
		public bool implied;

		// Token: 0x04003EA4 RID: 16036
		public bool reflexive;

		// Token: 0x04003EA5 RID: 16037
		public int opinionOffset;

		// Token: 0x04003EA6 RID: 16038
		public float generationChanceFactor;

		// Token: 0x04003EA7 RID: 16039
		public float romanceChanceFactor = 1f;

		// Token: 0x04003EA8 RID: 16040
		public float incestOpinionOffset;

		// Token: 0x04003EA9 RID: 16041
		public bool familyByBloodRelation;

		// Token: 0x04003EAA RID: 16042
		public bool removeOnDeath;

		// Token: 0x04003EAB RID: 16043
		public bool removeOnLeftBehind;

		// Token: 0x04003EAC RID: 16044
		public float inbredChanceOnChild;

		// Token: 0x04003EAD RID: 16045
		public ThoughtDef diedThought;

		// Token: 0x04003EAE RID: 16046
		public ThoughtDef diedThoughtFemale;

		// Token: 0x04003EAF RID: 16047
		public ThoughtDef lostThought;

		// Token: 0x04003EB0 RID: 16048
		public ThoughtDef lostThoughtFemale;

		// Token: 0x04003EB1 RID: 16049
		public List<ThoughtDef> soldThoughts;

		// Token: 0x04003EB2 RID: 16050
		public ThoughtDef killedThought;

		// Token: 0x04003EB3 RID: 16051
		public ThoughtDef killedThoughtFemale;

		// Token: 0x04003EB4 RID: 16052
		[Unsaved(false)]
		private PawnRelationWorker workerInt;
	}
}
