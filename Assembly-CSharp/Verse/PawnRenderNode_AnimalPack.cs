using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000398 RID: 920
	public class PawnRenderNode_AnimalPack : PawnRenderNode_AnimalPart
	{
		// Token: 0x06001C00 RID: 7168 RVA: 0x000AD9EE File Offset: 0x000ABBEE
		public PawnRenderNode_AnimalPack(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x000AD9FC File Offset: 0x000ABBFC
		public override Graphic GraphicFor(Pawn pawn)
		{
			if (!pawn.RaceProps.packAnimal)
			{
				return null;
			}
			PawnKindLifeStage curKindLifeStage = pawn.ageTracker.CurKindLifeStage;
			Graphic graphic;
			if (pawn.gender != Gender.Female || curKindLifeStage.femaleGraphicData == null)
			{
				graphic = curKindLifeStage.bodyGraphicData.Graphic;
			}
			else
			{
				graphic = curKindLifeStage.femaleGraphicData.Graphic;
			}
			return GraphicDatabase.Get<Graphic_Multi>(graphic.path + "Pack", ShaderDatabase.Cutout, graphic.drawSize, Color.white);
		}
	}
}
