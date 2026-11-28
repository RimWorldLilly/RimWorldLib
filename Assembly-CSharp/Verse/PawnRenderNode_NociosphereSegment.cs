using System;

namespace Verse
{
	// Token: 0x020003A7 RID: 935
	public class PawnRenderNode_NociosphereSegment : PawnRenderNode
	{
		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001C33 RID: 7219 RVA: 0x000AE52E File Offset: 0x000AC72E
		public new PawnRenderNodeProperties_NociosphereSegment Props
		{
			get
			{
				return (PawnRenderNodeProperties_NociosphereSegment)this.props;
			}
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x000AE53B File Offset: 0x000AC73B
		public PawnRenderNode_NociosphereSegment(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x000AE546 File Offset: 0x000AC746
		public override Graphic GraphicFor(Pawn pawn)
		{
			return GraphicDatabase.Get<Graphic_Single>(this.Props.texPath, ShaderDatabase.CutoutComplexBlend, this.Props.maskPath);
		}
	}
}
