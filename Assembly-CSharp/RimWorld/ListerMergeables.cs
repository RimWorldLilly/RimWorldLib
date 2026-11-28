using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001281 RID: 4737
	public class ListerMergeables
	{
		// Token: 0x06007565 RID: 30053 RVA: 0x0029331E File Offset: 0x0029151E
		public ListerMergeables(Map map)
		{
			this.map = map;
		}

		// Token: 0x06007566 RID: 30054 RVA: 0x00293343 File Offset: 0x00291543
		public List<Thing> ThingsPotentiallyNeedingMerging()
		{
			return this.mergeables;
		}

		// Token: 0x06007567 RID: 30055 RVA: 0x0029334B File Offset: 0x0029154B
		public void Notify_Spawned(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06007568 RID: 30056 RVA: 0x00293354 File Offset: 0x00291554
		public void Notify_DeSpawned(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06007569 RID: 30057 RVA: 0x0029335D File Offset: 0x0029155D
		public void Notify_Unforbidden(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x0600756A RID: 30058 RVA: 0x00293366 File Offset: 0x00291566
		public void Notify_Forbidden(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x0600756B RID: 30059 RVA: 0x00293370 File Offset: 0x00291570
		public void Notify_SlotGroupChanged(SlotGroup sg)
		{
			if (sg.CellsList != null)
			{
				for (int i = 0; i < sg.CellsList.Count; i++)
				{
					this.RecalcAllInCell(sg.CellsList[i]);
				}
			}
		}

		// Token: 0x0600756C RID: 30060 RVA: 0x002933AD File Offset: 0x002915AD
		public void Notify_ThingStackChanged(Thing t)
		{
			this.Check(t);
		}

		// Token: 0x0600756D RID: 30061 RVA: 0x002933B8 File Offset: 0x002915B8
		public void RecalcAllInCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				this.Check(thingList[i]);
			}
		}

		// Token: 0x0600756E RID: 30062 RVA: 0x002933F0 File Offset: 0x002915F0
		private void Check(Thing t)
		{
			if (this.ShouldBeMergeable(t))
			{
				if (!this.mergeables.Contains(t))
				{
					this.mergeables.Add(t);
					return;
				}
			}
			else
			{
				this.mergeables.Remove(t);
			}
		}

		// Token: 0x0600756F RID: 30063 RVA: 0x00293423 File Offset: 0x00291623
		private bool ShouldBeMergeable(Thing t)
		{
			return !t.IsForbidden(Faction.OfPlayer) && t.GetSlotGroup() != null && t.stackCount != t.def.stackLimit;
		}

		// Token: 0x06007570 RID: 30064 RVA: 0x00293454 File Offset: 0x00291654
		private void CheckAdd(Thing t)
		{
			if (this.ShouldBeMergeable(t) && !this.mergeables.Contains(t))
			{
				this.mergeables.Add(t);
			}
		}

		// Token: 0x06007571 RID: 30065 RVA: 0x00293479 File Offset: 0x00291679
		private void TryRemove(Thing t)
		{
			if (t.def.category == ThingCategory.Item)
			{
				this.mergeables.Remove(t);
			}
		}

		// Token: 0x06007572 RID: 30066 RVA: 0x00293498 File Offset: 0x00291698
		internal string DebugString()
		{
			if (Time.frameCount % 10 == 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("======= All mergeables (Count " + this.mergeables.Count.ToString() + ")");
				int num = 0;
				foreach (Thing thing in this.mergeables)
				{
					stringBuilder.AppendLine(thing.ThingID);
					num++;
					if (num > 200)
					{
						break;
					}
				}
				this.debugOutput = stringBuilder.ToString();
			}
			return this.debugOutput;
		}

		// Token: 0x04004983 RID: 18819
		private Map map;

		// Token: 0x04004984 RID: 18820
		private List<Thing> mergeables = new List<Thing>();

		// Token: 0x04004985 RID: 18821
		private string debugOutput = "uninitialized";
	}
}
