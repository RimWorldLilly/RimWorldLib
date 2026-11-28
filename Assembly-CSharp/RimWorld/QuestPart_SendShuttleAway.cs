using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F2 RID: 4338
	public class QuestPart_SendShuttleAway : QuestPart
	{
		// Token: 0x17001283 RID: 4739
		// (get) Token: 0x06006C4C RID: 27724 RVA: 0x0025FFD3 File Offset: 0x0025E1D3
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_SendShuttleAway.<get_QuestLookTargets>d__4 <get_QuestLookTargets>d__ = new QuestPart_SendShuttleAway.<get_QuestLookTargets>d__4(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006C4D RID: 27725 RVA: 0x0025FFE3 File Offset: 0x0025E1E3
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.shuttle != null)
			{
				SendShuttleAwayQuestPartUtility.SendAway(this.shuttle, this.dropEverything);
			}
		}

		// Token: 0x06006C4E RID: 27726 RVA: 0x00260018 File Offset: 0x0025E218
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.dropEverything, "dropEverything", false, false);
		}

		// Token: 0x06006C4F RID: 27727 RVA: 0x00260058 File Offset: 0x0025E258
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
				IntVec3 center = DropCellFinder.RandomDropSpot(randomPlayerHomeMap, true);
				this.shuttle = ThingMaker.MakeThing(ThingDefOf.Shuttle, null);
				GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShuttleIncoming, this.shuttle), center, randomPlayerHomeMap, ThingPlaceMode.Near, null, null, null, 1);
			}
		}

		// Token: 0x06006C50 RID: 27728 RVA: 0x002600D3 File Offset: 0x0025E2D3
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.shuttle != null)
			{
				this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
			}
		}

		// Token: 0x040044EF RID: 17647
		public string inSignal;

		// Token: 0x040044F0 RID: 17648
		public Thing shuttle;

		// Token: 0x040044F1 RID: 17649
		public bool dropEverything;
	}
}
