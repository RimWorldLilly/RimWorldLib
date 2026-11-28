using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020020A5 RID: 8357
	public static class GeneUIUtility
	{
		// Token: 0x0600D296 RID: 53910 RVA: 0x004D5EAC File Offset: 0x004D40AC
		public static void DrawGenesInfo(Rect rect, Thing target, float initialHeight, ref Vector2 size, ref Vector2 scrollPosition, GeneSet pregnancyGenes = null)
		{
			Rect rect2 = rect;
			Rect position = rect2.ContractedBy(10f);
			if (Prefs.DevMode)
			{
				GeneUIUtility.DoDebugButton(new Rect(rect2.xMax - 18f - 125f, 5f, 115f, Text.LineHeight), target, pregnancyGenes);
			}
			GUI.BeginGroup(position);
			float num = BiostatsTable.HeightForBiostats(GeneUIUtility.arc);
			Rect rect3 = new Rect(0f, 0f, position.width, position.height - num - 12f);
			GeneUIUtility.DrawGeneSections(rect3, target, pregnancyGenes, ref scrollPosition);
			Rect rect4 = new Rect(0f, rect3.yMax + 6f, position.width - 140f - 4f, num);
			rect4.yMax = rect3.yMax + num + 6f;
			if (!(target is Pawn))
			{
				rect4.width = position.width;
			}
			BiostatsTable.Draw(rect4, GeneUIUtility.gcx, GeneUIUtility.met, GeneUIUtility.arc, false, false, -1);
			GeneUIUtility.TryDrawXenotype(target, rect4.xMax + 4f, rect4.y + Text.LineHeight / 2f);
			if (Event.current.type == EventType.layout)
			{
				float num2 = GeneUIUtility.endogenesHeight + GeneUIUtility.xenogenesHeight + num + 12f + 70f;
				if (num2 > initialHeight)
				{
					size.y = Mathf.Min(num2, (float)(UI.screenHeight - 35) - 165f - 30f);
				}
				else
				{
					size.y = initialHeight;
				}
				GeneUIUtility.xenogenesHeight = 0f;
				GeneUIUtility.endogenesHeight = 0f;
			}
			GUI.EndGroup();
		}

		// Token: 0x0600D297 RID: 53911 RVA: 0x004D604C File Offset: 0x004D424C
		private static void DrawGeneSections(Rect rect, Thing target, GeneSet genesOverride, ref Vector2 scrollPosition)
		{
			GeneUIUtility.RecacheGenes(target, genesOverride);
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, GeneUIUtility.scrollHeight);
			float num = 0f;
			Widgets.BeginScrollView(rect.AtZero(), ref scrollPosition, rect2, true);
			Rect containingRect = rect2;
			containingRect.y = scrollPosition.y;
			containingRect.height = rect.height;
			if (target is Pawn)
			{
				if (GeneUIUtility.endogenes.Any<Gene>())
				{
					GeneUIUtility.DrawSection(rect, false, GeneUIUtility.endogenes.Count, ref num, ref GeneUIUtility.endogenesHeight, delegate(int i, Rect r)
					{
						GeneUIUtility.DrawGene(GeneUIUtility.endogenes[i], r, GeneType.Endogene, true, true);
					}, containingRect);
					num += 12f;
				}
				GeneUIUtility.DrawSection(rect, true, GeneUIUtility.xenogenes.Count, ref num, ref GeneUIUtility.xenogenesHeight, delegate(int i, Rect r)
				{
					GeneUIUtility.DrawGene(GeneUIUtility.xenogenes[i], r, GeneType.Xenogene, true, true);
				}, containingRect);
			}
			else
			{
				GeneType geneType = (genesOverride != null || target is HumanEmbryo) ? GeneType.Endogene : GeneType.Xenogene;
				GeneUIUtility.DrawSection(rect, geneType == GeneType.Xenogene, GeneUIUtility.geneDefs.Count, ref num, ref GeneUIUtility.xenogenesHeight, delegate(int i, Rect r)
				{
					GeneUIUtility.DrawGeneDef(GeneUIUtility.geneDefs[i], r, geneType, null, true, true, false);
				}, containingRect);
			}
			if (Event.current.type == EventType.layout)
			{
				GeneUIUtility.scrollHeight = num;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
		}

		// Token: 0x0600D298 RID: 53912 RVA: 0x004D61B0 File Offset: 0x004D43B0
		private static void RecacheGenes(Thing target, GeneSet genesOverride)
		{
			GeneUIUtility.geneDefs.Clear();
			GeneUIUtility.xenogenes.Clear();
			GeneUIUtility.endogenes.Clear();
			GeneUIUtility.gcx = 0;
			GeneUIUtility.met = 0;
			GeneUIUtility.arc = 0;
			Pawn pawn = target as Pawn;
			GeneSetHolderBase geneSetHolderBase = target as GeneSetHolderBase;
			GeneSet geneSet = ((geneSetHolderBase != null) ? geneSetHolderBase.GeneSet : null) ?? genesOverride;
			if (pawn != null)
			{
				foreach (Gene gene in pawn.genes.Xenogenes)
				{
					if (!gene.Overridden)
					{
						GeneUIUtility.<RecacheGenes>g__AddBiostats|22_0(gene.def);
					}
					GeneUIUtility.xenogenes.Add(gene);
				}
				foreach (Gene gene2 in pawn.genes.Endogenes)
				{
					if (gene2.def.endogeneCategory == EndogeneCategory.Melanin)
					{
						if (pawn.genes.Endogenes.Any((Gene x) => x.def.skinColorOverride != null))
						{
							continue;
						}
					}
					if (!gene2.Overridden)
					{
						GeneUIUtility.<RecacheGenes>g__AddBiostats|22_0(gene2.def);
					}
					GeneUIUtility.endogenes.Add(gene2);
				}
				GeneUIUtility.xenogenes.SortGenes();
				GeneUIUtility.endogenes.SortGenes();
				return;
			}
			if (geneSet != null)
			{
				foreach (GeneDef item in geneSet.GenesListForReading)
				{
					GeneUIUtility.geneDefs.Add(item);
				}
				GeneUIUtility.gcx = geneSet.ComplexityTotal;
				GeneUIUtility.met = geneSet.MetabolismTotal;
				GeneUIUtility.arc = geneSet.ArchitesTotal;
				GeneUIUtility.geneDefs.SortGeneDefs();
			}
		}

		// Token: 0x0600D299 RID: 53913 RVA: 0x004D63A4 File Offset: 0x004D45A4
		private static void DrawSection(Rect rect, bool xeno, int count, ref float curY, ref float sectionHeight, Action<int, Rect> drawer, Rect containingRect)
		{
			Widgets.Label(10f, ref curY, rect.width, (xeno ? "Xenogenes" : "Endogenes").Translate().CapitalizeFirst(), (xeno ? "XenogenesDesc" : "EndogenesDesc").Translate());
			float num = curY;
			Rect rect2 = new Rect(rect.x, curY, rect.width, sectionHeight);
			if (xeno && count == 0)
			{
				Text.Anchor = TextAnchor.UpperCenter;
				GUI.color = ColoredText.SubtleGrayColor;
				rect2.height = Text.LineHeight;
				Widgets.Label(rect2, "(" + "NoXenogermImplanted".Translate() + ")");
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				curY += 90f;
			}
			else
			{
				Widgets.DrawMenuSection(rect2);
				float num2 = (rect.width - 12f - 630f - 36f) / 2f;
				curY += num2;
				int num3 = 0;
				int num4 = 0;
				for (int i = 0; i < count; i++)
				{
					if (num4 >= 6)
					{
						num4 = 0;
						num3++;
					}
					else if (i > 0)
					{
						num4++;
					}
					Rect rect3 = new Rect(num2 + (float)num4 * 90f + (float)num4 * 6f, curY + (float)num3 * 90f + (float)num3 * 6f, 90f, 90f);
					if (containingRect.Overlaps(rect3))
					{
						drawer(i, rect3);
					}
				}
				curY += (float)(num3 + 1) * 90f + (float)num3 * 6f + num2;
			}
			if (Event.current.type == EventType.layout)
			{
				sectionHeight = curY - num;
			}
		}

		// Token: 0x0600D29A RID: 53914 RVA: 0x004D6560 File Offset: 0x004D4760
		private static void TryDrawXenotype(Thing target, float x, float y)
		{
			Pawn sourcePawn = target as Pawn;
			if (sourcePawn == null)
			{
				return;
			}
			Rect rect = new Rect(x, y, 140f, Text.LineHeight);
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(rect, sourcePawn.genes.XenotypeLabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect.center.x - 17f, rect.yMax + 4f, 34f, 34f);
			GUI.color = XenotypeDef.IconColor;
			GUI.DrawTexture(position, sourcePawn.genes.XenotypeIcon);
			GUI.color = Color.white;
			rect.yMax = position.yMax;
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, () => ("Xenotype".Translate() + ": " + sourcePawn.genes.XenotypeLabelCap).Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + sourcePawn.genes.XenotypeDescShort, 883938493);
			}
			if (Widgets.ButtonInvisible(rect, true) && !sourcePawn.genes.UniqueXenotype)
			{
				Find.WindowStack.Add(new Dialog_InfoCard(sourcePawn.genes.Xenotype, null));
			}
		}

		// Token: 0x0600D29B RID: 53915 RVA: 0x004D6688 File Offset: 0x004D4888
		private static void DoDebugButton(Rect buttonRect, Thing target, GeneSet genesOverride)
		{
			GeneUIUtility.<>c__DisplayClass25_0 CS$<>8__locals1 = new GeneUIUtility.<>c__DisplayClass25_0();
			CS$<>8__locals1.sourcePawn = (target as Pawn);
			GeneUIUtility.<>c__DisplayClass25_0 CS$<>8__locals2 = CS$<>8__locals1;
			GeneSetHolderBase geneSetHolderBase = target as GeneSetHolderBase;
			CS$<>8__locals2.geneSet = (((geneSetHolderBase != null) ? geneSetHolderBase.GeneSet : null) ?? genesOverride);
			if (Widgets.ButtonText(buttonRect, "Devtool...", true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				string label = (genesOverride != null || target is HumanEmbryo) ? "Add gene" : "Add xenogene";
				list.Add(new FloatMenuOption(label, delegate()
				{
					WindowStack windowStack = Find.WindowStack;
					Action<GeneDef> callback;
					if ((callback = CS$<>8__locals1.<>9__9) == null)
					{
						callback = (CS$<>8__locals1.<>9__9 = delegate(GeneDef geneDef)
						{
							base.<DoDebugButton>g__AddGene|0(geneDef, true);
						});
					}
					windowStack.Add(new Dialog_DebugOptionListLister(DebugToolsPawns.Options_AddGene(callback), null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				if (CS$<>8__locals1.sourcePawn != null)
				{
					list.Add(new FloatMenuOption("Add endogene", delegate()
					{
						WindowStack windowStack = Find.WindowStack;
						Action<GeneDef> callback;
						if ((callback = CS$<>8__locals1.<>9__10) == null)
						{
							callback = (CS$<>8__locals1.<>9__10 = delegate(GeneDef geneDef)
							{
								base.<DoDebugButton>g__AddGene|0(geneDef, false);
							});
						}
						windowStack.Add(new Dialog_DebugOptionListLister(DebugToolsPawns.Options_AddGene(callback), null));
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					if (GeneUIUtility.xenogenes.Any<Gene>() || GeneUIUtility.endogenes.Any<Gene>())
					{
						list.Add(new FloatMenuOption("Remove gene", delegate()
						{
							List<DebugMenuOption> list2 = new List<DebugMenuOption>();
							List<Gene> list3 = new List<Gene>();
							list3.AddRange(GeneUIUtility.endogenes);
							list3.AddRange(GeneUIUtility.xenogenes);
							foreach (Gene gene2 in list3)
							{
								Gene gene = gene2;
								list2.Add(new DebugMenuOption(gene.LabelCap, DebugMenuOptionMode.Action, delegate()
								{
									CS$<>8__locals1.sourcePawn.genes.RemoveGene(gene);
								}));
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					list.Add(new FloatMenuOption("Add all genes (xenogene)", delegate()
					{
						CS$<>8__locals1.sourcePawn.genes.Debug_AddAllGenes(true);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					list.Add(new FloatMenuOption("Add all genes (endogene)", delegate()
					{
						CS$<>8__locals1.sourcePawn.genes.Debug_AddAllGenes(false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					list.Add(new FloatMenuOption("Apply xenotype", delegate()
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (XenotypeDef xenotype2 in DefDatabase<XenotypeDef>.AllDefs)
						{
							XenotypeDef xenotype = xenotype2;
							list2.Add(new DebugMenuOption(xenotype.LabelCap, DebugMenuOptionMode.Action, delegate()
							{
								CS$<>8__locals1.sourcePawn.genes.SetXenotype(xenotype);
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					if (!CS$<>8__locals1.sourcePawn.genes.UniqueXenotype)
					{
						list.Add(new FloatMenuOption("Reset genes to base xenotype", delegate()
						{
							List<Gene> list2 = CS$<>8__locals1.sourcePawn.genes.Endogenes;
							for (int i = list2.Count - 1; i >= 0; i--)
							{
								Gene gene = list2[i];
								if (gene.def.endogeneCategory != EndogeneCategory.Melanin && gene.def.endogeneCategory != EndogeneCategory.HairColor)
								{
									CS$<>8__locals1.sourcePawn.genes.RemoveGene(gene);
								}
							}
							CS$<>8__locals1.sourcePawn.genes.SetXenotype(CS$<>8__locals1.sourcePawn.genes.Xenotype);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				else if (GeneUIUtility.geneDefs.Any<GeneDef>() && CS$<>8__locals1.geneSet != null)
				{
					list.Add(new FloatMenuOption("Remove gene", delegate()
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (GeneDef gene2 in GeneUIUtility.geneDefs)
						{
							GeneDef gene = gene2;
							list2.Add(new DebugMenuOption(gene.LabelCap, DebugMenuOptionMode.Action, delegate()
							{
								CS$<>8__locals1.geneSet.Debug_RemoveGene(gene);
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0600D29C RID: 53916 RVA: 0x004D6898 File Offset: 0x004D4A98
		public static void DrawGene(Gene gene, Rect geneRect, GeneType geneType, bool doBackground = true, bool clickable = true)
		{
			GeneUIUtility.DrawGeneBasics(gene.def, geneRect, geneType, doBackground, clickable, !gene.Active);
			if (Mouse.IsOver(geneRect))
			{
				string text = gene.LabelCap.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + gene.def.DescriptionFull;
				if (gene.Overridden)
				{
					text += "\n\n";
					if (gene.overriddenByGene.def == gene.def)
					{
						text += ("OverriddenByIdenticalGene".Translate() + ": " + gene.overriddenByGene.LabelCap).Colorize(ColorLibrary.RedReadable);
					}
					else
					{
						text += ("OverriddenByGene".Translate() + ": " + gene.overriddenByGene.LabelCap).Colorize(ColorLibrary.RedReadable);
					}
				}
				if (clickable)
				{
					text = text + "\n\n" + "ClickForMoreInfo".Translate().ToString().Colorize(ColoredText.SubtleGrayColor);
				}
				TooltipHandler.TipRegion(geneRect, text);
			}
		}

		// Token: 0x0600D29D RID: 53917 RVA: 0x004D69C4 File Offset: 0x004D4BC4
		public static void DrawGeneDef(GeneDef gene, Rect geneRect, GeneType geneType, Func<string> extraTooltip = null, bool doBackground = true, bool clickable = true, bool overridden = false)
		{
			GeneUIUtility.DrawGeneBasics(gene, geneRect, geneType, doBackground, clickable, overridden);
			if (Mouse.IsOver(geneRect))
			{
				TooltipHandler.TipRegion(geneRect, delegate()
				{
					string text = gene.LabelCap.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + gene.DescriptionFull;
					if (extraTooltip != null)
					{
						string text2 = extraTooltip();
						if (!text2.NullOrEmpty())
						{
							text = text + "\n\n" + text2.Colorize(ColorLibrary.RedReadable);
						}
					}
					if (clickable)
					{
						text = text + "\n\n" + "ClickForMoreInfo".Translate().ToString().Colorize(ColoredText.SubtleGrayColor);
					}
					return text;
				}, 316238373);
			}
		}

		// Token: 0x0600D29E RID: 53918 RVA: 0x004D6A24 File Offset: 0x004D4C24
		private static void DrawGeneBasics(GeneDef gene, Rect geneRect, GeneType geneType, bool doBackground, bool clickable, bool overridden)
		{
			GUI.BeginGroup(geneRect);
			Rect rect = geneRect.AtZero();
			if (doBackground)
			{
				Widgets.DrawHighlight(rect);
				GUI.color = new Color(1f, 1f, 1f, 0.05f);
				Widgets.DrawBox(rect, 1, null);
				GUI.color = Color.white;
			}
			float num = rect.width - Text.LineHeight;
			Rect rect2 = new Rect(geneRect.width / 2f - num / 2f, 0f, num, num);
			Color iconColor = gene.IconColor;
			if (overridden)
			{
				iconColor.a = 0.75f;
				GUI.color = ColoredText.SubtleGrayColor;
			}
			CachedTexture cachedTexture = GeneUIUtility.GeneBackground_Archite;
			if (gene.biostatArc == 0)
			{
				if (geneType != GeneType.Endogene)
				{
					if (geneType == GeneType.Xenogene)
					{
						cachedTexture = GeneUIUtility.GeneBackground_Xenogene;
					}
				}
				else
				{
					cachedTexture = GeneUIUtility.GeneBackground_Endogene;
				}
			}
			GUI.DrawTexture(rect2, cachedTexture.Texture);
			Widgets.DefIcon(rect2, gene, null, 0.9f, null, false, new Color?(iconColor), null, null, 1f);
			Text.Font = GameFont.Tiny;
			float num2 = Text.CalcHeight(gene.LabelCap, rect.width);
			Rect rect3 = new Rect(0f, rect.yMax - num2, rect.width, num2);
			GUI.DrawTexture(new Rect(rect3.x, rect3.yMax - num2, rect3.width, num2), TexUI.GrayTextBG);
			Text.Anchor = TextAnchor.LowerCenter;
			if (overridden)
			{
				GUI.color = ColoredText.SubtleGrayColor;
			}
			if (doBackground && num2 < (Text.LineHeight - 2f) * 2f)
			{
				rect3.y -= 3f;
			}
			Widgets.Label(rect3, gene.LabelCap);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			if (clickable)
			{
				if (Widgets.ButtonInvisible(rect, true))
				{
					Find.WindowStack.Add(new Dialog_InfoCard(gene, null));
				}
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
			}
			GUI.EndGroup();
		}

		// Token: 0x0600D29F RID: 53919 RVA: 0x004D6C1D File Offset: 0x004D4E1D
		private static void DrawStat(Rect iconRect, CachedTexture icon, string stat, float iconWidth)
		{
			GUI.DrawTexture(iconRect, icon.Texture);
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.LabelFit(new Rect(iconRect.xMax, iconRect.y, 38f - iconWidth, iconWidth), stat);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600D2A0 RID: 53920 RVA: 0x004D6C5C File Offset: 0x004D4E5C
		public static void DrawBiostats(int gcx, int met, int arc, ref float curX, float curY, float margin = 6f)
		{
			float num = GeneCreationDialogBase.GeneSize.y / 3f;
			float num2 = 0f;
			float num3 = Text.LineHeightOf(GameFont.Small);
			Rect iconRect = new Rect(curX, curY + margin + num2, num3, num3);
			GeneUIUtility.DrawStat(iconRect, GeneUtility.GCXTex, gcx.ToString(), num3);
			Rect rect = new Rect(curX, iconRect.y, 38f, num3);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, "Complexity".Translate().CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + "ComplexityDesc".Translate());
			}
			num2 += num;
			if (met != 0)
			{
				Rect iconRect2 = new Rect(curX, curY + margin + num2, num3, num3);
				GeneUIUtility.DrawStat(iconRect2, GeneUtility.METTex, met.ToStringWithSign(), num3);
				Rect rect2 = new Rect(curX, iconRect2.y, 38f, num3);
				if (Mouse.IsOver(rect2))
				{
					Widgets.DrawHighlight(rect2);
					TooltipHandler.TipRegion(rect2, "Metabolism".Translate().CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + "MetabolismDesc".Translate());
				}
				num2 += num;
			}
			if (arc > 0)
			{
				Rect iconRect3 = new Rect(curX, curY + margin + num2, num3, num3);
				GeneUIUtility.DrawStat(iconRect3, GeneUtility.ARCTex, arc.ToString(), num3);
				Rect rect3 = new Rect(curX, iconRect3.y, 38f, num3);
				if (Mouse.IsOver(rect3))
				{
					Widgets.DrawHighlight(rect3);
					TooltipHandler.TipRegion(rect3, "ArchitesRequired".Translate().CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + "ArchitesRequiredDesc".Translate());
				}
			}
			curX += 34f;
		}

		// Token: 0x0600D2A2 RID: 53922 RVA: 0x004D6EDA File Offset: 0x004D50DA
		[CompilerGenerated]
		internal static void <RecacheGenes>g__AddBiostats|22_0(GeneDef gene)
		{
			GeneUIUtility.gcx += gene.biostatCpx;
			GeneUIUtility.met += gene.biostatMet;
			GeneUIUtility.arc += gene.biostatArc;
		}

		// Token: 0x0400857D RID: 34173
		private static List<GeneDef> geneDefs = new List<GeneDef>();

		// Token: 0x0400857E RID: 34174
		private static List<Gene> xenogenes = new List<Gene>();

		// Token: 0x0400857F RID: 34175
		private static List<Gene> endogenes = new List<Gene>();

		// Token: 0x04008580 RID: 34176
		private static float xenogenesHeight;

		// Token: 0x04008581 RID: 34177
		private static float endogenesHeight;

		// Token: 0x04008582 RID: 34178
		private static float scrollHeight;

		// Token: 0x04008583 RID: 34179
		private static int gcx;

		// Token: 0x04008584 RID: 34180
		private static int met;

		// Token: 0x04008585 RID: 34181
		private static int arc;

		// Token: 0x04008586 RID: 34182
		private static readonly Color CapsuleBoxColor = new Color(0.25f, 0.25f, 0.25f);

		// Token: 0x04008587 RID: 34183
		private static readonly Color CapsuleBoxColorOverridden = new Color(0.15f, 0.15f, 0.15f);

		// Token: 0x04008588 RID: 34184
		private static readonly CachedTexture GeneBackground_Archite = new CachedTexture("UI/Icons/Genes/GeneBackground_ArchiteGene");

		// Token: 0x04008589 RID: 34185
		private static readonly CachedTexture GeneBackground_Xenogene = new CachedTexture("UI/Icons/Genes/GeneBackground_Xenogene");

		// Token: 0x0400858A RID: 34186
		private static readonly CachedTexture GeneBackground_Endogene = new CachedTexture("UI/Icons/Genes/GeneBackground_Endogene");

		// Token: 0x0400858B RID: 34187
		private const float OverriddenGeneIconAlpha = 0.75f;

		// Token: 0x0400858C RID: 34188
		private const float XenogermIconSize = 34f;

		// Token: 0x0400858D RID: 34189
		private const float XenotypeLabelWidth = 140f;

		// Token: 0x0400858E RID: 34190
		private const float GeneGap = 6f;

		// Token: 0x0400858F RID: 34191
		private const float GeneSize = 90f;

		// Token: 0x04008590 RID: 34192
		public const float BiostatsWidth = 38f;
	}
}
