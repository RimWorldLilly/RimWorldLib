using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F7 RID: 6391
	public class CompProperties_MeditationFocus : CompProperties_StatOffsetBase
	{
		// Token: 0x0600A036 RID: 41014 RVA: 0x003960E5 File Offset: 0x003942E5
		public CompProperties_MeditationFocus()
		{
			this.compClass = typeof(CompMeditationFocus);
		}

		// Token: 0x0600A037 RID: 41015 RVA: 0x00396108 File Offset: 0x00394308
		public override IEnumerable<string> GetExplanationAbstract(ThingDef def)
		{
			CompProperties_MeditationFocus.<GetExplanationAbstract>d__2 <GetExplanationAbstract>d__ = new CompProperties_MeditationFocus.<GetExplanationAbstract>d__2(-2);
			<GetExplanationAbstract>d__.<>4__this = this;
			<GetExplanationAbstract>d__.<>3__def = def;
			return <GetExplanationAbstract>d__;
		}

		// Token: 0x0600A038 RID: 41016 RVA: 0x00396120 File Offset: 0x00394320
		public override void ResolveReferences(ThingDef parent)
		{
			base.PostLoadSpecial(parent);
			for (int i = 0; i < this.offsets.Count; i++)
			{
				this.offsets[i].ResolveReferences();
			}
		}

		// Token: 0x04005EC4 RID: 24260
		public List<MeditationFocusDef> focusTypes = new List<MeditationFocusDef>();
	}
}
