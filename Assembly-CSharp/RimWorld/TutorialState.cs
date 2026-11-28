using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E1F RID: 7711
	public class TutorialState : IExposable
	{
		// Token: 0x0600C859 RID: 51289 RVA: 0x004970D8 File Offset: 0x004952D8
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.startingItems != null)
			{
				this.startingItems.RemoveAll((Thing it) => it == null || it.Destroyed || (it.Map == null && it.MapHeld == null));
			}
			Scribe_Collections.Look<Thing>(ref this.startingItems, "startingItems", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<CellRect>(ref this.roomRect, "roomRect", default(CellRect), false);
			Scribe_Values.Look<CellRect>(ref this.sandbagsRect, "sandbagsRect", default(CellRect), false);
			Scribe_Values.Look<int>(ref this.endTick, "endTick", -1, false);
			Scribe_Values.Look<bool>(ref this.introDone, "introDone", false, false);
			if (this.startingItems != null)
			{
				this.startingItems.RemoveAll((Thing it) => it == null);
			}
		}

		// Token: 0x0600C85A RID: 51290 RVA: 0x004971C1 File Offset: 0x004953C1
		public void Notify_TutorialEnding()
		{
			this.startingItems.Clear();
			this.roomRect = default(CellRect);
			this.sandbagsRect = default(CellRect);
			this.endTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600C85B RID: 51291 RVA: 0x004971F6 File Offset: 0x004953F6
		public void AddStartingItem(Thing t)
		{
			if (this.startingItems.Contains(t))
			{
				return;
			}
			this.startingItems.Add(t);
		}

		// Token: 0x04007204 RID: 29188
		public List<Thing> startingItems = new List<Thing>();

		// Token: 0x04007205 RID: 29189
		public CellRect roomRect;

		// Token: 0x04007206 RID: 29190
		public CellRect sandbagsRect;

		// Token: 0x04007207 RID: 29191
		public int endTick = -1;

		// Token: 0x04007208 RID: 29192
		public bool introDone;
	}
}
