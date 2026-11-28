using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020003A5 RID: 933
	public class PawnRenderNode_Apparel : PawnRenderNode
	{
		// Token: 0x06001C2B RID: 7211 RVA: 0x000AE386 File Offset: 0x000AC586
		public PawnRenderNode_Apparel(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
			this.useHeadMesh = (props.parentTagDef == PawnRenderNodeTagDefOf.ApparelHead);
			this.meshSet = this.MeshSetFor(pawn);
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x000AE3B1 File Offset: 0x000AC5B1
		public PawnRenderNode_Apparel(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree, Apparel apparel) : base(pawn, props, tree)
		{
			this.apparel = apparel;
			this.useHeadMesh = (props.parentTagDef == PawnRenderNodeTagDefOf.ApparelHead);
			this.meshSet = this.MeshSetFor(pawn);
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x000AE3E4 File Offset: 0x000AC5E4
		public PawnRenderNode_Apparel(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree, Apparel apparel, bool useHeadMesh) : base(pawn, props, tree)
		{
			this.apparel = apparel;
			this.useHeadMesh = useHeadMesh;
			this.meshSet = this.MeshSetFor(pawn);
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x000AE40C File Offset: 0x000AC60C
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			if (this.apparel == null)
			{
				return base.MeshSetFor(pawn);
			}
			if (base.Props.overrideMeshSize != null)
			{
				return MeshPool.GetMeshSetForSize(base.Props.overrideMeshSize.Value.x, base.Props.overrideMeshSize.Value.y);
			}
			if (this.useHeadMesh)
			{
				return HumanlikeMeshPoolUtility.GetHumanlikeHeadSetForPawn(pawn, 1f, 1f);
			}
			return HumanlikeMeshPoolUtility.GetHumanlikeBodySetForPawn(pawn, 1f, 1f);
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x000AE494 File Offset: 0x000AC694
		protected override IEnumerable<Graphic> GraphicsFor(Pawn pawn)
		{
			PawnRenderNode_Apparel.<GraphicsFor>d__5 <GraphicsFor>d__ = new PawnRenderNode_Apparel.<GraphicsFor>d__5(-2);
			<GraphicsFor>d__.<>4__this = this;
			<GraphicsFor>d__.<>3__pawn = pawn;
			return <GraphicsFor>d__;
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x000AE4AB File Offset: 0x000AC6AB
		protected override int TexSeedFor(Pawn pawn)
		{
			return base.TexSeedFor(pawn) + this.apparel.thingIDNumber;
		}

		// Token: 0x04001704 RID: 5892
		public bool useHeadMesh;
	}
}
