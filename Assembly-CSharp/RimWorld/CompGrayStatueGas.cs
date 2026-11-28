using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019EF RID: 6639
	public class CompGrayStatueGas : CompGrayStatue
	{
		// Token: 0x17001D47 RID: 7495
		// (get) Token: 0x0600A72F RID: 42799 RVA: 0x003B9086 File Offset: 0x003B7286
		private CompProperties_GrayStatueGas Props
		{
			get
			{
				return (CompProperties_GrayStatueGas)this.props;
			}
		}

		// Token: 0x17001D48 RID: 7496
		// (get) Token: 0x0600A730 RID: 42800 RVA: 0x003B9093 File Offset: 0x003B7293
		private int GasAmount
		{
			get
			{
				return Mathf.Min(Mathf.CeilToInt((float)this.parent.GetRoom(RegionType.Set_All).CellCount * 0.15f * 255f), 12750);
			}
		}

		// Token: 0x0600A731 RID: 42801 RVA: 0x003B90C4 File Offset: 0x003B72C4
		protected override void Trigger(Pawn target)
		{
			if (this.Props.gas == GasType.DeadlifeDust)
			{
				GasUtility.AddDeadifeGas(this.parent.Position, this.parent.Map, Faction.OfEntities, this.GasAmount);
				return;
			}
			GasUtility.AddGas(this.parent.Position, this.parent.Map, this.Props.gas, this.GasAmount);
		}

		// Token: 0x0600A732 RID: 42802 RVA: 0x003B9133 File Offset: 0x003B7333
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompGrayStatueGas.<CompGetGizmosExtra>d__7 <CompGetGizmosExtra>d__ = new CompGrayStatueGas.<CompGetGizmosExtra>d__7(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0400627B RID: 25211
		private const float RoomCellGasFactor = 0.15f;

		// Token: 0x0400627C RID: 25212
		private const int MaxGas = 12750;
	}
}
