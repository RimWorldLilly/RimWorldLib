using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F47 RID: 8007
	public static class SmoothableWallUtility
	{
		// Token: 0x0600CC5E RID: 52318 RVA: 0x004B5598 File Offset: 0x004B3798
		public static void Notify_SmoothedByPawn(Thing t, Pawn p)
		{
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				IntVec3 c = t.Position + GenAdj.CardinalDirections[i];
				if (c.InBounds(t.Map))
				{
					Building edifice = c.GetEdifice(t.Map);
					if (edifice != null && edifice.def.IsSmoothable)
					{
						bool flag = true;
						int num = 0;
						for (int j = 0; j < GenAdj.CardinalDirections.Length; j++)
						{
							IntVec3 intVec = edifice.Position + GenAdj.CardinalDirections[j];
							if (!SmoothableWallUtility.IsBlocked(intVec, t.Map))
							{
								flag = false;
								break;
							}
							Building edifice2 = intVec.GetEdifice(t.Map);
							if (edifice2 != null && edifice2.def.IsSmoothed)
							{
								num++;
							}
						}
						if (flag && num >= 2)
						{
							for (int k = 0; k < GenAdj.DiagonalDirections.Length; k++)
							{
								if (!SmoothableWallUtility.IsBlocked(edifice.Position + GenAdj.DiagonalDirections[k], t.Map))
								{
									SmoothableWallUtility.SmoothWall(edifice, p);
									break;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600CC5F RID: 52319 RVA: 0x004B56C4 File Offset: 0x004B38C4
		public static void Notify_BuildingDestroying(Thing t, DestroyMode mode)
		{
			if (mode != DestroyMode.KillFinalize && mode != DestroyMode.Deconstruct)
			{
				return;
			}
			if (!t.def.IsSmoothed)
			{
				return;
			}
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				IntVec3 c = t.Position + GenAdj.CardinalDirections[i];
				if (c.InBounds(t.Map))
				{
					Building edifice = c.GetEdifice(t.Map);
					if (edifice != null && edifice.def.IsSmoothed)
					{
						bool flag = true;
						for (int j = 0; j < GenAdj.CardinalDirections.Length; j++)
						{
							if (!SmoothableWallUtility.IsBlocked(edifice.Position + GenAdj.CardinalDirections[j], t.Map))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							edifice.Destroy(DestroyMode.WillReplace);
							GenSpawn.Spawn(ThingMaker.MakeThing(edifice.def.building.unsmoothedThing, edifice.Stuff), edifice.Position, t.Map, edifice.Rotation, WipeMode.Vanish, false, false);
						}
					}
				}
			}
		}

		// Token: 0x0600CC60 RID: 52320 RVA: 0x004B57CC File Offset: 0x004B39CC
		public static Thing SmoothWall(Thing target, Pawn smoother)
		{
			Map map = target.Map;
			target.Destroy(DestroyMode.WillReplace);
			Thing thing = ThingMaker.MakeThing(target.def.building.smoothedThing, target.Stuff);
			thing.SetFaction(((smoother != null) ? smoother.Faction : null) ?? Faction.OfPlayer, null);
			GenSpawn.Spawn(thing, target.Position, map, target.Rotation, WipeMode.Vanish, false, false);
			map.designationManager.TryRemoveDesignation(target.Position, DesignationDefOf.SmoothWall);
			return thing;
		}

		// Token: 0x0600CC61 RID: 52321 RVA: 0x004B584C File Offset: 0x004B3A4C
		private static bool IsBlocked(IntVec3 pos, Map map)
		{
			if (!pos.InBounds(map))
			{
				return false;
			}
			if (pos.Walkable(map))
			{
				return false;
			}
			Building edifice = pos.GetEdifice(map);
			return edifice != null && (edifice.def.IsSmoothed || edifice.def.building.isNaturalRock);
		}
	}
}
