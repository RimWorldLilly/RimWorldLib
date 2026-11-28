using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020019FB RID: 6651
	public class CompLongRangeMineralScanner : CompScanner
	{
		// Token: 0x17001D51 RID: 7505
		// (get) Token: 0x0600A77A RID: 42874 RVA: 0x003BA708 File Offset: 0x003B8908
		public new CompProperties_LongRangeMineralScanner Props
		{
			get
			{
				return this.props as CompProperties_LongRangeMineralScanner;
			}
		}

		// Token: 0x0600A77B RID: 42875 RVA: 0x003BA715 File Offset: 0x003B8915
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.targetMineable, "targetMineable");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.targetMineable == null)
			{
				this.SetDefaultTargetMineral();
			}
		}

		// Token: 0x0600A77C RID: 42876 RVA: 0x003BA743 File Offset: 0x003B8943
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.SetDefaultTargetMineral();
		}

		// Token: 0x0600A77D RID: 42877 RVA: 0x003BA752 File Offset: 0x003B8952
		private void SetDefaultTargetMineral()
		{
			this.targetMineable = ThingDefOf.MineableGold;
		}

		// Token: 0x0600A77E RID: 42878 RVA: 0x003BA760 File Offset: 0x003B8960
		protected override void DoFind(Pawn worker)
		{
			Slate slate = new Slate();
			slate.Set<Map>("map", this.parent.Map, false);
			slate.Set<ThingDef>("targetMineable", this.targetMineable, false);
			slate.Set<ThingDef>("targetMineableThing", this.targetMineable.building.mineableThing, false);
			slate.Set<Pawn>("worker", worker, false);
			if (!QuestScriptDefOf.LongRangeMineralScannerLump.CanRun(slate, this.parent.Map))
			{
				return;
			}
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.LongRangeMineralScannerLump, slate);
			Find.LetterStack.ReceiveLetter(quest.name, quest.description, LetterDefOf.PositiveEvent, null, null, quest, null, null, 0, true);
		}

		// Token: 0x0600A77F RID: 42879 RVA: 0x003BA811 File Offset: 0x003B8A11
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompLongRangeMineralScanner.<CompGetGizmosExtra>d__7 <CompGetGizmosExtra>d__ = new CompLongRangeMineralScanner.<CompGetGizmosExtra>d__7(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04006293 RID: 25235
		private ThingDef targetMineable;
	}
}
