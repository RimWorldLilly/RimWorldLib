using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001060 RID: 4192
	public class QuestPart_Filter_AllThingsHacked : QuestPart_Filter
	{
		// Token: 0x06006964 RID: 26980 RVA: 0x00252F3C File Offset: 0x0025113C
		protected override bool Pass(SignalArgs args)
		{
			if (this.things.NullOrEmpty<Thing>())
			{
				return false;
			}
			foreach (Thing thing in this.things)
			{
				if (!thing.IsHackable() || !thing.IsHacked())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006965 RID: 26981 RVA: 0x00252FB0 File Offset: 0x002511B0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x0400430D RID: 17165
		public List<Thing> things = new List<Thing>();
	}
}
