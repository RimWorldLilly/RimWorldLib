using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001091 RID: 4241
	public abstract class QuestPart_MTB : QuestPartActivable
	{
		// Token: 0x17001238 RID: 4664
		// (get) Token: 0x06006A44 RID: 27204
		protected abstract float MTBDays { get; }

		// Token: 0x06006A45 RID: 27205 RVA: 0x00256284 File Offset: 0x00254484
		public override void QuestPartTick()
		{
			float mtbdays = this.MTBDays;
			if (mtbdays > 0f && Find.TickManager.TicksGame % 10 == 0 && Rand.MTBEventOccurs(mtbdays, 60000f, 10f))
			{
				base.Complete();
			}
		}

		// Token: 0x06006A46 RID: 27206 RVA: 0x002562C8 File Offset: 0x002544C8
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "MTB occurs " + this.ToString(), true, true, true, null))
			{
				base.Complete();
			}
			curY += rect.height + 4f;
		}

		// Token: 0x0400437E RID: 17278
		private const int CheckIntervalTicks = 10;
	}
}
