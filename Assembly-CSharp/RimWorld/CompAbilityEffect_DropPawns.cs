using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013F2 RID: 5106
	public class CompAbilityEffect_DropPawns : CompAbilityEffect_WithDest
	{
		// Token: 0x170015AE RID: 5550
		// (get) Token: 0x06007EBD RID: 32445 RVA: 0x002D4C4B File Offset: 0x002D2E4B
		public new CompProperties_DropPawns Props
		{
			get
			{
				return (CompProperties_DropPawns)this.props;
			}
		}

		// Token: 0x06007EBE RID: 32446 RVA: 0x002D4C58 File Offset: 0x002D2E58
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < this.Props.amount; i++)
			{
				Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(this.Props.pawnKindDef, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				list.Add(item);
			}
			DropPodUtility.DropThingsNear(target.Cell, this.parent.pawn.Map, list, 110, false, false, true, true, true, null);
		}
	}
}
