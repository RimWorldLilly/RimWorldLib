using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200039B RID: 923
	public class PawnRenderNode_Body : PawnRenderNode
	{
		// Token: 0x06001C0A RID: 7178 RVA: 0x000ADF99 File Offset: 0x000AC199
		public PawnRenderNode_Body(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x000ADFA4 File Offset: 0x000AC1A4
		public override Graphic GraphicFor(Pawn pawn)
		{
			Shader shader = base.ShaderFor(pawn);
			if (shader == null)
			{
				return null;
			}
			if (pawn.Drawer.renderer.CurRotDrawMode == RotDrawMode.Dessicated)
			{
				return GraphicDatabase.Get<Graphic_Multi>(pawn.story.bodyType.bodyDessicatedGraphicPath, shader);
			}
			if (pawn.IsMutant && !pawn.mutant.Def.bodyTypeGraphicPaths.NullOrEmpty<BodyTypeGraphicData>())
			{
				string bodyGraphicPath = pawn.mutant.Def.GetBodyGraphicPath(pawn);
				if (bodyGraphicPath != null)
				{
					return GraphicDatabase.Get<Graphic_Multi>(bodyGraphicPath, shader, Vector2.one, this.ColorFor(pawn));
				}
			}
			if (ModsConfig.AnomalyActive && pawn.IsCreepJoiner && pawn.story.bodyType != null && !pawn.creepjoiner.form.bodyTypeGraphicPaths.NullOrEmpty<BodyTypeGraphicData>())
			{
				return GraphicDatabase.Get<Graphic_Multi>(pawn.creepjoiner.form.GetBodyGraphicPath(pawn), shader, Vector2.one, this.ColorFor(pawn));
			}
			Pawn_StoryTracker story = pawn.story;
			bool flag;
			if (story == null)
			{
				flag = (null != null);
			}
			else
			{
				BodyTypeDef bodyType = story.bodyType;
				flag = (((bodyType != null) ? bodyType.bodyNakedGraphicPath : null) != null);
			}
			if (!flag)
			{
				return null;
			}
			return GraphicDatabase.Get<Graphic_Multi>(pawn.story.bodyType.bodyNakedGraphicPath, shader, Vector2.one, this.ColorFor(pawn));
		}
	}
}
