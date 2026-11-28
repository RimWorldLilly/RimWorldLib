using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200115D RID: 4445
	public class IncidentQueue : IExposable
	{
		// Token: 0x17001327 RID: 4903
		// (get) Token: 0x06006FA4 RID: 28580 RVA: 0x0027096B File Offset: 0x0026EB6B
		public int Count
		{
			get
			{
				return this.queuedIncidents.Count;
			}
		}

		// Token: 0x17001328 RID: 4904
		// (get) Token: 0x06006FA5 RID: 28581 RVA: 0x00270978 File Offset: 0x0026EB78
		public string DebugQueueReadout
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (QueuedIncident queuedIncident in this.queuedIncidents)
				{
					stringBuilder.AppendLine(queuedIncident.ToString() + " (in " + (queuedIncident.FireTick - Find.TickManager.TicksGame).ToString() + " ticks)");
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06006FA6 RID: 28582 RVA: 0x00270A08 File Offset: 0x0026EC08
		public IEnumerator GetEnumerator()
		{
			IncidentQueue.<GetEnumerator>d__5 <GetEnumerator>d__ = new IncidentQueue.<GetEnumerator>d__5(0);
			<GetEnumerator>d__.<>4__this = this;
			return <GetEnumerator>d__;
		}

		// Token: 0x06006FA7 RID: 28583 RVA: 0x00270A17 File Offset: 0x0026EC17
		public void Clear()
		{
			this.queuedIncidents.Clear();
		}

		// Token: 0x06006FA8 RID: 28584 RVA: 0x00270A24 File Offset: 0x0026EC24
		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedIncident>(ref this.queuedIncidents, "queuedIncidents", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06006FA9 RID: 28585 RVA: 0x00270A3C File Offset: 0x0026EC3C
		public bool Add(QueuedIncident qi)
		{
			this.queuedIncidents.Add(qi);
			this.queuedIncidents.Sort((QueuedIncident a, QueuedIncident b) => a.FireTick.CompareTo(b.FireTick));
			return true;
		}

		// Token: 0x06006FAA RID: 28586 RVA: 0x00270A78 File Offset: 0x0026EC78
		public bool Add(IncidentDef def, int fireTick, IncidentParms parms = null, int retryDurationTicks = 0)
		{
			QueuedIncident qi = new QueuedIncident(new FiringIncident(def, null, parms), fireTick, retryDurationTicks);
			this.Add(qi);
			return true;
		}

		// Token: 0x06006FAB RID: 28587 RVA: 0x00270AA0 File Offset: 0x0026ECA0
		public void IncidentQueueTick()
		{
			for (int i = this.queuedIncidents.Count - 1; i >= 0; i--)
			{
				QueuedIncident queuedIncident = this.queuedIncidents[i];
				if (!queuedIncident.TriedToFire)
				{
					if (queuedIncident.FireTick <= Find.TickManager.TicksGame)
					{
						bool flag = Find.Storyteller.TryFire(queuedIncident.FiringIncident, true);
						queuedIncident.Notify_TriedToFire();
						if (flag || queuedIncident.RetryDurationTicks == 0)
						{
							this.queuedIncidents.Remove(queuedIncident);
						}
					}
				}
				else if (queuedIncident.FireTick + queuedIncident.RetryDurationTicks <= Find.TickManager.TicksGame)
				{
					this.queuedIncidents.Remove(queuedIncident);
				}
				else if (Find.TickManager.TicksGame % 833 == Rand.RangeSeeded(0, 833, queuedIncident.FireTick))
				{
					bool flag2 = Find.Storyteller.TryFire(queuedIncident.FiringIncident, true);
					queuedIncident.Notify_TriedToFire();
					if (flag2)
					{
						this.queuedIncidents.Remove(queuedIncident);
					}
				}
			}
		}

		// Token: 0x06006FAC RID: 28588 RVA: 0x00270B9C File Offset: 0x0026ED9C
		public void Notify_MapRemoved(Map map)
		{
			this.queuedIncidents.RemoveAll((QueuedIncident x) => x.FiringIncident.parms.target == map);
		}

		// Token: 0x04004775 RID: 18293
		private List<QueuedIncident> queuedIncidents = new List<QueuedIncident>();
	}
}
