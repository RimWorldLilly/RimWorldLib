using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021C3 RID: 8643
	public class QuestNode_GetLargestClearArea : QuestNode
	{
		// Token: 0x0600D734 RID: 55092 RVA: 0x004FD984 File Offset: 0x004FBB84
		protected override bool TestRunInt(Slate slate)
		{
			int largestSize = this.GetLargestSize(slate);
			slate.Set<int>(this.storeAs.GetValue(slate), largestSize, false);
			return largestSize >= this.failIfSmaller.GetValue(slate);
		}

		// Token: 0x0600D735 RID: 55093 RVA: 0x004FD9C0 File Offset: 0x004FBBC0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int largestSize = this.GetLargestSize(slate);
			slate.Set<int>(this.storeAs.GetValue(slate), largestSize, false);
		}

		// Token: 0x0600D736 RID: 55094 RVA: 0x004FD9F0 File Offset: 0x004FBBF0
		private int GetLargestSize(Slate slate)
		{
			Map mapResolved = this.map.GetValue(slate) ?? slate.Get<Map>("map", null, false);
			if (mapResolved == null)
			{
				return 0;
			}
			int value = this.max.GetValue(slate);
			CellRect cellRect = LargestAreaFinder.FindLargestRect(mapResolved, (IntVec3 x) => this.IsClear(x, mapResolved), value);
			return Mathf.Min(new int[]
			{
				cellRect.Width,
				cellRect.Height,
				value
			});
		}

		// Token: 0x0600D737 RID: 55095 RVA: 0x004FDA80 File Offset: 0x004FBC80
		private bool IsClear(IntVec3 c, Map map)
		{
			if (!c.GetAffordances(map).Contains(TerrainAffordanceDefOf.Heavy))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.IsBuildingArtificial && thingList[i].Faction == Faction.OfPlayer)
				{
					return false;
				}
				if (thingList[i].def.mineable)
				{
					bool flag = false;
					for (int j = 0; j < 8; j++)
					{
						IntVec3 c2 = c + GenAdj.AdjacentCells[j];
						if (c2.InBounds(map) && c2.GetFirstMineable(map) == null)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0400888E RID: 34958
		public SlateRef<Map> map;

		// Token: 0x0400888F RID: 34959
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04008890 RID: 34960
		public SlateRef<int> failIfSmaller;

		// Token: 0x04008891 RID: 34961
		public SlateRef<int> max;
	}
}
