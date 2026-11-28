using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003A6 RID: 934
	public class PawnRenderNode_Devourer : PawnRenderNode_AnimalPart_Body
	{
		// Token: 0x06001C31 RID: 7217 RVA: 0x000AE4C0 File Offset: 0x000AC6C0
		public PawnRenderNode_Devourer(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x000AE4CC File Offset: 0x000AC6CC
		public override Graphic GraphicFor(Pawn pawn)
		{
			CompDevourer compDevourer;
			if (pawn.TryGetComp(out compDevourer) && compDevourer.Digesting)
			{
				Graphic graphic = pawn.ageTracker.CurKindLifeStage.bodyGraphicData.Graphic;
				return GraphicDatabase.Get<Graphic_Multi>(graphic.path + "_Closed", ShaderDatabase.Cutout, graphic.drawSize, Color.white);
			}
			return base.GraphicFor(pawn);
		}
	}
}
