using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D90 RID: 7568
	public class MainButtonWorker_ToggleMechTab : MainButtonWorker_ToggleTab
	{
		// Token: 0x17002477 RID: 9335
		// (get) Token: 0x0600C463 RID: 50275 RVA: 0x0047E1C8 File Offset: 0x0047C3C8
		public override bool Disabled
		{
			get
			{
				if (base.Disabled)
				{
					return true;
				}
				Map currentMap = Find.CurrentMap;
				int num = (currentMap != null) ? currentMap.uniqueID : int.MinValue;
				if ((long)GenTicks.TicksGame - this.lastDisabledCheckTick < 180L && this.lastDisabledCheckMapId == num)
				{
					return this.lastDisabled;
				}
				this.lastDisabledCheckMapId = num;
				this.lastDisabledCheckTick = (long)GenTicks.TicksGame;
				if (currentMap != null)
				{
					List<Pawn> list = currentMap.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].RaceProps.IsMechanoid)
						{
							this.lastDisabled = false;
							return false;
						}
					}
					List<Pawn> list2 = currentMap.mapPawns.PawnsInFaction(Faction.OfPlayer);
					for (int j = 0; j < list2.Count; j++)
					{
						if (list2[j].RaceProps.IsMechanoid)
						{
							this.lastDisabled = false;
							return false;
						}
					}
				}
				this.lastDisabled = true;
				return true;
			}
		}

		// Token: 0x17002478 RID: 9336
		// (get) Token: 0x0600C464 RID: 50276 RVA: 0x0047E2C2 File Offset: 0x0047C4C2
		public override bool Visible
		{
			get
			{
				return !this.Disabled;
			}
		}

		// Token: 0x0400702F RID: 28719
		private const long DisabledCheckInterval = 180L;

		// Token: 0x04007030 RID: 28720
		private long lastDisabledCheckTick = -10000000L;

		// Token: 0x04007031 RID: 28721
		private int lastDisabledCheckMapId = int.MinValue;

		// Token: 0x04007032 RID: 28722
		private bool lastDisabled;
	}
}
