using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010EF RID: 4335
	public class QuestPart_RequirePawnsCurrentlyOnShuttle : QuestPart
	{
		// Token: 0x06006C40 RID: 27712 RVA: 0x0025FDBC File Offset: 0x0025DFBC
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
			if (compShuttle.requiredPawns.Contains(pawn))
			{
				compShuttle.requiredPawns.Remove(pawn);
			}
		}

		// Token: 0x06006C41 RID: 27713 RVA: 0x0025FDF0 File Offset: 0x0025DFF0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal && this.shuttle != null)
			{
				CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
				compShuttle.requiredColonistCount = this.requiredColonistCount;
				compShuttle.requiredItems.Clear();
				compShuttle.requiredPawns.Clear();
				foreach (Thing thing in ((IEnumerable<Thing>)this.shuttle.TryGetComp<CompTransporter>().innerContainer))
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null && !compShuttle.requiredPawns.Contains(pawn))
					{
						compShuttle.requiredPawns.Add(pawn);
					}
				}
			}
		}

		// Token: 0x06006C42 RID: 27714 RVA: 0x0025FEB4 File Offset: 0x0025E0B4
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
		}

		// Token: 0x06006C43 RID: 27715 RVA: 0x0025FECE File Offset: 0x0025E0CE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignl", null, false);
			Scribe_Values.Look<int>(ref this.requiredColonistCount, "requiredColonistCount", 0, false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
		}

		// Token: 0x040044EA RID: 17642
		public string inSignal;

		// Token: 0x040044EB RID: 17643
		public Thing shuttle;

		// Token: 0x040044EC RID: 17644
		public int requiredColonistCount;
	}
}
