using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F98 RID: 8088
	public class StatPart_OverseerStatOffset : StatPart
	{
		// Token: 0x0600CD8D RID: 52621 RVA: 0x004B8D4C File Offset: 0x004B6F4C
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetOffset(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x0600CD8E RID: 52622 RVA: 0x004B8D6C File Offset: 0x004B6F6C
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetOffset(req, out num) && num != 0f)
			{
				return this.label + ": +" + num.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600CD8F RID: 52623 RVA: 0x004B8DA4 File Offset: 0x004B6FA4
		private bool TryGetOffset(StatRequest req, out float offset)
		{
			if (ModsConfig.BiotechActive && req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					Pawn overseer = pawn.GetOverseer();
					if (overseer != null)
					{
						offset = overseer.GetStatValue(this.stat, true, -1);
						return true;
					}
				}
			}
			offset = 0f;
			return false;
		}

		// Token: 0x04008311 RID: 33553
		private StatDef stat;

		// Token: 0x04008312 RID: 33554
		[MustTranslate]
		private string label;
	}
}
