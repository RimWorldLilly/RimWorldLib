using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200053E RID: 1342
	public class CompProperties_ColorableAnimated : CompProperties
	{
		// Token: 0x06002772 RID: 10098 RVA: 0x000F66DE File Offset: 0x000F48DE
		public CompProperties_ColorableAnimated()
		{
			this.compClass = typeof(CompColorable_Animated);
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x000F6708 File Offset: 0x000F4908
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_ColorableAnimated.<ConfigErrors>d__4 <ConfigErrors>d__ = new CompProperties_ColorableAnimated.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04001C48 RID: 7240
		public int changeInterval = 1;

		// Token: 0x04001C49 RID: 7241
		public bool startWithRandom;

		// Token: 0x04001C4A RID: 7242
		public List<Color> colors = new List<Color>();
	}
}
