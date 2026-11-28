using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001A54 RID: 6740
	public class Blueprint_Install : Blueprint
	{
		// Token: 0x17001DA3 RID: 7587
		// (get) Token: 0x0600A8FD RID: 43261 RVA: 0x003BF9D4 File Offset: 0x003BDBD4
		public Thing MiniToInstallOrBuildingToReinstall
		{
			get
			{
				if (this.miniToInstall != null)
				{
					return this.miniToInstall;
				}
				if (this.buildingToReinstall != null)
				{
					return this.buildingToReinstall;
				}
				throw new InvalidOperationException("Nothing to install.");
			}
		}

		// Token: 0x17001DA4 RID: 7588
		// (get) Token: 0x0600A8FE RID: 43262 RVA: 0x003BF9FE File Offset: 0x003BDBFE
		public Thing ThingToInstall
		{
			get
			{
				return this.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();
			}
		}

		// Token: 0x17001DA5 RID: 7589
		// (get) Token: 0x0600A8FF RID: 43263 RVA: 0x003BFA0B File Offset: 0x003BDC0B
		// (set) Token: 0x0600A900 RID: 43264 RVA: 0x003BFA1E File Offset: 0x003BDC1E
		public override ThingStyleDef StyleDef
		{
			get
			{
				Thing thingToInstall = this.ThingToInstall;
				if (thingToInstall == null)
				{
					return null;
				}
				return thingToInstall.StyleDef;
			}
			set
			{
				this.ThingToInstall.StyleDef = value;
			}
		}

		// Token: 0x17001DA6 RID: 7590
		// (get) Token: 0x0600A901 RID: 43265 RVA: 0x003BFA2C File Offset: 0x003BDC2C
		protected override float WorkTotal
		{
			get
			{
				return (float)((this.ThingToInstall.def.plant != null) ? -1 : 150);
			}
		}

		// Token: 0x0600A902 RID: 43266 RVA: 0x003BFA49 File Offset: 0x003BDC49
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MinifiedThing>(ref this.miniToInstall, "miniToInstall", false);
			Scribe_References.Look<Building>(ref this.buildingToReinstall, "buildingToReinstall", false);
		}

		// Token: 0x0600A903 RID: 43267 RVA: 0x003BFA73 File Offset: 0x003BDC73
		public override BuildableDef EntityToBuild()
		{
			Thing thingToInstall = this.ThingToInstall;
			if (thingToInstall == null)
			{
				return null;
			}
			return thingToInstall.def;
		}

		// Token: 0x0600A904 RID: 43268 RVA: 0x003BFA86 File Offset: 0x003BDC86
		public override ThingStyleDef EntityToBuildStyle()
		{
			Thing thingToInstall = this.ThingToInstall;
			if (thingToInstall == null)
			{
				return null;
			}
			return thingToInstall.StyleDef;
		}

		// Token: 0x0600A905 RID: 43269 RVA: 0x003BFA99 File Offset: 0x003BDC99
		public override ThingDef EntityToBuildStuff()
		{
			Thing thingToInstall = this.ThingToInstall;
			if (thingToInstall == null)
			{
				return null;
			}
			return thingToInstall.Stuff;
		}

		// Token: 0x0600A906 RID: 43270 RVA: 0x003BFAAC File Offset: 0x003BDCAC
		public override List<ThingDefCountClass> TotalMaterialCost()
		{
			Log.Error("Called MaterialsNeededTotal on a Blueprint_Install.");
			return new List<ThingDefCountClass>();
		}

		// Token: 0x0600A907 RID: 43271 RVA: 0x003BFAC0 File Offset: 0x003BDCC0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			map.listerBuildings.RegisterInstallBlueprint(this);
			if (this.buildingToReinstall != null)
			{
				this.overrideGraphicIndex = new int?(this.buildingToReinstall.overrideGraphicIndex ?? this.buildingToReinstall.thingIDNumber);
			}
			else if (this.miniToInstall != null)
			{
				this.overrideGraphicIndex = new int?(this.miniToInstall.InnerThing.overrideGraphicIndex ?? this.miniToInstall.InnerThing.thingIDNumber);
			}
			if (!respawningAfterLoad && !this.MiniToInstallOrBuildingToReinstall.BeingTransportedOnGravship && (this.MiniToInstallOrBuildingToReinstall.Destroyed || !this.MiniToInstallOrBuildingToReinstall.SpawnedOrAnyParentSpawned))
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600A908 RID: 43272 RVA: 0x003BFB96 File Offset: 0x003BDD96
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Map.listerBuildings.DeregisterInstallBlueprint(this);
			base.DeSpawn(mode);
		}

		// Token: 0x0600A909 RID: 43273 RVA: 0x003BFBB0 File Offset: 0x003BDDB0
		protected override Thing MakeSolidThing(out bool shouldSelect)
		{
			Thing thingToInstall = this.ThingToInstall;
			if (this.miniToInstall != null)
			{
				foreach (Designation designation in base.Map.designationManager.AllDesignationsOn(this.miniToInstall))
				{
					Designation newDes = new Designation(thingToInstall, designation.def, designation.colorDef);
					base.Map.designationManager.AddDesignation(newDes);
				}
				shouldSelect = Find.Selector.IsSelected(this.miniToInstall);
				this.miniToInstall.InnerThing = null;
				this.miniToInstall.Destroy(DestroyMode.Vanish);
			}
			else
			{
				shouldSelect = false;
			}
			return thingToInstall;
		}

		// Token: 0x0600A90A RID: 43274 RVA: 0x003BFC78 File Offset: 0x003BDE78
		public override bool TryReplaceWithSolidThing(Pawn workerPawn, out Thing createdThing, out bool jobEnded)
		{
			Map map = base.Map;
			bool flag = base.TryReplaceWithSolidThing(workerPawn, out createdThing, out jobEnded);
			if (flag)
			{
				if (this.overrideGraphicIndex != null)
				{
					createdThing.overrideGraphicIndex = this.overrideGraphicIndex;
				}
				if (createdThing is Plant)
				{
					SoundDefOf.Replant_Complete.PlayOneShot(new TargetInfo(base.Position, map, false));
				}
				else
				{
					SoundDefOf.Building_Complete.PlayOneShot(new TargetInfo(base.Position, map, false));
				}
				workerPawn.records.Increment(RecordDefOf.ThingsInstalled);
			}
			return flag;
		}

		// Token: 0x0600A90B RID: 43275 RVA: 0x003BFD05 File Offset: 0x003BDF05
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Blueprint_Install.<GetGizmos>d__20 <GetGizmos>d__ = new Blueprint_Install.<GetGizmos>d__20(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600A90C RID: 43276 RVA: 0x003BFD18 File Offset: 0x003BDF18
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.buildingToReinstall != null)
			{
				GenDraw.DrawLineBetween(this.buildingToReinstall.TrueCenter(), this.TrueCenter());
			}
			if (this.ThingToInstall.def.drawPlaceWorkersWhileInstallBlueprintSelected && this.ThingToInstall.def.PlaceWorkers != null)
			{
				List<PlaceWorker> placeWorkers = this.ThingToInstall.def.PlaceWorkers;
				for (int i = 0; i < placeWorkers.Count; i++)
				{
					placeWorkers[i].DrawGhost(this.ThingToInstall.def, base.Position, base.Rotation, Color.white, this.ThingToInstall);
				}
			}
		}

		// Token: 0x0600A90D RID: 43277 RVA: 0x003BFDBD File Offset: 0x003BDFBD
		internal void SetThingToInstallFromMinified(MinifiedThing itemToInstall)
		{
			this.miniToInstall = itemToInstall;
			this.buildingToReinstall = null;
		}

		// Token: 0x0600A90E RID: 43278 RVA: 0x003BFDCD File Offset: 0x003BDFCD
		internal void SetBuildingToReinstall(Building buildingToReinstall)
		{
			if (!buildingToReinstall.def.Minifiable)
			{
				Log.Error("Tried to reinstall non-minifiable building.");
				return;
			}
			this.miniToInstall = null;
			this.buildingToReinstall = buildingToReinstall;
		}

		// Token: 0x04006311 RID: 25361
		private MinifiedThing miniToInstall;

		// Token: 0x04006312 RID: 25362
		private Building buildingToReinstall;
	}
}
