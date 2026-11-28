using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200122B RID: 4651
	public class RoomContents_ChemfuelStorage : RoomContentsWorker
	{
		// Token: 0x06007382 RID: 29570 RVA: 0x00287610 File Offset: 0x00285810
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomGenUtility.GenerateRows(ThingDefOf.Shelf, room, map, ThingDefOf.Steel);
			int num = RoomContents_ChemfuelStorage.ChemfuelCountRange.RandomInRange;
			int num2 = 99;
			Func<IntVec3, bool> <>9__0;
			while (num > 0 && num2-- > 0)
			{
				Map map2 = map;
				int contractedBy = 0;
				int padding = 0;
				Func<IntVec3, bool> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => this.ShelfValidator(map, c, ThingDefOf.Chemfuel)));
				}
				IntVec3 loc;
				if (!room.TryGetRandomCellInRoom(map2, out loc, contractedBy, padding, validator, true))
				{
					break;
				}
				int num3 = Rand.Range(25, 50);
				num3 = Mathf.Min(num3, num);
				Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Chemfuel, null), loc, map, WipeMode.Vanish);
				thing.stackCount = num3;
				thing.SetForbidden(true, true);
				num -= num3;
			}
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x06007383 RID: 29571 RVA: 0x002876F0 File Offset: 0x002858F0
		private bool ShelfValidator(Map map, IntVec3 c, ThingDef itemDef)
		{
			Building_Storage building_Storage = c.GetFirstThing(map, ThingDefOf.Shelf) as Building_Storage;
			return building_Storage != null && building_Storage.SpaceRemainingFor(itemDef) != 0;
		}

		// Token: 0x040048E9 RID: 18665
		private static readonly IntRange ChemfuelCountRange = new IntRange(300, 500);
	}
}
