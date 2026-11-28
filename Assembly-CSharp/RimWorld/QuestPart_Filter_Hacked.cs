using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001075 RID: 4213
	public class QuestPart_Filter_Hacked : QuestPart_Filter
	{
		// Token: 0x060069A4 RID: 27044 RVA: 0x00253BF0 File Offset: 0x00251DF0
		protected override bool Pass(SignalArgs args)
		{
			Thing thing;
			if (args.TryGetArg<Thing>("SUBJECT", out thing))
			{
				CompHackable compHackable = thing.TryGetComp<CompHackable>();
				if (compHackable != null)
				{
					return compHackable.IsHacked;
				}
			}
			return false;
		}
	}
}
