using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200135D RID: 4957
	public class CompPowerPlantElectroharvester : CompPowerPlant
	{
		// Token: 0x170014F3 RID: 5363
		// (get) Token: 0x06007AC8 RID: 31432 RVA: 0x002BB703 File Offset: 0x002B9903
		protected override float DesiredPowerOutput
		{
			get
			{
				return this.GetPowerOutput();
			}
		}

		// Token: 0x170014F4 RID: 5364
		// (get) Token: 0x06007AC9 RID: 31433 RVA: 0x002BB70C File Offset: 0x002B990C
		public CompFacility FacilityComp
		{
			get
			{
				CompFacility result;
				if ((result = this.facilityComp) == null)
				{
					result = (this.facilityComp = this.parent.GetComp<CompFacility>());
				}
				return result;
			}
		}

		// Token: 0x170014F5 RID: 5365
		// (get) Token: 0x06007ACA RID: 31434 RVA: 0x002BB737 File Offset: 0x002B9937
		public List<Thing> Platforms
		{
			get
			{
				return this.FacilityComp.LinkedBuildings;
			}
		}

		// Token: 0x06007ACB RID: 31435 RVA: 0x002BB744 File Offset: 0x002B9944
		private float GetPowerOutput()
		{
			float num = 0f;
			foreach (Thing thing in this.Platforms)
			{
				Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
				if (building_HoldingPlatform != null && building_HoldingPlatform.Occupied)
				{
					num += (float)Mathf.RoundToInt(building_HoldingPlatform.HeldPawn.BodySize * -base.Props.PowerConsumption * 0.1f);
				}
			}
			return Mathf.Clamp(num, 0f, -base.Props.PowerConsumption);
		}

		// Token: 0x06007ACC RID: 31436 RVA: 0x002BB7E4 File Offset: 0x002B99E4
		public override string CompInspectStringExtra()
		{
			string str = "";
			if (this.Platforms.Empty<Thing>())
			{
				str = "ElectroharvesterNoPlatform".Translate() + "\n";
			}
			else if (this.GetPowerOutput() == 0f)
			{
				str = "ElectroharvesterNoEntity".Translate() + "\n";
			}
			return str + base.CompInspectStringExtra();
		}

		// Token: 0x04004D82 RID: 19842
		private CompFacility facilityComp;
	}
}
