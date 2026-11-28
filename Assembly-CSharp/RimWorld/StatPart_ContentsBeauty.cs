using System;

namespace RimWorld
{
	// Token: 0x02001F7A RID: 8058
	public class StatPart_ContentsBeauty : StatPart
	{
		// Token: 0x0600CD09 RID: 52489 RVA: 0x004B6E8C File Offset: 0x004B508C
		public override void TransformValue(StatRequest req, ref float val)
		{
			IBeautyContainer beautyContainer = req.Thing as IBeautyContainer;
			if (beautyContainer != null)
			{
				val += beautyContainer.BeautyOffset;
			}
		}

		// Token: 0x0600CD0A RID: 52490 RVA: 0x004B6EB4 File Offset: 0x004B50B4
		public override string ExplanationPart(StatRequest req)
		{
			IBeautyContainer beautyContainer = req.Thing as IBeautyContainer;
			if (beautyContainer != null)
			{
				return beautyContainer.BeautyOffsetExplanation;
			}
			return null;
		}
	}
}
