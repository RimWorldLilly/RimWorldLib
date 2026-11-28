using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008A2 RID: 2210
	public static class GenPath
	{
		// Token: 0x06004452 RID: 17490 RVA: 0x001AC270 File Offset: 0x001AA470
		public static TargetInfo ResolvePathMode(Pawn pawn, TargetInfo dest, ref PathEndMode peMode)
		{
			if (dest.HasThing && !dest.Thing.Spawned)
			{
				peMode = PathEndMode.Touch;
				return dest;
			}
			if (peMode == PathEndMode.InteractionCell)
			{
				if (!dest.HasThing)
				{
					string str = "Pathed to cell ";
					TargetInfo targetInfo = dest;
					Log.Error(str + targetInfo.ToString() + " with PathEndMode.InteractionCell.");
				}
				peMode = PathEndMode.OnCell;
				return new TargetInfo(dest.Thing.InteractionCell, dest.Thing.Map, false);
			}
			if (peMode == PathEndMode.ClosestTouch)
			{
				peMode = GenPath.ResolveClosestTouchPathMode(pawn, dest.Map, dest.Cell);
			}
			return dest;
		}

		// Token: 0x06004453 RID: 17491 RVA: 0x001AC308 File Offset: 0x001AA508
		public static PathEndMode ResolveClosestTouchPathMode(Pawn pawn, Map map, IntVec3 target)
		{
			if (GenPath.ShouldNotEnterCell(pawn, map, target))
			{
				return PathEndMode.Touch;
			}
			return PathEndMode.OnCell;
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x001AC318 File Offset: 0x001AA518
		private static bool ShouldNotEnterCell(Pawn pawn, Map map, IntVec3 dest)
		{
			if (map.pathing.For(pawn).pathGrid.Cost(dest) > 30)
			{
				return true;
			}
			if (!dest.Walkable(map))
			{
				return true;
			}
			if (pawn != null)
			{
				if (dest.IsForbidden(pawn))
				{
					return true;
				}
				Building_Door building_Door = dest.GetEdifice(map) as Building_Door;
				if (building_Door != null)
				{
					if (building_Door.IsForbidden(pawn))
					{
						return true;
					}
					if (!building_Door.PawnCanOpen(pawn))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x001AC382 File Offset: 0x001AA582
		public static string SpeedPercentString(float extraPathTicks)
		{
			return (13f / (extraPathTicks + 13f)).ToStringPercent();
		}
	}
}
