using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200173F RID: 5951
	public class CompRitualEffect_IntervalSpawnCircle : CompRitualEffect_IntervalSpawnBurst
	{
		// Token: 0x1700195C RID: 6492
		// (get) Token: 0x0600943A RID: 37946 RVA: 0x00360742 File Offset: 0x0035E942
		protected new CompProperties_RitualEffectIntervalSpawnCircle Props
		{
			get
			{
				return (CompProperties_RitualEffectIntervalSpawnCircle)this.props;
			}
		}

		// Token: 0x0600943B RID: 37947 RVA: 0x00360750 File Offset: 0x0035E950
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return CellRect.CenteredOn(ritual.selectedTarget.Cell, this.Props.area.x / 2, this.Props.area.z / 2).ClipInsideMap(ritual.Map).Cells.RandomElementByWeight(delegate(IntVec3 c)
			{
				float f = Mathf.Max(Mathf.Abs((c - ritual.selectedTarget.Cell).LengthHorizontal - this.Props.radius), 1f);
				return 1f / Mathf.Pow(f, this.Props.concentration);
			}).ToVector3Shifted() + Rand.UnitVector3 * 0.5f;
		}
	}
}
