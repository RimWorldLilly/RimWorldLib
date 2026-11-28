using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200061C RID: 1564
	public abstract class LetterWithTimeout : Letter
	{
		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x060030FA RID: 12538 RVA: 0x001394C2 File Offset: 0x001376C2
		public bool TimeoutActive
		{
			get
			{
				return this.disappearAtTick >= 0;
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x060030FB RID: 12539 RVA: 0x001394D0 File Offset: 0x001376D0
		public bool TimeoutPassed
		{
			get
			{
				return this.TimeoutActive && Find.TickManager.TicksGame >= this.disappearAtTick;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x060030FC RID: 12540 RVA: 0x001394F1 File Offset: 0x001376F1
		public virtual bool LastTickBeforeTimeout
		{
			get
			{
				return this.TimeoutActive && this.disappearAtTick <= Find.TickManager.TicksGame + 1;
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x060030FD RID: 12541 RVA: 0x00139514 File Offset: 0x00137714
		public override bool ShouldAutomaticallyOpenLetter
		{
			get
			{
				return this.LastTickBeforeTimeout;
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x060030FE RID: 12542 RVA: 0x0013951C File Offset: 0x0013771C
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && !this.TimeoutPassed;
			}
		}

		// Token: 0x060030FF RID: 12543 RVA: 0x00139533 File Offset: 0x00137733
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.disappearAtTick, "disappearAtTick", -1, false);
		}

		// Token: 0x06003100 RID: 12544 RVA: 0x0013954D File Offset: 0x0013774D
		public void StartTimeout(int duration)
		{
			this.disappearAtTick = Find.TickManager.TicksGame + duration;
		}

		// Token: 0x06003101 RID: 12545 RVA: 0x00139564 File Offset: 0x00137764
		protected override string PostProcessedLabel()
		{
			string text = base.PostProcessedLabel();
			if (this.TimeoutActive)
			{
				text += " (" + Mathf.RoundToInt((float)(this.disappearAtTick - Find.TickManager.TicksGame) / 2500f).ToString() + "LetterHour".Translate() + ")";
			}
			return text;
		}

		// Token: 0x04001F7E RID: 8062
		public int disappearAtTick = -1;
	}
}
