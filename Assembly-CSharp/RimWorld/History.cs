using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102E RID: 4142
	public sealed class History : IExposable
	{
		// Token: 0x0600685D RID: 26717 RVA: 0x0024E1AC File Offset: 0x0024C3AC
		public History()
		{
			this.autoRecorderGroups = new List<HistoryAutoRecorderGroup>();
			this.AddOrRemoveHistoryRecorderGroups();
			this.curveDrawerStyle = new SimpleCurveDrawerStyle();
			this.curveDrawerStyle.DrawMeasures = true;
			this.curveDrawerStyle.DrawPoints = false;
			this.curveDrawerStyle.DrawBackground = true;
			this.curveDrawerStyle.DrawBackgroundLines = false;
			this.curveDrawerStyle.DrawLegend = true;
			this.curveDrawerStyle.DrawCurveMousePoint = true;
			this.curveDrawerStyle.OnlyPositiveValues = true;
			this.curveDrawerStyle.UseFixedSection = true;
			this.curveDrawerStyle.UseAntiAliasedLines = true;
			this.curveDrawerStyle.PointsRemoveOptimization = true;
			this.curveDrawerStyle.MeasureLabelsXCount = 10;
			this.curveDrawerStyle.MeasureLabelsYCount = 5;
			this.curveDrawerStyle.XIntegersOnly = true;
			this.curveDrawerStyle.LabelX = "Day".Translate();
		}

		// Token: 0x0600685E RID: 26718 RVA: 0x0024E2C0 File Offset: 0x0024C4C0
		public void HistoryTick()
		{
			for (int i = 0; i < this.autoRecorderGroups.Count; i++)
			{
				this.autoRecorderGroups[i].Tick();
			}
			this.historyEventsManager.HistoryEventsManagerTick();
		}

		// Token: 0x0600685F RID: 26719 RVA: 0x0024E2FF File Offset: 0x0024C4FF
		public List<HistoryAutoRecorderGroup> Groups()
		{
			return this.autoRecorderGroups;
		}

		// Token: 0x06006860 RID: 26720 RVA: 0x0024E308 File Offset: 0x0024C508
		public void ExposeData()
		{
			Scribe_Deep.Look<Archive>(ref this.archive, "archive", Array.Empty<object>());
			Scribe_Collections.Look<HistoryAutoRecorderGroup>(ref this.autoRecorderGroups, "autoRecorderGroups", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastPsylinkAvailable, "lastPsylinkAvailable", -999999, false);
			Scribe_Values.Look<int>(ref this.lastTickPlayerRaidedSomeone, "lastTickPlayerRaidedSomeone", -9999999, false);
			Scribe_Deep.Look<HistoryEventsManager>(ref this.historyEventsManager, "historyEventsManager", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.mechanitorEverDied, "mechanitorEverDied", false, false);
			Scribe_Values.Look<bool>(ref this.mechlinkEverAvailable, "mechlinkEverAvailable", false, false);
			Scribe_Values.Look<bool>(ref this.mechanoidDatacoreReadOrLost, "mechanoidDatacoreReadOrLost", false, false);
			Scribe_Values.Look<bool>(ref this.mechanoidDatacoreOpportunityAvailable, "mechanoidDatacoreOpportunityAvailable", false, false);
			Scribe_Values.Look<bool>(ref this.everThirdTrimesterPregnancy, "everThirdTrimesterPregnancy", false, false);
			Scribe_Values.Look<bool>(ref this.everCapturedUnrecruitablePawn, "everCapturedUnrecruitablePawn", false, false);
			Scribe_Values.Look<bool>(ref this.duplicateSicknessDiscovered, "duplicateSicknessDiscovered", false, false);
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.AddOrRemoveHistoryRecorderGroups();
				if (this.lastPsylinkAvailable == -999999)
				{
					this.lastPsylinkAvailable = Find.TickManager.TicksGame;
				}
			}
		}

		// Token: 0x06006861 RID: 26721 RVA: 0x0024E430 File Offset: 0x0024C630
		public void Notify_PsylinkAvailable()
		{
			this.lastPsylinkAvailable = Find.TickManager.TicksGame;
		}

		// Token: 0x06006862 RID: 26722 RVA: 0x0024E442 File Offset: 0x0024C642
		public void Notify_PlayerRaidedSomeone()
		{
			this.lastTickPlayerRaidedSomeone = Find.TickManager.TicksGame;
		}

		// Token: 0x06006863 RID: 26723 RVA: 0x0024E454 File Offset: 0x0024C654
		public void Notify_MechanitorDied()
		{
			this.mechanitorEverDied = true;
		}

		// Token: 0x06006864 RID: 26724 RVA: 0x0024E45D File Offset: 0x0024C65D
		public void Notify_MechlinkAvailable()
		{
			this.mechlinkEverAvailable = true;
		}

		// Token: 0x06006865 RID: 26725 RVA: 0x0024E466 File Offset: 0x0024C666
		public void Notify_MechanoidDatacoreOppurtunityAvailable()
		{
			this.mechanoidDatacoreOpportunityAvailable = true;
		}

		// Token: 0x06006866 RID: 26726 RVA: 0x0024E46F File Offset: 0x0024C66F
		public void Notify_MechanoidDatacoreReadOrLost()
		{
			this.mechanoidDatacoreReadOrLost = true;
		}

		// Token: 0x06006867 RID: 26727 RVA: 0x0024E478 File Offset: 0x0024C678
		public void Notify_DuplicateSicknessDiscovered()
		{
			this.duplicateSicknessDiscovered = true;
		}

		// Token: 0x06006868 RID: 26728 RVA: 0x0024E481 File Offset: 0x0024C681
		public void FinalizeInit()
		{
			this.lastPsylinkAvailable = Find.TickManager.TicksGame;
		}

		// Token: 0x06006869 RID: 26729 RVA: 0x0024E494 File Offset: 0x0024C694
		private void AddOrRemoveHistoryRecorderGroups()
		{
			if (this.autoRecorderGroups.RemoveAll((HistoryAutoRecorderGroup x) => x == null) != 0)
			{
				Log.Warning("Some history auto recorder groups were null.");
			}
			using (IEnumerator<HistoryAutoRecorderGroupDef> enumerator = DefDatabase<HistoryAutoRecorderGroupDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HistoryAutoRecorderGroupDef def = enumerator.Current;
					if (!this.autoRecorderGroups.Any((HistoryAutoRecorderGroup x) => x.def == def))
					{
						HistoryAutoRecorderGroup historyAutoRecorderGroup = new HistoryAutoRecorderGroup();
						historyAutoRecorderGroup.def = def;
						historyAutoRecorderGroup.AddOrRemoveHistoryRecorders();
						this.autoRecorderGroups.Add(historyAutoRecorderGroup);
					}
				}
			}
			this.autoRecorderGroups.RemoveAll((HistoryAutoRecorderGroup x) => x.def == null);
		}

		// Token: 0x04004274 RID: 17012
		public Archive archive = new Archive();

		// Token: 0x04004275 RID: 17013
		private List<HistoryAutoRecorderGroup> autoRecorderGroups;

		// Token: 0x04004276 RID: 17014
		public SimpleCurveDrawerStyle curveDrawerStyle;

		// Token: 0x04004277 RID: 17015
		public int lastPsylinkAvailable = -999999;

		// Token: 0x04004278 RID: 17016
		public int lastTickPlayerRaidedSomeone = -9999999;

		// Token: 0x04004279 RID: 17017
		public bool mechanitorEverDied;

		// Token: 0x0400427A RID: 17018
		public bool mechlinkEverAvailable;

		// Token: 0x0400427B RID: 17019
		public bool mechanoidDatacoreOpportunityAvailable;

		// Token: 0x0400427C RID: 17020
		public bool mechanoidDatacoreReadOrLost;

		// Token: 0x0400427D RID: 17021
		public bool everThirdTrimesterPregnancy;

		// Token: 0x0400427E RID: 17022
		public bool everCapturedUnrecruitablePawn;

		// Token: 0x0400427F RID: 17023
		public bool duplicateSicknessDiscovered;

		// Token: 0x04004280 RID: 17024
		public HistoryEventsManager historyEventsManager = new HistoryEventsManager();
	}
}
