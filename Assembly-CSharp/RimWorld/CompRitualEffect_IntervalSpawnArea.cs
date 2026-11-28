using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200173B RID: 5947
	public class CompRitualEffect_IntervalSpawnArea : CompRitualEffect_IntervalSpawnBurst
	{
		// Token: 0x17001959 RID: 6489
		// (get) Token: 0x06009430 RID: 37936 RVA: 0x003604C8 File Offset: 0x0035E6C8
		protected new CompProperties_RitualEffectIntervalSpawnArea Props
		{
			get
			{
				return (CompProperties_RitualEffectIntervalSpawnArea)this.props;
			}
		}

		// Token: 0x06009431 RID: 37937 RVA: 0x003604D8 File Offset: 0x0035E6D8
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			CellRect cellRect = CellRect.CenteredOn(ritual.selectedTarget.Cell, this.Props.area.x / 2, this.Props.area.z / 2).ClipInsideMap(ritual.Map);
			if (this.Props.smoothEdges)
			{
				return cellRect.Cells.RandomElementByWeight((IntVec3 c) => 1f / Mathf.Max((c - ritual.selectedTarget.Cell).LengthHorizontal, 1f)).ToVector3Shifted() + Rand.UnitVector3 * 0.5f;
			}
			return cellRect.RandomVector3;
		}
	}
}
