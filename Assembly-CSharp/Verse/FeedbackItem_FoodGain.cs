using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005FE RID: 1534
	public class FeedbackItem_FoodGain : FeedbackItem
	{
		// Token: 0x06003028 RID: 12328 RVA: 0x00135269 File Offset: 0x00133469
		public FeedbackItem_FoodGain(Vector2 ScreenPos, int Amount) : base(ScreenPos)
		{
			this.Amount = Amount;
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x0013527C File Offset: 0x0013347C
		public override void FeedbackOnGUI()
		{
			string str = this.Amount.ToString() + " food";
			base.DrawFloatingText(str, Color.yellow);
		}

		// Token: 0x04001ED5 RID: 7893
		protected int Amount;
	}
}
