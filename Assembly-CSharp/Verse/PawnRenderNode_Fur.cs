using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200039C RID: 924
	public class PawnRenderNode_Fur : PawnRenderNode
	{
		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06001C0C RID: 7180 RVA: 0x000AE0D1 File Offset: 0x000AC2D1
		protected override Shader DefaultShader
		{
			get
			{
				return ShaderDatabase.CutoutSkinOverlay;
			}
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x000AE0D8 File Offset: 0x000AC2D8
		public PawnRenderNode_Fur(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x000AE0E4 File Offset: 0x000AC2E4
		public override Graphic GraphicFor(Pawn pawn)
		{
			if (!ModLister.CheckBiotech("Fur"))
			{
				return null;
			}
			Pawn_StoryTracker story = pawn.story;
			if (((story != null) ? story.furDef : null) == null)
			{
				return null;
			}
			Pawn_StoryTracker story2 = pawn.story;
			return GraphicDatabase.Get<Graphic_Multi>((story2 != null) ? story2.furDef.GetFurBodyGraphicPath(pawn) : null, base.ShaderFor(pawn), Vector2.one, this.ColorFor(pawn));
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x000AE145 File Offset: 0x000AC345
		public override Color ColorFor(Pawn pawn)
		{
			return pawn.story.HairColor;
		}
	}
}
