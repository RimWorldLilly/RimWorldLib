using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EDA RID: 3802
	public class CompProperties_Glower : CompProperties
	{
		// Token: 0x0600639F RID: 25503 RVA: 0x0023BF84 File Offset: 0x0023A184
		public CompProperties_Glower()
		{
			this.compClass = typeof(CompGlower);
		}

		// Token: 0x060063A0 RID: 25504 RVA: 0x0023BFD7 File Offset: 0x0023A1D7
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Glower.<ConfigErrors>d__7 <ConfigErrors>d__ = new CompProperties_Glower.<ConfigErrors>d__7(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x040037F4 RID: 14324
		public float overlightRadius;

		// Token: 0x040037F5 RID: 14325
		public float glowRadius = 14f;

		// Token: 0x040037F6 RID: 14326
		public ColorInt glowColor = new ColorInt(255, 255, 255, 0) * 1.45f;

		// Token: 0x040037F7 RID: 14327
		public bool colorPickerEnabled;

		// Token: 0x040037F8 RID: 14328
		public bool darklightToggle;

		// Token: 0x040037F9 RID: 14329
		public bool overrideIsCavePlant;
	}
}
