using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001435 RID: 5173
	public class CompAbilityEffect_ReleaseGas : CompAbilityEffect
	{
		// Token: 0x170015D2 RID: 5586
		// (get) Token: 0x06007F8B RID: 32651 RVA: 0x002D8AF5 File Offset: 0x002D6CF5
		private new CompProperties_AbilityReleaseGas Props
		{
			get
			{
				return (CompProperties_AbilityReleaseGas)this.props;
			}
		}

		// Token: 0x170015D3 RID: 5587
		// (get) Token: 0x06007F8C RID: 32652 RVA: 0x002D8B02 File Offset: 0x002D6D02
		private int TotalGas
		{
			get
			{
				return Mathf.CeilToInt((float)(this.Props.cellsToFill * 255));
			}
		}

		// Token: 0x06007F8D RID: 32653 RVA: 0x002D8B1C File Offset: 0x002D6D1C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Pawn pawn = target.Pawn;
			if (this.Props.gasType == GasType.DeadlifeDust)
			{
				GasUtility.AddDeadifeGas(pawn.Position, pawn.Map, pawn.Faction, this.TotalGas);
			}
			else
			{
				GasUtility.AddGas(pawn.Position, pawn.Map, this.Props.gasType, this.TotalGas);
			}
			base.Apply(target, dest);
		}
	}
}
