using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D0 RID: 4304
	public class QuestPart_MechCluster : QuestPart
	{
		// Token: 0x1700126A RID: 4714
		// (get) Token: 0x06006B9A RID: 27546 RVA: 0x0025BEC7 File Offset: 0x0025A0C7
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_MechCluster.<get_QuestLookTargets>d__7 <get_QuestLookTargets>d__ = new QuestPart_MechCluster.<get_QuestLookTargets>d__7(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006B9B RID: 27547 RVA: 0x0025BED8 File Offset: 0x0025A0D8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (this.mapParent == null || !this.mapParent.HasMap || !this.quest.IsParentSuitableForQuest(this.mapParent))
			{
				this.mapParent = this.quest.TryFindNewSuitableMapParentForRetarget();
			}
			if (signal.tag == this.inSignal && this.mapParent != null && this.mapParent.HasMap)
			{
				this.spawnedClusterPos = this.dropSpot;
				if (this.spawnedClusterPos == IntVec3.Invalid)
				{
					this.spawnedClusterPos = MechClusterUtility.FindClusterPosition(this.mapParent.Map, this.sketch, 100, 0.5f);
				}
				if (this.spawnedClusterPos == IntVec3.Invalid)
				{
					return;
				}
				MechClusterUtility.SpawnCluster(this.spawnedClusterPos, this.mapParent.Map, this.sketch, true, false, this.tag);
				Find.LetterStack.ReceiveLetter("LetterLabelMechClusterArrived".Translate(), "LetterMechClusterArrived".Translate(), LetterDefOf.ThreatBig, new TargetInfo(this.spawnedClusterPos, this.mapParent.Map, false), null, this.quest, null, null, 0, true);
			}
		}

		// Token: 0x06006B9C RID: 27548 RVA: 0x0025C01C File Offset: 0x0025A21C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Deep.Look<MechClusterSketch>(ref this.sketch, "sketch", Array.Empty<object>());
			Scribe_Values.Look<IntVec3>(ref this.dropSpot, "dropSpot", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.spawnedClusterPos, "spawnedClusterPos", default(IntVec3), false);
		}

		// Token: 0x04004464 RID: 17508
		public MechClusterSketch sketch;

		// Token: 0x04004465 RID: 17509
		public string inSignal;

		// Token: 0x04004466 RID: 17510
		public string tag;

		// Token: 0x04004467 RID: 17511
		public MapParent mapParent;

		// Token: 0x04004468 RID: 17512
		public IntVec3 dropSpot = IntVec3.Invalid;

		// Token: 0x04004469 RID: 17513
		private IntVec3 spawnedClusterPos = IntVec3.Invalid;
	}
}
