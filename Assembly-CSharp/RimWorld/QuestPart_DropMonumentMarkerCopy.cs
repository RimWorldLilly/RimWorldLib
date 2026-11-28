using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010BD RID: 4285
	public class QuestPart_DropMonumentMarkerCopy : QuestPart
	{
		// Token: 0x1700125A RID: 4698
		// (get) Token: 0x06006B2A RID: 27434 RVA: 0x00259C14 File Offset: 0x00257E14
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_DropMonumentMarkerCopy.<get_QuestLookTargets>d__6 <get_QuestLookTargets>d__ = new QuestPart_DropMonumentMarkerCopy.<get_QuestLookTargets>d__6(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006B2B RID: 27435 RVA: 0x00259C24 File Offset: 0x00257E24
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.copy = null;
				MonumentMarker arg = signal.args.GetArg<MonumentMarker>("SUBJECT");
				if (this.mapParent == null || !this.mapParent.HasMap || !this.quest.IsParentSuitableForQuest(this.mapParent))
				{
					this.mapParent = this.quest.TryFindNewSuitableMapParentForRetarget();
				}
				if (arg != null && this.mapParent != null && this.mapParent.HasMap)
				{
					Map map = this.mapParent.Map;
					IntVec3 dropCenter = DropCellFinder.RandomDropSpot(map, true);
					this.copy = (MonumentMarker)ThingMaker.MakeThing(ThingDefOf.MonumentMarker, null);
					this.copy.sketch = arg.sketch.DeepCopy();
					if (!arg.questTags.NullOrEmpty<string>())
					{
						this.copy.questTags = new List<string>();
						this.copy.questTags.AddRange(arg.questTags);
					}
					DropPodUtility.DropThingsNear(dropCenter, map, Gen.YieldSingle<Thing>(this.copy.MakeMinified(DestroyMode.Vanish)), 110, false, false, true, false, true, null);
				}
				if (!this.outSignalResult.NullOrEmpty())
				{
					if (this.copy != null)
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalResult, this.copy.Named("SUBJECT")));
						return;
					}
					Find.SignalManager.SendSignal(new Signal(this.outSignalResult, false));
				}
			}
		}

		// Token: 0x06006B2C RID: 27436 RVA: 0x00259DA3 File Offset: 0x00257FA3
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.destroyOrPassToWorldOnCleanup && this.copy != null)
			{
				QuestPart_DestroyThingsOrPassToWorld.Destroy(this.copy);
			}
		}

		// Token: 0x06006B2D RID: 27437 RVA: 0x00259DC8 File Offset: 0x00257FC8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalResult, "outSignalResult", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<MonumentMarker>(ref this.copy, "copy", false);
			Scribe_Values.Look<bool>(ref this.destroyOrPassToWorldOnCleanup, "destroyOrPassToWorldOnCleanup", false, false);
		}

		// Token: 0x06006B2E RID: 27438 RVA: 0x00259E34 File Offset: 0x00258034
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
			}
		}

		// Token: 0x0400441D RID: 17437
		public MapParent mapParent;

		// Token: 0x0400441E RID: 17438
		public string inSignal;

		// Token: 0x0400441F RID: 17439
		public string outSignalResult;

		// Token: 0x04004420 RID: 17440
		public bool destroyOrPassToWorldOnCleanup;

		// Token: 0x04004421 RID: 17441
		private MonumentMarker copy;
	}
}
