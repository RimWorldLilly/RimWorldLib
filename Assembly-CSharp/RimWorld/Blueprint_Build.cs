using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A52 RID: 6738
	public class Blueprint_Build : Blueprint, IHaulEnroute, ILoadReferenceable
	{
		// Token: 0x17001D9F RID: 7583
		// (get) Token: 0x0600A8E9 RID: 43241 RVA: 0x003BF508 File Offset: 0x003BD708
		public override string Label
		{
			get
			{
				string text = this.def.entityDefToBuild.label;
				if (base.StyleSourcePrecept != null)
				{
					text = base.StyleSourcePrecept.TransformThingLabel(text);
				}
				if (this.stuffToUse != null)
				{
					return "ThingMadeOfStuffLabel".Translate(this.stuffToUse.LabelAsStuff, text) + "BlueprintLabelExtra".Translate();
				}
				return text + "BlueprintLabelExtra".Translate();
			}
		}

		// Token: 0x17001DA0 RID: 7584
		// (get) Token: 0x0600A8EA RID: 43242 RVA: 0x003BF58D File Offset: 0x003BD78D
		protected override float WorkTotal
		{
			get
			{
				return this.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.WorkToBuild, this.stuffToUse);
			}
		}

		// Token: 0x17001DA1 RID: 7585
		// (get) Token: 0x0600A8EB RID: 43243 RVA: 0x003BF5AA File Offset: 0x003BD7AA
		public ThingDef BuildDef
		{
			get
			{
				return this.def.entityDefToBuild as ThingDef;
			}
		}

		// Token: 0x0600A8EC RID: 43244 RVA: 0x003BF5BC File Offset: 0x003BD7BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.stuffToUse, "stuffToUse");
			Scribe_Defs.Look<ThingStyleDef>(ref this.selectedStyleDef, "selectedStyleDef");
			Scribe_Values.Look<bool>(ref this.styleOverridden, "styleOverridden", false, false);
			Scribe_Values.Look<ColorInt?>(ref this.glowerColorOverride, "glowerColorOverride", null, false);
		}

		// Token: 0x0600A8ED RID: 43245 RVA: 0x003BF61B File Offset: 0x003BD81B
		public override ThingStyleDef EntityToBuildStyle()
		{
			return this.selectedStyleDef ?? this.StyleDef;
		}

		// Token: 0x0600A8EE RID: 43246 RVA: 0x003BF62D File Offset: 0x003BD82D
		public override BuildableDef EntityToBuild()
		{
			return this.def.entityDefToBuild;
		}

		// Token: 0x0600A8EF RID: 43247 RVA: 0x003BF63A File Offset: 0x003BD83A
		public override ThingDef EntityToBuildStuff()
		{
			return this.stuffToUse;
		}

		// Token: 0x0600A8F0 RID: 43248 RVA: 0x003BF642 File Offset: 0x003BD842
		public override List<ThingDefCountClass> TotalMaterialCost()
		{
			return this.def.entityDefToBuild.CostListAdjusted(this.stuffToUse, true);
		}

		// Token: 0x0600A8F1 RID: 43249 RVA: 0x003BF65C File Offset: 0x003BD85C
		protected override Thing MakeSolidThing(out bool shouldSelect)
		{
			Frame frame = (Frame)ThingMaker.MakeThing(this.def.entityDefToBuild.frameDef, this.stuffToUse);
			frame.StyleSourcePrecept = base.StyleSourcePrecept;
			frame.StyleDef = this.StyleDef;
			frame.glowerColorOverride = this.glowerColorOverride;
			shouldSelect = false;
			base.Map.enrouteManager.SendReservations(this, frame);
			return frame;
		}

		// Token: 0x0600A8F2 RID: 43250 RVA: 0x003BF6C4 File Offset: 0x003BD8C4
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Blueprint_Build.<GetGizmos>d__17 <GetGizmos>d__ = new Blueprint_Build.<GetGizmos>d__17(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600A8F3 RID: 43251 RVA: 0x003BF6D4 File Offset: 0x003BD8D4
		private void ChangeStyleOfAllSelected(BuildableDef buildable, ThingStyleDef styleDef)
		{
			foreach (object obj in Find.Selector.SelectedObjects)
			{
				Blueprint_Build blueprint_Build = obj as Blueprint_Build;
				if (blueprint_Build != null && blueprint_Build.def.entityDefToBuild == buildable)
				{
					blueprint_Build.StyleDef = styleDef;
					blueprint_Build.styleOverridden = true;
					blueprint_Build.DirtyMapMesh(blueprint_Build.Map);
				}
			}
		}

		// Token: 0x0600A8F4 RID: 43252 RVA: 0x003BF754 File Offset: 0x003BD954
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Building edifice = base.Position.GetEdifice(base.Map);
			bool flag;
			if (edifice == null)
			{
				flag = false;
			}
			else
			{
				BuildingProperties building = edifice.def.building;
				flag = ((building != null) ? new bool?(building.supportsWallAttachments) : null).GetValueOrDefault();
			}
			if (!flag && (mode == DestroyMode.Cancel || (mode != DestroyMode.Vanish && !this.def.IsBlueprint)))
			{
				foreach (Thing thing in GenConstruct.GetAttachedBuildings(this))
				{
					thing.DeSpawn(mode);
				}
			}
			base.DeSpawn(mode);
		}

		// Token: 0x0600A8F5 RID: 43253 RVA: 0x003BF808 File Offset: 0x003BDA08
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine("ContainedResources".Translate() + ":");
			bool flag = true;
			foreach (ThingDefCountClass thingDefCountClass in this.TotalMaterialCost())
			{
				if (!flag)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(thingDefCountClass.thingDef.LabelCap + ": 0 / " + thingDefCountClass.count.ToString());
				flag = false;
			}
			ThingStyleDef styleDef = this.StyleDef;
			if (((styleDef != null) ? styleDef.Category : null) != null && base.StyleSourcePrecept == null)
			{
				stringBuilder.AppendInNewLine("Style".Translate() + ": " + this.StyleDef.Category.LabelCap);
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x0400630C RID: 25356
		public ThingDef stuffToUse;

		// Token: 0x0400630D RID: 25357
		public ThingStyleDef selectedStyleDef;

		// Token: 0x0400630E RID: 25358
		public bool styleOverridden;

		// Token: 0x0400630F RID: 25359
		public ColorInt? glowerColorOverride;

		// Token: 0x04006310 RID: 25360
		private static readonly CachedTexture ChangeStyleTex = new CachedTexture("UI/Gizmos/ChangeStyle");
	}
}
