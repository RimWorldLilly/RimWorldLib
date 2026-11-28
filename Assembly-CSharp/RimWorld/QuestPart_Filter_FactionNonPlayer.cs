using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001073 RID: 4211
	public class QuestPart_Filter_FactionNonPlayer : QuestPart_Filter
	{
		// Token: 0x060069A0 RID: 27040 RVA: 0x00253B70 File Offset: 0x00251D70
		protected override bool Pass(SignalArgs args)
		{
			Faction faction;
			if (args.TryGetArg<Faction>("FACTION", out faction))
			{
				return faction != Faction.OfPlayer;
			}
			Thing thing;
			return args.TryGetArg<Thing>("SUBJECT", out thing) && thing.Faction != Faction.OfPlayer;
		}
	}
}
