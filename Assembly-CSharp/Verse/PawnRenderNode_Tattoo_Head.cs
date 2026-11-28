using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003AD RID: 941
	public class PawnRenderNode_Tattoo_Head : PawnRenderNode_Tattoo
	{
		// Token: 0x06001C44 RID: 7236 RVA: 0x000AE7D5 File Offset: 0x000AC9D5
		public PawnRenderNode_Tattoo_Head(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x000AE7E0 File Offset: 0x000AC9E0
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			Pawn_StyleTracker style = pawn.style;
			if (((style != null) ? style.FaceTattoo : null) == null || pawn.style.FaceTattoo.noGraphic)
			{
				return null;
			}
			return HumanlikeMeshPoolUtility.GetHumanlikeHairSetForPawn(pawn, 1f, 1f);
		}

		// Token: 0x06001C46 RID: 7238 RVA: 0x000AE81C File Offset: 0x000ACA1C
		public override Graphic GraphicFor(Pawn pawn)
		{
			if (!ModLister.CheckIdeology("Head tattoo"))
			{
				return null;
			}
			Pawn_StyleTracker style = pawn.style;
			if (((style != null) ? style.FaceTattoo : null) == null || pawn.style.FaceTattoo.noGraphic)
			{
				return null;
			}
			return pawn.style.FaceTattoo.GraphicFor(pawn, this.ColorFor(pawn));
		}
	}
}
