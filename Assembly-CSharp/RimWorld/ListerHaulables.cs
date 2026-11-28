using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001280 RID: 4736
	public class ListerHaulables
	{
		// Token: 0x0600754D RID: 30029 RVA: 0x00292D94 File Offset: 0x00290F94
		public ListerHaulables(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600754E RID: 30030 RVA: 0x00292DB9 File Offset: 0x00290FB9
		public ICollection<Thing> ThingsPotentiallyNeedingHauling()
		{
			return this.haulables;
		}

		// Token: 0x0600754F RID: 30031 RVA: 0x00292DC1 File Offset: 0x00290FC1
		public void Notify_Spawned(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06007550 RID: 30032 RVA: 0x00292DCA File Offset: 0x00290FCA
		public void Notify_DeSpawned(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06007551 RID: 30033 RVA: 0x00292DD3 File Offset: 0x00290FD3
		public void HaulDesignationAdded(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06007552 RID: 30034 RVA: 0x00292DDC File Offset: 0x00290FDC
		public void HaulDesignationRemoved(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06007553 RID: 30035 RVA: 0x00292DE5 File Offset: 0x00290FE5
		public void Notify_Unforbidden(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06007554 RID: 30036 RVA: 0x00292DEE File Offset: 0x00290FEE
		public void Notify_Forbidden(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06007555 RID: 30037 RVA: 0x00292DF7 File Offset: 0x00290FF7
		public void Notify_AddedThing(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06007556 RID: 30038 RVA: 0x00292E00 File Offset: 0x00291000
		public void Notify_SlotGroupChanged(SlotGroup sg)
		{
			List<IntVec3> cellsList = sg.CellsList;
			if (cellsList != null)
			{
				sg.RemoveHaulDesignationOnStoredThings();
				for (int i = 0; i < cellsList.Count; i++)
				{
					this.RecalcAllInCell(cellsList[i]);
				}
			}
		}

		// Token: 0x06007557 RID: 30039 RVA: 0x00292E3C File Offset: 0x0029103C
		public void Notify_HaulSourceChanged(IHaulSource holder)
		{
			foreach (Thing t in ((IEnumerable<Thing>)holder.GetDirectlyHeldThings()))
			{
				this.Check(t);
			}
		}

		// Token: 0x06007558 RID: 30040 RVA: 0x00292E8C File Offset: 0x0029108C
		public void ListerHaulablesTick()
		{
			ListerHaulables.groupCycleIndex++;
			if (ListerHaulables.groupCycleIndex >= 2147473647)
			{
				ListerHaulables.groupCycleIndex = 0;
			}
			this.CellsCheckTick(this.map.haulDestinationManager.AllGroupsListForReading);
			this.HaulSourcesCheckTick(this.map.haulDestinationManager.AllHaulSourcesListForReading);
		}

		// Token: 0x06007559 RID: 30041 RVA: 0x00292EE4 File Offset: 0x002910E4
		private void CellsCheckTick(List<SlotGroup> sgList)
		{
			if (sgList.Count == 0)
			{
				return;
			}
			int num = ListerHaulables.groupCycleIndex % sgList.Count;
			SlotGroup slotGroup = sgList[ListerHaulables.groupCycleIndex % sgList.Count];
			if (slotGroup.CellsList.Count == 0)
			{
				return;
			}
			while (ListerHaulables.cellCycleIndices.Count <= num)
			{
				ListerHaulables.cellCycleIndices.Add(0);
			}
			if (ListerHaulables.cellCycleIndices[num] >= 2147473647)
			{
				ListerHaulables.cellCycleIndices[num] = 0;
			}
			for (int i = 0; i < 4; i++)
			{
				List<int> list = ListerHaulables.cellCycleIndices;
				int index = num;
				int num2 = list[index];
				list[index] = num2 + 1;
				this.RecalcAllInCell(slotGroup.CellsList[ListerHaulables.cellCycleIndices[num] % slotGroup.CellsList.Count]);
			}
		}

		// Token: 0x0600755A RID: 30042 RVA: 0x00292FAC File Offset: 0x002911AC
		private void HaulSourcesCheckTick(List<IHaulSource> haulList)
		{
			if (haulList.Count == 0)
			{
				return;
			}
			int num = Mathf.CeilToInt((float)haulList.Count / 4f);
			int num2 = ListerHaulables.groupCycleIndex % num;
			int num3 = 0;
			while (num3 < 4 && num3 < haulList.Count)
			{
				int index = num2 + num3;
				IHaulSource haulSource = haulList[index];
				if (haulSource.GetDirectlyHeldThings().Count != 0)
				{
					this.RecalculateAllInHaulSource(haulSource);
				}
				num3++;
			}
		}

		// Token: 0x0600755B RID: 30043 RVA: 0x00293018 File Offset: 0x00291218
		public void RecalcAllInCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				this.Check(thingList[i]);
			}
		}

		// Token: 0x0600755C RID: 30044 RVA: 0x00293050 File Offset: 0x00291250
		public void RecalcAllInCells(IEnumerable<IntVec3> cells)
		{
			foreach (IntVec3 c in cells)
			{
				this.RecalcAllInCell(c);
			}
		}

		// Token: 0x0600755D RID: 30045 RVA: 0x00293098 File Offset: 0x00291298
		public void RecalculateAllInHaulSources(IList<IHaulSource> sources)
		{
			foreach (IHaulSource haulSource in sources)
			{
				foreach (Thing t in ((IEnumerable<Thing>)haulSource.GetDirectlyHeldThings()))
				{
					this.Check(t);
				}
			}
		}

		// Token: 0x0600755E RID: 30046 RVA: 0x00293114 File Offset: 0x00291314
		public void RecalculateAllInHaulSource(IHaulSource source)
		{
			foreach (Thing t in ((IEnumerable<Thing>)source.GetDirectlyHeldThings()))
			{
				this.Check(t);
			}
		}

		// Token: 0x0600755F RID: 30047 RVA: 0x00293164 File Offset: 0x00291364
		private void Check(Thing t)
		{
			if (this.ShouldBeHaulable(t))
			{
				this.haulables.Add(t);
				return;
			}
			this.haulables.Remove(t);
		}

		// Token: 0x06007560 RID: 30048 RVA: 0x0029318C File Offset: 0x0029138C
		private bool ShouldBeHaulable(Thing t)
		{
			if (t.IsForbidden(Faction.OfPlayer))
			{
				return false;
			}
			if (!t.def.alwaysHaulable)
			{
				if (!t.def.EverHaulable)
				{
					return false;
				}
				if (this.map.designationManager.DesignationOn(t, DesignationDefOf.Haul) == null && !t.IsInAnyStorage())
				{
					return false;
				}
			}
			if (t.IsInValidBestStorage())
			{
				return false;
			}
			IHaulSource haulSource = t.ParentHolder as IHaulSource;
			return haulSource == null || haulSource.HaulSourceEnabled;
		}

		// Token: 0x06007561 RID: 30049 RVA: 0x0029320A File Offset: 0x0029140A
		private void CheckAdd(Thing t)
		{
			if (this.ShouldBeHaulable(t))
			{
				this.haulables.Add(t);
			}
		}

		// Token: 0x06007562 RID: 30050 RVA: 0x00293222 File Offset: 0x00291422
		private void TryRemove(Thing t)
		{
			if (t.def.category == ThingCategory.Item && this.haulables.Contains(t))
			{
				this.haulables.Remove(t);
			}
		}

		// Token: 0x06007563 RID: 30051 RVA: 0x00293250 File Offset: 0x00291450
		internal string DebugString()
		{
			if (Time.frameCount % 10 == 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("======= All haulables (Count " + this.haulables.Count.ToString() + ")");
				int num = 0;
				foreach (Thing thing in this.haulables)
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

		// Token: 0x0400497C RID: 18812
		private Map map;

		// Token: 0x0400497D RID: 18813
		private HashSet<Thing> haulables = new HashSet<Thing>();

		// Token: 0x0400497E RID: 18814
		private const int CellsPerTick = 4;

		// Token: 0x0400497F RID: 18815
		private const int HaulSourcesPerTick = 4;

		// Token: 0x04004980 RID: 18816
		private static int groupCycleIndex = 0;

		// Token: 0x04004981 RID: 18817
		private static readonly List<int> cellCycleIndices = new List<int>();

		// Token: 0x04004982 RID: 18818
		private string debugOutput = "uninitialized";
	}
}
