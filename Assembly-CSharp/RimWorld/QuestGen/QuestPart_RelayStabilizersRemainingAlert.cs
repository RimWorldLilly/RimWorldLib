using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002255 RID: 8789
	public class QuestPart_RelayStabilizersRemainingAlert : QuestPart_Alert
	{
		// Token: 0x0600D943 RID: 55619 RVA: 0x00508075 File Offset: 0x00506275
		public QuestPart_RelayStabilizersRemainingAlert()
		{
		}

		// Token: 0x0600D944 RID: 55620 RVA: 0x00508088 File Offset: 0x00506288
		public QuestPart_RelayStabilizersRemainingAlert(Site site, string inSignalEnable, string inSignalDisable, string inSignalStabilizerDealtWith, int stabilizerCount)
		{
			this.site = site;
			this.inSignalEnable = inSignalEnable;
			this.inSignalDisable = inSignalDisable;
			this.inSignalStabilizerDealtWith = inSignalStabilizerDealtWith;
			this.stabilizerCount = stabilizerCount;
			this.stabilizersRemaining = stabilizerCount;
		}

		// Token: 0x17002600 RID: 9728
		// (get) Token: 0x0600D945 RID: 55621 RVA: 0x005080C8 File Offset: 0x005062C8
		public override string AlertLabel
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 3);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("RelayStabilizersRemainingAlertLabel".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.stabilizersRemaining);
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.stabilizerCount);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x17002601 RID: 9729
		// (get) Token: 0x0600D946 RID: 55622 RVA: 0x00508128 File Offset: 0x00506328
		public override string AlertExplanation
		{
			get
			{
				return "RelayStabilizersRemainingAlertDescription".Translate();
			}
		}

		// Token: 0x17002602 RID: 9730
		// (get) Token: 0x0600D947 RID: 55623 RVA: 0x00508139 File Offset: 0x00506339
		public override AlertReport AlertReport
		{
			get
			{
				if (this.stabilizersRemaining <= 0)
				{
					return AlertReport.Inactive;
				}
				return AlertReport.Active;
			}
		}

		// Token: 0x0600D948 RID: 55624 RVA: 0x00508150 File Offset: 0x00506350
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			this.stabilizersRemaining = this.stabilizerCount;
			for (int i = 0; i < this.things.Count; i++)
			{
				if (this.things[i].DestroyedOrNull() || this.things[i].IsHacked())
				{
					this.stabilizersRemaining--;
				}
			}
		}

		// Token: 0x0600D949 RID: 55625 RVA: 0x005081BC File Offset: 0x005063BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_Values.Look<string>(ref this.inSignalStabilizerDealtWith, "inSignalStabilizerDealtWith", null, false);
			Scribe_Values.Look<int>(ref this.stabilizerCount, "stabilizerCount", 0, false);
			Scribe_Values.Look<int>(ref this.stabilizersRemaining, "stabilizersRemaining", 0, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04008AE8 RID: 35560
		private Site site;

		// Token: 0x04008AE9 RID: 35561
		private string inSignalStabilizerDealtWith;

		// Token: 0x04008AEA RID: 35562
		private int stabilizerCount;

		// Token: 0x04008AEB RID: 35563
		private int stabilizersRemaining;

		// Token: 0x04008AEC RID: 35564
		public List<Thing> things = new List<Thing>();
	}
}
