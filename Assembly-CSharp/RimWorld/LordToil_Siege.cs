using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CF4 RID: 3316
	public class LordToil_Siege : LordToil
	{
		// Token: 0x1700105F RID: 4191
		// (get) Token: 0x06005D90 RID: 23952 RVA: 0x00224274 File Offset: 0x00222474
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.siegeCenter;
			}
		}

		// Token: 0x17001060 RID: 4192
		// (get) Token: 0x06005D91 RID: 23953 RVA: 0x00224281 File Offset: 0x00222481
		private LordToilData_Siege Data
		{
			get
			{
				return (LordToilData_Siege)this.data;
			}
		}

		// Token: 0x17001061 RID: 4193
		// (get) Token: 0x06005D92 RID: 23954 RVA: 0x0022428E File Offset: 0x0022248E
		private IEnumerable<Frame> Frames
		{
			get
			{
				LordToil_Siege.<get_Frames>d__17 <get_Frames>d__ = new LordToil_Siege.<get_Frames>d__17(-2);
				<get_Frames>d__.<>4__this = this;
				return <get_Frames>d__;
			}
		}

		// Token: 0x17001062 RID: 4194
		// (get) Token: 0x06005D93 RID: 23955 RVA: 0x0022429E File Offset: 0x0022249E
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005D94 RID: 23956 RVA: 0x002242A1 File Offset: 0x002224A1
		public LordToil_Siege(IntVec3 siegeCenter, float blueprintPoints)
		{
			this.data = new LordToilData_Siege();
			this.Data.siegeCenter = siegeCenter;
			this.Data.blueprintPoints = blueprintPoints;
		}

		// Token: 0x06005D95 RID: 23957 RVA: 0x002242D8 File Offset: 0x002224D8
		public override void Init()
		{
			base.Init();
			LordToilData_Siege data = this.Data;
			data.baseRadius = Mathf.InverseLerp(14f, 25f, (float)this.lord.ownedPawns.Count / 50f);
			data.baseRadius = Mathf.Clamp(data.baseRadius, 14f, 25f);
			List<Thing> list = new List<Thing>();
			foreach (Blueprint_Build blueprint_Build in SiegeBlueprintPlacer.PlaceBlueprints(data.siegeCenter, base.Map, this.lord.faction, data.blueprintPoints))
			{
				data.blueprints.Add(blueprint_Build);
				using (List<ThingDefCountClass>.Enumerator enumerator2 = blueprint_Build.TotalMaterialCost().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ThingDefCountClass cost = enumerator2.Current;
						Thing thing = list.FirstOrDefault((Thing t) => t.def == cost.thingDef);
						if (thing != null)
						{
							thing.stackCount += cost.count;
						}
						else
						{
							Thing thing2 = ThingMaker.MakeThing(cost.thingDef, null);
							thing2.stackCount = cost.count;
							list.Add(thing2);
						}
					}
				}
				ThingDef thingDef = blueprint_Build.def.entityDefToBuild as ThingDef;
				if (thingDef != null)
				{
					ThingDef thingDef2 = TurretGunUtility.TryFindRandomShellDef(thingDef, false, false, true, this.lord.faction.def.techLevel, false, 250f, this.lord.faction);
					if (thingDef2 != null)
					{
						Thing thing3 = ThingMaker.MakeThing(thingDef2, null);
						thing3.stackCount = 5;
						list.Add(thing3);
					}
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].stackCount = Mathf.CeilToInt((float)list[i].stackCount * Rand.Range(1f, 1.2f));
			}
			List<List<Thing>> list2 = new List<List<Thing>>();
			for (int j = 0; j < list.Count; j++)
			{
				while (list[j].stackCount > list[j].def.stackLimit)
				{
					int num = Mathf.CeilToInt((float)list[j].def.stackLimit * Rand.Range(0.9f, 0.999f));
					Thing thing4 = ThingMaker.MakeThing(list[j].def, null);
					thing4.stackCount = num;
					list[j].stackCount -= num;
					list.Add(thing4);
				}
			}
			List<Thing> list3 = new List<Thing>();
			for (int k = 0; k < list.Count; k++)
			{
				list3.Add(list[k]);
				if (k % 2 == 1 || k == list.Count - 1)
				{
					list2.Add(list3);
					list3 = new List<Thing>();
				}
			}
			List<Thing> list4 = new List<Thing>();
			int num2 = Mathf.RoundToInt(LordToil_Siege.MealCountRangePerRaider.RandomInRange * (float)this.lord.ownedPawns.Count);
			for (int l = 0; l < num2; l++)
			{
				Thing item = ThingMaker.MakeThing(ThingDefOf.MealSurvivalPack, null);
				list4.Add(item);
			}
			list2.Add(list4);
			DropPodUtility.DropThingGroupsNear(data.siegeCenter, base.Map, list2, 110, false, false, true, true, true, false, null);
			data.desiredBuilderFraction = LordToil_Siege.BuilderCountFraction.RandomInRange;
		}

		// Token: 0x06005D96 RID: 23958 RVA: 0x002246A0 File Offset: 0x002228A0
		public override void UpdateAllDuties()
		{
			LordToilData_Siege data = this.Data;
			if (this.lord.ticksInToil < 450)
			{
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					this.SetAsDefender(this.lord.ownedPawns[i]);
				}
				return;
			}
			this.rememberedDuties.Clear();
			int num = Mathf.RoundToInt((float)this.lord.ownedPawns.Count * data.desiredBuilderFraction);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = (from b in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial)
			where b.def.hasInteractionCell && b.Faction == this.lord.faction && b.Position.InHorDistOf(this.FlagLoc, data.baseRadius)
			select b).Count<Thing>();
			if (num < num2)
			{
				num = num2;
			}
			int num3 = 0;
			for (int j = 0; j < this.lord.ownedPawns.Count; j++)
			{
				Pawn pawn = this.lord.ownedPawns[j];
				if (pawn.mindState.duty.def == DutyDefOf.Build)
				{
					this.rememberedDuties.Add(pawn, DutyDefOf.Build);
					this.SetAsBuilder(pawn);
					num3++;
				}
			}
			int num4 = num - num3;
			Func<Pawn, bool> <>9__1;
			for (int k = 0; k < num4; k++)
			{
				IEnumerable<Pawn> ownedPawns = this.lord.ownedPawns;
				Func<Pawn, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((Pawn pa) => !this.rememberedDuties.ContainsKey(pa) && this.CanBeBuilder(pa)));
				}
				Pawn pawn2;
				if (ownedPawns.Where(predicate).TryRandomElement(out pawn2))
				{
					this.rememberedDuties.Add(pawn2, DutyDefOf.Build);
					this.SetAsBuilder(pawn2);
					num3++;
				}
			}
			for (int l = 0; l < this.lord.ownedPawns.Count; l++)
			{
				Pawn pawn3 = this.lord.ownedPawns[l];
				if (!this.rememberedDuties.ContainsKey(pawn3))
				{
					this.SetAsDefender(pawn3);
					this.rememberedDuties.Add(pawn3, DutyDefOf.Defend);
				}
			}
			if (num3 == 0)
			{
				this.lord.ReceiveMemo("NoBuilders");
				return;
			}
		}

		// Token: 0x06005D97 RID: 23959 RVA: 0x002248BE File Offset: 0x00222ABE
		public override void Notify_PawnLost(Pawn victim, PawnLostCondition cond)
		{
			this.UpdateAllDuties();
			base.Notify_PawnLost(victim, cond);
		}

		// Token: 0x06005D98 RID: 23960 RVA: 0x002248CE File Offset: 0x00222ACE
		public override void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
			base.Notify_ConstructionFailed(pawn, frame, newBlueprint);
			if (frame.Faction == this.lord.faction && newBlueprint != null)
			{
				this.Data.blueprints.Add(newBlueprint);
			}
		}

		// Token: 0x06005D99 RID: 23961 RVA: 0x00224900 File Offset: 0x00222B00
		private bool CanBeBuilder(Pawn p)
		{
			return !p.WorkTypeIsDisabled(WorkTypeDefOf.Construction) && !p.WorkTypeIsDisabled(WorkTypeDefOf.Firefighter);
		}

		// Token: 0x06005D9A RID: 23962 RVA: 0x00224920 File Offset: 0x00222B20
		private void SetAsBuilder(Pawn p)
		{
			LordToilData_Siege data = this.Data;
			p.mindState.duty = new PawnDuty(DutyDefOf.Build, data.siegeCenter, -1f);
			p.mindState.duty.radius = data.baseRadius;
			int minLevel = Mathf.Max(ThingDefOf.Sandbags.constructionSkillPrerequisite, ThingDefOf.Turret_Mortar.constructionSkillPrerequisite);
			p.skills.GetSkill(SkillDefOf.Construction).EnsureMinLevelWithMargin(minLevel);
			p.workSettings.EnableAndInitialize();
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				if (workTypeDef == WorkTypeDefOf.Construction)
				{
					p.workSettings.SetPriority(workTypeDef, 1);
				}
				else
				{
					p.workSettings.Disable(workTypeDef);
				}
			}
		}

		// Token: 0x06005D9B RID: 23963 RVA: 0x002249F0 File Offset: 0x00222BF0
		private void SetAsDefender(Pawn p)
		{
			LordToilData_Siege data = this.Data;
			p.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.siegeCenter, -1f);
			p.mindState.duty.radius = data.baseRadius;
		}

		// Token: 0x06005D9C RID: 23964 RVA: 0x00224A40 File Offset: 0x00222C40
		public override void LordToilTick()
		{
			base.LordToilTick();
			LordToilData_Siege data = this.Data;
			if (this.lord.ticksInToil == 450)
			{
				this.lord.CurLordToil.UpdateAllDuties();
			}
			if (this.lord.ticksInToil > 450 && this.lord.ticksInToil % 500 == 0)
			{
				this.UpdateAllDuties();
			}
			if (Find.TickManager.TicksGame % 500 == 0)
			{
				if (!(from frame in this.Frames
				where !frame.Destroyed
				select frame).Any<Frame>())
				{
					if (!(from blue in data.blueprints
					where !blue.Destroyed
					select blue).Any<Blueprint>() && !base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial).Any((Thing b) => b.Faction == this.lord.faction && b.def.building.buildingTags.Contains("Artillery")))
					{
						this.lord.ReceiveMemo("NoArtillery");
						return;
					}
				}
				int num = GenRadial.NumCellsInRadius(20f);
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				for (int i = 0; i < num; i++)
				{
					IntVec3 c = data.siegeCenter + GenRadial.RadialPattern[i];
					if (c.InBounds(base.Map))
					{
						List<Thing> thingList = c.GetThingList(base.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							Thing thing = thingList[j];
							if (thing.def.IsShell)
							{
								ThingDef projectileWhenLoaded = thing.def.projectileWhenLoaded;
								if (((projectileWhenLoaded != null) ? projectileWhenLoaded.projectile : null) != null && thing.def.projectileWhenLoaded.projectile.damageDef.harmsHealth)
								{
									num2 += thing.stackCount;
								}
							}
							if (thing.def == ThingDefOf.MealSurvivalPack)
							{
								num3 += thing.stackCount;
							}
							if (thing.def == ThingDefOf.ReinforcedBarrel)
							{
								num5 += thing.stackCount;
							}
							if (!Find.Storyteller.difficulty.classicMortars && thing.def.building != null && thing.def.building.IsMortar && thing.Faction == this.lord.faction)
							{
								CompRefuelable compRefuelable = thing.TryGetComp<CompRefuelable>();
								if (compRefuelable != null)
								{
									num4++;
									if (compRefuelable.Props.fuelFilter.Allows(ThingDefOf.ReinforcedBarrel) && compRefuelable.HasFuel)
									{
										num5++;
									}
								}
							}
						}
					}
				}
				if (num2 < 4)
				{
					ThingDef thingDef = TurretGunUtility.TryFindRandomShellDef(ThingDefOf.Turret_Mortar, false, false, true, this.lord.faction.def.techLevel, false, 250f, this.lord.faction);
					if (thingDef != null)
					{
						this.DropSupplies(thingDef, 6);
					}
				}
				if (num3 < 5)
				{
					this.DropSupplies(ThingDefOf.MealSurvivalPack, 12);
				}
				if (!Find.Storyteller.difficulty.classicMortars && num5 < num4)
				{
					this.DropSupplies(ThingDefOf.ReinforcedBarrel, num4 - num5);
				}
			}
		}

		// Token: 0x06005D9D RID: 23965 RVA: 0x00224D60 File Offset: 0x00222F60
		private void DropSupplies(ThingDef thingDef, int count)
		{
			List<Thing> list = new List<Thing>();
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			thing.stackCount = count;
			list.Add(thing);
			DropPodUtility.DropThingsNear(this.Data.siegeCenter, base.Map, list, 110, false, false, true, true, true, null);
		}

		// Token: 0x06005D9E RID: 23966 RVA: 0x00224DA8 File Offset: 0x00222FA8
		public override void Cleanup()
		{
			LordToilData_Siege data = this.Data;
			data.blueprints.RemoveAll((Blueprint blue) => blue.Destroyed);
			for (int i = 0; i < data.blueprints.Count; i++)
			{
				data.blueprints[i].Destroy(DestroyMode.Cancel);
			}
			foreach (Frame frame in this.Frames.ToList<Frame>())
			{
				frame.Destroy(DestroyMode.Cancel);
			}
			foreach (Building building in this.lord.ownedBuildings)
			{
				building.SetFaction(null, null);
			}
		}

		// Token: 0x04003519 RID: 13593
		public Dictionary<Pawn, DutyDef> rememberedDuties = new Dictionary<Pawn, DutyDef>();

		// Token: 0x0400351A RID: 13594
		private const float BaseRadiusMin = 14f;

		// Token: 0x0400351B RID: 13595
		private const float BaseRadiusMax = 25f;

		// Token: 0x0400351C RID: 13596
		private static readonly FloatRange MealCountRangePerRaider = new FloatRange(1f, 3f);

		// Token: 0x0400351D RID: 13597
		private const int StartBuildingDelay = 450;

		// Token: 0x0400351E RID: 13598
		private static readonly FloatRange BuilderCountFraction = new FloatRange(0.25f, 0.4f);

		// Token: 0x0400351F RID: 13599
		private const float FractionLossesToAssault = 0.4f;

		// Token: 0x04003520 RID: 13600
		private const int InitalShellsPerCannon = 5;

		// Token: 0x04003521 RID: 13601
		private const int ReplenishAtShells = 4;

		// Token: 0x04003522 RID: 13602
		private const int ShellReplenishCount = 6;

		// Token: 0x04003523 RID: 13603
		private const int ReplenishAtMeals = 5;

		// Token: 0x04003524 RID: 13604
		private const int MealReplenishCount = 12;
	}
}
