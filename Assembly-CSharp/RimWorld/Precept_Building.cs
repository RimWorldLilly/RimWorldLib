using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001642 RID: 5698
	public class Precept_Building : Precept_ThingStyle
	{
		// Token: 0x170018A2 RID: 6306
		// (get) Token: 0x06008EB9 RID: 36537 RVA: 0x00347428 File Offset: 0x00345628
		public override bool CanRegenerate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170018A3 RID: 6307
		// (get) Token: 0x06008EBA RID: 36538 RVA: 0x0034742B File Offset: 0x0034562B
		protected override string NameRootSymbol
		{
			get
			{
				return "r_ideoBuildingName";
			}
		}

		// Token: 0x06008EBB RID: 36539 RVA: 0x00347432 File Offset: 0x00345632
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, null);
			if (!ModLister.CheckIdeology("Ideology building"))
			{
				return;
			}
			this.GeneratePresenceDemand();
		}

		// Token: 0x06008EBC RID: 36540 RVA: 0x0034744F File Offset: 0x0034564F
		public override void Regenerate(Ideo ideo, FactionDef generatingFor = null)
		{
			this.GeneratePresenceDemand();
			if (this.UsesGeneratedName)
			{
				base.RegenerateName();
			}
			this.ClearTipCache();
			this.Notify_ThingDefSet();
		}

		// Token: 0x06008EBD RID: 36541 RVA: 0x00347474 File Offset: 0x00345674
		protected void GeneratePresenceDemand()
		{
			this.presenceDemand = new IdeoBuildingPresenceDemand(this);
			this.presenceDemand.minExpectation = this.def.buildingMinExpectations.RandomElement<ExpectationDef>();
			int num = Mathf.CeilToInt(this.def.roomRequirementCountCurve.Evaluate(Rand.Value));
			if ((base.ThingDef.ritualFocus == null || !base.ThingDef.ritualFocus.consumable) && num > 0)
			{
				this.presenceDemand.roomRequirements = new List<RoomRequirement>();
				List<RoomRequirement> list = this.def.buildingRoomRequirements.ToList<RoomRequirement>();
				list.Shuffle<RoomRequirement>();
				for (int i = 0; i < num; i++)
				{
					this.presenceDemand.roomRequirements.Add(list[i]);
				}
				this.presenceDemand.roomRequirements.AddRange(this.def.buildingRoomRequirementsFixed);
			}
		}

		// Token: 0x06008EBE RID: 36542 RVA: 0x0034754B File Offset: 0x0034574B
		public override string TransformThingLabel(string label)
		{
			return label + " (" + base.LabelCap + ")";
		}

		// Token: 0x06008EBF RID: 36543 RVA: 0x00347564 File Offset: 0x00345764
		private string CostListString()
		{
			List<string> list = new List<string>();
			if (!base.ThingDef.CostList.NullOrEmpty<ThingDefCountClass>())
			{
				for (int i = 0; i < base.ThingDef.CostList.Count; i++)
				{
					list.Add(base.ThingDef.CostList[i].thingDef.LabelCap + " x" + base.ThingDef.CostList[i].count.ToString());
				}
			}
			if (!base.ThingDef.stuffCategories.NullOrEmpty<StuffCategoryDef>())
			{
				string text = "";
				for (int j = 0; j < base.ThingDef.stuffCategories.Count; j++)
				{
					if (j > 0)
					{
						text += "/";
					}
					text += base.ThingDef.stuffCategories[j].label;
				}
				list.Add(text.CapitalizeFirst() + " x" + base.ThingDef.CostStuffCount.ToString());
			}
			return list.ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x06008EC0 RID: 36544 RVA: 0x0034768C File Offset: 0x0034588C
		public override string GetTip()
		{
			if (this.tipCached.NullOrEmpty())
			{
				Precept.tmpCompsDesc.Clear();
				ThingDef thingDef = base.ThingDef;
				if (((thingDef != null) ? thingDef.description : null) != null)
				{
					Precept.tmpCompsDesc.Append(base.ThingDef.description);
				}
				else if (!base.Description.NullOrEmpty())
				{
					Precept.tmpCompsDesc.Append(base.Description);
				}
				Precept.tmpCompsDesc.AppendLine();
				Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("IdeoBuildingVariationOf".Translate() + ": "));
				Precept.tmpCompsDesc.AppendInNewLine(base.ThingDef.LabelCap.Resolve().Indented("    "));
				Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("Size".Translate().CapitalizeFirst() + ": ").Indented("    "));
				Precept.tmpCompsDesc.Append(base.ThingDef.size.x.ToString() + "x" + base.ThingDef.size.z.ToString());
				Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("Cost".Translate().CapitalizeFirst() + ": ").Indented("    "));
				Precept.tmpCompsDesc.Append(this.CostListString());
				if (base.ThingDef.ritualFocus == null || !base.ThingDef.ritualFocus.consumable)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("IdeoBuildingMinimumExpectations".Translate() + ": "));
					Precept.tmpCompsDesc.Append(this.presenceDemand.minExpectation.LabelCap);
					if (!this.presenceDemand.roomRequirements.NullOrEmpty<RoomRequirement>())
					{
						Precept.tmpCompsDesc.AppendLine();
						Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoomRequirements".Translate() + ":"));
						Precept.tmpCompsDesc.AppendInNewLine(this.presenceDemand.RoomRequirementsInfo.ToLineList(" -  ", true));
					}
				}
				this.tipCached = Precept.tmpCompsDesc.ToString();
			}
			return this.tipCached;
		}

		// Token: 0x06008EC1 RID: 36545 RVA: 0x00347900 File Offset: 0x00345B00
		public override List<Thought_Situational> SituationThoughtsToAdd(Pawn pawn, List<Thought_Situational> activeThoughts)
		{
			this.tmpSituationalThoughts.Clear();
			if (!pawn.IsFreeColonist)
			{
				return this.tmpSituationalThoughts;
			}
			Map mapHeld = pawn.MapHeld;
			if (mapHeld != null)
			{
				if (!this.presenceDemand.AppliesTo(mapHeld))
				{
					return this.tmpSituationalThoughts;
				}
				if (!this.presenceDemand.BuildingPresent(mapHeld))
				{
					if (!activeThoughts.Any(delegate(Thought_Situational t)
					{
						Thought_IdeoMissingBuilding thought_IdeoMissingBuilding2 = t as Thought_IdeoMissingBuilding;
						return thought_IdeoMissingBuilding2 != null && thought_IdeoMissingBuilding2.demand == this.presenceDemand;
					}))
					{
						Thought_IdeoMissingBuilding thought_IdeoMissingBuilding = (Thought_IdeoMissingBuilding)ThoughtMaker.MakeThought(ThoughtDefOf.IdeoBuildingMissing);
						if (thought_IdeoMissingBuilding != null)
						{
							thought_IdeoMissingBuilding.pawn = pawn;
							thought_IdeoMissingBuilding.demand = this.presenceDemand;
							thought_IdeoMissingBuilding.sourcePrecept = this;
							this.tmpSituationalThoughts.Add(thought_IdeoMissingBuilding);
						}
					}
				}
				else if (!this.presenceDemand.RequirementsSatisfied(mapHeld) && !activeThoughts.Any(delegate(Thought_Situational t)
				{
					Thought_IdeoDisrespectedBuilding thought_IdeoDisrespectedBuilding2 = t as Thought_IdeoDisrespectedBuilding;
					return thought_IdeoDisrespectedBuilding2 != null && thought_IdeoDisrespectedBuilding2.demand == this.presenceDemand;
				}))
				{
					Thought_IdeoDisrespectedBuilding thought_IdeoDisrespectedBuilding = (Thought_IdeoDisrespectedBuilding)ThoughtMaker.MakeThought(ThoughtDefOf.IdeoBuildingDisrespected);
					if (thought_IdeoDisrespectedBuilding != null)
					{
						thought_IdeoDisrespectedBuilding.pawn = pawn;
						thought_IdeoDisrespectedBuilding.demand = this.presenceDemand;
						thought_IdeoDisrespectedBuilding.sourcePrecept = this;
						this.tmpSituationalThoughts.Add(thought_IdeoDisrespectedBuilding);
					}
				}
			}
			return this.tmpSituationalThoughts;
		}

		// Token: 0x06008EC2 RID: 36546 RVA: 0x00347A0D File Offset: 0x00345C0D
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			Precept_Building.<EditFloatMenuOptions>d__12 <EditFloatMenuOptions>d__ = new Precept_Building.<EditFloatMenuOptions>d__12(-2);
			<EditFloatMenuOptions>d__.<>4__this = this;
			return <EditFloatMenuOptions>d__;
		}

		// Token: 0x06008EC3 RID: 36547 RVA: 0x00347A1D File Offset: 0x00345C1D
		public override IEnumerable<Alert> GetAlerts()
		{
			Precept_Building.<GetAlerts>d__13 <GetAlerts>d__ = new Precept_Building.<GetAlerts>d__13(-2);
			<GetAlerts>d__.<>4__this = this;
			return <GetAlerts>d__;
		}

		// Token: 0x06008EC4 RID: 36548 RVA: 0x00347A2D File Offset: 0x00345C2D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<IdeoBuildingPresenceDemand>(ref this.presenceDemand, "presenceDemand", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.presenceDemand.parent = this;
			}
		}

		// Token: 0x06008EC5 RID: 36549 RVA: 0x00347A60 File Offset: 0x00345C60
		public override void CopyTo(Precept precept)
		{
			base.CopyTo(precept);
			Precept_Building precept_Building = (Precept_Building)precept;
			precept_Building.presenceDemand = this.presenceDemand.Copy();
			precept_Building.presenceDemand.parent = precept_Building;
		}

		// Token: 0x06008EC6 RID: 36550 RVA: 0x00347A98 File Offset: 0x00345C98
		public override string InspectStringExtra(Thing thing)
		{
			return ("Stat_Thing_RelatedToIdeos_Name".Translate() + ": " + this.ideo.name.ApplyTag(this.ideo)).Resolve();
		}

		// Token: 0x06008EC7 RID: 36551 RVA: 0x00347ADC File Offset: 0x00345CDC
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(Thing thing)
		{
			Precept_Building.<SpecialDisplayStats>d__17 <SpecialDisplayStats>d__ = new Precept_Building.<SpecialDisplayStats>d__17(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x04005776 RID: 22390
		public IdeoBuildingPresenceDemand presenceDemand;
	}
}
