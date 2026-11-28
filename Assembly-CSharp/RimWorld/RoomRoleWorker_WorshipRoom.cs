using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200138A RID: 5002
	public class RoomRoleWorker_WorshipRoom : RoomRoleWorker
	{
		// Token: 0x06007BC4 RID: 31684 RVA: 0x002C0B34 File Offset: 0x002BED34
		public override string PostProcessedLabel(string baseLabel, Room room)
		{
			Ideo ideo = this.DominatingIdeo(room);
			if (ideo == null || ideo.WorshipRoomLabel.NullOrEmpty())
			{
				return base.PostProcessedLabel(baseLabel, room);
			}
			return ideo.WorshipRoomLabel;
		}

		// Token: 0x06007BC5 RID: 31685 RVA: 0x002C0B68 File Offset: 0x002BED68
		public override float GetScore(Room room)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return -1f;
			}
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			int num = 0;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i].def.isAltar)
				{
					ThingWithComps thingWithComps = containedAndAdjacentThings[i] as ThingWithComps;
					CompStyleable compStyleable = (thingWithComps != null) ? thingWithComps.compStyleable : null;
					bool flag;
					if (compStyleable == null)
					{
						flag = (null != null);
					}
					else
					{
						Precept_ThingStyle sourcePrecept = compStyleable.SourcePrecept;
						if (sourcePrecept == null)
						{
							flag = (null != null);
						}
						else
						{
							Ideo ideo = sourcePrecept.ideo;
							flag = (((ideo != null) ? ideo.StructureMeme : null) != null);
						}
					}
					if (flag)
					{
						num++;
					}
				}
			}
			return (float)((num == 0) ? 0 : Mathf.Max(2000, num * 75));
		}

		// Token: 0x06007BC6 RID: 31686 RVA: 0x002C0C08 File Offset: 0x002BEE08
		private Ideo DominatingIdeo(Room room)
		{
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i].def.isAltar)
				{
					ThingWithComps thingWithComps = containedAndAdjacentThings[i] as ThingWithComps;
					CompStyleable compStyleable = (thingWithComps != null) ? thingWithComps.compStyleable : null;
					bool flag;
					if (compStyleable == null)
					{
						flag = (null != null);
					}
					else
					{
						Precept_ThingStyle sourcePrecept = compStyleable.SourcePrecept;
						if (sourcePrecept == null)
						{
							flag = (null != null);
						}
						else
						{
							Ideo ideo = sourcePrecept.ideo;
							flag = (((ideo != null) ? ideo.StructureMeme : null) != null);
						}
					}
					if (flag)
					{
						return compStyleable.SourcePrecept.ideo;
					}
				}
			}
			return null;
		}

		// Token: 0x04004DF8 RID: 19960
		private const int MinScore = 2000;
	}
}
