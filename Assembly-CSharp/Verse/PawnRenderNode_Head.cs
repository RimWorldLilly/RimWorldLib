using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003A8 RID: 936
	public class PawnRenderNode_Head : PawnRenderNode
	{
		// Token: 0x06001C36 RID: 7222 RVA: 0x000AE568 File Offset: 0x000AC768
		public PawnRenderNode_Head(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x000AE574 File Offset: 0x000AC774
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			if (this.props.overrideMeshSize != null)
			{
				return MeshPool.GetMeshSetForSize(this.props.overrideMeshSize.Value.x, this.props.overrideMeshSize.Value.y);
			}
			return HumanlikeMeshPoolUtility.GetHumanlikeHeadSetForPawn(pawn, 1f, 1f);
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x000AE5D4 File Offset: 0x000AC7D4
		public override Graphic GraphicFor(Pawn pawn)
		{
			if (!pawn.health.hediffSet.HasHead)
			{
				return null;
			}
			if (pawn.Drawer.renderer.CurRotDrawMode == RotDrawMode.Dessicated)
			{
				return HeadTypeDefOf.Skull.GetGraphic(pawn, Color.white);
			}
			Pawn_StoryTracker story = pawn.story;
			if (story == null)
			{
				return null;
			}
			HeadTypeDef headType = story.headType;
			if (headType == null)
			{
				return null;
			}
			return headType.GetGraphic(pawn, this.ColorFor(pawn));
		}
	}
}
