using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B5 RID: 4277
	public class QuestPart_DestroyThingsOrPassToWorld : QuestPart
	{
		// Token: 0x17001253 RID: 4691
		// (get) Token: 0x06006B02 RID: 27394 RVA: 0x0025925B File Offset: 0x0025745B
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_DestroyThingsOrPassToWorld.<get_QuestLookTargets>d__4 <get_QuestLookTargets>d__ = new QuestPart_DestroyThingsOrPassToWorld.<get_QuestLookTargets>d__4(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006B03 RID: 27395 RVA: 0x0025926B File Offset: 0x0025746B
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				QuestPart_DestroyThingsOrPassToWorld.Destroy(this.things);
			}
		}

		// Token: 0x06006B04 RID: 27396 RVA: 0x00259294 File Offset: 0x00257494
		public static void Destroy(List<Thing> things)
		{
			for (int i = things.Count - 1; i >= 0; i--)
			{
				QuestPart_DestroyThingsOrPassToWorld.Destroy(things[i]);
			}
		}

		// Token: 0x06006B05 RID: 27397 RVA: 0x002592C0 File Offset: 0x002574C0
		public static void Destroy(Thing thing)
		{
			Thing thing2;
			if (thing.ParentHolder is MinifiedThing)
			{
				thing2 = (Thing)thing.ParentHolder;
			}
			else
			{
				thing2 = thing;
			}
			if (!thing2.Destroyed)
			{
				thing2.DestroyOrPassToWorld(DestroyMode.QuestLogic);
			}
		}

		// Token: 0x06006B06 RID: 27398 RVA: 0x002592FC File Offset: 0x002574FC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.questLookTargets, "questLookTargets", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x06006B07 RID: 27399 RVA: 0x0025937C File Offset: 0x0025757C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				List<Thing> source = Find.RandomPlayerHomeMap.listerThings.ThingsInGroup(ThingRequestGroup.Plant);
				this.things.Clear();
				this.things.Add(source.FirstOrDefault<Thing>());
			}
		}

		// Token: 0x04004406 RID: 17414
		public string inSignal;

		// Token: 0x04004407 RID: 17415
		public List<Thing> things = new List<Thing>();

		// Token: 0x04004408 RID: 17416
		public bool questLookTargets = true;
	}
}
