using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001053 RID: 4179
	public class QuestPart_PassOutInterval : QuestPartActivable
	{
		// Token: 0x06006933 RID: 26931 RVA: 0x00252374 File Offset: 0x00250574
		public override void QuestPartTick()
		{
			if (this.currentInterval < 0)
			{
				foreach (string tag in this.outSignals)
				{
					Find.SignalManager.SendSignal(new Signal(tag, false));
				}
				this.currentInterval = this.ticksInterval.RandomInRange;
			}
			this.currentInterval--;
		}

		// Token: 0x06006934 RID: 26932 RVA: 0x002523FC File Offset: 0x002505FC
		protected override void ProcessQuestSignal(Signal signal)
		{
			if (this.inSignalsDisable.Contains(signal.tag))
			{
				this.Disable();
			}
		}

		// Token: 0x06006935 RID: 26933 RVA: 0x00252418 File Offset: 0x00250618
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.inSignalsDisable, "inSignalsDisable", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.currentInterval, "currentInterval", 0, false);
			Scribe_Values.Look<IntRange>(ref this.ticksInterval, "ticksInterval", default(IntRange), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.outSignals = (this.outSignals ?? new List<string>());
				this.inSignalsDisable = (this.inSignalsDisable ?? new List<string>());
			}
		}

		// Token: 0x06006936 RID: 26934 RVA: 0x002524B8 File Offset: 0x002506B8
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "Reset Interval " + this.ToString(), true, true, true, null))
			{
				this.currentInterval = 0;
			}
			curY += rect.height + 4f;
		}

		// Token: 0x040042F3 RID: 17139
		public IntRange ticksInterval;

		// Token: 0x040042F4 RID: 17140
		public List<string> outSignals = new List<string>();

		// Token: 0x040042F5 RID: 17141
		public List<string> inSignalsDisable = new List<string>();

		// Token: 0x040042F6 RID: 17142
		private int currentInterval;
	}
}
