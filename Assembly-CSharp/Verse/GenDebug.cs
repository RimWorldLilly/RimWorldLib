using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006E5 RID: 1765
	public static class GenDebug
	{
		// Token: 0x060037E1 RID: 14305 RVA: 0x00165CA0 File Offset: 0x00163EA0
		public static void DebugPlaceSphere(Vector3 Loc, float Scale)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject.transform.position = Loc;
			gameObject.transform.localScale = new Vector3(Scale, Scale, Scale);
		}

		// Token: 0x060037E2 RID: 14306 RVA: 0x00165CC8 File Offset: 0x00163EC8
		public static void LogList<T>(IEnumerable<T> list)
		{
			foreach (T t in list)
			{
				Log.Message("    " + t.ToString());
			}
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x00165D28 File Offset: 0x00163F28
		public static void ClearArea(CellRect r, Map map)
		{
			r.ClipInsideMap(map);
			foreach (IntVec3 c in r)
			{
				map.roofGrid.SetRoof(c, null);
			}
			foreach (IntVec3 c2 in r)
			{
				foreach (Thing thing in c2.GetThingList(map).ToList<Thing>())
				{
					if (thing.def.destroyable)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x00165E10 File Offset: 0x00164010
		public static void SpawnArea(CellRect r, Map map, ThingDef def)
		{
			foreach (IntVec3 loc in r)
			{
				GenSpawn.Spawn(def, loc, map, WipeMode.Vanish);
			}
		}
	}
}
