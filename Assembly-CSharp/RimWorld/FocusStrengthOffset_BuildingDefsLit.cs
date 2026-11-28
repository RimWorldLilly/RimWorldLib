using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001906 RID: 6406
	public class FocusStrengthOffset_BuildingDefsLit : FocusStrengthOffset_BuildingDefs
	{
		// Token: 0x0600A096 RID: 41110 RVA: 0x0039733E File Offset: 0x0039553E
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			return this.BuildingLit(parent) && base.CanApply(parent, user);
		}

		// Token: 0x0600A097 RID: 41111 RVA: 0x00397353 File Offset: 0x00395553
		protected override float OffsetForBuilding(Thing b)
		{
			if (this.BuildingLit(b))
			{
				return base.OffsetFor(b.def);
			}
			return 0f;
		}

		// Token: 0x0600A098 RID: 41112 RVA: 0x00397370 File Offset: 0x00395570
		private bool BuildingLit(Thing b)
		{
			CompGlower compGlower = b.TryGetComp<CompGlower>();
			return compGlower != null && compGlower.Glows;
		}

		// Token: 0x0600A099 RID: 41113 RVA: 0x00397390 File Offset: 0x00395590
		protected override int BuildingCount(Thing parent)
		{
			if (parent == null || !parent.Spawned)
			{
				return 0;
			}
			int num = 0;
			List<Thing> forCell = parent.Map.listerBuldingOfDefInProximity.GetForCell(parent.Position, this.radius, this.defs, parent);
			for (int i = 0; i < forCell.Count; i++)
			{
				Thing b = forCell[i];
				if (this.BuildingLit(b))
				{
					num++;
				}
			}
			return Math.Min(num, this.maxBuildings);
		}
	}
}
