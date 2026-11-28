using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F13 RID: 7955
	public static class DoorUtility
	{
		// Token: 0x0600CACD RID: 51917 RVA: 0x004A67D2 File Offset: 0x004A49D2
		public static IEnumerable<IntVec3> WallRequirementCells(ThingDef def, IntVec3 pos, Rot4 rot)
		{
			DoorUtility.<WallRequirementCells>d__0 <WallRequirementCells>d__ = new DoorUtility.<WallRequirementCells>d__0(-2);
			<WallRequirementCells>d__.<>3__def = def;
			<WallRequirementCells>d__.<>3__pos = pos;
			<WallRequirementCells>d__.<>3__rot = rot;
			return <WallRequirementCells>d__;
		}

		// Token: 0x0600CACE RID: 51918 RVA: 0x004A67F0 File Offset: 0x004A49F0
		public static bool EncapsulatingWallAt(IntVec3 cell, Map map, bool includeUnbuilt = false)
		{
			if (!cell.InBounds(map))
			{
				return false;
			}
			if (includeUnbuilt)
			{
				List<Thing> thingList = cell.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Blueprint blueprint = thingList[i] as Blueprint;
					if (blueprint != null)
					{
						ThingDef thingDef = blueprint.def.entityDefToBuild as ThingDef;
						if (thingDef != null && thingDef.Fillage == FillCategory.Full && !thingDef.IsDoor)
						{
							return true;
						}
					}
					Frame frame = thingList[i] as Frame;
					if (frame != null && frame.BuildDef.Fillage == FillCategory.Full && !frame.BuildDef.IsDoor)
					{
						return true;
					}
				}
			}
			Building edifice = cell.GetEdifice(map);
			return edifice != null && edifice.def.Fillage == FillCategory.Full && !edifice.def.IsDoor;
		}

		// Token: 0x0600CACF RID: 51919 RVA: 0x004A68BC File Offset: 0x004A4ABC
		public static Rot4 DoorRotationAt(IntVec3 loc, Map map, bool preferFences)
		{
			int num = 0;
			int num2 = 0;
			int num3 = num + DoorUtility.AlignQualityAgainst(loc, IntVec3.East, map, preferFences) + DoorUtility.AlignQualityAgainst(loc, IntVec3.West, map, preferFences);
			num2 += DoorUtility.AlignQualityAgainst(loc, IntVec3.North, map, preferFences);
			num2 += DoorUtility.AlignQualityAgainst(loc, IntVec3.South, map, preferFences);
			if (num3 >= num2)
			{
				return Rot4.North;
			}
			return Rot4.East;
		}

		// Token: 0x0600CAD0 RID: 51920 RVA: 0x004A6918 File Offset: 0x004A4B18
		private static int AlignQualityAgainst(IntVec3 c, IntVec3 offset, Map map, bool preferFences)
		{
			IntVec3 c2 = c + offset;
			if (!c2.InBounds(map))
			{
				return 0;
			}
			if (!c2.WalkableByNormal(map))
			{
				return 9;
			}
			List<Thing> thingList = c2.GetThingList(map);
			int i = 0;
			while (i < thingList.Count)
			{
				Thing thing = thingList[i];
				if (typeof(Building_Door).IsAssignableFrom(thing.def.thingClass))
				{
					if ((c - offset).GetDoor(map) == null)
					{
						return 1;
					}
					return 5;
				}
				else if (thing.def.IsFence)
				{
					if (!preferFences)
					{
						return 1;
					}
					return 10;
				}
				else
				{
					Thing thing2 = thing as Blueprint;
					if (thing2 != null)
					{
						if (thing2.def.entityDefToBuild.passability == Traversability.Impassable)
						{
							return 9;
						}
						ThingDef thingDef = thing2.def.entityDefToBuild as ThingDef;
						if (thingDef != null && thingDef.IsFence)
						{
							if (!preferFences)
							{
								return 1;
							}
							return 10;
						}
						else if (typeof(Building_Door).IsAssignableFrom(thing.def.thingClass))
						{
							return 1;
						}
					}
					i++;
				}
			}
			return 0;
		}
	}
}
