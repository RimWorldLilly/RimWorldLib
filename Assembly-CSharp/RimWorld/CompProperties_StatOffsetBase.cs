using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001997 RID: 6551
	public class CompProperties_StatOffsetBase : CompProperties
	{
		// Token: 0x0600A46D RID: 42093 RVA: 0x003AC07F File Offset: 0x003AA27F
		public virtual IEnumerable<string> GetExplanationAbstract(ThingDef def)
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x0600A46E RID: 42094 RVA: 0x003AC088 File Offset: 0x003AA288
		public virtual float GetMaxOffset(Thing parent = null)
		{
			float num = 0f;
			for (int i = 0; i < this.offsets.Count; i++)
			{
				num += this.offsets[i].MaxOffset(parent);
			}
			return num;
		}

		// Token: 0x04006133 RID: 24883
		public StatDef statDef;

		// Token: 0x04006134 RID: 24884
		public List<FocusStrengthOffset> offsets = new List<FocusStrengthOffset>();
	}
}
