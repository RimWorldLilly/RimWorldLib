using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001283 RID: 4739
	public class FleshmassMapComponent : MapComponent
	{
		// Token: 0x06007575 RID: 30069 RVA: 0x002935E4 File Offset: 0x002917E4
		public FleshmassMapComponent(Map map) : base(map)
		{
		}

		// Token: 0x06007576 RID: 30070 RVA: 0x00293604 File Offset: 0x00291804
		public override void MapComponentTick()
		{
			if (this.fleshmassDestroyTicks == null || this.fleshmassDestroyTicks.Count == 0)
			{
				return;
			}
			int num = this.fleshmassDestroyTicks.Peek();
			while (Find.TickManager.TicksGame >= num && this.fleshmassToDestroy.Count > 0)
			{
				Thing thing = this.fleshmassToDestroy.Dequeue();
				this.fleshmassDestroyTicks.Dequeue();
				if (thing != null && !thing.Destroyed)
				{
					if (this.destroyInChunks)
					{
						thing.Kill(null, null);
					}
					else
					{
						thing.Destroy(DestroyMode.KillFinalize);
					}
				}
				if (this.fleshmassDestroyTicks.Count > 0)
				{
					num = this.fleshmassDestroyTicks.Peek();
				}
			}
		}

		// Token: 0x06007577 RID: 30071 RVA: 0x002936B0 File Offset: 0x002918B0
		public void DestroyFleshmass(int destructionPeriodTicks, float rateExponent = 1f, bool destroyInChunks = false, Thing onlyFromSource = null)
		{
			PriorityQueue<Thing, int> priorityQueue = new PriorityQueue<Thing, int>();
			List<Thing> list = this.fleshmassToDestroy.ToList<Thing>();
			List<int> list2 = this.fleshmassDestroyTicks.ToList<int>();
			for (int i = 0; i < this.fleshmassToDestroy.Count; i++)
			{
				priorityQueue.Enqueue(list[i], list2[i]);
			}
			this.fleshmassToDestroy.Clear();
			this.fleshmassDestroyTicks.Clear();
			this.destroyInChunks = destroyInChunks;
			foreach (Thing thing in this.map.listerThings.ThingsOfDef(ThingDefOf.Fleshmass_Active))
			{
				if (onlyFromSource != null)
				{
					CompFleshmass compFleshmass = thing.TryGetComp<CompFleshmass>();
					if (((compFleshmass != null) ? compFleshmass.source : null) != onlyFromSource)
					{
						continue;
					}
				}
				float num = 1f - Mathf.Pow(Rand.Value, rateExponent);
				priorityQueue.Enqueue(thing, Find.TickManager.TicksGame + Mathf.RoundToInt((float)destructionPeriodTicks * num));
			}
			if (onlyFromSource != null)
			{
				goto IL_193;
			}
			using (List<Thing>.Enumerator enumerator = this.map.listerThings.ThingsOfDef(ThingDefOf.Fleshmass).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Thing element = enumerator.Current;
					float num2 = 1f - Mathf.Pow(Rand.Value, rateExponent);
					priorityQueue.Enqueue(element, Find.TickManager.TicksGame + Mathf.RoundToInt((float)destructionPeriodTicks * num2));
				}
				goto IL_193;
			}
			IL_167:
			Thing item;
			int item2;
			priorityQueue.TryPeek(out item, out item2);
			priorityQueue.Dequeue();
			this.fleshmassToDestroy.Enqueue(item);
			this.fleshmassDestroyTicks.Enqueue(item2);
			IL_193:
			if (priorityQueue.Count <= 0)
			{
				return;
			}
			goto IL_167;
		}

		// Token: 0x06007578 RID: 30072 RVA: 0x00293878 File Offset: 0x00291A78
		public override void ExposeData()
		{
			Scribe_Collections.Look<Thing>(ref this.fleshmassToDestroy, "fleshmassToDestroy", LookMode.Reference);
			Scribe_Collections.Look<int>(ref this.fleshmassDestroyTicks, "fleshmassDestroyTicks", LookMode.Value);
			Scribe_Values.Look<bool>(ref this.destroyInChunks, "destroyInChunks", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.fleshmassToDestroy == null)
				{
					this.fleshmassToDestroy = new Queue<Thing>();
				}
				if (this.fleshmassDestroyTicks == null)
				{
					this.fleshmassDestroyTicks = new Queue<int>();
				}
			}
		}

		// Token: 0x04004986 RID: 18822
		private Queue<Thing> fleshmassToDestroy = new Queue<Thing>();

		// Token: 0x04004987 RID: 18823
		private Queue<int> fleshmassDestroyTicks = new Queue<int>();

		// Token: 0x04004988 RID: 18824
		private bool destroyInChunks;
	}
}
