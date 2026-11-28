using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200039A RID: 922
	public class PawnRenderNode_AnimalPart_Body : PawnRenderNode_AnimalPart
	{
		// Token: 0x06001C05 RID: 7173 RVA: 0x000ADDBB File Offset: 0x000ABFBB
		public PawnRenderNode_AnimalPart_Body(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x000ADDC6 File Offset: 0x000ABFC6
		[return: TupleElementNames(new string[]
		{
			"state",
			"graphic"
		})]
		protected override IEnumerable<ValueTuple<GraphicStateDef, Graphic>> StateGraphicsFor(Pawn pawn)
		{
			PawnRenderNode_AnimalPart_Body.<StateGraphicsFor>d__1 <StateGraphicsFor>d__ = new PawnRenderNode_AnimalPart_Body.<StateGraphicsFor>d__1(-2);
			<StateGraphicsFor>d__.<>4__this = this;
			<StateGraphicsFor>d__.<>3__pawn = pawn;
			return <StateGraphicsFor>d__;
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x000ADDE0 File Offset: 0x000ABFE0
		[return: TupleElementNames(new string[]
		{
			"state",
			"graphic"
		})]
		private ValueTuple<GraphicStateDef, Graphic>? GetSwimmingGraphic(Pawn pawn)
		{
			PawnKindLifeStage curKindLifeStage = pawn.ageTracker.CurKindLifeStage;
			if (curKindLifeStage.swimmingGraphicData == null)
			{
				return null;
			}
			Graphic graphic = curKindLifeStage.swimmingGraphicData.Graphic;
			if (pawn.gender == Gender.Female && curKindLifeStage.femaleSwimmingGraphicData != null)
			{
				graphic = curKindLifeStage.femaleSwimmingGraphicData.Graphic;
			}
			AlternateGraphic alternateGraphic;
			int num;
			if (pawn.TryGetAlternate(out alternateGraphic, out num))
			{
				graphic = alternateGraphic.GetSwimmingGraphic(graphic);
			}
			Color color = graphic.Color;
			Color color2 = graphic.ColorTwo;
			if (pawn.IsMutant)
			{
				color = MutantUtility.GetMutantSkinColor(pawn, color);
				color2 = MutantUtility.GetMutantSkinColor(pawn, color2);
			}
			color = pawn.health.hediffSet.GetSkinColor(color);
			color2 = pawn.health.hediffSet.GetSkinColor(color2);
			graphic = graphic.GetColoredVersion(graphic.Shader, color, color2);
			return new ValueTuple<GraphicStateDef, Graphic>?(new ValueTuple<GraphicStateDef, Graphic>(GraphicStateDefOf.Swimming, graphic));
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x000ADEB8 File Offset: 0x000AC0B8
		[return: TupleElementNames(new string[]
		{
			"state",
			"graphic"
		})]
		private ValueTuple<GraphicStateDef, Graphic>? GetStationaryGraphic(Pawn pawn)
		{
			PawnKindLifeStage curKindLifeStage = pawn.ageTracker.CurKindLifeStage;
			if (curKindLifeStage.stationaryGraphicData == null)
			{
				return null;
			}
			Graphic graphic = curKindLifeStage.stationaryGraphicData.Graphic;
			if (pawn.gender == Gender.Female && curKindLifeStage.femaleStationaryGraphicData != null)
			{
				graphic = curKindLifeStage.femaleStationaryGraphicData.Graphic;
			}
			AlternateGraphic alternateGraphic;
			int num;
			if (pawn.TryGetAlternate(out alternateGraphic, out num))
			{
				graphic = alternateGraphic.GetStationaryGraphic(graphic);
			}
			Color color = graphic.Color;
			Color color2 = graphic.ColorTwo;
			if (pawn.IsMutant)
			{
				color = MutantUtility.GetMutantSkinColor(pawn, color);
				color2 = MutantUtility.GetMutantSkinColor(pawn, color2);
			}
			color = pawn.health.hediffSet.GetSkinColor(color);
			color2 = pawn.health.hediffSet.GetSkinColor(color2);
			graphic = graphic.GetColoredVersion(graphic.Shader, color, color2);
			return new ValueTuple<GraphicStateDef, Graphic>?(new ValueTuple<GraphicStateDef, Graphic>(GraphicStateDefOf.Stationary, graphic));
		}
	}
}
