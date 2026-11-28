using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001A51 RID: 6737
	public abstract class Blueprint : ThingWithComps, IConstructible
	{
		// Token: 0x17001D9C RID: 7580
		// (get) Token: 0x0600A8D4 RID: 43220 RVA: 0x003BF0E8 File Offset: 0x003BD2E8
		public override string Label
		{
			get
			{
				return this.def.entityDefToBuild.label + "BlueprintLabelExtra".Translate();
			}
		}

		// Token: 0x17001D9D RID: 7581
		// (get) Token: 0x0600A8D5 RID: 43221
		protected abstract float WorkTotal { get; }

		// Token: 0x17001D9E RID: 7582
		// (get) Token: 0x0600A8D6 RID: 43222 RVA: 0x003BF110 File Offset: 0x003BD310
		public override Graphic Graphic
		{
			get
			{
				ThingStyleDef styleDef = this.StyleDef;
				if (((styleDef != null) ? styleDef.blueprintGraphicData : null) != null)
				{
					if (this.styleGraphicInt == null)
					{
						this.styleGraphicInt = styleDef.blueprintGraphicData.Graphic;
					}
					return this.styleGraphicInt;
				}
				return base.Graphic;
			}
		}

		// Token: 0x0600A8D7 RID: 43223 RVA: 0x003BF158 File Offset: 0x003BD358
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Blueprint.<GetGizmos>d__6 <GetGizmos>d__ = new Blueprint.<GetGizmos>d__6(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600A8D8 RID: 43224 RVA: 0x003BF168 File Offset: 0x003BD368
		public virtual bool TryReplaceWithSolidThing(Pawn workerPawn, out Thing createdThing, out bool jobEnded)
		{
			bool flag = Find.Selector.IsSelected(this);
			jobEnded = false;
			if (GenConstruct.FirstBlockingThing(this, workerPawn) != null)
			{
				workerPawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				jobEnded = true;
				createdThing = null;
				return false;
			}
			bool flag2;
			createdThing = this.MakeSolidThing(out flag2);
			Map map = base.Map;
			if (this.Graphic is Graphic_Random)
			{
				createdThing.overrideGraphicIndex = new int?(this.thingIDNumber);
			}
			GenSpawn.WipeExistingThings(base.Position, base.Rotation, createdThing.def, map, DestroyMode.Deconstruct);
			if (createdThing is Plant)
			{
				Plant plant = base.Position.GetPlant(base.Map);
				if (plant != null)
				{
					plant.Destroy(DestroyMode.Vanish);
				}
			}
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			if (createdThing.def.CanHaveFaction)
			{
				createdThing.SetFactionDirect(workerPawn.Faction);
			}
			Thing thing = GenSpawn.Spawn(createdThing, base.Position, map, base.Rotation, WipeMode.Vanish, false, false);
			if ((flag2 || flag) && thing != null)
			{
				Find.Selector.Select(thing, false, false);
			}
			foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned)
			{
				pawn.pather.NotifyThingTransformed(this, thing);
			}
			foreach (IntVec3 intVec in thing.OccupiedRect().Cells)
			{
				Plan plan;
				if (map.planManager.TryGetPlan(intVec, out plan))
				{
					plan.RemoveCell(intVec);
				}
			}
			return true;
		}

		// Token: 0x0600A8D9 RID: 43225 RVA: 0x003BF31C File Offset: 0x003BD51C
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (WorldComponent_GravshipController.CutsceneInProgress && !WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				return;
			}
			base.DrawAt(drawLoc, flip);
		}

		// Token: 0x0600A8DA RID: 43226 RVA: 0x003BF335 File Offset: 0x003BD535
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			map.blueprintGrid.Register(this);
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x0600A8DB RID: 43227 RVA: 0x003BF34B File Offset: 0x003BD54B
		public void InheritStyle(Precept_ThingStyle styleSource, ThingStyleDef styleDef)
		{
			base.StyleSourcePrecept = styleSource;
			if (styleSource == null)
			{
				this.StyleDef = styleDef;
			}
		}

		// Token: 0x0600A8DC RID: 43228 RVA: 0x003BF35E File Offset: 0x003BD55E
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			map.blueprintGrid.DeRegister(this);
		}

		// Token: 0x0600A8DD RID: 43229
		protected abstract Thing MakeSolidThing(out bool shouldSelect);

		// Token: 0x0600A8DE RID: 43230
		public abstract List<ThingDefCountClass> TotalMaterialCost();

		// Token: 0x0600A8DF RID: 43231 RVA: 0x003BF378 File Offset: 0x003BD578
		public bool IsCompleted()
		{
			return false;
		}

		// Token: 0x0600A8E0 RID: 43232 RVA: 0x003BF37C File Offset: 0x003BD57C
		public int ThingCountNeeded(ThingDef stuff)
		{
			foreach (ThingDefCountClass thingDefCountClass in this.TotalMaterialCost())
			{
				if (thingDefCountClass.thingDef == stuff)
				{
					return thingDefCountClass.count;
				}
			}
			return 0;
		}

		// Token: 0x0600A8E1 RID: 43233 RVA: 0x003BF3E0 File Offset: 0x003BD5E0
		public int SpaceRemainingFor(ThingDef stuff)
		{
			return this.ThingCountNeeded(stuff);
		}

		// Token: 0x0600A8E2 RID: 43234
		public abstract BuildableDef EntityToBuild();

		// Token: 0x0600A8E3 RID: 43235
		public abstract ThingStyleDef EntityToBuildStyle();

		// Token: 0x0600A8E4 RID: 43236
		public abstract ThingDef EntityToBuildStuff();

		// Token: 0x0600A8E5 RID: 43237 RVA: 0x003BF3EC File Offset: 0x003BD5EC
		public Thing BlockingHaulableOnTop()
		{
			if (this.def.entityDefToBuild.passability == Traversability.Standable)
			{
				return null;
			}
			foreach (IntVec3 c in this.OccupiedRect())
			{
				List<Thing> thingList = c.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing.def.EverHaulable)
					{
						return thing;
					}
				}
			}
			return null;
		}

		// Token: 0x0600A8E6 RID: 43238 RVA: 0x003BF48C File Offset: 0x003BD68C
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			stringBuilder.AppendLineIfNotEmpty();
			float workTotal = this.WorkTotal;
			if (!Mathf.Approximately(workTotal, -1f))
			{
				stringBuilder.Append("WorkLeft".Translate() + ": " + workTotal.ToStringWorkAmount());
			}
			return stringBuilder.ToString();
		}
	}
}
