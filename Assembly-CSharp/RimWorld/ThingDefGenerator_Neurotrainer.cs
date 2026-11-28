using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA2 RID: 3746
	public class ThingDefGenerator_Neurotrainer
	{
		// Token: 0x060062A2 RID: 25250 RVA: 0x002389C2 File Offset: 0x00236BC2
		public static IEnumerable<ThingDef> ImpliedThingDefs(bool hotReload = false)
		{
			ThingDefGenerator_Neurotrainer.<ImpliedThingDefs>d__3 <ImpliedThingDefs>d__ = new ThingDefGenerator_Neurotrainer.<ImpliedThingDefs>d__3(-2);
			<ImpliedThingDefs>d__.<>3__hotReload = hotReload;
			return <ImpliedThingDefs>d__;
		}

		// Token: 0x060062A3 RID: 25251 RVA: 0x002389D4 File Offset: 0x00236BD4
		private static ThingDef BaseNeurotrainer(string defName, bool hotReload = false)
		{
			ThingDef thingDef = hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, false) ?? new ThingDef()) : new ThingDef();
			thingDef.defName = defName;
			thingDef.category = ThingCategory.Item;
			thingDef.selectable = true;
			thingDef.thingClass = typeof(ThingWithComps);
			thingDef.comps = new List<CompProperties>
			{
				new CompProperties_UseEffectPlaySound
				{
					soundOnUsed = SoundDefOf.MechSerumUsed
				},
				new CompProperties_UseEffectDestroySelf
				{
					compClass = typeof(CompUseEffect_DestroySelf)
				},
				new CompProperties_Forbiddable()
			};
			thingDef.graphicData = new GraphicData
			{
				texPath = "Things/Item/Special/MechSerumNeurotrainer",
				graphicClass = typeof(Graphic_Single)
			};
			thingDef.drawGUIOverlay = false;
			thingDef.statBases = new List<StatModifier>
			{
				new StatModifier
				{
					stat = StatDefOf.MaxHitPoints,
					value = 80f
				},
				new StatModifier
				{
					stat = StatDefOf.Mass,
					value = 0.2f
				},
				new StatModifier
				{
					stat = StatDefOf.DeteriorationRate,
					value = 2f
				},
				new StatModifier
				{
					stat = StatDefOf.Flammability,
					value = 0.2f
				}
			};
			thingDef.techLevel = TechLevel.Ultra;
			thingDef.altitudeLayer = AltitudeLayer.Item;
			thingDef.alwaysHaulable = true;
			thingDef.rotatable = false;
			thingDef.pathCost = 14;
			thingDef.tradeTags = new List<string>
			{
				"ExoticMisc"
			};
			thingDef.stackLimit = 1;
			thingDef.tradeNeverStack = true;
			thingDef.forceDebugSpawnable = true;
			thingDef.drawerType = DrawerType.MapMeshOnly;
			return thingDef;
		}

		// Token: 0x04003650 RID: 13904
		public static string NeurotrainerDefPrefix = "Neurotrainer";

		// Token: 0x04003651 RID: 13905
		public static string PsytrainerDefPrefix = "Psytrainer";

		// Token: 0x04003652 RID: 13906
		private const int MaxAbilityLevel = 6;
	}
}
