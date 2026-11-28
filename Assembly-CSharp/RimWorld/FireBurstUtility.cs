using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013FD RID: 5117
	public static class FireBurstUtility
	{
		// Token: 0x06007EE5 RID: 32485 RVA: 0x002D5BA8 File Offset: 0x002D3DA8
		public static void ThrowFuelTick(IntVec3 position, float radius, Map map)
		{
			if (!Rand.Chance(0.15f))
			{
				return;
			}
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(position, radius, true).InRandomOrder(null))
			{
				if (GenSight.LineOfSight(position, intVec, map, true, null, 0, 0) && FilthMaker.TryMakeFilth(intVec, map, ThingDefOf.Filth_Fuel, 1, FilthSourceFlags.None, true))
				{
					break;
				}
			}
		}

		// Token: 0x04004F95 RID: 20373
		private const float FuelSpawnChancePerCastingTick = 0.15f;
	}
}
