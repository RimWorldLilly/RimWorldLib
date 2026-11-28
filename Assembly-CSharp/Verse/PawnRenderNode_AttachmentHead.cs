using System;

namespace Verse
{
	// Token: 0x020003AB RID: 939
	public class PawnRenderNode_AttachmentHead : PawnRenderNode
	{
		// Token: 0x06001C3F RID: 7231 RVA: 0x000AE723 File Offset: 0x000AC923
		public PawnRenderNode_AttachmentHead(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x000AE72E File Offset: 0x000AC92E
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			return HumanlikeMeshPoolUtility.GetHumanlikeHairSetForPawn(pawn, 1f, 1f);
		}
	}
}
