using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003B9 RID: 953
	public class PawnRenderNodeProperties_Tattoo : PawnRenderNodeProperties
	{
		// Token: 0x06001C8B RID: 7307 RVA: 0x000AFD99 File Offset: 0x000ADF99
		public override void ResolveReferences()
		{
			this.skipFlag = RenderSkipFlagDefOf.Tattoos;
		}
	}
}
