using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001095 RID: 4245
	public class QuestPart_AssaultThings : QuestPart_MakeLord
	{
		// Token: 0x06006A56 RID: 27222 RVA: 0x002565FA File Offset: 0x002547FA
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_AssaultThings(this.faction, this.things, 1f, false), base.Map, null);
		}

		// Token: 0x06006A57 RID: 27223 RVA: 0x00256628 File Offset: 0x00254828
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04004387 RID: 17287
		public List<Thing> things = new List<Thing>();
	}
}
