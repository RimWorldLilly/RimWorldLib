using System;

namespace Verse
{
	// Token: 0x020003A9 RID: 937
	public class PawnRenderNode_Stump : PawnRenderNode
	{
		// Token: 0x06001C39 RID: 7225 RVA: 0x000AE63D File Offset: 0x000AC83D
		public PawnRenderNode_Stump(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x000AE648 File Offset: 0x000AC848
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			return HumanlikeMeshPoolUtility.GetHumanlikeHeadSetForPawn(pawn, 1f, 1f);
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x000AE65A File Offset: 0x000AC85A
		public override Graphic GraphicFor(Pawn pawn)
		{
			if (!pawn.health.hediffSet.HasHead)
			{
				return base.GraphicFor(pawn);
			}
			return null;
		}
	}
}
