using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002257 RID: 8791
	public class QuestPart_CerebrexCore : QuestPartActivable
	{
		// Token: 0x17002603 RID: 9731
		// (get) Token: 0x0600D94D RID: 55629 RVA: 0x00508445 File Offset: 0x00506645
		private string StabilizerDestroyedSignal
		{
			get
			{
				return this.stabilizerTag + ".Destroyed";
			}
		}

		// Token: 0x0600D94E RID: 55630 RVA: 0x00508457 File Offset: 0x00506657
		public QuestPart_CerebrexCore()
		{
		}

		// Token: 0x0600D94F RID: 55631 RVA: 0x0050845F File Offset: 0x0050665F
		public QuestPart_CerebrexCore(Site site, string inSignalEnable, string coreTag, string stabilizerTag)
		{
			this.site = site;
			this.inSignalEnable = inSignalEnable;
			this.coreTag = coreTag;
			this.stabilizerTag = stabilizerTag;
		}

		// Token: 0x0600D950 RID: 55632 RVA: 0x00508484 File Offset: 0x00506684
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_Collections.Look<Thing>(ref this.stabilizers, "stabilizers", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Thing>(ref this.core, "core", false);
			Scribe_Values.Look<string>(ref this.coreTag, "coreTag", null, false);
			Scribe_Values.Look<string>(ref this.stabilizerTag, "stabilizerTag", null, false);
		}

		// Token: 0x0600D951 RID: 55633 RVA: 0x005084F4 File Offset: 0x005066F4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			Thing thing;
			signal.args.TryGetArg<Thing>("SUBJECT", out thing);
			if (signal.tag == this.inSignalEnable && this.SetupTags())
			{
				return;
			}
			if (signal.tag != this.StabilizerDestroyedSignal)
			{
				return;
			}
			CompCerebrexCore compCerebrexCore = this.core.TryGetComp<CompCerebrexCore>();
			float num = (compCerebrexCore.stabilizersRemaining == 1) ? 1.5f : 1f;
			MechhiveUtility.FireRaid(this.site.Map, StorytellerUtility.DefaultThreatPointsNow(this.site.Map) * num, -1, "MechhiveDefenseResponseLetterLabel".Translate(), "MechhiveDefenseResponseLetterText".Translate());
			compCerebrexCore.Notify_StabilizerDisabled();
		}

		// Token: 0x0600D952 RID: 55634 RVA: 0x005085B4 File Offset: 0x005067B4
		private bool SetupTags()
		{
			this.stabilizers = this.site.Map.listerThings.ThingsOfDef(ThingDefOf.CerebrexStabilizer).ToList<Thing>();
			this.core = this.site.Map.listerThings.ThingsOfDef(ThingDefOf.CerebrexCore).FirstOrDefault<Thing>();
			if (this.stabilizers.NullOrEmpty<Thing>() || this.stabilizers.Count != 3 || this.core == null)
			{
				Log.Error("Mechhive Quest: Could not find cerebrex core or stabilizers.");
				return true;
			}
			foreach (Thing thing in this.stabilizers)
			{
				QuestUtility.AddQuestTag(ref thing.questTags, this.stabilizerTag);
			}
			QuestUtility.AddQuestTag(ref this.core.questTags, this.coreTag);
			return false;
		}

		// Token: 0x04008AED RID: 35565
		private Site site;

		// Token: 0x04008AEE RID: 35566
		private List<Thing> stabilizers;

		// Token: 0x04008AEF RID: 35567
		private Thing core;

		// Token: 0x04008AF0 RID: 35568
		private string coreTag;

		// Token: 0x04008AF1 RID: 35569
		private string stabilizerTag;
	}
}
