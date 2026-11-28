using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001277 RID: 4727
	public class RoomPart_SentryDrone : RoomPartWorker
	{
		// Token: 0x06007506 RID: 29958 RVA: 0x002914BC File Offset: 0x0028F6BC
		public RoomPart_SentryDrone(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x06007507 RID: 29959 RVA: 0x002914C8 File Offset: 0x0028F6C8
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			RoomPart_SentryDrone.<>c__DisplayClass1_0 CS$<>8__locals1 = new RoomPart_SentryDrone.<>c__DisplayClass1_0();
			CS$<>8__locals1.map = map;
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			IntVec3 loc;
			if (!room.TryGetRandomCellInRoom(CS$<>8__locals1.map, out loc, 0, 0, new Func<IntVec3, bool>(CS$<>8__locals1.<FillRoom>g__Validator|0), false))
			{
				Log.Error("Failed to find cell to spawn sentry drone.");
				return;
			}
			GenSpawn.Spawn(PawnGenerator.GeneratePawn(PawnKindDefOf.Drone_Sentry, faction, null), loc, CS$<>8__locals1.map, WipeMode.Vanish);
		}
	}
}
