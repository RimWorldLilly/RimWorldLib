using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019E5 RID: 6629
	public class CompProperties_EntityHolderPlatform : CompProperties_EntityHolder
	{
		// Token: 0x0600A6EE RID: 42734 RVA: 0x003B7C33 File Offset: 0x003B5E33
		public CompProperties_EntityHolderPlatform()
		{
			this.compClass = typeof(CompEntityHolderPlatform);
		}

		// Token: 0x04006251 RID: 25169
		[NoTranslate]
		public string untetheredGraphicTexPath;

		// Token: 0x04006252 RID: 25170
		[NoTranslate]
		public string tilingChainTexPath;

		// Token: 0x04006253 RID: 25171
		[NoTranslate]
		public string baseChainFastenerTexPath;

		// Token: 0x04006254 RID: 25172
		[NoTranslate]
		public string targetChainFastenerTexPath;

		// Token: 0x04006255 RID: 25173
		public SoundDef entityLungeSoundHi;

		// Token: 0x04006256 RID: 25174
		public SoundDef entityLungeSoundLow;
	}
}
