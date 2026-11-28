using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001F35 RID: 7989
	public abstract class Reward : IExposable
	{
		// Token: 0x17002527 RID: 9511
		// (get) Token: 0x0600CBBF RID: 52159 RVA: 0x004B235A File Offset: 0x004B055A
		public virtual bool MakesUseOfChosenPawnSignal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002528 RID: 9512
		// (get) Token: 0x0600CBC0 RID: 52160 RVA: 0x004B235D File Offset: 0x004B055D
		public virtual IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				return Enumerable.Empty<GenUI.AnonymousStackElement>();
			}
		}

		// Token: 0x17002529 RID: 9513
		// (get) Token: 0x0600CBC1 RID: 52161 RVA: 0x004B2364 File Offset: 0x004B0564
		public virtual float TotalMarketValue
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x0600CBC2 RID: 52162
		public abstract void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed);

		// Token: 0x0600CBC3 RID: 52163
		public abstract IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules);

		// Token: 0x0600CBC4 RID: 52164
		public abstract string GetDescription(RewardsGeneratorParams parms);

		// Token: 0x0600CBC5 RID: 52165 RVA: 0x004B236B File Offset: 0x004B056B
		public virtual void Notify_Used()
		{
			this.usedOrCleanedUp = true;
		}

		// Token: 0x0600CBC6 RID: 52166 RVA: 0x004B2374 File Offset: 0x004B0574
		public virtual void Notify_PreCleanup()
		{
			this.usedOrCleanedUp = true;
		}

		// Token: 0x0600CBC7 RID: 52167 RVA: 0x004B237D File Offset: 0x004B057D
		public virtual void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.usedOrCleanedUp, "usedOrCleanedUp", false, false);
		}

		// Token: 0x040081F1 RID: 33265
		protected bool usedOrCleanedUp;
	}
}
