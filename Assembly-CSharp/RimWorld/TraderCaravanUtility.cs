using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001B95 RID: 7061
	public static class TraderCaravanUtility
	{
		// Token: 0x0600B537 RID: 46391 RVA: 0x003FD550 File Offset: 0x003FB750
		public static TraderCaravanRole GetTraderCaravanRole(this Pawn p)
		{
			if (p.kindDef == PawnKindDefOf.Slave)
			{
				return TraderCaravanRole.Chattel;
			}
			if (p.kindDef.trader)
			{
				return TraderCaravanRole.Trader;
			}
			if (p.kindDef.RaceProps.packAnimal && p.inventory.innerContainer.Any)
			{
				return TraderCaravanRole.Carrier;
			}
			if (p.RaceProps.Animal)
			{
				return TraderCaravanRole.Chattel;
			}
			return TraderCaravanRole.Guard;
		}

		// Token: 0x0600B538 RID: 46392 RVA: 0x003FD5B4 File Offset: 0x003FB7B4
		public static Pawn FindTrader(Lord lord)
		{
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].GetTraderCaravanRole() == TraderCaravanRole.Trader)
				{
					return lord.ownedPawns[i];
				}
			}
			return null;
		}

		// Token: 0x0600B539 RID: 46393 RVA: 0x003FD5F9 File Offset: 0x003FB7F9
		public static float GenerateGuardPoints()
		{
			return (float)Rand.Range(550, 1000);
		}
	}
}
