using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000940 RID: 2368
	public static class LordMaker
	{
		// Token: 0x060048C9 RID: 18633 RVA: 0x001C83D4 File Offset: 0x001C65D4
		public static Lord MakeNewLord(Faction faction, LordJob lordJob, Map map, IEnumerable<Pawn> startingPawns = null)
		{
			if (map == null)
			{
				Log.Warning("Tried to create a lord with null map.");
				return null;
			}
			Lord lord = new Lord();
			lord.loadID = Find.UniqueIDsManager.GetNextLordID();
			lord.faction = faction;
			map.lordManager.AddLord(lord);
			if (startingPawns != null)
			{
				lord.AddPawns(startingPawns, false);
			}
			lord.SetJob(lordJob, false);
			lord.GotoToil(lord.Graph.StartingToil);
			if (startingPawns != null)
			{
				foreach (Pawn p in lord.ownedPawns)
				{
					lordJob.Notify_PawnAdded(p);
				}
			}
			return lord;
		}
	}
}
