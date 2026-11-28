using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200202C RID: 8236
	public abstract class GenStep_ScattererBestFit : GenStep_Scatterer
	{
		// Token: 0x1700257D RID: 9597
		// (get) Token: 0x0600D0A7 RID: 53415
		protected abstract IntVec2 Size { get; }

		// Token: 0x0600D0A8 RID: 53416
		public abstract bool CollisionAt(IntVec3 cell, Map map);

		// Token: 0x0600D0A9 RID: 53417 RVA: 0x004CC07C File Offset: 0x004CA27C
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.Standable(map))
			{
				return false;
			}
			if (c.Roofed(map))
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false)))
			{
				return false;
			}
			CellRect cellRect = CellRect.CenteredOn(c, this.Size.x, this.Size.z);
			if (!cellRect.FullyContainedWithin(CellRect.WholeMap(map)))
			{
				return false;
			}
			int num = 0;
			foreach (IntVec3 intVec in cellRect)
			{
				if (!intVec.InBounds(map))
				{
					return false;
				}
				List<Thing> thingList = intVec.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (!thingList[i].def.destroyable)
					{
						return false;
					}
				}
				if (this.CollisionAt(intVec, map))
				{
					num++;
				}
			}
			if (num < GenStep_ScattererBestFit.bestOccupiedScore)
			{
				GenStep_ScattererBestFit.bestOccupiedScore = num;
				GenStep_ScattererBestFit.bestCellWithLeastOccupiedCollisions = new IntVec3?(c);
			}
			return num == 0;
		}

		// Token: 0x0600D0AA RID: 53418 RVA: 0x004CC1A8 File Offset: 0x004CA3A8
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			GenStep_ScattererBestFit.bestOccupiedScore = int.MaxValue;
			GenStep_ScattererBestFit.bestCellWithLeastOccupiedCollisions = null;
			if (this.CanScatterAt(map.Center, map))
			{
				result = map.Center;
				return true;
			}
			int num = Mathf.FloorToInt((float)Mathf.Min(map.Size.x, map.Size.z) / 2f);
			Vector3 v = IntVec3.North.ToVector3();
			for (int i = 5; i <= num; i += 5)
			{
				for (int j = 0; j < 5; j++)
				{
					float angle = Rand.Range(0f, 360f);
					Vector3 a = v.RotatedBy(angle);
					int num2 = Rand.Range(i - 5, i);
					IntVec3 intVec = map.Center + (a * (float)num2).ToIntVec3();
					if (this.CanScatterAt(intVec, map))
					{
						result = intVec;
						return true;
					}
				}
			}
			if (GenStep_ScattererBestFit.bestCellWithLeastOccupiedCollisions != null)
			{
				result = GenStep_ScattererBestFit.bestCellWithLeastOccupiedCollisions.Value;
				return true;
			}
			if (this.warnOnFail)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Scatterer ");
				defaultInterpolatedStringHandler.AppendFormatted(this.ToString());
				defaultInterpolatedStringHandler.AppendLiteral(" from def ");
				defaultInterpolatedStringHandler.AppendFormatted(this.def.defName);
				defaultInterpolatedStringHandler.AppendLiteral(" could not find cell to generate at.");
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x040084C9 RID: 33993
		private const int DistanceStep = 5;

		// Token: 0x040084CA RID: 33994
		private const int AttemptsPerStep = 5;

		// Token: 0x040084CB RID: 33995
		private static IntVec3? bestCellWithLeastOccupiedCollisions;

		// Token: 0x040084CC RID: 33996
		private static int bestOccupiedScore;
	}
}
