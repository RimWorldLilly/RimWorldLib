using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AAB RID: 6827
	[StaticConstructorOnStartup]
	public class Building_Electroharvester : Building
	{
		// Token: 0x17001EC9 RID: 7881
		// (get) Token: 0x0600ACA3 RID: 44195 RVA: 0x003CE2B0 File Offset: 0x003CC4B0
		public CompPowerPlantElectroharvester Electroharvester
		{
			get
			{
				CompPowerPlantElectroharvester result;
				if ((result = this.cachedElectroharvester) == null)
				{
					result = (this.cachedElectroharvester = base.GetComp<CompPowerPlantElectroharvester>());
				}
				return result;
			}
		}

		// Token: 0x17001ECA RID: 7882
		// (get) Token: 0x0600ACA4 RID: 44196 RVA: 0x003CE2D8 File Offset: 0x003CC4D8
		public CompPowerTrader PowerTrader
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.compPowerTrader) == null)
				{
					result = (this.compPowerTrader = base.GetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x17001ECB RID: 7883
		// (get) Token: 0x0600ACA5 RID: 44197 RVA: 0x003CE300 File Offset: 0x003CC500
		public CompCableConnection CableConnection
		{
			get
			{
				CompCableConnection result;
				if ((result = this.cableConnection) == null)
				{
					result = (this.cableConnection = base.GetComp<CompCableConnection>());
				}
				return result;
			}
		}

		// Token: 0x17001ECC RID: 7884
		// (get) Token: 0x0600ACA6 RID: 44198 RVA: 0x003CE326 File Offset: 0x003CC526
		public bool PowerOn
		{
			get
			{
				return this.PowerTrader.PowerOn;
			}
		}

		// Token: 0x0600ACA7 RID: 44199 RVA: 0x003CE333 File Offset: 0x003CC533
		public override bool IsWorking()
		{
			return this.PowerOn;
		}

		// Token: 0x0600ACA8 RID: 44200 RVA: 0x003CE33B File Offset: 0x003CC53B
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.Initialize();
			}
		}

		// Token: 0x0600ACA9 RID: 44201 RVA: 0x003CE350 File Offset: 0x003CC550
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.Electroharvester.FacilityComp.OnLinkAdded -= this.OnLinkAdded;
			this.Electroharvester.FacilityComp.OnLinkRemoved -= this.OnLinkRemoved;
		}

		// Token: 0x0600ACAA RID: 44202 RVA: 0x003CE39C File Offset: 0x003CC59C
		private void Initialize()
		{
			if (this.initalized)
			{
				this.RebuildCables();
				return;
			}
			this.initalized = true;
			this.Electroharvester.FacilityComp.OnLinkAdded += this.OnLinkAdded;
			this.Electroharvester.FacilityComp.OnLinkRemoved += this.OnLinkRemoved;
			foreach (Thing thing in this.Electroharvester.Platforms)
			{
				Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
				if (building_HoldingPlatform != null)
				{
					building_HoldingPlatform.innerContainer.OnContentsChanged += this.RebuildCables;
				}
			}
			this.RebuildCables();
		}

		// Token: 0x0600ACAB RID: 44203 RVA: 0x003CE460 File Offset: 0x003CC660
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (!this.initalized)
			{
				this.Initialize();
			}
		}

		// Token: 0x0600ACAC RID: 44204 RVA: 0x003CE478 File Offset: 0x003CC678
		private void OnLinkRemoved(CompFacility facility, Thing thing)
		{
			Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
			if (building_HoldingPlatform == null)
			{
				return;
			}
			building_HoldingPlatform.innerContainer.OnContentsChanged -= this.RebuildCables;
			this.RebuildCables();
		}

		// Token: 0x0600ACAD RID: 44205 RVA: 0x003CE4AD File Offset: 0x003CC6AD
		public override void Notify_DefsHotReloaded()
		{
			base.Notify_DefsHotReloaded();
			this.RebuildCables();
		}

		// Token: 0x0600ACAE RID: 44206 RVA: 0x003CE4BC File Offset: 0x003CC6BC
		private void OnLinkAdded(CompFacility facility, Thing thing)
		{
			Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
			if (building_HoldingPlatform == null)
			{
				return;
			}
			building_HoldingPlatform.innerContainer.OnContentsChanged += this.RebuildCables;
			this.RebuildCables();
		}

		// Token: 0x0600ACAF RID: 44207 RVA: 0x003CE4F1 File Offset: 0x003CC6F1
		private void RebuildCables()
		{
			this.CableConnection.RebuildCables(this.Electroharvester.Platforms, delegate(Thing thing)
			{
				Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
				return building_HoldingPlatform != null && building_HoldingPlatform.Occupied;
			});
		}

		// Token: 0x04006437 RID: 25655
		private CompPowerPlantElectroharvester cachedElectroharvester;

		// Token: 0x04006438 RID: 25656
		private CompPowerTrader compPowerTrader;

		// Token: 0x04006439 RID: 25657
		private bool initalized;

		// Token: 0x0400643A RID: 25658
		private CompCableConnection cableConnection;
	}
}
