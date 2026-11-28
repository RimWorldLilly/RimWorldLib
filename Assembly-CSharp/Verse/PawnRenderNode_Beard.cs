using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003AC RID: 940
	public class PawnRenderNode_Beard : PawnRenderNode
	{
		// Token: 0x06001C41 RID: 7233 RVA: 0x000AE740 File Offset: 0x000AC940
		public PawnRenderNode_Beard(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x000AE74B File Offset: 0x000AC94B
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			Pawn_StyleTracker style = pawn.style;
			if (((style != null) ? style.beardDef : null) == null || pawn.style.beardDef.noGraphic)
			{
				return null;
			}
			return HumanlikeMeshPoolUtility.GetHumanlikeBeardSetForPawn(pawn, 1f, 1f);
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x000AE788 File Offset: 0x000AC988
		public override Graphic GraphicFor(Pawn pawn)
		{
			Pawn_StyleTracker style = pawn.style;
			if (((style != null) ? style.beardDef : null) == null || pawn.style.beardDef.noGraphic)
			{
				return null;
			}
			return pawn.style.beardDef.GraphicFor(pawn, this.ColorFor(pawn));
		}
	}
}
