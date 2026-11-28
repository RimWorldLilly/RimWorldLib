using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200125F RID: 4703
	public class RoomContents_Stockpile : RoomContentsWorker
	{
		// Token: 0x06007476 RID: 29814 RVA: 0x0028BD60 File Offset: 0x00289F60
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomGenUtility.GenerateRows(ThingDefOf.Shelf, room, map, ThingDefOf.Steel);
			PocketMapExit pocketMapExit = map.listerThings.ThingsOfDef(ThingDefOf.AncientHatchExit).FirstOrDefault<Thing>() as PocketMapExit;
			AncientHatch ancientHatch = (((pocketMapExit != null) ? pocketMapExit.entrance : null) ?? PocketMapUtility.currentlyGeneratingPortal) as AncientHatch;
			if (ancientHatch != null)
			{
				switch (ancientHatch.stockpileType)
				{
				case TileMutatorWorker_Stockpile.StockpileType.Medicine:
					this.GenerateItems(map, room, ThingDefOf.MedicineUltratech, new IntRange(5, 10));
					this.GenerateItems(map, room, ThingDefOf.MedicineIndustrial, new IntRange(30, 60));
					break;
				case TileMutatorWorker_Stockpile.StockpileType.Chemfuel:
					this.GenerateItems(map, room, ThingDefOf.Chemfuel, new IntRange(750, 1200));
					break;
				case TileMutatorWorker_Stockpile.StockpileType.Component:
					this.GenerateItems(map, room, ThingDefOf.ComponentIndustrial, new IntRange(25, 50));
					this.GenerateItems(map, room, ThingDefOf.ComponentSpacer, new IntRange(2, 3));
					break;
				case TileMutatorWorker_Stockpile.StockpileType.Weapons:
					this.GenerateWeapons(map, room, new IntRange(2, 3));
					break;
				case TileMutatorWorker_Stockpile.StockpileType.Gravcore:
					this.GenerateItems(map, room, ThingDefOf.Gravcore, IntRange.One);
					this.GenerateItems(map, room, ThingDefOf.GravlitePanel, new IntRange(150, 200));
					break;
				case TileMutatorWorker_Stockpile.StockpileType.Drugs:
					this.GenerateItems(map, room, ThingDefOf.SmokeleafJoint, new IntRange(5, 20));
					this.GenerateItems(map, room, ThingDefOf.Flake, new IntRange(25, 100));
					this.GenerateItems(map, room, ThingDefOf.Yayo, new IntRange(25, 60));
					this.GenerateItems(map, room, ThingDefOf.GoJuice, new IntRange(-10, 10));
					this.GenerateItems(map, room, ThingDefOf.WakeUp, new IntRange(-10, 10));
					this.GenerateItems(map, room, ThingDefOf.Luciferium, new IntRange(5, 15));
					break;
				}
			}
			LayoutRoomDef layoutRoomDef = room.defs.FirstOrDefault((LayoutRoomDef x) => x.thingSetMakerDef != null);
			ThingSetMakerDef thingSetMakerDef = (layoutRoomDef != null) ? layoutRoomDef.thingSetMakerDef : null;
			if (thingSetMakerDef != null)
			{
				ThingSetMakerParams parms = default(ThingSetMakerParams);
				parms.totalMarketValueRange = new FloatRange?(new FloatRange(2200f));
				List<Thing> items = thingSetMakerDef.root.Generate(parms);
				this.GenerateItems(map, room, items);
			}
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x06007477 RID: 29815 RVA: 0x0028BFA8 File Offset: 0x0028A1A8
		private void GenerateItems(Map map, LayoutRoom room, List<Thing> items)
		{
			int num = 999;
			while (items.Count > 0 && num-- > 0)
			{
				Thing itemToSpawn = items.Last<Thing>();
				items.Remove(itemToSpawn);
				IntVec3 loc;
				if (!room.TryGetRandomCellInRoom(map, out loc, 0, 0, (IntVec3 c) => this.ShelfValidator(map, c, itemToSpawn.def), true))
				{
					break;
				}
				GenSpawn.Spawn(itemToSpawn, loc, map, WipeMode.Vanish).SetForbidden(true, true);
			}
		}

		// Token: 0x06007478 RID: 29816 RVA: 0x0028C04C File Offset: 0x0028A24C
		private void GenerateItems(Map map, LayoutRoom room, ThingDef itemDef, IntRange countRange)
		{
			int num = countRange.RandomInRange;
			int num2 = 99;
			Func<IntVec3, bool> <>9__0;
			while (num > 0 && num2-- > 0)
			{
				Map map2 = map;
				int contractedBy = 0;
				int padding = 0;
				Func<IntVec3, bool> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => this.ShelfValidator(map, c, itemDef)));
				}
				IntVec3 loc;
				if (!room.TryGetRandomCellInRoom(map2, out loc, contractedBy, padding, validator, true))
				{
					break;
				}
				int num3 = Rand.Range(1, Mathf.Min(countRange.max / 4, itemDef.stackLimit));
				num3 = Mathf.Min(num3, num);
				Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(itemDef, null), loc, map, WipeMode.Vanish);
				thing.stackCount = num3;
				thing.SetForbidden(true, true);
				num -= num3;
			}
		}

		// Token: 0x06007479 RID: 29817 RVA: 0x0028C120 File Offset: 0x0028A320
		private void GenerateWeapons(Map map, LayoutRoom room, IntRange countRange)
		{
			int num = countRange.RandomInRange;
			int num2 = 999;
			while (num > 0 && num2-- > 0)
			{
				Thing weapon;
				if (EquipmentUtility.TryGenerateWeaponByTag(RoomContents_Stockpile.WeaponTags.RandomElement<string>(), out weapon, 99999f) && !weapon.HasThingCategory(ThingCategoryDefOf.WeaponsUnique))
				{
					CompQuality compQuality;
					if (weapon.TryGetComp(out compQuality))
					{
						compQuality.SetQuality(QualityUtility.GenerateFromGaussian(1f, QualityCategory.Legendary, QualityCategory.Excellent, QualityCategory.Excellent), new ArtGenerationContext?(ArtGenerationContext.Outsider));
					}
					IntVec3 loc;
					if (room.TryGetRandomCellInRoom(map, out loc, 0, 0, (IntVec3 c) => this.ShelfValidator(map, c, weapon.def), true))
					{
						GenSpawn.Spawn(weapon, loc, map, WipeMode.Vanish).SetForbidden(true, true);
						num--;
					}
				}
			}
		}

		// Token: 0x0600747A RID: 29818 RVA: 0x0028C214 File Offset: 0x0028A414
		private bool ShelfValidator(Map map, IntVec3 c, ThingDef itemDef)
		{
			Building_Storage building_Storage = c.GetFirstThing(map, ThingDefOf.Shelf) as Building_Storage;
			return building_Storage != null && building_Storage.SpaceRemainingFor(itemDef) != 0;
		}

		// Token: 0x04004932 RID: 18738
		private static readonly List<string> WeaponTags = new List<string>
		{
			"IndustrialGunAdvanced",
			"SpacerGun",
			"RangedHeavy"
		};
	}
}
