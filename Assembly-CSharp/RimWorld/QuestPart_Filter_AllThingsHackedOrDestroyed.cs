using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001061 RID: 4193
	public class QuestPart_Filter_AllThingsHackedOrDestroyed : QuestPart_Filter_AllThingsHacked
	{
		// Token: 0x06006967 RID: 26983 RVA: 0x00253020 File Offset: 0x00251220
		protected override bool Pass(SignalArgs args)
		{
			if (this.things.Count == 0)
			{
				return true;
			}
			int num = 0;
			for (int i = 0; i < this.things.Count; i++)
			{
				if (this.things[i].DestroyedOrNull() || this.things[i].IsHacked())
				{
					num++;
				}
			}
			return num >= this.things.Count;
		}
	}
}
