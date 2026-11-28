using System;

namespace Verse
{
	// Token: 0x020003AE RID: 942
	public class PawnRenderNode_Parent : PawnRenderNode
	{
		// Token: 0x06001C47 RID: 7239 RVA: 0x000AE877 File Offset: 0x000ACA77
		public PawnRenderNode_Parent(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x000AE882 File Offset: 0x000ACA82
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x000AE885 File Offset: 0x000ACA85
		public override Graphic GraphicFor(Pawn pawn)
		{
			return null;
		}
	}
}
