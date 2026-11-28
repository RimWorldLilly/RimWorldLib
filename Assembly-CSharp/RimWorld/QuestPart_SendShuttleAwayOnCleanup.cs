using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F4 RID: 4340
	public class QuestPart_SendShuttleAwayOnCleanup : QuestPart
	{
		// Token: 0x17001284 RID: 4740
		// (get) Token: 0x06006C53 RID: 27731 RVA: 0x00260162 File Offset: 0x0025E362
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_SendShuttleAwayOnCleanup.<get_QuestLookTargets>d__3 <get_QuestLookTargets>d__ = new QuestPart_SendShuttleAwayOnCleanup.<get_QuestLookTargets>d__3(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006C54 RID: 27732 RVA: 0x00260172 File Offset: 0x0025E372
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.shuttle != null)
			{
				SendShuttleAwayQuestPartUtility.SendAway(this.shuttle, this.dropEverything);
			}
		}

		// Token: 0x06006C55 RID: 27733 RVA: 0x00260193 File Offset: 0x0025E393
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.dropEverything, "dropEverything", false, false);
		}

		// Token: 0x06006C56 RID: 27734 RVA: 0x002601BE File Offset: 0x0025E3BE
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.shuttle != null)
			{
				this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
			}
		}

		// Token: 0x040044F2 RID: 17650
		public Thing shuttle;

		// Token: 0x040044F3 RID: 17651
		public bool dropEverything;
	}
}
