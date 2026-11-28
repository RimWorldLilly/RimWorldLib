using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003C4 RID: 964
	public class PawnRenderNodeWorker_Fur : PawnRenderNodeWorker_Body
	{
		// Token: 0x06001CA0 RID: 7328 RVA: 0x000B02DF File Offset: 0x000AE4DF
		public override bool ShouldListOnGraph(PawnRenderNode node, PawnDrawParms parms)
		{
			Pawn_StoryTracker story = parms.pawn.story;
			return ((story != null) ? story.furDef : null) != null;
		}
	}
}
