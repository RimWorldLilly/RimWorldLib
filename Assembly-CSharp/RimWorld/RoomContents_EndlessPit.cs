using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001250 RID: 4688
	public class RoomContents_EndlessPit : RoomContentsWorker
	{
		// Token: 0x170013CC RID: 5068
		// (get) Token: 0x06007448 RID: 29768 RVA: 0x0028B082 File Offset: 0x00289282
		private IEnumerable<ThingDef> PitDefs
		{
			get
			{
				return new RoomContents_EndlessPit.<get_PitDefs>d__1(-2);
			}
		}

		// Token: 0x06007449 RID: 29769 RVA: 0x0028B08C File Offset: 0x0028928C
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			ThingDef thingDef = this.PitDefs.RandomElement<ThingDef>();
			IntVec3 loc;
			if (!room.TryGetRandomCellInRoom(thingDef, map, out loc, null, 3, 1, null, false))
			{
				return;
			}
			GenSpawn.Spawn(ThingMaker.MakeThing(thingDef, null), loc, map, thingDef.rotatable ? Rot4.Random : Rot4.North, WipeMode.Vanish, false, false);
		}
	}
}
