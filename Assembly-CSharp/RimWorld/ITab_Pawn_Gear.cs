using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001D72 RID: 7538
	public class ITab_Pawn_Gear : ITab
	{
		// Token: 0x17002436 RID: 9270
		// (get) Token: 0x0600C364 RID: 50020 RVA: 0x004754B0 File Offset: 0x004736B0
		public override bool IsVisible
		{
			get
			{
				Pawn selPawnForGear = this.SelPawnForGear;
				return selPawnForGear.ageTracker.CurLifeStage != LifeStageDefOf.HumanlikeBaby && (!ModsConfig.AnomalyActive || !selPawnForGear.RaceProps.IsAnomalyEntity) && (!ModsConfig.OdysseyActive || !selPawnForGear.RaceProps.IsDrone) && (this.ShouldShowInventory(selPawnForGear) || this.ShouldShowApparel(selPawnForGear) || this.ShouldShowEquipment(selPawnForGear));
			}
		}

		// Token: 0x17002437 RID: 9271
		// (get) Token: 0x0600C365 RID: 50021 RVA: 0x00475520 File Offset: 0x00473720
		private bool CanControl
		{
			get
			{
				Pawn selPawnForGear = this.SelPawnForGear;
				return !selPawnForGear.Downed && !selPawnForGear.InMentalState && selPawnForGear.CarriedBy == null && (selPawnForGear.Faction == Faction.OfPlayer || selPawnForGear.IsPrisonerOfColony) && (!selPawnForGear.IsPrisonerOfColony || !selPawnForGear.Spawned || selPawnForGear.Map.mapPawns.AnyFreeColonistSpawned) && (!selPawnForGear.IsPrisonerOfColony || (!PrisonBreakUtility.IsPrisonBreaking(selPawnForGear) && (selPawnForGear.CurJob == null || !selPawnForGear.CurJob.exitMapOnArrival)));
			}
		}

		// Token: 0x17002438 RID: 9272
		// (get) Token: 0x0600C366 RID: 50022 RVA: 0x004755B1 File Offset: 0x004737B1
		private bool CanControlColonist
		{
			get
			{
				return this.CanControl && this.SelPawnForGear.IsColonistPlayerControlled;
			}
		}

		// Token: 0x17002439 RID: 9273
		// (get) Token: 0x0600C367 RID: 50023 RVA: 0x004755C8 File Offset: 0x004737C8
		private Pawn SelPawnForGear
		{
			get
			{
				if (this.SelPawn != null)
				{
					return this.SelPawn;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				string str = "Gear tab on non-pawn non-corpse ";
				Thing selThing = base.SelThing;
				throw new InvalidOperationException(str + ((selThing != null) ? selThing.ToString() : null));
			}
		}

		// Token: 0x0600C368 RID: 50024 RVA: 0x0047561B File Offset: 0x0047381B
		public ITab_Pawn_Gear()
		{
			this.size = new Vector2(460f, 450f);
			this.labelKey = "TabGear";
			this.tutorTag = "Gear";
		}

		// Token: 0x0600C369 RID: 50025 RVA: 0x0047565C File Offset: 0x0047385C
		protected override void FillTab()
		{
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 20f, this.size.x, this.size.y - 20f);
			Rect rect2 = rect.ContractedBy(10f);
			Rect rect3 = new Rect(rect2.x, rect2.y, rect2.width, rect2.height);
			if (Prefs.DevMode && Widgets.ButtonText(new Rect(rect.xMax - 18f - 125f, 5f, 115f, Text.LineHeight), "Dev tool...", true, true, true, null))
			{
				Find.WindowStack.Add(new FloatMenu(DebugToolsPawns.PawnGearDevOptions(this.SelPawnForGear)));
			}
			Widgets.BeginGroup(rect3);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Rect outRect = new Rect(0f, 0f, rect3.width, rect3.height);
			Rect viewRect = new Rect(0f, 0f, rect3.width - 16f, this.scrollViewHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			float num = 0f;
			this.TryDrawMassInfo(ref num, viewRect.width);
			this.TryDrawComfyTemperatureRange(ref num, viewRect.width);
			if (this.ShouldShowOverallArmor(this.SelPawnForGear))
			{
				Widgets.ListSeparator(ref num, viewRect.width, "OverallArmor".Translate());
				this.TryDrawOverallArmor(ref num, viewRect.width, StatDefOf.ArmorRating_Sharp, "ArmorSharp".Translate());
				this.TryDrawOverallArmor(ref num, viewRect.width, StatDefOf.ArmorRating_Blunt, "ArmorBlunt".Translate());
				this.TryDrawOverallArmor(ref num, viewRect.width, StatDefOf.ArmorRating_Heat, "ArmorHeat".Translate());
			}
			if (this.ShouldShowEquipment(this.SelPawnForGear))
			{
				Widgets.ListSeparator(ref num, viewRect.width, "Equipment".Translate());
				ITab_Pawn_Gear.workingEquipmentList.Clear();
				ITab_Pawn_Gear.workingEquipmentList.AddRange(this.SelPawnForGear.equipment.AllEquipmentListForReading);
				ITab_Pawn_Gear.workingEquipmentList.AddRange(from x in this.SelPawnForGear.apparel.WornApparel
				where x.def.apparel.layers.Contains(ApparelLayerDefOf.Belt)
				select x);
				foreach (Thing thing in ITab_Pawn_Gear.workingEquipmentList)
				{
					this.DrawThingRow(ref num, viewRect.width, thing, false);
				}
			}
			if (this.ShouldShowApparel(this.SelPawnForGear))
			{
				Widgets.ListSeparator(ref num, viewRect.width, "Apparel".Translate());
				foreach (Apparel thing2 in from x in this.SelPawnForGear.apparel.WornApparel
				where !x.def.apparel.layers.Contains(ApparelLayerDefOf.Belt)
				select x into ap
				orderby ap.def.apparel.bodyPartGroups[0].listOrder descending
				select ap)
				{
					this.DrawThingRow(ref num, viewRect.width, thing2, false);
				}
			}
			if (this.ShouldShowInventory(this.SelPawnForGear))
			{
				Widgets.ListSeparator(ref num, viewRect.width, "Inventory".Translate());
				ITab_Pawn_Gear.workingInvList.Clear();
				ITab_Pawn_Gear.workingInvList.AddRange(this.SelPawnForGear.inventory.innerContainer);
				for (int i = 0; i < ITab_Pawn_Gear.workingInvList.Count; i++)
				{
					this.DrawThingRow(ref num, viewRect.width, ITab_Pawn_Gear.workingInvList[i], true);
				}
				ITab_Pawn_Gear.workingInvList.Clear();
			}
			if (Event.current.type == EventType.layout)
			{
				if (num + 70f > 450f)
				{
					this.size.y = Mathf.Min(num + 70f, (float)(UI.screenHeight - 35) - 165f - 30f);
				}
				else
				{
					this.size.y = 450f;
				}
				this.scrollViewHeight = num + 20f;
			}
			Widgets.EndScrollView();
			Widgets.EndGroup();
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600C36A RID: 50026 RVA: 0x00475B10 File Offset: 0x00473D10
		private void DrawThingRow(ref float y, float width, Thing thing, bool inventory = false)
		{
			Rect rect = new Rect(0f, y, width, 28f);
			Widgets.InfoCardButton(rect.width - 24f, y, thing);
			rect.width -= 24f;
			bool flag = false;
			if (this.CanControl && (inventory || this.CanControlColonist || (this.SelPawnForGear.Spawned && !this.SelPawnForGear.Map.IsPlayerHome)))
			{
				Rect rect2 = new Rect(rect.width - 24f, y, 24f, 24f);
				bool flag2 = false;
				if (this.SelPawnForGear.IsQuestLodger())
				{
					flag2 = (inventory || !EquipmentUtility.QuestLodgerCanUnequip(thing, this.SelPawnForGear));
				}
				bool flag3 = !inventory && this.SelPawnForGear.kindDef.destroyGearOnDrop;
				Apparel apparel = thing as Apparel;
				bool flag4 = apparel != null && this.SelPawnForGear.apparel != null && this.SelPawnForGear.apparel.IsLocked(apparel);
				flag = (flag2 || flag4 || flag3);
				if (Mouse.IsOver(rect2))
				{
					if (flag4)
					{
						TooltipHandler.TipRegion(rect2, "DropThingLocked".Translate());
					}
					else if (flag2)
					{
						TooltipHandler.TipRegion(rect2, "DropThingLodger".Translate());
					}
					else
					{
						TooltipHandler.TipRegion(rect2, "DropThing".Translate());
					}
				}
				Color color = flag ? Color.grey : Color.white;
				Color mouseoverColor = flag ? color : GenUI.MouseoverColor;
				if (Widgets.ButtonImage(rect2, TexButton.Drop, color, mouseoverColor, !flag, null) && !flag)
				{
					Action action = delegate()
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.InterfaceDrop(thing);
					};
					if (!ModsConfig.BiotechActive || !MechanitorUtility.TryConfirmBandwidthLossFromDroppingThing(this.SelPawnForGear, thing, action))
					{
						action();
					}
				}
				rect.width -= 24f;
			}
			if (this.CanControlColonist)
			{
				if (FoodUtility.WillIngestFromInventoryNow(this.SelPawnForGear, thing))
				{
					Rect rect3 = new Rect(rect.width - 24f, y, 24f, 24f);
					TooltipHandler.TipRegionByKey(rect3, "ConsumeThing", thing.LabelNoCount, thing);
					if (Widgets.ButtonImage(rect3, TexButton.Ingest, true, null))
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						FoodUtility.IngestFromInventoryNow(this.SelPawnForGear, thing);
					}
				}
				rect.width -= 24f;
			}
			Rect rect4 = rect;
			rect4.xMin = rect4.xMax - 60f;
			CaravanThingsTabUtility.DrawMass(thing, rect4);
			rect.width -= 60f;
			if (Mouse.IsOver(rect))
			{
				GUI.color = ITab_Pawn_Gear.HighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (thing.def.DrawMatSingle != null && thing.def.DrawMatSingle.mainTexture != null)
			{
				Widgets.ThingIcon(new Rect(4f, y, 28f, 28f), thing, 1f, null, false, 1f, false);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = ITab_Pawn_Gear.ThingLabelColor;
			Rect rect5 = new Rect(36f, y, rect.width - 36f, rect.height);
			string text = thing.LabelCap;
			Apparel apparel2 = thing as Apparel;
			if (apparel2 != null && this.SelPawnForGear.outfits != null && this.SelPawnForGear.outfits.forcedHandler.IsForced(apparel2))
			{
				text += ", " + "ApparelForcedLower".Translate();
			}
			if (flag)
			{
				text += " (" + "ApparelLockedLower".Translate() + ")";
			}
			Text.WordWrap = false;
			Widgets.Label(rect5, text.Truncate(rect5.width, null));
			Text.WordWrap = true;
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, thing.GetTooltip());
			}
			y += 28f;
		}

		// Token: 0x0600C36B RID: 50027 RVA: 0x00475F94 File Offset: 0x00474194
		private void TryDrawOverallArmor(ref float curY, float width, StatDef stat, string label)
		{
			float num = 0f;
			float num2 = Mathf.Clamp01(this.SelPawnForGear.GetStatValue(stat, true, -1) / 2f);
			List<BodyPartRecord> allParts = this.SelPawnForGear.RaceProps.body.AllParts;
			List<Apparel> list = (this.SelPawnForGear.apparel != null) ? this.SelPawnForGear.apparel.WornApparel : null;
			for (int i = 0; i < allParts.Count; i++)
			{
				float num3 = 1f - num2;
				if (list != null)
				{
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].def.apparel.CoversBodyPart(allParts[i]))
						{
							float num4 = Mathf.Clamp01(list[j].GetStatValue(stat, true, -1) / 2f);
							num3 *= 1f - num4;
						}
					}
				}
				num += allParts[i].coverageAbs * (1f - num3);
			}
			num = Mathf.Clamp(num * 2f, 0f, 2f);
			Rect rect = new Rect(0f, curY, width, 100f);
			Widgets.Label(rect, label.Truncate(120f, null));
			rect.xMin += 120f;
			Widgets.Label(rect, num.ToStringPercent());
			curY += 22f;
		}

		// Token: 0x0600C36C RID: 50028 RVA: 0x00476104 File Offset: 0x00474304
		private void TryDrawMassInfo(ref float curY, float width)
		{
			if (this.SelPawnForGear.Dead || !this.ShouldShowInventory(this.SelPawnForGear))
			{
				return;
			}
			Rect rect = new Rect(0f, curY, width, 22f);
			float num = MassUtility.GearAndInventoryMass(this.SelPawnForGear);
			float num2 = MassUtility.Capacity(this.SelPawnForGear, null);
			Widgets.Label(rect, "MassCarried".Translate(num.ToString("0.##"), num2.ToString("0.##")));
			curY += 22f;
		}

		// Token: 0x0600C36D RID: 50029 RVA: 0x00476194 File Offset: 0x00474394
		private void TryDrawComfyTemperatureRange(ref float curY, float width)
		{
			if (this.SelPawnForGear.Dead)
			{
				return;
			}
			Rect rect = new Rect(0f, curY, width, 22f);
			float statValue = this.SelPawnForGear.GetStatValue(StatDefOf.ComfyTemperatureMin, true, -1);
			float statValue2 = this.SelPawnForGear.GetStatValue(StatDefOf.ComfyTemperatureMax, true, -1);
			Widgets.Label(rect, "ComfyTemperatureRange".Translate() + ": " + statValue.ToStringTemperature("F0") + " ~ " + statValue2.ToStringTemperature("F0"));
			curY += 22f;
		}

		// Token: 0x0600C36E RID: 50030 RVA: 0x00476234 File Offset: 0x00474434
		private void InterfaceDrop(Thing t)
		{
			ThingWithComps thingWithComps = t as ThingWithComps;
			Apparel apparel = t as Apparel;
			if (apparel != null && this.SelPawnForGear.apparel != null && this.SelPawnForGear.apparel.WornApparel.Contains(apparel))
			{
				this.SelPawnForGear.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.RemoveApparel, apparel), new JobTag?(JobTag.Misc), false);
				return;
			}
			if (thingWithComps != null && this.SelPawnForGear.equipment != null && this.SelPawnForGear.equipment.AllEquipmentListForReading.Contains(thingWithComps))
			{
				this.SelPawnForGear.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.DropEquipment, thingWithComps), new JobTag?(JobTag.Misc), false);
				return;
			}
			if (!t.def.destroyOnDrop)
			{
				Thing thing;
				this.SelPawnForGear.inventory.innerContainer.TryDrop(t, this.SelPawnForGear.Position, this.SelPawnForGear.Map, ThingPlaceMode.Near, out thing, null, null);
			}
		}

		// Token: 0x0600C36F RID: 50031 RVA: 0x00476330 File Offset: 0x00474530
		[Obsolete("Will be removed in a future update, use FoodUtility.IngestFromInventoryNow()")]
		private void InterfaceIngest(Thing t)
		{
			FoodUtility.IngestFromInventoryNow(this.SelPawnForGear, t);
		}

		// Token: 0x0600C370 RID: 50032 RVA: 0x0047633E File Offset: 0x0047453E
		private bool ShouldShowInventory(Pawn p)
		{
			return p.RaceProps.Humanlike || p.inventory.innerContainer.Any;
		}

		// Token: 0x0600C371 RID: 50033 RVA: 0x0047635F File Offset: 0x0047455F
		private bool ShouldShowApparel(Pawn p)
		{
			return p.apparel != null && (p.RaceProps.Humanlike || p.apparel.WornApparel.Any<Apparel>());
		}

		// Token: 0x0600C372 RID: 50034 RVA: 0x0047638A File Offset: 0x0047458A
		private bool ShouldShowEquipment(Pawn p)
		{
			return p.equipment != null;
		}

		// Token: 0x0600C373 RID: 50035 RVA: 0x00476398 File Offset: 0x00474598
		private bool ShouldShowOverallArmor(Pawn p)
		{
			return p.RaceProps.Humanlike || this.ShouldShowApparel(p) || p.GetStatValue(StatDefOf.ArmorRating_Sharp, true, -1) > 0f || p.GetStatValue(StatDefOf.ArmorRating_Blunt, true, -1) > 0f || p.GetStatValue(StatDefOf.ArmorRating_Heat, true, -1) > 0f;
		}

		// Token: 0x04006FA7 RID: 28583
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04006FA8 RID: 28584
		private float scrollViewHeight;

		// Token: 0x04006FA9 RID: 28585
		private const float TopPadding = 20f;

		// Token: 0x04006FAA RID: 28586
		public static readonly Color ThingLabelColor = new Color(0.9f, 0.9f, 0.9f, 1f);

		// Token: 0x04006FAB RID: 28587
		public static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		// Token: 0x04006FAC RID: 28588
		private const float ThingIconSize = 28f;

		// Token: 0x04006FAD RID: 28589
		private const float ThingRowHeight = 28f;

		// Token: 0x04006FAE RID: 28590
		private const float ThingLeftX = 36f;

		// Token: 0x04006FAF RID: 28591
		private const float StandardLineHeight = 22f;

		// Token: 0x04006FB0 RID: 28592
		private const float InitialHeight = 450f;

		// Token: 0x04006FB1 RID: 28593
		private static List<Thing> workingInvList = new List<Thing>();

		// Token: 0x04006FB2 RID: 28594
		private static List<Thing> workingEquipmentList = new List<Thing>();
	}
}
