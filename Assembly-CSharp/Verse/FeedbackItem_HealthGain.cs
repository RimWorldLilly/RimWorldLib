using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005FF RID: 1535
	public class FeedbackItem_HealthGain : FeedbackItem
	{
		// Token: 0x0600302A RID: 12330 RVA: 0x001352AB File Offset: 0x001334AB
		public FeedbackItem_HealthGain(Vector2 ScreenPos, int Amount, Pawn Healer) : base(ScreenPos)
		{
			this.Amount = Amount;
			this.Healer = Healer;
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x001352C4 File Offset: 0x001334C4
		public override void FeedbackOnGUI()
		{
			string text;
			if (this.Amount >= 0)
			{
				text = "+";
			}
			else
			{
				text = "-";
			}
			text += this.Amount.ToString();
			base.DrawFloatingText(text, Color.red);
		}

		// Token: 0x04001ED6 RID: 7894
		protected Pawn Healer;

		// Token: 0x04001ED7 RID: 7895
		protected int Amount;
	}
}
