using System;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002299 RID: 8857
	public class QuestPart_ScannerDurationRemainingAlert : QuestPart_Alert
	{
		// Token: 0x0600DAB0 RID: 55984 RVA: 0x005169FB File Offset: 0x00514BFB
		public QuestPart_ScannerDurationRemainingAlert()
		{
		}

		// Token: 0x0600DAB1 RID: 55985 RVA: 0x00516A03 File Offset: 0x00514C03
		public QuestPart_ScannerDurationRemainingAlert(Site site, string inSignalEnable, string inSignalDisable, string completedSignal, string raidSignal, int duration, int fireRaidRemainingTicks)
		{
			this.site = site;
			this.inSignalEnable = inSignalEnable;
			this.inSignalDisable = inSignalDisable;
			this.completedSignal = completedSignal;
			this.raidSignal = raidSignal;
			this.duration = duration;
			this.fireRaidRemainingTicks = fireRaidRemainingTicks;
		}

		// Token: 0x17002624 RID: 9764
		// (get) Token: 0x0600DAB2 RID: 55986 RVA: 0x00516A40 File Offset: 0x00514C40
		public override string AlertLabel
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("SurveyScannerAlertLabel".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(Mathf.Max(this.endTick - GenTicks.TicksGame, 0).ToStringTicksToPeriod(true, false, true, true, false));
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x17002625 RID: 9765
		// (get) Token: 0x0600DAB3 RID: 55987 RVA: 0x00516A9D File Offset: 0x00514C9D
		public override string AlertExplanation
		{
			get
			{
				return "SurveyScannerAlertDescription".Translate();
			}
		}

		// Token: 0x17002626 RID: 9766
		// (get) Token: 0x0600DAB4 RID: 55988 RVA: 0x00516AAE File Offset: 0x00514CAE
		public override AlertReport AlertReport
		{
			get
			{
				return base.State == QuestPartState.Enabled;
			}
		}

		// Token: 0x17002627 RID: 9767
		// (get) Token: 0x0600DAB5 RID: 55989 RVA: 0x00516ABE File Offset: 0x00514CBE
		public override GlobalTargetInfo? AlertCulpritTarget
		{
			get
			{
				return new GlobalTargetInfo?(this.scanner);
			}
		}

		// Token: 0x0600DAB6 RID: 55990 RVA: 0x00516AD0 File Offset: 0x00514CD0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignalEnable)
			{
				this.scanner = this.site.Map.listerThings.ThingsOfDef(ThingDefOf.SurveyScanner).FirstOrDefault<Thing>();
				this.endTick = GenTicks.TicksGame + this.duration;
				if (this.fireRaidRemainingTicks > 0)
				{
					this.fireRaidTick = Mathf.Max(GenTicks.TicksGame + 2500, this.endTick - this.fireRaidRemainingTicks);
				}
			}
		}

		// Token: 0x0600DAB7 RID: 55991 RVA: 0x00516B5C File Offset: 0x00514D5C
		public override void QuestPartTick()
		{
			if (GenTicks.TicksGame >= this.endTick)
			{
				Find.SignalManager.SendSignal(new Signal(this.completedSignal, false));
				base.Complete();
				return;
			}
			if (GenTicks.TicksGame >= this.fireRaidTick && this.fireRaidTick > 0)
			{
				this.fireRaidTick = -1;
				Find.SignalManager.SendSignal(new Signal(this.raidSignal, false));
			}
		}

		// Token: 0x0600DAB8 RID: 55992 RVA: 0x00516BC8 File Offset: 0x00514DC8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_References.Look<Thing>(ref this.scanner, "scanner", false);
			Scribe_Values.Look<string>(ref this.completedSignal, "completedSignal", null, false);
			Scribe_Values.Look<string>(ref this.raidSignal, "raidSignal", null, false);
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<int>(ref this.endTick, "endTick", 0, false);
			Scribe_Values.Look<int>(ref this.fireRaidTick, "fireRaidTick", 0, false);
			Scribe_Values.Look<int>(ref this.fireRaidRemainingTicks, "fireRaidRemainingTicks", 0, false);
		}

		// Token: 0x04008BE6 RID: 35814
		private Site site;

		// Token: 0x04008BE7 RID: 35815
		private Thing scanner;

		// Token: 0x04008BE8 RID: 35816
		private int duration;

		// Token: 0x04008BE9 RID: 35817
		private int endTick;

		// Token: 0x04008BEA RID: 35818
		private int fireRaidTick;

		// Token: 0x04008BEB RID: 35819
		private int fireRaidRemainingTicks;

		// Token: 0x04008BEC RID: 35820
		private string completedSignal;

		// Token: 0x04008BED RID: 35821
		private string raidSignal;
	}
}
