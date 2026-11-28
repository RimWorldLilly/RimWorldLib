using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001594 RID: 5524
	public class Pawn_ConnectionsTracker : IExposable
	{
		// Token: 0x17001732 RID: 5938
		// (get) Token: 0x0600879C RID: 34716 RVA: 0x003106C3 File Offset: 0x0030E8C3
		public List<Thing> ConnectedThings
		{
			get
			{
				return this.connectedThings;
			}
		}

		// Token: 0x0600879D RID: 34717 RVA: 0x003106CB File Offset: 0x0030E8CB
		public Pawn_ConnectionsTracker()
		{
		}

		// Token: 0x0600879E RID: 34718 RVA: 0x003106DE File Offset: 0x0030E8DE
		public Pawn_ConnectionsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600879F RID: 34719 RVA: 0x003106F8 File Offset: 0x0030E8F8
		public void ConnectTo(Thing thing)
		{
			this.connectedThings.Add(thing);
		}

		// Token: 0x060087A0 RID: 34720 RVA: 0x00310708 File Offset: 0x0030E908
		public void Notify_PawnKilled()
		{
			for (int i = this.connectedThings.Count - 1; i >= 0; i--)
			{
				CompTreeConnection compTreeConnection = this.connectedThings[i].TryGetComp<CompTreeConnection>();
				if (compTreeConnection != null)
				{
					compTreeConnection.Notify_PawnDied(this.pawn);
				}
				this.connectedThings.RemoveAt(i);
			}
		}

		// Token: 0x060087A1 RID: 34721 RVA: 0x0031075C File Offset: 0x0030E95C
		public void Notify_ConnectedThingDestroyed(Thing thing)
		{
			if (this.connectedThings.Remove(thing))
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				bool flag;
				if (needs == null)
				{
					flag = (null != null);
				}
				else
				{
					Need_Mood mood = needs.mood;
					if (mood == null)
					{
						flag = (null != null);
					}
					else
					{
						ThoughtHandler thoughts = mood.thoughts;
						flag = (((thoughts != null) ? thoughts.memories : null) != null);
					}
				}
				if (flag)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ConnectedTreeDied, null, null);
				}
			}
		}

		// Token: 0x060087A2 RID: 34722 RVA: 0x003107CE File Offset: 0x0030E9CE
		public void Notify_ConnectedThingLeftBehind(Thing thing)
		{
			this.connectedThings.Remove(thing);
		}

		// Token: 0x060087A3 RID: 34723 RVA: 0x003107DD File Offset: 0x0030E9DD
		public IEnumerable<Gizmo> GetGizmos()
		{
			Pawn_ConnectionsTracker.<GetGizmos>d__10 <GetGizmos>d__ = new Pawn_ConnectionsTracker.<GetGizmos>d__10(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x060087A4 RID: 34724 RVA: 0x003107F0 File Offset: 0x0030E9F0
		private void DrawConnectionLines()
		{
			foreach (Thing t in this.ConnectedThings)
			{
				this.DrawConnectionLine(t);
			}
		}

		// Token: 0x060087A5 RID: 34725 RVA: 0x00310844 File Offset: 0x0030EA44
		public void DrawConnectionLine(Thing t)
		{
			if (t.Spawned && t.Map == this.pawn.Map)
			{
				GenDraw.DrawLineBetween(this.pawn.TrueCenter(), t.TrueCenter(), SimpleColor.Orange, 0.2f);
			}
		}

		// Token: 0x060087A6 RID: 34726 RVA: 0x00310880 File Offset: 0x0030EA80
		public void ExposeData()
		{
			Scribe_Collections.Look<Thing>(ref this.connectedThings, "connectedThings", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.connectedThings.RemoveAll((Thing x) => x == null || x.Destroyed);
			}
		}

		// Token: 0x040053BB RID: 21435
		private Pawn pawn;

		// Token: 0x040053BC RID: 21436
		private List<Thing> connectedThings = new List<Thing>();
	}
}
