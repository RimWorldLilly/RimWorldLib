using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009C9 RID: 2505
	public class JobDriver_BuildRoof : JobDriver_AffectRoof
	{
		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x06004C15 RID: 19477 RVA: 0x001D6586 File Offset: 0x001D4786
		protected override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06004C16 RID: 19478 RVA: 0x001D6589 File Offset: 0x001D4789
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_BuildRoof.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_BuildRoof.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004C17 RID: 19479 RVA: 0x001D659C File Offset: 0x001D479C
		protected override void DoEffect()
		{
			for (int i = 0; i < 9; i++)
			{
				IntVec3 intVec = base.Cell + GenAdj.AdjacentCellsAndInside[i];
				if (intVec.InBounds(base.Map) && base.Map.areaManager.BuildRoof[intVec] && !intVec.Roofed(base.Map) && RoofCollapseUtility.WithinRangeOfRoofHolder(intVec, base.Map, false) && RoofUtility.FirstBlockingThing(intVec, base.Map) == null)
				{
					base.Map.roofGrid.SetRoof(intVec, RoofDefOf.RoofConstructed);
					MoteMaker.PlaceTempRoof(intVec, base.Map);
					List<Thing> list = base.Map.thingGrid.ThingsListAt(intVec);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						CompWakeUpDormant compWakeUpDormant;
						if (thing.def.building != null && thing.def.building.IsMortar && thing.TryGetComp(out compWakeUpDormant))
						{
							compWakeUpDormant.Activate(this.pawn, true, false, false);
						}
					}
				}
			}
		}

		// Token: 0x06004C18 RID: 19480 RVA: 0x001D66BD File Offset: 0x001D48BD
		protected override bool DoWorkFailOn()
		{
			return base.Cell.Roofed(base.Map);
		}
	}
}
