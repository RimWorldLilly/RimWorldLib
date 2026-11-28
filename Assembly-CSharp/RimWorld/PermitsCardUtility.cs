using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001CEC RID: 7404
	[StaticConstructorOnStartup]
	public static class PermitsCardUtility
	{
		// Token: 0x170022AA RID: 8874
		// (get) Token: 0x0600BE8A RID: 48778 RVA: 0x0044D4F4 File Offset: 0x0044B6F4
		private static bool ShowSwitchFactionButton
		{
			get
			{
				int num = 0;
				foreach (Faction faction in Find.FactionManager.AllFactionsVisible)
				{
					if (!faction.IsPlayer && !faction.def.permanentEnemy && !faction.temporary)
					{
						using (IEnumerator<RoyalTitlePermitDef> enumerator2 = DefDatabase<RoyalTitlePermitDef>.AllDefs.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (enumerator2.Current.faction == faction.def)
								{
									num++;
									break;
								}
							}
						}
					}
				}
				return num > 1;
			}
		}

		// Token: 0x0600BE8B RID: 48779 RVA: 0x0044D5A8 File Offset: 0x0044B7A8
		private static int TotalReturnPermitsCost(Pawn pawn)
		{
			int num = 8;
			List<FactionPermit> allFactionPermits = pawn.royalty.AllFactionPermits;
			for (int i = 0; i < allFactionPermits.Count; i++)
			{
				if (allFactionPermits[i].OnCooldown && allFactionPermits[i].Permit.royalAid != null)
				{
					num += allFactionPermits[i].Permit.royalAid.favorCost;
				}
			}
			return num;
		}

		// Token: 0x0600BE8C RID: 48780 RVA: 0x0044D610 File Offset: 0x0044B810
		public static void DrawRecordsCard(Rect rect, Pawn pawn)
		{
			if (!ModLister.CheckRoyalty("Permit"))
			{
				return;
			}
			rect.yMax -= 4f;
			if (PermitsCardUtility.ShowSwitchFactionButton)
			{
				Rect rect2 = new Rect(rect.x, rect.y, 32f, 32f);
				if (Widgets.ButtonImage(rect2, PermitsCardUtility.SwitchFactionIcon, true, null))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (Faction faction in Find.FactionManager.AllFactionsVisibleInViewOrder)
					{
						if (!faction.IsPlayer && !faction.def.permanentEnemy)
						{
							Faction localFaction = faction;
							list.Add(new FloatMenuOption(localFaction.Name, delegate()
							{
								PermitsCardUtility.selectedFaction = localFaction;
								PermitsCardUtility.selectedPermit = null;
							}, localFaction.def.FactionIcon, localFaction.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
						}
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				TooltipHandler.TipRegion(rect2, "SwitchFaction_Desc".Translate());
			}
			if (PermitsCardUtility.selectedFaction.def.HasRoyalTitles)
			{
				string label = "ReturnAllPermits".Translate();
				Rect rect3 = new Rect(rect.xMax - 180f, rect.y - 4f, 180f, 51f);
				int num = PermitsCardUtility.TotalReturnPermitsCost(pawn);
				if (Widgets.ButtonText(rect3, label, true, true, true, null))
				{
					if (!pawn.royalty.PermitsFromFaction(PermitsCardUtility.selectedFaction).Any<FactionPermit>())
					{
						Messages.Message("NoPermitsToReturn".Translate(pawn.Named("PAWN")), new LookTargets(pawn), MessageTypeDefOf.RejectInput, false);
					}
					else if (pawn.royalty.GetFavor(PermitsCardUtility.selectedFaction) < num)
					{
						Messages.Message("NotEnoughFavor".Translate(num.Named("FAVORCOST"), PermitsCardUtility.selectedFaction.def.royalFavorLabel.Named("FAVOR"), pawn.Named("PAWN"), pawn.royalty.GetFavor(PermitsCardUtility.selectedFaction).Named("CURFAVOR")), MessageTypeDefOf.RejectInput, true);
					}
					else
					{
						string str = "ReturnAllPermits_Confirm".Translate(8.Named("BASEFAVORCOST"), num.Named("FAVORCOST"), PermitsCardUtility.selectedFaction.def.royalFavorLabel.Named("FAVOR"), PermitsCardUtility.selectedFaction.Named("FACTION"));
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(str, delegate()
						{
							pawn.royalty.RefundPermits(8, PermitsCardUtility.selectedFaction);
						}, true, null, WindowLayer.Dialog));
					}
				}
				TooltipHandler.TipRegion(rect3, "ReturnAllPermits_Desc".Translate(8.Named("BASEFAVORCOST"), num.Named("FAVORCOST"), PermitsCardUtility.selectedFaction.def.royalFavorLabel.Named("FAVOR")));
			}
			RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(PermitsCardUtility.selectedFaction);
			Rect rect4 = new Rect(rect.xMax - 360f - 4f, rect.y - 4f, 360f, 55f);
			string text = "CurrentTitle".Translate() + ": " + ((currentTitle != null) ? currentTitle.GetLabelFor(pawn).CapitalizeFirst() : "None".Translate()) + "\n" + "UnusedPermits".Translate() + ": " + pawn.royalty.GetPermitPoints(PermitsCardUtility.selectedFaction).ToString();
			if (!PermitsCardUtility.selectedFaction.def.royalFavorLabel.NullOrEmpty())
			{
				text = string.Concat(new string[]
				{
					text,
					"\n",
					PermitsCardUtility.selectedFaction.def.royalFavorLabel.CapitalizeFirst(),
					": ",
					pawn.royalty.GetFavor(PermitsCardUtility.selectedFaction).ToString()
				});
			}
			Widgets.Label(rect4, text);
			rect.yMin += 55f;
			Rect rect5 = new Rect(rect);
			rect5.width *= 0.33f;
			PermitsCardUtility.DoLeftRect(rect5, pawn);
			PermitsCardUtility.DoRightRect(new Rect(rect)
			{
				xMin = rect5.xMax + 10f
			}, pawn);
		}

		// Token: 0x0600BE8D RID: 48781 RVA: 0x0044DB4C File Offset: 0x0044BD4C
		private static void DoLeftRect(Rect rect, Pawn pawn)
		{
			float num = 0f;
			RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(PermitsCardUtility.selectedFaction);
			Rect rect2 = new Rect(rect);
			Widgets.BeginGroup(rect2);
			if (PermitsCardUtility.selectedPermit != null)
			{
				Text.Font = GameFont.Medium;
				Rect rect3 = new Rect(0f, num, rect2.width, 0f);
				Widgets.LabelCacheHeight(ref rect3, PermitsCardUtility.selectedPermit.LabelCap, true, false);
				Text.Font = GameFont.Small;
				num += rect3.height;
				if (!PermitsCardUtility.selectedPermit.description.NullOrEmpty())
				{
					Rect rect4 = new Rect(0f, num, rect2.width, 0f);
					Widgets.LabelCacheHeight(ref rect4, PermitsCardUtility.selectedPermit.description, true, false);
					num += rect4.height + 16f;
				}
				Rect rect5 = new Rect(0f, num, rect2.width, 0f);
				string text = "Cooldown".Translate() + ": " + "PeriodDays".Translate(PermitsCardUtility.selectedPermit.cooldownDays);
				if (PermitsCardUtility.selectedPermit.royalAid != null && PermitsCardUtility.selectedPermit.royalAid.favorCost > 0 && !PermitsCardUtility.selectedFaction.def.royalFavorLabel.NullOrEmpty())
				{
					text = text + ("\n" + "CooldownUseFavorCost".Translate(PermitsCardUtility.selectedFaction.def.royalFavorLabel.Named("HONOR")).CapitalizeFirst() + ": ") + PermitsCardUtility.selectedPermit.royalAid.favorCost.ToString();
				}
				if (PermitsCardUtility.selectedPermit.minTitle != null)
				{
					text = text + "\n" + "RequiresTitle".Translate(PermitsCardUtility.selectedPermit.minTitle.GetLabelForBothGenders()).Resolve().Colorize((currentTitle != null && currentTitle.seniority >= PermitsCardUtility.selectedPermit.minTitle.seniority) ? Color.white : ColorLibrary.RedReadable);
				}
				if (PermitsCardUtility.selectedPermit.prerequisite != null)
				{
					text = text + "\n" + "UpgradeFrom".Translate(PermitsCardUtility.selectedPermit.prerequisite.LabelCap).Resolve().Colorize(PermitsCardUtility.PermitUnlocked(PermitsCardUtility.selectedPermit.prerequisite, pawn) ? Color.white : ColorLibrary.RedReadable);
				}
				Widgets.LabelCacheHeight(ref rect5, text, true, false);
				num += rect5.height + 4f;
				Rect rect6 = new Rect(0f, rect2.height - 50f, rect2.width, 50f);
				if (PermitsCardUtility.selectedPermit.AvailableForPawn(pawn, PermitsCardUtility.selectedFaction) && !PermitsCardUtility.PermitUnlocked(PermitsCardUtility.selectedPermit, pawn) && Widgets.ButtonText(rect6, "AcceptPermit".Translate(), true, true, true, null))
				{
					SoundDefOf.Quest_Accepted.PlayOneShotOnCamera(null);
					pawn.royalty.AddPermit(PermitsCardUtility.selectedPermit, PermitsCardUtility.selectedFaction);
				}
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600BE8E RID: 48782 RVA: 0x0044DE80 File Offset: 0x0044C080
		private static void DoRightRect(Rect rect, Pawn pawn)
		{
			Widgets.DrawMenuSection(rect);
			if (PermitsCardUtility.selectedFaction == null)
			{
				return;
			}
			List<RoyalTitlePermitDef> allDefsListForReading = DefDatabase<RoyalTitlePermitDef>.AllDefsListForReading;
			Rect outRect = rect.ContractedBy(10f);
			Rect rect2 = default(Rect);
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				RoyalTitlePermitDef permit = allDefsListForReading[i];
				if (PermitsCardUtility.CanDrawPermit(permit))
				{
					rect2.width = Mathf.Max(rect2.width, PermitsCardUtility.DrawPosition(permit).x + 200f + 26f);
					rect2.height = Mathf.Max(rect2.height, PermitsCardUtility.DrawPosition(permit).y + 50f + 26f);
				}
			}
			Widgets.BeginScrollView(outRect, ref PermitsCardUtility.rightScrollPosition, rect2, true);
			Widgets.BeginGroup(rect2.ContractedBy(10f));
			PermitsCardUtility.DrawLines();
			for (int j = 0; j < allDefsListForReading.Count; j++)
			{
				RoyalTitlePermitDef royalTitlePermitDef = allDefsListForReading[j];
				if (PermitsCardUtility.CanDrawPermit(royalTitlePermitDef))
				{
					Vector2 vector = PermitsCardUtility.DrawPosition(royalTitlePermitDef);
					Rect rect3 = new Rect(vector.x, vector.y, 200f, 50f);
					Color color = Widgets.NormalOptionColor;
					Color color2 = PermitsCardUtility.PermitUnlocked(royalTitlePermitDef, pawn) ? TexUI.OldFinishedResearchColor : TexUI.AvailResearchColor;
					Color borderColor;
					if (PermitsCardUtility.selectedPermit == royalTitlePermitDef)
					{
						borderColor = TexUI.HighlightBorderResearchColor;
						color2 += TexUI.HighlightBgResearchColor;
					}
					else
					{
						borderColor = TexUI.DefaultBorderResearchColor;
					}
					if (!royalTitlePermitDef.AvailableForPawn(pawn, PermitsCardUtility.selectedFaction) && !PermitsCardUtility.PermitUnlocked(royalTitlePermitDef, pawn))
					{
						color = Color.red;
					}
					if (Widgets.CustomButtonText(ref rect3, string.Empty, color2, color, borderColor, default(Color), false, 1f, true, true, 1f))
					{
						SoundDefOf.Click.PlayOneShotOnCamera(null);
						PermitsCardUtility.selectedPermit = royalTitlePermitDef;
					}
					TextAnchor anchor = Text.Anchor;
					Color color3 = GUI.color;
					GUI.color = color;
					Text.Anchor = TextAnchor.MiddleCenter;
					Widgets.Label(rect3, royalTitlePermitDef.LabelCap);
					GUI.color = color3;
					Text.Anchor = anchor;
				}
			}
			Widgets.EndGroup();
			Widgets.EndScrollView();
		}

		// Token: 0x0600BE8F RID: 48783 RVA: 0x0044E088 File Offset: 0x0044C288
		private static void DrawLines()
		{
			Vector2 start = default(Vector2);
			Vector2 end = default(Vector2);
			List<RoyalTitlePermitDef> allDefsListForReading = DefDatabase<RoyalTitlePermitDef>.AllDefsListForReading;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < allDefsListForReading.Count; j++)
				{
					RoyalTitlePermitDef royalTitlePermitDef = allDefsListForReading[j];
					if (PermitsCardUtility.CanDrawPermit(royalTitlePermitDef))
					{
						Vector2 vector = PermitsCardUtility.DrawPosition(royalTitlePermitDef);
						start.x = vector.x;
						start.y = vector.y + 25f;
						RoyalTitlePermitDef prerequisite = royalTitlePermitDef.prerequisite;
						if (prerequisite != null)
						{
							Vector2 vector2 = PermitsCardUtility.DrawPosition(prerequisite);
							end.x = vector2.x + 200f;
							end.y = vector2.y + 25f;
							if ((i == 1 && PermitsCardUtility.selectedPermit == royalTitlePermitDef) || PermitsCardUtility.selectedPermit == prerequisite)
							{
								Widgets.DrawLine(start, end, TexUI.HighlightLineResearchColor, 4f);
							}
							else if (i == 0)
							{
								Widgets.DrawLine(start, end, TexUI.DefaultLineResearchColor, 2f);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600BE90 RID: 48784 RVA: 0x0044E194 File Offset: 0x0044C394
		private static bool PermitUnlocked(RoyalTitlePermitDef permit, Pawn pawn)
		{
			if (pawn.royalty.HasPermit(permit, PermitsCardUtility.selectedFaction))
			{
				return true;
			}
			List<FactionPermit> allFactionPermits = pawn.royalty.AllFactionPermits;
			for (int i = 0; i < allFactionPermits.Count; i++)
			{
				if (allFactionPermits[i].Permit.prerequisite == permit && allFactionPermits[i].Faction == PermitsCardUtility.selectedFaction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600BE91 RID: 48785 RVA: 0x0044E1FD File Offset: 0x0044C3FD
		private static Vector2 DrawPosition(RoyalTitlePermitDef permit)
		{
			Vector2 a = new Vector2(permit.uiPosition.x * 200f, permit.uiPosition.y * 50f);
			return a + a * PermitsCardUtility.PermitOptionSpacing;
		}

		// Token: 0x0600BE92 RID: 48786 RVA: 0x0044E236 File Offset: 0x0044C436
		private static bool CanDrawPermit(RoyalTitlePermitDef permit)
		{
			return permit.permitPointCost > 0 && (permit.faction == null || permit.faction == PermitsCardUtility.selectedFaction.def);
		}

		// Token: 0x04006D58 RID: 27992
		private static Vector2 rightScrollPosition;

		// Token: 0x04006D59 RID: 27993
		public static RoyalTitlePermitDef selectedPermit;

		// Token: 0x04006D5A RID: 27994
		public static Faction selectedFaction;

		// Token: 0x04006D5B RID: 27995
		private const float LeftRectPercent = 0.33f;

		// Token: 0x04006D5C RID: 27996
		private const float TitleHeight = 55f;

		// Token: 0x04006D5D RID: 27997
		private const float ReturnButtonWidth = 180f;

		// Token: 0x04006D5E RID: 27998
		private const float PermitOptionWidth = 200f;

		// Token: 0x04006D5F RID: 27999
		private const float PermitOptionHeight = 50f;

		// Token: 0x04006D60 RID: 28000
		private const float AcceptButtonHeight = 50f;

		// Token: 0x04006D61 RID: 28001
		private const float SwitchFactionsButtonSize = 32f;

		// Token: 0x04006D62 RID: 28002
		private const float LineWidthNotSelected = 2f;

		// Token: 0x04006D63 RID: 28003
		private const float LineWidthSelected = 4f;

		// Token: 0x04006D64 RID: 28004
		private const int ReturnPermitsCost = 8;

		// Token: 0x04006D65 RID: 28005
		private static readonly Vector2 PermitOptionSpacing = new Vector2(0.25f, 0.35f);

		// Token: 0x04006D66 RID: 28006
		private static readonly Texture2D SwitchFactionIcon = ContentFinder<Texture2D>.Get("UI/Icons/SwitchFaction", true);
	}
}
