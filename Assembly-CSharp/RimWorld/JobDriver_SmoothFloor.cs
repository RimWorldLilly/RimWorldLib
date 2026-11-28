using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C7 RID: 2503
	public class JobDriver_SmoothFloor : JobDriver_AffectFloor
	{
		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x06004C08 RID: 19464 RVA: 0x001D6497 File Offset: 0x001D4697
		protected override int BaseWorkAmount
		{
			get
			{
				return 2800;
			}
		}

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x06004C09 RID: 19465 RVA: 0x001D649E File Offset: 0x001D469E
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothFloor;
			}
		}

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x06004C0A RID: 19466 RVA: 0x001D64A5 File Offset: 0x001D46A5
		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.SmoothingSpeed;
			}
		}

		// Token: 0x06004C0B RID: 19467 RVA: 0x001D64AC File Offset: 0x001D46AC
		public JobDriver_SmoothFloor()
		{
			this.clearSnow = true;
		}

		// Token: 0x06004C0C RID: 19468 RVA: 0x001D64BC File Offset: 0x001D46BC
		protected override void DoEffect(IntVec3 c)
		{
			TerrainDef smoothedTerrain = base.TargetLocA.GetTerrain(base.Map).smoothedTerrain;
			base.Map.terrainGrid.SetTerrain(base.TargetLocA, smoothedTerrain);
			FilthMaker.RemoveAllFilth(base.TargetLocA, base.Map);
		}
	}
}
