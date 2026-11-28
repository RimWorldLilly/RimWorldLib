using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020023EF RID: 9199
	public class WITab_Caravan_Gear : WITab
	{
		// Token: 0x17002893 RID: 10387
		// (get) Token: 0x0600E5CD RID: 58829 RVA: 0x00556083 File Offset: 0x00554283
		private List<Pawn> Pawns
		{
			get
			{
				return base.SelCaravan.PawnsListForReading;
			}
		}

		// Token: 0x0600E5CE RID: 58830 RVA: 0x00556090 File Offset: 0x00554290
		public WITab_Caravan_Gear()
		{
			this.labelKey = "TabCaravanGear";
		}

		// Token: 0x0600E5CF RID: 58831 RVA: 0x005560A4 File Offset: 0x005542A4
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.leftPaneWidth = 469f;
			this.rightPaneWidth = 345f;
			this.size.x = this.leftPaneWidth + this.rightPaneWidth;
			this.size.y = Mathf.Min(550f, this.PaneTopY - 30f);
		}

		// Token: 0x0600E5D0 RID: 58832 RVA: 0x00556106 File Offset: 0x00554306
		public override void OnOpen()
		{
			base.OnOpen();
			this.draggedItem = null;
		}

		// Token: 0x0600E5D1 RID: 58833 RVA: 0x00556118 File Offset: 0x00554318
		protected override void FillTab()
		{
			Text.Font = GameFont.Small;
			this.CheckDraggedItemStillValid();
			this.CheckDropDraggedItem();
			Rect rect = new Rect(0f, 0f, this.leftPaneWidth, this.size.y);
			Widgets.BeginGroup(rect);
			this.DoLeftPane();
			Widgets.EndGroup();
			Widgets.BeginGroup(new Rect(rect.xMax, 0f, this.rightPaneWidth, this.size.y));
			this.DoRightPane();
			Widgets.EndGroup();
			if (this.draggedItem != null && this.droppedDraggedItem)
			{
				this.droppedDraggedItem = false;
				this.draggedItem = null;
			}
		}

		// Token: 0x0600E5D2 RID: 58834 RVA: 0x005561BC File Offset: 0x005543BC
		private void DoLeftPane()
		{
			Rect rect = new Rect(0f, 0f, this.leftPaneWidth, this.size.y).ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, this.leftPaneScrollViewHeight);
			float num = 0f;
			Widgets.BeginScrollView(rect, ref this.leftPaneScrollPosition, rect2, true);
			this.DoPawnRows(ref num, rect2, rect);
			if (Event.current.type == EventType.layout)
			{
				this.leftPaneScrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600E5D3 RID: 58835 RVA: 0x00556258 File Offset: 0x00554458
		private void DoRightPane()
		{
			Rect rect = new Rect(0f, 0f, this.rightPaneWidth, this.size.y).ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, this.rightPaneScrollViewHeight);
			if (this.draggedItem != null && rect.Contains(Event.current.mousePosition) && this.CurrentWearerOf(this.draggedItem) != null)
			{
				Widgets.DrawHighlight(rect);
				if (this.droppedDraggedItem)
				{
					this.MoveDraggedItemToInventory();
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				}
			}
			float num = 0f;
			Widgets.BeginScrollView(rect, ref this.rightPaneScrollPosition, rect2, true);
			this.DoInventoryRows(ref num, rect2, rect);
			if (Event.current.type == EventType.layout)
			{
				this.rightPaneScrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600E5D4 RID: 58836 RVA: 0x00556340 File Offset: 0x00554540
		protected override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (this.draggedItem != null)
			{
				Vector2 mousePosition = Event.current.mousePosition;
				Rect rect = new Rect(mousePosition.x - this.draggedItemPosOffset.x, mousePosition.y - this.draggedItemPosOffset.y, 32f, 32f);
				Find.WindowStack.ImmediateWindow(1283641090, rect, WindowLayer.Super, delegate
				{
					if (this.draggedItem == null)
					{
						return;
					}
					Widgets.ThingIcon(rect.AtZero(), this.draggedItem, 1f, null, false, 1f, false);
				}, false, false, 0f, null, false);
			}
			this.CheckDropDraggedItem();
		}

		// Token: 0x0600E5D5 RID: 58837 RVA: 0x005563E0 File Offset: 0x005545E0
		private void DoPawnRows(ref float curY, Rect scrollViewRect, Rect scrollOutRect)
		{
			List<Pawn> pawns = this.Pawns;
			Text.Font = GameFont.Tiny;
			GUI.color = Color.gray;
			Widgets.Label(new Rect(135f, curY + 6f, 200f, 100f), "DragToRearrange".Translate());
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanColonists".Translate());
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (pawn.IsColonist)
				{
					this.DoPawnRow(ref curY, scrollViewRect, scrollOutRect, pawn);
				}
			}
			bool flag = false;
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn2 = pawns[j];
				if (pawn2.IsPrisoner)
				{
					if (!flag)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanPrisoners".Translate());
						flag = true;
					}
					this.DoPawnRow(ref curY, scrollViewRect, scrollOutRect, pawn2);
				}
			}
		}

		// Token: 0x0600E5D6 RID: 58838 RVA: 0x005564DC File Offset: 0x005546DC
		private void DoPawnRow(ref float curY, Rect viewRect, Rect scrollOutRect, Pawn p)
		{
			float num = this.leftPaneScrollPosition.y - 40f;
			float num2 = this.leftPaneScrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoPawnRow(new Rect(0f, curY, viewRect.width, 40f), p);
			}
			curY += 40f;
		}

		// Token: 0x0600E5D7 RID: 58839 RVA: 0x00556544 File Offset: 0x00554744
		private void DoPawnRow(Rect rect, Pawn p)
		{
			Widgets.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			CaravanThingsTabUtility.DoAbandonButton(rect2, p, base.SelCaravan);
			rect2.width -= 24f;
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, p);
			rect2.width -= 24f;
			bool flag = this.draggedItem != null && rect2.Contains(Event.current.mousePosition) && this.CurrentWearerOf(this.draggedItem) != p;
			if ((Mouse.IsOver(rect2) && this.draggedItem == null) || flag)
			{
				Widgets.DrawHighlight(rect2);
			}
			if (flag && this.droppedDraggedItem)
			{
				this.TryEquipDraggedItem(p);
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, p, 1f, null, false, 1f, false);
			Rect bgRect = new Rect(rect3.xMax + 4f, 11f, 100f, 18f);
			GenMapUI.DrawPawnLabel(p, bgRect, 1f, 100f, null, GameFont.Small, false, false);
			float xMax = bgRect.xMax;
			if (p.equipment != null)
			{
				List<ThingWithComps> allEquipmentListForReading = p.equipment.AllEquipmentListForReading;
				for (int i = 0; i < allEquipmentListForReading.Count; i++)
				{
					this.DoEquippedGear(allEquipmentListForReading[i], p, ref xMax);
				}
			}
			if (p.apparel != null)
			{
				WITab_Caravan_Gear.tmpApparel.Clear();
				WITab_Caravan_Gear.tmpApparel.AddRange(p.apparel.WornApparel);
				WITab_Caravan_Gear.tmpApparel.SortBy((Apparel x) => x.def.apparel.LastLayer.drawOrder, (Apparel x) => -x.def.apparel.HumanBodyCoverage);
				for (int j = 0; j < WITab_Caravan_Gear.tmpApparel.Count; j++)
				{
					this.DoEquippedGear(WITab_Caravan_Gear.tmpApparel[j], p, ref xMax);
				}
			}
			if (p.Downed && !p.ageTracker.CurLifeStage.alwaysDowned)
			{
				GUI.color = new Color(1f, 0f, 0f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect.height / 2f, rect.width);
				GUI.color = Color.white;
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600E5D8 RID: 58840 RVA: 0x005567F0 File Offset: 0x005549F0
		private void DoInventoryRows(ref float curY, Rect scrollViewRect, Rect scrollOutRect)
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(base.SelCaravan);
			Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanWeaponsAndApparel".Translate());
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (this.IsVisibleWeapon(thing.def))
				{
					if (!flag)
					{
						flag = true;
					}
					this.DoInventoryRow(ref curY, scrollViewRect, scrollOutRect, thing);
				}
			}
			bool flag2 = false;
			for (int j = 0; j < list.Count; j++)
			{
				Thing thing2 = list[j];
				if (thing2.def.IsApparel)
				{
					if (!flag2)
					{
						flag2 = true;
					}
					this.DoInventoryRow(ref curY, scrollViewRect, scrollOutRect, thing2);
				}
			}
			if (!flag && !flag2)
			{
				Widgets.NoneLabel(ref curY, scrollViewRect.width, null);
			}
		}

		// Token: 0x0600E5D9 RID: 58841 RVA: 0x005568B4 File Offset: 0x00554AB4
		private void DoInventoryRow(ref float curY, Rect viewRect, Rect scrollOutRect, Thing t)
		{
			float num = this.rightPaneScrollPosition.y - 30f;
			float num2 = this.rightPaneScrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoInventoryRow(new Rect(0f, curY, viewRect.width, 30f), t);
			}
			curY += 30f;
		}

		// Token: 0x0600E5DA RID: 58842 RVA: 0x0055691C File Offset: 0x00554B1C
		private void DoInventoryRow(Rect rect, Thing t)
		{
			Widgets.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, t);
			rect2.width -= 24f;
			if (this.draggedItem == null && Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			float num = (t == this.draggedItem) ? 0.5f : 1f;
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, t, num, null, false, 1f, false);
			GUI.color = new Color(1f, 1f, 1f, num);
			Rect rect4 = new Rect(rect3.xMax + 4f, 0f, 250f, 30f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect4, t.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.color = Color.white;
			if (Event.current.type == EventType.mouseDown && Event.current.button == 0 && Mouse.IsOver(rect2))
			{
				this.draggedItem = t;
				this.droppedDraggedItem = false;
				this.draggedItemPosOffset = new Vector2(16f, 16f);
				Event.current.Use();
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600E5DB RID: 58843 RVA: 0x00556AA4 File Offset: 0x00554CA4
		private void DoEquippedGear(Thing t, Pawn p, ref float curX)
		{
			Rect rect = new Rect(curX, 4f, 32f, 32f);
			bool flag = Mouse.IsOver(rect);
			float alpha;
			if (t == this.draggedItem)
			{
				alpha = 0.2f;
			}
			else if (flag && this.draggedItem == null)
			{
				alpha = 0.75f;
			}
			else
			{
				alpha = 1f;
			}
			Widgets.ThingIcon(rect, t, alpha, null, false, 1f, false);
			curX += 32f;
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, t.LabelCap);
			}
			if (Event.current.type == EventType.mouseDown && Event.current.button == 0 && flag)
			{
				this.draggedItem = t;
				this.droppedDraggedItem = false;
				this.draggedItemPosOffset = Event.current.mousePosition - rect.position;
				Event.current.Use();
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600E5DC RID: 58844 RVA: 0x00556B94 File Offset: 0x00554D94
		private void CheckDraggedItemStillValid()
		{
			if (this.draggedItem == null)
			{
				return;
			}
			if (this.draggedItem.Destroyed)
			{
				this.draggedItem = null;
				return;
			}
			if (this.CurrentWearerOf(this.draggedItem) != null)
			{
				return;
			}
			if (CaravanInventoryUtility.AllInventoryItems(base.SelCaravan).Contains(this.draggedItem))
			{
				return;
			}
			this.draggedItem = null;
		}

		// Token: 0x0600E5DD RID: 58845 RVA: 0x00556BEE File Offset: 0x00554DEE
		private void CheckDropDraggedItem()
		{
			if (this.draggedItem == null)
			{
				return;
			}
			if (Event.current.type == EventType.mouseUp || Event.current.rawType == EventType.mouseUp)
			{
				this.droppedDraggedItem = true;
			}
		}

		// Token: 0x0600E5DE RID: 58846 RVA: 0x00556C1A File Offset: 0x00554E1A
		private bool IsVisibleWeapon(ThingDef t)
		{
			return t.IsWeapon && t != ThingDefOf.WoodLog && t != ThingDefOf.Beer;
		}

		// Token: 0x0600E5DF RID: 58847 RVA: 0x00556C3C File Offset: 0x00554E3C
		private Pawn CurrentWearerOf(Thing t)
		{
			IThingHolder parentHolder = t.ParentHolder;
			if (parentHolder is Pawn_EquipmentTracker || parentHolder is Pawn_ApparelTracker)
			{
				return (Pawn)parentHolder.ParentHolder;
			}
			return null;
		}

		// Token: 0x0600E5E0 RID: 58848 RVA: 0x00556C70 File Offset: 0x00554E70
		private void MoveDraggedItemToInventory()
		{
			this.droppedDraggedItem = false;
			Apparel apparel = this.draggedItem as Apparel;
			if (apparel != null && this.CurrentWearerOf(apparel) != null && this.CurrentWearerOf(apparel).apparel.IsLocked(apparel))
			{
				Messages.Message("MessageCantUnequipLockedApparel".Translate(), this.CurrentWearerOf(apparel), MessageTypeDefOf.RejectInput, false);
				this.draggedItem = null;
				return;
			}
			Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(this.draggedItem, this.Pawns, null, null);
			if (pawn != null)
			{
				this.draggedItem.holdingOwner.TryTransferToContainer(this.draggedItem, pawn.inventory.innerContainer, 1, true);
			}
			else
			{
				string str = "Could not find any pawn to move ";
				Thing thing = this.draggedItem;
				Log.Warning(str + ((thing != null) ? thing.ToString() : null) + " to.");
			}
			this.draggedItem = null;
		}

		// Token: 0x0600E5E1 RID: 58849 RVA: 0x00556D48 File Offset: 0x00554F48
		private void TryEquipDraggedItem(Pawn p)
		{
			WITab_Caravan_Gear.<>c__DisplayClass41_0 CS$<>8__locals1 = new WITab_Caravan_Gear.<>c__DisplayClass41_0();
			CS$<>8__locals1.p = p;
			CS$<>8__locals1.<>4__this = this;
			this.droppedDraggedItem = false;
			string str;
			if (!EquipmentUtility.CanEquip(this.draggedItem, CS$<>8__locals1.p, out str, true))
			{
				Messages.Message("MessageCantEquipCustom".Translate(str.CapitalizeFirst()), CS$<>8__locals1.p, MessageTypeDefOf.RejectInput, false);
				this.draggedItem = null;
				return;
			}
			if (this.draggedItem.def.IsWeapon)
			{
				if (CS$<>8__locals1.p.guest.IsPrisoner)
				{
					Messages.Message("MessageCantEquipCustom".Translate("MessagePrisonerCannotEquipWeapon".Translate(CS$<>8__locals1.p.Named("PAWN"))), CS$<>8__locals1.p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
				if (CS$<>8__locals1.p.WorkTagIsDisabled(WorkTags.Violent))
				{
					Messages.Message("MessageCantEquipIncapableOfViolence".Translate(CS$<>8__locals1.p.LabelShort, CS$<>8__locals1.p), CS$<>8__locals1.p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
				if (CS$<>8__locals1.p.WorkTagIsDisabled(WorkTags.Shooting) && this.draggedItem.def.IsRangedWeapon)
				{
					Messages.Message("MessageCantEquipIncapableOfShooting".Translate(CS$<>8__locals1.p.LabelShort, CS$<>8__locals1.p), CS$<>8__locals1.p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
				if (!CS$<>8__locals1.p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
				{
					Messages.Message("MessageCantEquipIncapableOfManipulation".Translate(), CS$<>8__locals1.p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
			}
			Apparel apparel = this.draggedItem as Apparel;
			CS$<>8__locals1.thingWithComps = (this.draggedItem as ThingWithComps);
			if (apparel != null && CS$<>8__locals1.p.apparel != null)
			{
				if (!ApparelUtility.HasPartsToWear(CS$<>8__locals1.p, apparel.def))
				{
					Messages.Message("MessageCantWearApparelMissingBodyParts".Translate(CS$<>8__locals1.p.LabelShort, CS$<>8__locals1.p), CS$<>8__locals1.p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
				if (this.CurrentWearerOf(apparel) != null && this.CurrentWearerOf(apparel).apparel.IsLocked(apparel))
				{
					Messages.Message("MessageCantUnequipLockedApparel".Translate(), CS$<>8__locals1.p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
				if (CS$<>8__locals1.p.apparel.WouldReplaceLockedApparel(apparel))
				{
					Messages.Message("MessageWouldReplaceLockedApparel".Translate(CS$<>8__locals1.p.LabelShort, CS$<>8__locals1.p), CS$<>8__locals1.p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
				WITab_Caravan_Gear.tmpExistingApparel.Clear();
				WITab_Caravan_Gear.tmpExistingApparel.AddRange(CS$<>8__locals1.p.apparel.WornApparel);
				for (int i = 0; i < WITab_Caravan_Gear.tmpExistingApparel.Count; i++)
				{
					if (!ApparelUtility.CanWearTogether(apparel.def, WITab_Caravan_Gear.tmpExistingApparel[i].def, CS$<>8__locals1.p.RaceProps.body))
					{
						CS$<>8__locals1.p.apparel.Remove(WITab_Caravan_Gear.tmpExistingApparel[i]);
						Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(WITab_Caravan_Gear.tmpExistingApparel[i], this.Pawns, null, null);
						if (pawn != null)
						{
							pawn.inventory.innerContainer.TryAdd(WITab_Caravan_Gear.tmpExistingApparel[i], true);
						}
						else
						{
							string str2 = "Could not find any pawn to move ";
							Apparel apparel2 = WITab_Caravan_Gear.tmpExistingApparel[i];
							Log.Warning(str2 + ((apparel2 != null) ? apparel2.ToString() : null) + " to.");
							WITab_Caravan_Gear.tmpExistingApparel[i].Destroy(DestroyMode.Vanish);
						}
					}
				}
				CS$<>8__locals1.p.apparel.Wear((Apparel)apparel.SplitOff(1), false, false);
				if (CS$<>8__locals1.p.outfits != null)
				{
					CS$<>8__locals1.p.outfits.forcedHandler.SetForced(apparel, true);
				}
			}
			else if (CS$<>8__locals1.thingWithComps != null && CS$<>8__locals1.p.equipment != null)
			{
				string personaWeaponConfirmationText = EquipmentUtility.GetPersonaWeaponConfirmationText(this.draggedItem, CS$<>8__locals1.p);
				if (!personaWeaponConfirmationText.NullOrEmpty())
				{
					Thing thing = this.draggedItem;
					Find.WindowStack.Add(new Dialog_MessageBox(personaWeaponConfirmationText, "Yes".Translate(), delegate()
					{
						base.<TryEquipDraggedItem>g__AddEquipment|0();
					}, "No".Translate(), null, null, false, null, null, WindowLayer.Dialog));
					this.draggedItem = null;
					return;
				}
				CS$<>8__locals1.<TryEquipDraggedItem>g__AddEquipment|0();
			}
			else
			{
				string str3 = "Could not make ";
				Pawn p2 = CS$<>8__locals1.p;
				string str4 = (p2 != null) ? p2.ToString() : null;
				string str5 = " equip or wear ";
				Thing thing2 = this.draggedItem;
				Log.Warning(str3 + str4 + str5 + ((thing2 != null) ? thing2.ToString() : null));
			}
			this.draggedItem = null;
		}

		// Token: 0x0400920C RID: 37388
		private Vector2 leftPaneScrollPosition;

		// Token: 0x0400920D RID: 37389
		private float leftPaneScrollViewHeight;

		// Token: 0x0400920E RID: 37390
		private Vector2 rightPaneScrollPosition;

		// Token: 0x0400920F RID: 37391
		private float rightPaneScrollViewHeight;

		// Token: 0x04009210 RID: 37392
		private Thing draggedItem;

		// Token: 0x04009211 RID: 37393
		private Vector2 draggedItemPosOffset;

		// Token: 0x04009212 RID: 37394
		private bool droppedDraggedItem;

		// Token: 0x04009213 RID: 37395
		private float leftPaneWidth;

		// Token: 0x04009214 RID: 37396
		private float rightPaneWidth;

		// Token: 0x04009215 RID: 37397
		private const float PawnRowHeight = 40f;

		// Token: 0x04009216 RID: 37398
		private const float ItemRowHeight = 30f;

		// Token: 0x04009217 RID: 37399
		private const float PawnLabelHeight = 18f;

		// Token: 0x04009218 RID: 37400
		private const float PawnLabelColumnWidth = 100f;

		// Token: 0x04009219 RID: 37401
		private const float GearLabelColumnWidth = 250f;

		// Token: 0x0400921A RID: 37402
		private const float SpaceAroundIcon = 4f;

		// Token: 0x0400921B RID: 37403
		private const float EquippedGearColumnWidth = 250f;

		// Token: 0x0400921C RID: 37404
		private const float EquippedGearIconSize = 32f;

		// Token: 0x0400921D RID: 37405
		private static List<Apparel> tmpApparel = new List<Apparel>();

		// Token: 0x0400921E RID: 37406
		private static List<ThingWithComps> tmpExistingEquipment = new List<ThingWithComps>();

		// Token: 0x0400921F RID: 37407
		private static List<Apparel> tmpExistingApparel = new List<Apparel>();
	}
}
