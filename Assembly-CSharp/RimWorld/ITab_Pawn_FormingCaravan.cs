using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001D71 RID: 7537
	public class ITab_Pawn_FormingCaravan : ITab
	{
		// Token: 0x17002435 RID: 9269
		// (get) Token: 0x0600C350 RID: 50000 RVA: 0x0047450E File Offset: 0x0047270E
		public override bool IsVisible
		{
			get
			{
				return this.SelPawn.IsFormingCaravan();
			}
		}

		// Token: 0x0600C351 RID: 50001 RVA: 0x0047451B File Offset: 0x0047271B
		public ITab_Pawn_FormingCaravan()
		{
			this.size = new Vector2(480f, 450f);
			this.labelKey = "TabFormingCaravan";
		}

		// Token: 0x0600C352 RID: 50002 RVA: 0x00474550 File Offset: 0x00472750
		protected override void FillTab()
		{
			this.thingsToSelect.Clear();
			Rect outRect = new Rect(default(Vector2), this.size).ContractedBy(10f);
			outRect.yMin += 20f;
			Rect rect = new Rect(0f, 0f, outRect.width - 16f, Mathf.Max(this.lastDrawnHeight, outRect.height));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect, true);
			float num = 0f;
			string status = ((LordJob_FormAndSendCaravan)this.SelPawn.GetLord().LordJob).Status;
			Widgets.Label(new Rect(0f, num, rect.width, 100f), status);
			num += 22f;
			num += 4f;
			this.DoPeopleAndAnimals(rect, ref num);
			num += 4f;
			this.DoItemsLists(rect, ref num);
			this.lastDrawnHeight = num;
			Widgets.EndScrollView();
			if (this.thingsToSelect.Any<Thing>())
			{
				ITab_Pawn_FormingCaravan.SelectNow(this.thingsToSelect);
				this.thingsToSelect.Clear();
			}
		}

		// Token: 0x0600C353 RID: 50003 RVA: 0x00474674 File Offset: 0x00472874
		public override void TabUpdate()
		{
			base.TabUpdate();
			if (this.SelPawn != null && this.SelPawn.GetLord() != null)
			{
				Lord lord = this.SelPawn.GetLord();
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					TargetHighlighter.Highlight(lord.ownedPawns[i], false, false, true);
				}
			}
		}

		// Token: 0x0600C354 RID: 50004 RVA: 0x004746D8 File Offset: 0x004728D8
		private void DoPeopleAndAnimals(Rect inRect, ref float curY)
		{
			Widgets.ListSeparator(ref curY, inRect.width, "CaravanMembers".Translate());
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			Lord lord = this.SelPawn.GetLord();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				Pawn pawn = lord.ownedPawns[i];
				if (pawn.IsFreeColonist)
				{
					num++;
					if (pawn.InMentalState)
					{
						num2++;
					}
				}
				else if (pawn.IsPrisoner)
				{
					num3++;
					if (pawn.InMentalState)
					{
						num4++;
					}
				}
				else if (pawn.RaceProps.Animal)
				{
					num5++;
					if (pawn.InMentalState)
					{
						num6++;
					}
					if (pawn.RaceProps.packAnimal)
					{
						num7++;
					}
				}
			}
			string pawnsCountLabel = this.GetPawnsCountLabel(num, num2, -1);
			string pawnsCountLabel2 = this.GetPawnsCountLabel(num3, num4, -1);
			string pawnsCountLabel3 = this.GetPawnsCountLabel(num5, num6, num7);
			float y = curY;
			float num8;
			this.DoPeopleAndAnimalsEntry(inRect, Faction.OfPlayer.def.pawnsPlural.CapitalizeFirst(), pawnsCountLabel, ref curY, out num8);
			float y2 = curY;
			float num9;
			this.DoPeopleAndAnimalsEntry(inRect, "CaravanPrisoners".Translate(), pawnsCountLabel2, ref curY, out num9);
			float y3 = curY;
			float num10;
			this.DoPeopleAndAnimalsEntry(inRect, "CaravanAnimals".Translate(), pawnsCountLabel3, ref curY, out num10);
			float width = Mathf.Max(new float[]
			{
				num8,
				num9,
				num10
			}) + 2f;
			Rect rect = new Rect(0f, y, width, 22f);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				this.HighlightColonists();
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.SelectColonistsLater();
			}
			Rect rect2 = new Rect(0f, y2, width, 22f);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
				this.HighlightPrisoners();
			}
			if (Widgets.ButtonInvisible(rect2, true))
			{
				this.SelectPrisonersLater();
			}
			Rect rect3 = new Rect(0f, y3, width, 22f);
			if (Mouse.IsOver(rect3))
			{
				Widgets.DrawHighlight(rect3);
				this.HighlightAnimals();
			}
			if (Widgets.ButtonInvisible(rect3, true))
			{
				this.SelectAnimalsLater();
			}
		}

		// Token: 0x0600C355 RID: 50005 RVA: 0x00474918 File Offset: 0x00472B18
		private void DoPeopleAndAnimalsEntry(Rect inRect, string leftLabel, string rightLabel, ref float curY, out float drawnWidth)
		{
			Rect rect = new Rect(0f, curY, inRect.width, 100f);
			Widgets.Label(rect, leftLabel);
			rect.xMin += 120f;
			Widgets.Label(rect, rightLabel);
			curY += 22f;
			drawnWidth = 120f + Text.CalcSize(rightLabel).x;
		}

		// Token: 0x0600C356 RID: 50006 RVA: 0x00474984 File Offset: 0x00472B84
		private void DoItemsLists(Rect inRect, ref float curY)
		{
			LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = (LordJob_FormAndSendCaravan)this.SelPawn.GetLord().LordJob;
			Rect rect = new Rect(0f, curY, (inRect.width - 10f) / 2f, inRect.height);
			float a = 0f;
			Widgets.BeginGroup(rect);
			Widgets.ListSeparator(ref a, rect.width, "ItemsToLoad".Translate());
			bool flag = false;
			for (int i = 0; i < lordJob_FormAndSendCaravan.transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = lordJob_FormAndSendCaravan.transferables[i];
				if (transferableOneWay.CountToTransfer > 0 && transferableOneWay.HasAnyThing)
				{
					flag = true;
					this.DoThingRow(transferableOneWay.AnyThing.GetInnerIfMinified().def, transferableOneWay.CountToTransfer, transferableOneWay.things, rect.width, ref a);
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref a, rect.width, null);
			}
			Widgets.EndGroup();
			Rect rect2 = new Rect((inRect.width + 10f) / 2f, curY, (inRect.width - 10f) / 2f, inRect.height);
			float b = 0f;
			Widgets.BeginGroup(rect2);
			Widgets.ListSeparator(ref b, rect2.width, "LoadedItems".Translate());
			bool flag2 = false;
			for (int j = 0; j < lordJob_FormAndSendCaravan.lord.ownedPawns.Count; j++)
			{
				Pawn pawn = lordJob_FormAndSendCaravan.lord.ownedPawns[j];
				if (!pawn.inventory.UnloadEverything)
				{
					for (int k = 0; k < pawn.inventory.innerContainer.Count; k++)
					{
						Thing thing = pawn.inventory.innerContainer[k];
						flag2 = true;
						ITab_Pawn_FormingCaravan.tmpSingleThing.Clear();
						ITab_Pawn_FormingCaravan.tmpSingleThing.Add(thing);
						this.DoThingRow(thing.def, thing.stackCount, ITab_Pawn_FormingCaravan.tmpSingleThing, rect2.width, ref b);
						ITab_Pawn_FormingCaravan.tmpSingleThing.Clear();
					}
				}
			}
			if (!flag2)
			{
				Widgets.NoneLabel(ref b, rect.width, null);
			}
			Widgets.EndGroup();
			curY += Mathf.Max(a, b);
		}

		// Token: 0x0600C357 RID: 50007 RVA: 0x00474BCC File Offset: 0x00472DCC
		private void SelectColonistsLater()
		{
			Lord lord = this.SelPawn.GetLord();
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].IsFreeColonist)
				{
					ITab_Pawn_FormingCaravan.tmpPawns.Add(lord.ownedPawns[i]);
				}
			}
			this.SelectLater(ITab_Pawn_FormingCaravan.tmpPawns);
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
		}

		// Token: 0x0600C358 RID: 50008 RVA: 0x00474C44 File Offset: 0x00472E44
		private void HighlightColonists()
		{
			Lord lord = this.SelPawn.GetLord();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].IsFreeColonist)
				{
					TargetHighlighter.Highlight(lord.ownedPawns[i], true, true, false);
				}
			}
		}

		// Token: 0x0600C359 RID: 50009 RVA: 0x00474CA0 File Offset: 0x00472EA0
		private void SelectPrisonersLater()
		{
			Lord lord = this.SelPawn.GetLord();
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].IsPrisoner)
				{
					ITab_Pawn_FormingCaravan.tmpPawns.Add(lord.ownedPawns[i]);
				}
			}
			this.SelectLater(ITab_Pawn_FormingCaravan.tmpPawns);
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
		}

		// Token: 0x0600C35A RID: 50010 RVA: 0x00474D18 File Offset: 0x00472F18
		private void HighlightPrisoners()
		{
			Lord lord = this.SelPawn.GetLord();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].IsPrisoner)
				{
					TargetHighlighter.Highlight(lord.ownedPawns[i], true, true, false);
				}
			}
		}

		// Token: 0x0600C35B RID: 50011 RVA: 0x00474D74 File Offset: 0x00472F74
		private void SelectAnimalsLater()
		{
			Lord lord = this.SelPawn.GetLord();
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].RaceProps.Animal)
				{
					ITab_Pawn_FormingCaravan.tmpPawns.Add(lord.ownedPawns[i]);
				}
			}
			this.SelectLater(ITab_Pawn_FormingCaravan.tmpPawns);
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
		}

		// Token: 0x0600C35C RID: 50012 RVA: 0x00474DF0 File Offset: 0x00472FF0
		private void HighlightAnimals()
		{
			Lord lord = this.SelPawn.GetLord();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].RaceProps.Animal)
				{
					TargetHighlighter.Highlight(lord.ownedPawns[i], true, true, false);
				}
			}
		}

		// Token: 0x0600C35D RID: 50013 RVA: 0x00474E50 File Offset: 0x00473050
		private void SelectLater(List<Thing> things)
		{
			this.thingsToSelect.Clear();
			this.thingsToSelect.AddRange(things);
		}

		// Token: 0x0600C35E RID: 50014 RVA: 0x00474E6C File Offset: 0x0047306C
		public static void SelectNow(List<Thing> things)
		{
			if (!things.Any<Thing>())
			{
				return;
			}
			Find.Selector.ClearSelection();
			bool flag = false;
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].SpawnedOrAnyParentSpawned)
				{
					if (!flag)
					{
						CameraJumper.TryJump(things[i], CameraJumper.MovementMode.Pan);
					}
					Find.Selector.Select(things[i], true, true);
					flag = true;
				}
			}
			if (!flag)
			{
				CameraJumper.TryJumpAndSelect(things[0], CameraJumper.MovementMode.Pan);
			}
		}

		// Token: 0x0600C35F RID: 50015 RVA: 0x00474EEC File Offset: 0x004730EC
		private string GetPawnsCountLabel(int count, int countInMentalState, int countPackAnimals)
		{
			string text = count.ToString();
			bool flag = countInMentalState > 0;
			bool flag2 = count > 0 && countPackAnimals >= 0;
			if (flag || flag2)
			{
				text += " (";
				if (flag2)
				{
					text += countPackAnimals.ToString() + " " + "PackAnimalsCountLower".Translate();
				}
				if (flag)
				{
					if (flag2)
					{
						text += ", ";
					}
					text += countInMentalState.ToString() + " " + "InMentalStateCountLower".Translate();
				}
				text += ")";
			}
			return text;
		}

		// Token: 0x0600C360 RID: 50016 RVA: 0x00474FA4 File Offset: 0x004731A4
		private void EndJobForEveryoneHauling(TransferableOneWay t, LordJob_FormAndSendCaravan lordJob)
		{
			List<Pawn> freeColonistsSpawned = base.SelThing.Map.mapPawns.FreeColonistsSpawned;
			for (int i = 0; i < freeColonistsSpawned.Count; i++)
			{
				if (freeColonistsSpawned[i].CurJobDef == JobDefOf.PrepareCaravan_GatherItems)
				{
					JobDriver_PrepareCaravan_GatherItems jobDriver_PrepareCaravan_GatherItems = freeColonistsSpawned[i].jobs.curDriver as JobDriver_PrepareCaravan_GatherItems;
					if (jobDriver_PrepareCaravan_GatherItems != null && jobDriver_PrepareCaravan_GatherItems.job.lord == lordJob.lord && jobDriver_PrepareCaravan_GatherItems.ToHaul != null && jobDriver_PrepareCaravan_GatherItems.ToHaul.def == t.ThingDef)
					{
						freeColonistsSpawned[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
		}

		// Token: 0x0600C361 RID: 50017 RVA: 0x00475048 File Offset: 0x00473248
		private void DropThingToLoad(TransferableOneWay t, int count)
		{
			t.ForceTo(t.CountToTransfer - count);
			this.EndJobForEveryoneHauling(t, (LordJob_FormAndSendCaravan)this.SelPawn.GetLord().LordJob);
		}

		// Token: 0x0600C362 RID: 50018 RVA: 0x00475074 File Offset: 0x00473274
		private void DoThingRow(ThingDef thingDef, int count, List<Thing> things, float width, ref float curY)
		{
			ITab_Pawn_FormingCaravan.<>c__DisplayClass33_0 CS$<>8__locals1 = new ITab_Pawn_FormingCaravan.<>c__DisplayClass33_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.thingDef = thingDef;
			CS$<>8__locals1.count = count;
			Rect rect = new Rect(0f, curY, width, 28f);
			CS$<>8__locals1.singleThingForClosure = ((things.Count == 1) ? things[0] : null);
			if (CS$<>8__locals1.count != 1 && Widgets.ButtonImage(new Rect(rect.x + rect.width - 24f, rect.y + (rect.height - 24f) / 2f, 24f, 24f), CaravanThingsTabUtility.AbandonSpecificCountButtonTex, true, null))
			{
				Find.WindowStack.Add(new Dialog_Slider("RemoveSliderText".Translate(CS$<>8__locals1.thingDef.label), 1, CS$<>8__locals1.count, new Action<int>(CS$<>8__locals1.<DoThingRow>g__DoDropThing|0), int.MinValue, 1f));
			}
			rect.width -= 24f;
			if (Widgets.ButtonImage(new Rect(rect.x + rect.width - 24f, rect.y + (rect.height - 24f) / 2f, 24f, 24f), CaravanThingsTabUtility.AbandonButtonTex, true, null))
			{
				string value = CS$<>8__locals1.thingDef.label;
				if (things.Count == 1 && things[0] is Pawn)
				{
					value = ((Pawn)things[0]).LabelShortCap;
				}
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmRemoveItemDialog".Translate(value), delegate()
				{
					base.<DoThingRow>g__DoDropThing|0(CS$<>8__locals1.count);
				}, false, null, WindowLayer.Dialog));
			}
			rect.width -= 24f;
			if (things.Count == 1)
			{
				Widgets.InfoCardButton(rect.width - 24f, curY, things[0]);
			}
			else
			{
				Widgets.InfoCardButton(rect.width - 24f, curY, CS$<>8__locals1.thingDef);
			}
			rect.width -= 24f;
			if (Mouse.IsOver(rect))
			{
				GUI.color = ITab_Pawn_FormingCaravan.ThingHighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (CS$<>8__locals1.thingDef.DrawMatSingle != null && CS$<>8__locals1.thingDef.DrawMatSingle.mainTexture != null)
			{
				Rect rect2 = new Rect(4f, curY, 28f, 28f);
				if (things.Count == 1)
				{
					Widgets.ThingIcon(rect2, things[0], 1f, null, false, 1f, false);
				}
				else
				{
					Widgets.ThingIcon(rect2, CS$<>8__locals1.thingDef, null, null, 1f, null, null, 1f);
				}
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = ITab_Pawn_FormingCaravan.ThingLabelColor;
			Rect rect3 = new Rect(36f, curY, rect.width - 36f, rect.height);
			string str;
			if (things.Count == 1 && CS$<>8__locals1.count == things[0].stackCount)
			{
				str = things[0].LabelCap;
			}
			else if (CS$<>8__locals1.thingDef == ThingDefOf.MinifiedThing)
			{
				str = GenLabel.ThingLabel(things[0].GetInnerIfMinified(), CS$<>8__locals1.count, true, true).CapitalizeFirst();
			}
			else
			{
				str = GenLabel.ThingLabel(CS$<>8__locals1.thingDef, null, CS$<>8__locals1.count).CapitalizeFirst();
			}
			Text.WordWrap = false;
			Widgets.Label(rect3, str.Truncate(rect3.width, null));
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
			TooltipHandler.TipRegion(rect, str);
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.SelectLater(things);
			}
			if (Mouse.IsOver(rect))
			{
				for (int i = 0; i < things.Count; i++)
				{
					TargetHighlighter.Highlight(things[i], true, true, false);
				}
			}
			curY += 28f;
		}

		// Token: 0x04006F99 RID: 28569
		private Vector2 scrollPosition;

		// Token: 0x04006F9A RID: 28570
		private float lastDrawnHeight;

		// Token: 0x04006F9B RID: 28571
		private List<Thing> thingsToSelect = new List<Thing>();

		// Token: 0x04006F9C RID: 28572
		private static List<Thing> tmpSingleThing = new List<Thing>();

		// Token: 0x04006F9D RID: 28573
		private const float TopPadding = 20f;

		// Token: 0x04006F9E RID: 28574
		private const float StandardLineHeight = 22f;

		// Token: 0x04006F9F RID: 28575
		private const float ExtraSpaceBetweenSections = 4f;

		// Token: 0x04006FA0 RID: 28576
		private const float SpaceBetweenItemsLists = 10f;

		// Token: 0x04006FA1 RID: 28577
		private const float ThingRowHeight = 28f;

		// Token: 0x04006FA2 RID: 28578
		private const float ThingIconSize = 28f;

		// Token: 0x04006FA3 RID: 28579
		private const float ThingLeftX = 36f;

		// Token: 0x04006FA4 RID: 28580
		private static readonly Color ThingLabelColor = ITab_Pawn_Gear.ThingLabelColor;

		// Token: 0x04006FA5 RID: 28581
		private static readonly Color ThingHighlightColor = ITab_Pawn_Gear.HighlightColor;

		// Token: 0x04006FA6 RID: 28582
		private static List<Thing> tmpPawns = new List<Thing>();
	}
}
