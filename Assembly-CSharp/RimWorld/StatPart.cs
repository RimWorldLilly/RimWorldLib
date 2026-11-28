using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F6A RID: 8042
	public abstract class StatPart
	{
		// Token: 0x0600CCC9 RID: 52425
		public abstract void TransformValue(StatRequest req, ref float val);

		// Token: 0x0600CCCA RID: 52426
		public abstract string ExplanationPart(StatRequest req);

		// Token: 0x0600CCCB RID: 52427 RVA: 0x004B6297 File Offset: 0x004B4497
		public virtual IEnumerable<string> ConfigErrors()
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x0600CCCC RID: 52428 RVA: 0x004B629E File Offset: 0x004B449E
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			return Enumerable.Empty<Dialog_InfoCard.Hyperlink>();
		}

		// Token: 0x0600CCCD RID: 52429 RVA: 0x004B62A5 File Offset: 0x004B44A5
		public virtual bool ForceShow(StatRequest req)
		{
			return false;
		}

		// Token: 0x040082E4 RID: 33508
		public float priority;

		// Token: 0x040082E5 RID: 33509
		[Unsaved(false)]
		public StatDef parentStat;
	}
}
