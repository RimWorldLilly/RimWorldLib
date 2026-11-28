using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200108C RID: 4236
	public class QuestPart_ShuttleLeaveDelay : QuestPart_Delay
	{
		// Token: 0x1700122F RID: 4655
		// (get) Token: 0x06006A21 RID: 27169 RVA: 0x00255B9A File Offset: 0x00253D9A
		public override AlertReport AlertReport
		{
			get
			{
				if (this.shuttle == null || !this.shuttle.Spawned)
				{
					return false;
				}
				return AlertReport.CulpritIs(this.shuttle);
			}
		}

		// Token: 0x17001230 RID: 4656
		// (get) Token: 0x06006A22 RID: 27170 RVA: 0x00255BC8 File Offset: 0x00253DC8
		public override bool AlertCritical
		{
			get
			{
				return base.TicksLeft < 60000;
			}
		}

		// Token: 0x17001231 RID: 4657
		// (get) Token: 0x06006A23 RID: 27171 RVA: 0x00255BD7 File Offset: 0x00253DD7
		public override string AlertLabel
		{
			get
			{
				return "QuestPartShuttleLeaveDelay".Translate(base.TicksLeft.ToStringTicksToPeriodVerbose(true, true));
			}
		}

		// Token: 0x17001232 RID: 4658
		// (get) Token: 0x06006A24 RID: 27172 RVA: 0x00255BFC File Offset: 0x00253DFC
		public override string AlertExplanation
		{
			get
			{
				if (this.quest.hidden)
				{
					return "QuestPartShuttleLeaveDelayDescHidden".Translate(base.TicksLeft.ToStringTicksToPeriodVerbose(true, true).Colorize(ColoredText.DateTimeColor));
				}
				return "QuestPartShuttleLeaveDelayDesc".Translate(this.quest.name, base.TicksLeft.ToStringTicksToPeriodVerbose(true, true).Colorize(ColoredText.DateTimeColor), this.shuttle.TryGetComp<CompShuttle>().RequiredThingsLabel);
			}
		}

		// Token: 0x17001233 RID: 4659
		// (get) Token: 0x06006A25 RID: 27173 RVA: 0x00255C92 File Offset: 0x00253E92
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_ShuttleLeaveDelay.<get_QuestLookTargets>d__11 <get_QuestLookTargets>d__ = new QuestPart_ShuttleLeaveDelay.<get_QuestLookTargets>d__11(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006A26 RID: 27174 RVA: 0x00255CA2 File Offset: 0x00253EA2
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (base.State == QuestPartState.Enabled && this.inSignalsDisable.Contains(signal.tag))
			{
				this.Disable();
			}
		}

		// Token: 0x06006A27 RID: 27175 RVA: 0x00255CCD File Offset: 0x00253ECD
		public override string ExtraInspectString(ISelectable target)
		{
			if (target == this.shuttle)
			{
				return "ShuttleLeaveDelayInspectString".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
			}
			return null;
		}

		// Token: 0x06006A28 RID: 27176 RVA: 0x00255D00 File Offset: 0x00253F00
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Collections.Look<string>(ref this.inSignalsDisable, "inSignalsDisable", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.inSignalsDisable == null)
			{
				this.inSignalsDisable = new List<string>();
			}
		}

		// Token: 0x06006A29 RID: 27177 RVA: 0x00255D55 File Offset: 0x00253F55
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.shuttle != null)
			{
				this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
			}
		}

		// Token: 0x04004373 RID: 17267
		public Thing shuttle;

		// Token: 0x04004374 RID: 17268
		public List<string> inSignalsDisable = new List<string>();
	}
}
