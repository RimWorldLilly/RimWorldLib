using System;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002258 RID: 8792
	public class QuestPart_StabilizersRemainingAlert : QuestPart_Alert
	{
		// Token: 0x17002604 RID: 9732
		// (get) Token: 0x0600D953 RID: 55635 RVA: 0x005086A0 File Offset: 0x005068A0
		private CompCerebrexCore CoreComp
		{
			get
			{
				if (this.coreComp == null)
				{
					this.coreComp = this.core.TryGetComp<CompCerebrexCore>();
				}
				return this.coreComp;
			}
		}

		// Token: 0x0600D954 RID: 55636 RVA: 0x005086C1 File Offset: 0x005068C1
		public QuestPart_StabilizersRemainingAlert()
		{
		}

		// Token: 0x0600D955 RID: 55637 RVA: 0x005086C9 File Offset: 0x005068C9
		public QuestPart_StabilizersRemainingAlert(Site site, string inSignalEnable, string inSignalDisable)
		{
			this.site = site;
			this.inSignalEnable = inSignalEnable;
			this.inSignalDisable = inSignalDisable;
		}

		// Token: 0x17002605 RID: 9733
		// (get) Token: 0x0600D956 RID: 55638 RVA: 0x005086E8 File Offset: 0x005068E8
		public override string AlertLabel
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 3);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("StabilizersRemainingAlertLabel".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.CoreComp.stabilizersRemaining);
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted<int>(3);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x17002606 RID: 9734
		// (get) Token: 0x0600D957 RID: 55639 RVA: 0x00508748 File Offset: 0x00506948
		public override string AlertExplanation
		{
			get
			{
				return "StabilizersRemainingAlertDescription".Translate();
			}
		}

		// Token: 0x17002607 RID: 9735
		// (get) Token: 0x0600D958 RID: 55640 RVA: 0x00508759 File Offset: 0x00506959
		public override AlertReport AlertReport
		{
			get
			{
				CompCerebrexCore compCerebrexCore = this.CoreComp;
				if (compCerebrexCore == null || compCerebrexCore.stabilizersRemaining <= 0)
				{
					return AlertReport.Inactive;
				}
				return AlertReport.Active;
			}
		}

		// Token: 0x0600D959 RID: 55641 RVA: 0x00508780 File Offset: 0x00506980
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignalEnable)
			{
				this.core = this.site.Map.listerThings.ThingsOfDef(ThingDefOf.CerebrexCore).FirstOrDefault<Thing>();
			}
		}

		// Token: 0x0600D95A RID: 55642 RVA: 0x005087CC File Offset: 0x005069CC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_References.Look<Thing>(ref this.core, "core", false);
		}

		// Token: 0x04008AF2 RID: 35570
		private Site site;

		// Token: 0x04008AF3 RID: 35571
		private Thing core;

		// Token: 0x04008AF4 RID: 35572
		private CompCerebrexCore coreComp;
	}
}
