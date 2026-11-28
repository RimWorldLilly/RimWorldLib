using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016DB RID: 5851
	public abstract class RitualOutcomeEffectWorker : IExposable
	{
		// Token: 0x17001924 RID: 6436
		// (get) Token: 0x06009260 RID: 37472 RVA: 0x003575CC File Offset: 0x003557CC
		public virtual bool ApplyOnFailure
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001925 RID: 6437
		// (get) Token: 0x06009261 RID: 37473 RVA: 0x003575CF File Offset: 0x003557CF
		public virtual bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001926 RID: 6438
		// (get) Token: 0x06009262 RID: 37474 RVA: 0x003575D2 File Offset: 0x003557D2
		public virtual bool ShowQuality
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001927 RID: 6439
		// (get) Token: 0x06009263 RID: 37475 RVA: 0x003575D5 File Offset: 0x003557D5
		public virtual string Description
		{
			get
			{
				return this.def.Description;
			}
		}

		// Token: 0x06009264 RID: 37476 RVA: 0x003575E2 File Offset: 0x003557E2
		public RitualOutcomeEffectWorker()
		{
		}

		// Token: 0x06009265 RID: 37477 RVA: 0x003575EA File Offset: 0x003557EA
		public RitualOutcomeEffectWorker(RitualOutcomeEffectDef def)
		{
			this.def = def;
			this.FillCompData();
		}

		// Token: 0x06009266 RID: 37478 RVA: 0x00357600 File Offset: 0x00355800
		public void ResetCompDatas()
		{
			foreach (RitualOutcomeComp_Data ritualOutcomeComp_Data in this.compDatas)
			{
				if (ritualOutcomeComp_Data != null)
				{
					ritualOutcomeComp_Data.Reset();
				}
			}
		}

		// Token: 0x06009267 RID: 37479 RVA: 0x00357658 File Offset: 0x00355858
		public void FillCompData()
		{
			if ((this.compDatas == null || this.compDatas.Count != this.def.comps.Count) && !this.def.comps.NullOrEmpty<RitualOutcomeComp>())
			{
				this.compDatas = new List<RitualOutcomeComp_Data>();
				foreach (RitualOutcomeComp ritualOutcomeComp in this.def.comps)
				{
					this.compDatas.Add(ritualOutcomeComp.MakeData());
				}
			}
		}

		// Token: 0x06009268 RID: 37480 RVA: 0x003576FC File Offset: 0x003558FC
		public RitualOutcomeComp_Data DataForComp(RitualOutcomeComp comp)
		{
			int num = this.def.comps.IndexOf(comp);
			if (num != -1)
			{
				return this.compDatas[num];
			}
			Log.Error("Can't find index for " + comp.GetType().Name);
			return null;
		}

		// Token: 0x06009269 RID: 37481
		public abstract void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual);

		// Token: 0x0600926A RID: 37482 RVA: 0x00357748 File Offset: 0x00355948
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<RitualOutcomeEffectDef>(ref this.def, "def");
			Scribe_Collections.Look<RitualOutcomeComp_Data>(ref this.compDatas, "compDatas", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && (this.compDatas.NullOrEmpty<RitualOutcomeComp_Data>() || this.compDatas.Count != this.def.comps.Count))
			{
				this.FillCompData();
			}
		}

		// Token: 0x0600926B RID: 37483 RVA: 0x003577B3 File Offset: 0x003559B3
		public Thought_Memory MakeMemory(Pawn p, LordJob_Ritual ritual, ThoughtDef overrideDef = null)
		{
			Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(overrideDef ?? this.def.memoryDef);
			thought_Memory.sourcePrecept = ritual.Ritual;
			return thought_Memory;
		}

		// Token: 0x0600926C RID: 37484 RVA: 0x003577DC File Offset: 0x003559DC
		public virtual void Tick(LordJob_Ritual ritual, float progressAmount = 1f)
		{
			if (!this.def.comps.NullOrEmpty<RitualOutcomeComp>())
			{
				foreach (RitualOutcomeComp ritualOutcomeComp in this.def.comps)
				{
					ritualOutcomeComp.Tick(ritual, this.DataForComp(ritualOutcomeComp), progressAmount);
				}
			}
		}

		// Token: 0x0600926D RID: 37485 RVA: 0x00357850 File Offset: 0x00355A50
		public virtual string ExtraAlertParagraph(Precept_Ritual ritual)
		{
			return null;
		}

		// Token: 0x0600926E RID: 37486 RVA: 0x00357853 File Offset: 0x00355A53
		public virtual string ExpectedQualityLabel()
		{
			return null;
		}

		// Token: 0x0600926F RID: 37487 RVA: 0x00357856 File Offset: 0x00355A56
		public virtual RitualOutcomePossibility GetForcedOutcome(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return null;
		}

		// Token: 0x06009270 RID: 37488 RVA: 0x00357859 File Offset: 0x00355A59
		public virtual IEnumerable<string> BlockingIssues(Precept_Ritual ritual, TargetInfo target, RitualRoleAssignments assignments)
		{
			RitualOutcomeEffectWorker.<BlockingIssues>d__22 <BlockingIssues>d__ = new RitualOutcomeEffectWorker.<BlockingIssues>d__22(-2);
			<BlockingIssues>d__.<>4__this = this;
			<BlockingIssues>d__.<>3__ritual = ritual;
			<BlockingIssues>d__.<>3__target = target;
			<BlockingIssues>d__.<>3__assignments = assignments;
			return <BlockingIssues>d__;
		}

		// Token: 0x040058C1 RID: 22721
		public RitualOutcomeEffectDef def;

		// Token: 0x040058C2 RID: 22722
		public List<RitualOutcomeComp_Data> compDatas;
	}
}
