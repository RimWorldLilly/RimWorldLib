using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200229D RID: 8861
	public class QuestPart_RandomWaves : QuestPartActivable
	{
		// Token: 0x0600DACA RID: 56010 RVA: 0x0051794D File Offset: 0x00515B4D
		public QuestPart_RandomWaves()
		{
		}

		// Token: 0x0600DACB RID: 56011 RVA: 0x00517960 File Offset: 0x00515B60
		public QuestPart_RandomWaves(string inSignalEnable, string outSignal, float mtbWavesHours, float minRefireWavesHours, float maxRefireWavesHours = 3.4028235E+38f)
		{
			this.inSignalEnable = inSignalEnable;
			this.outSignal = outSignal;
			this.mtbWavesHours = mtbWavesHours;
			this.minRefireWavesHours = minRefireWavesHours;
			this.maxRefireWavesHours = maxRefireWavesHours;
		}

		// Token: 0x0600DACC RID: 56012 RVA: 0x00517998 File Offset: 0x00515B98
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag != this.inSignalEnable)
			{
				return;
			}
			this.lastWaveTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600DACD RID: 56013 RVA: 0x005179C8 File Offset: 0x00515BC8
		public override void QuestPartTick()
		{
			if (((float)Find.TickManager.TicksGame > (float)this.lastWaveTick + this.minRefireWavesHours * 2500f && Rand.MTBEventOccurs(this.mtbWavesHours, 2500f, 1f)) || (float)Find.TickManager.TicksGame > (float)this.lastWaveTick + this.maxRefireWavesHours * 2500f)
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal, false));
				this.lastWaveTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600DACE RID: 56014 RVA: 0x00517A54 File Offset: 0x00515C54
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<int>(ref this.lastWaveTick, "lastWaveTick", 0, false);
			Scribe_Values.Look<float>(ref this.mtbWavesHours, "mtbWavesHours", 40f, false);
			Scribe_Values.Look<float>(ref this.minRefireWavesHours, "minRefireWavesHours", 24f, false);
			Scribe_Values.Look<float>(ref this.maxRefireWavesHours, "maxRefireWavesHours", 72f, false);
		}

		// Token: 0x04008C02 RID: 35842
		private string outSignal;

		// Token: 0x04008C03 RID: 35843
		private int lastWaveTick = -99999;

		// Token: 0x04008C04 RID: 35844
		private float mtbWavesHours;

		// Token: 0x04008C05 RID: 35845
		private float minRefireWavesHours;

		// Token: 0x04008C06 RID: 35846
		private float maxRefireWavesHours;
	}
}
