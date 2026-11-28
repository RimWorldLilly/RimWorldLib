using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010AA RID: 4266
	public class QuestPart_Alert : QuestPartActivable
	{
		// Token: 0x17001247 RID: 4679
		// (get) Token: 0x06006AC1 RID: 27329 RVA: 0x002580F9 File Offset: 0x002562F9
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_Alert.<get_QuestLookTargets>d__9 <get_QuestLookTargets>d__ = new QuestPart_Alert.<get_QuestLookTargets>d__9(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x17001248 RID: 4680
		// (get) Token: 0x06006AC2 RID: 27330 RVA: 0x00258109 File Offset: 0x00256309
		public override string AlertLabel
		{
			get
			{
				return this.resolvedLabel;
			}
		}

		// Token: 0x17001249 RID: 4681
		// (get) Token: 0x06006AC3 RID: 27331 RVA: 0x00258111 File Offset: 0x00256311
		public override string AlertExplanation
		{
			get
			{
				return this.resolvedExplanation;
			}
		}

		// Token: 0x1700124A RID: 4682
		// (get) Token: 0x06006AC4 RID: 27332 RVA: 0x00258119 File Offset: 0x00256319
		public override bool AlertCritical
		{
			get
			{
				return this.critical;
			}
		}

		// Token: 0x1700124B RID: 4683
		// (get) Token: 0x06006AC5 RID: 27333 RVA: 0x00258121 File Offset: 0x00256321
		public override AlertReport AlertReport
		{
			get
			{
				if (base.State != QuestPartState.Enabled)
				{
					return AlertReport.Inactive;
				}
				if (this.resolvedLookTargets.IsValid())
				{
					return AlertReport.CulpritsAre(this.resolvedLookTargets.targets);
				}
				return AlertReport.Active;
			}
		}

		// Token: 0x06006AC6 RID: 27334 RVA: 0x00258158 File Offset: 0x00256358
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.resolvedLabel = receivedArgs.GetFormattedText(this.label);
			this.resolvedExplanation = receivedArgs.GetFormattedText(this.explanation);
			this.resolvedLookTargets = this.lookTargets;
			if (this.getLookTargetsFromSignal && !this.resolvedLookTargets.IsValid())
			{
				SignalArgsUtility.TryGetLookTargets(receivedArgs, "SUBJECT", out this.resolvedLookTargets);
			}
		}

		// Token: 0x06006AC7 RID: 27335 RVA: 0x002581DC File Offset: 0x002563DC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<string>(ref this.explanation, "explanation", null, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.critical, "critical", false, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", false, false);
			Scribe_Values.Look<string>(ref this.resolvedLabel, "resolvedLabel", null, false);
			Scribe_Values.Look<string>(ref this.resolvedExplanation, "resolvedExplanation", null, false);
			Scribe_Deep.Look<LookTargets>(ref this.resolvedLookTargets, "resolvedLookTargets", Array.Empty<object>());
		}

		// Token: 0x06006AC8 RID: 27336 RVA: 0x00258285 File Offset: 0x00256485
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.label = "DEV: Test";
			this.explanation = "Test text";
		}

		// Token: 0x040043DB RID: 17371
		public string label;

		// Token: 0x040043DC RID: 17372
		public string explanation;

		// Token: 0x040043DD RID: 17373
		public LookTargets lookTargets;

		// Token: 0x040043DE RID: 17374
		public bool critical;

		// Token: 0x040043DF RID: 17375
		public bool getLookTargetsFromSignal;

		// Token: 0x040043E0 RID: 17376
		private string resolvedLabel;

		// Token: 0x040043E1 RID: 17377
		private string resolvedExplanation;

		// Token: 0x040043E2 RID: 17378
		private LookTargets resolvedLookTargets;
	}
}
