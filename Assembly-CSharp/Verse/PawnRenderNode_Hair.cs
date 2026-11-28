using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003AA RID: 938
	public class PawnRenderNode_Hair : PawnRenderNode
	{
		// Token: 0x06001C3C RID: 7228 RVA: 0x000AE677 File Offset: 0x000AC877
		public PawnRenderNode_Hair(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x000AE682 File Offset: 0x000AC882
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			Pawn_StoryTracker story = pawn.story;
			if (((story != null) ? story.hairDef : null) == null || pawn.story.hairDef.noGraphic)
			{
				return null;
			}
			return HumanlikeMeshPoolUtility.GetHumanlikeHairSetForPawn(pawn, 1f, 1f);
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x000AE6BC File Offset: 0x000AC8BC
		public override Graphic GraphicFor(Pawn pawn)
		{
			Pawn_StoryTracker story = pawn.story;
			if (((story != null) ? story.hairDef : null) == null || pawn.story.hairDef.noGraphic || pawn.DevelopmentalStage.Baby() || pawn.DevelopmentalStage.Newborn())
			{
				return null;
			}
			return pawn.story.hairDef.GraphicFor(pawn, this.ColorFor(pawn));
		}
	}
}
