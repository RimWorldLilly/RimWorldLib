using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B6 RID: 4278
	public class QuestPart_DestroyThingsOrPassToWorldOnCleanup : QuestPart
	{
		// Token: 0x17001254 RID: 4692
		// (get) Token: 0x06006B09 RID: 27401 RVA: 0x002593FB File Offset: 0x002575FB
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_DestroyThingsOrPassToWorldOnCleanup.<get_QuestLookTargets>d__3 <get_QuestLookTargets>d__ = new QuestPart_DestroyThingsOrPassToWorldOnCleanup.<get_QuestLookTargets>d__3(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006B0A RID: 27402 RVA: 0x0025940B File Offset: 0x0025760B
		public override void Cleanup()
		{
			base.Cleanup();
			QuestPart_DestroyThingsOrPassToWorld.Destroy(this.things);
		}

		// Token: 0x06006B0B RID: 27403 RVA: 0x00259420 File Offset: 0x00257620
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.questLookTargets, "questLookTargets", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04004409 RID: 17417
		public List<Thing> things = new List<Thing>();

		// Token: 0x0400440A RID: 17418
		public bool questLookTargets = true;
	}
}
