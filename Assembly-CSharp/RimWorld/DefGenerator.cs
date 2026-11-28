using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E97 RID: 3735
	public static class DefGenerator
	{
		// Token: 0x0600627D RID: 25213 RVA: 0x00236854 File Offset: 0x00234A54
		public static void GenerateImpliedDefs_PreResolve(bool hotReload = false)
		{
			foreach (TerrainDef def in TerrainDefGenerator_Carpet.ImpliedTerrainDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<TerrainDef>(def, hotReload);
			}
			foreach (ThingDef def2 in ThingDefGenerator_Buildings.ImpliedBlueprintAndFrameDefs(hotReload).Concat(ThingDefGenerator_Meat.ImpliedMeatDefs(hotReload)).Concat(ThingDefGenerator_Techprints.ImpliedTechprintDefs(hotReload)).Concat(ThingDefGenerator_Corpses.ImpliedCorpseDefs(hotReload)))
			{
				DefGenerator.AddImpliedDef<ThingDef>(def2, hotReload);
			}
			DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.Silent);
			foreach (TerrainDef def3 in TerrainDefGenerator_Stone.ImpliedTerrainDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<TerrainDef>(def3, hotReload);
			}
			foreach (RecipeDef def4 in RecipeDefGenerator.ImpliedRecipeDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<RecipeDef>(def4, hotReload);
			}
			foreach (PawnColumnDef def5 in PawnColumnDefGenerator.ImpliedPawnColumnDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<PawnColumnDef>(def5, hotReload);
			}
			foreach (ThingDef def6 in ThingDefGenerator_Neurotrainer.ImpliedThingDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<ThingDef>(def6, hotReload);
			}
			foreach (GeneDef def7 in GeneDefGenerator.ImpliedGeneDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<GeneDef>(def7, hotReload);
			}
			foreach (ThoughtDef def8 in GeneDefGenerator.ImpliedThoughtDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<ThoughtDef>(def8, hotReload);
			}
			AnimationDefGenerator_Flying.InitializeNeededDefs(hotReload);
			foreach (GraphicStateDef def9 in AnimationDefGenerator_Flying.ImpliedGraphicStateDefs())
			{
				DefGenerator.AddImpliedDef<GraphicStateDef>(def9, hotReload);
			}
			foreach (AnimationDef def10 in AnimationDefGenerator_Flying.ImpliedAnimationDefs())
			{
				DefGenerator.AddImpliedDef<AnimationDef>(def10, hotReload);
			}
		}

		// Token: 0x0600627E RID: 25214 RVA: 0x00236AE8 File Offset: 0x00234CE8
		public static void GenerateImpliedDefs_PostResolve(bool hotReload = false)
		{
			foreach (KeyBindingCategoryDef def in KeyBindingDefGenerator.ImpliedKeyBindingCategoryDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<KeyBindingCategoryDef>(def, hotReload);
			}
			foreach (KeyBindingDef def2 in KeyBindingDefGenerator.ImpliedKeyBindingDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<KeyBindingDef>(def2, hotReload);
			}
		}

		// Token: 0x0600627F RID: 25215 RVA: 0x00236B70 File Offset: 0x00234D70
		public static void AddImpliedDef<T>(T def, bool hotReload = false) where T : Def, new()
		{
			def.generated = true;
			def.ResolveDefNameHash();
			ModContentPack modContentPack = def.modContentPack;
			if (modContentPack != null)
			{
				modContentPack.AddDef(def, "ImpliedDefs");
			}
			def.PostLoad();
			if (hotReload && DefDatabase<T>.GetNamed(def.defName, false) != null)
			{
				return;
			}
			DefDatabase<T>.Add(def);
		}

		// Token: 0x04003629 RID: 13865
		public const int StandardItemPathCost = 14;
	}
}
