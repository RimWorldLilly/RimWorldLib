using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018A8 RID: 6312
	public class CompProperties_GoldenCube : CompProperties_Interactable
	{
		// Token: 0x06009E08 RID: 40456 RVA: 0x0038BB0E File Offset: 0x00389D0E
		public CompProperties_GoldenCube()
		{
			this.compClass = typeof(CompGoldenCube);
		}

		// Token: 0x04005D82 RID: 23938
		[MustTranslate]
		public string letterDeactivatedLabel;

		// Token: 0x04005D83 RID: 23939
		[MustTranslate]
		public string letterDeactivatedDesc;

		// Token: 0x04005D84 RID: 23940
		[MustTranslate]
		public string letterDeactivatedAppend;
	}
}
