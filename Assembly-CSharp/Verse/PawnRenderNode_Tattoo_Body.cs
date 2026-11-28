using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200039E RID: 926
	public class PawnRenderNode_Tattoo_Body : PawnRenderNode_Tattoo
	{
		// Token: 0x06001C13 RID: 7187 RVA: 0x000AE1E5 File Offset: 0x000AC3E5
		public PawnRenderNode_Tattoo_Body(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x000AE1F0 File Offset: 0x000AC3F0
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			Pawn_StyleTracker style = pawn.style;
			if (((style != null) ? style.BodyTattoo : null) == null || pawn.style.BodyTattoo.noGraphic)
			{
				return null;
			}
			return HumanlikeMeshPoolUtility.GetHumanlikeBodySetForPawn(pawn, 1f, 1f);
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x000AE22C File Offset: 0x000AC42C
		public override Graphic GraphicFor(Pawn pawn)
		{
			if (!ModLister.CheckIdeology("Body tattoo"))
			{
				return null;
			}
			Pawn_StyleTracker style = pawn.style;
			if (((style != null) ? style.BodyTattoo : null) == null || pawn.style.BodyTattoo.noGraphic)
			{
				return null;
			}
			return pawn.style.BodyTattoo.GraphicFor(pawn, this.ColorFor(pawn));
		}
	}
}
