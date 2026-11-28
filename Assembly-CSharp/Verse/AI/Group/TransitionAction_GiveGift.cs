using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200095F RID: 2399
	public class TransitionAction_GiveGift : TransitionAction
	{
		// Token: 0x06004961 RID: 18785 RVA: 0x001CA430 File Offset: 0x001C8630
		public override void DoAction(Transition trans)
		{
			if (this.gifts.NullOrEmpty<Thing>())
			{
				return;
			}
			VisitorGiftForPlayerUtility.GiveGift(trans.target.lord.ownedPawns, trans.target.lord.faction, this.gifts);
			this.gifts.Clear();
		}

		// Token: 0x04002F0D RID: 12045
		public List<Thing> gifts;
	}
}
