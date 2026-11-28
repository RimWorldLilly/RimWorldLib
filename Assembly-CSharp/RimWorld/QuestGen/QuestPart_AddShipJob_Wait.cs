using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021A4 RID: 8612
	public abstract class QuestPart_AddShipJob_Wait : QuestPart_AddShipJob
	{
		// Token: 0x0600D6C9 RID: 54985 RVA: 0x004FBDB8 File Offset: 0x004F9FB8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.leaveImmediatelyWhenSatisfied, "leaveImmediatelyWhenSatisfied", false, false);
			Scribe_Values.Look<bool>(ref this.showGizmos, "showGizmos", false, false);
			Scribe_Collections.Look<Thing>(ref this.sendAwayIfAllDespawned, "sendAwayIfAllDespawned", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.sendAwayIfAnyDespawnedDownedOrDead, "sendAwayIfAnyDespawnedDownedOrDead", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<Thing> list = this.sendAwayIfAllDespawned;
				if (list != null)
				{
					list.RemoveAll((Thing x) => x == null);
				}
				List<Thing> list2 = this.sendAwayIfAnyDespawnedDownedOrDead;
				if (list2 == null)
				{
					return;
				}
				list2.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x0400880E RID: 34830
		public bool leaveImmediatelyWhenSatisfied;

		// Token: 0x0400880F RID: 34831
		public bool showGizmos = true;

		// Token: 0x04008810 RID: 34832
		public List<Thing> sendAwayIfAllDespawned;

		// Token: 0x04008811 RID: 34833
		public List<Thing> sendAwayIfAnyDespawnedDownedOrDead;
	}
}
