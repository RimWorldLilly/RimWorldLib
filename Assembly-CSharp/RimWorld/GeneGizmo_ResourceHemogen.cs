using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002083 RID: 8323
	[StaticConstructorOnStartup]
	public class GeneGizmo_ResourceHemogen : GeneGizmo_Resource
	{
		// Token: 0x170025B5 RID: 9653
		// (get) Token: 0x0600D1EB RID: 53739 RVA: 0x004D19B7 File Offset: 0x004CFBB7
		// (set) Token: 0x0600D1EC RID: 53740 RVA: 0x004D19BE File Offset: 0x004CFBBE
		protected override bool DraggingBar
		{
			get
			{
				return GeneGizmo_ResourceHemogen.draggingBar;
			}
			set
			{
				GeneGizmo_ResourceHemogen.draggingBar = value;
			}
		}

		// Token: 0x0600D1ED RID: 53741 RVA: 0x004D19C6 File Offset: 0x004CFBC6
		public GeneGizmo_ResourceHemogen(Gene_Resource gene, List<IGeneResourceDrain> drainGenes, Color barColor, Color barHighlightColor) : base(gene, drainGenes, barColor, barHighlightColor)
		{
		}

		// Token: 0x0600D1EE RID: 53742 RVA: 0x004D19E0 File Offset: 0x004CFBE0
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			float num = Mathf.Repeat(Time.time, 0.85f);
			float num2 = 1f;
			if (num < 0.1f)
			{
				num2 = num / 0.1f;
			}
			else if (num >= 0.25f)
			{
				num2 = 1f - (num - 0.25f) / 0.6f;
			}
			MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
			Command_Ability command_Ability = MapGizmoUtility.LastMouseOverGizmo as Command_Ability;
			if (command_Ability != null && this.gene.Max != 0f)
			{
				foreach (CompAbilityEffect compAbilityEffect in command_Ability.Ability.EffectComps)
				{
					CompAbilityEffect_HemogenCost compAbilityEffect_HemogenCost = compAbilityEffect as CompAbilityEffect_HemogenCost;
					if (compAbilityEffect_HemogenCost != null && compAbilityEffect_HemogenCost.Props.hemogenCost > 1E-45f)
					{
						Rect rect = this.barRect.ContractedBy(3f);
						float width = rect.width;
						float num3 = this.gene.Value / this.gene.Max;
						rect.xMax = rect.xMin + width * num3;
						float num4 = Mathf.Min(compAbilityEffect_HemogenCost.Props.hemogenCost / this.gene.Max, 1f);
						rect.xMin = Mathf.Max(rect.xMin, rect.xMax - width * num4);
						GUI.color = new Color(1f, 1f, 1f, num2 * 0.7f);
						GenUI.DrawTextureWithMaterial(rect, GeneGizmo_ResourceHemogen.HemogenCostTex, null, default(Rect));
						GUI.color = Color.white;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600D1EF RID: 53743 RVA: 0x004D1BBC File Offset: 0x004CFDBC
		protected override void DrawHeader(Rect headerRect, ref bool mouseOverElement)
		{
			if (this.IsDraggable)
			{
				Gene_Resource gene = this.gene;
				Gene_Hemogen hemogenGene = gene as Gene_Hemogen;
				if (hemogenGene != null)
				{
					headerRect.xMax -= 24f;
					Rect rect = new Rect(headerRect.xMax, headerRect.y, 24f, 24f);
					Widgets.DefIcon(rect, ThingDefOf.HemogenPack, null, 1f, null, false, null, null, null, 1f);
					GUI.DrawTexture(new Rect(rect.center.x, rect.y, rect.width / 2f, rect.height / 2f), hemogenGene.hemogenPacksAllowed ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);
					if (Widgets.ButtonInvisible(rect, true))
					{
						hemogenGene.hemogenPacksAllowed = !hemogenGene.hemogenPacksAllowed;
						if (hemogenGene.hemogenPacksAllowed)
						{
							SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						}
						else
						{
							SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
						}
					}
					if (Mouse.IsOver(rect))
					{
						Widgets.DrawHighlight(rect);
						string onOff = (hemogenGene.hemogenPacksAllowed ? "On" : "Off").Translate().ToString().UncapitalizeFirst();
						TooltipHandler.TipRegion(rect, () => "AutoTakeHemogenDesc".Translate(this.gene.pawn.Named("PAWN"), hemogenGene.PostProcessValue(hemogenGene.targetValue).Named("MIN"), onOff.Named("ONOFF")).Resolve(), 828267373);
						mouseOverElement = true;
					}
				}
			}
			base.DrawHeader(headerRect, ref mouseOverElement);
		}

		// Token: 0x0600D1F0 RID: 53744 RVA: 0x004D1D5C File Offset: 0x004CFF5C
		protected override string GetTooltip()
		{
			this.tmpDrainGenes.Clear();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
			defaultInterpolatedStringHandler.AppendFormatted(this.gene.ResourceLabel.CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor));
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.gene.ValueForDisplay);
			defaultInterpolatedStringHandler.AppendLiteral(" / ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.gene.MaxForDisplay);
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			if (this.gene.pawn.IsColonistPlayerControlled || this.gene.pawn.IsPrisonerOfColony)
			{
				if (this.gene.targetValue <= 0f)
				{
					text += "NeverConsumeHemogen".Translate().ToString();
				}
				else
				{
					text = text + ("ConsumeHemogenBelow".Translate() + ": ") + this.gene.PostProcessValue(this.gene.targetValue).ToString();
				}
			}
			if (!this.drainGenes.NullOrEmpty<IGeneResourceDrain>())
			{
				float num = 0f;
				foreach (IGeneResourceDrain geneResourceDrain in this.drainGenes)
				{
					if (geneResourceDrain.CanOffset)
					{
						this.tmpDrainGenes.Add(new Pair<IGeneResourceDrain, float>(geneResourceDrain, geneResourceDrain.ResourceLossPerDay));
						num += geneResourceDrain.ResourceLossPerDay;
					}
				}
				if (num != 0f)
				{
					string text2 = (num < 0f) ? "RegenerationRate".Translate() : "DrainRate".Translate();
					text = string.Concat(new string[]
					{
						text,
						"\n\n",
						text2,
						": ",
						"PerDay".Translate(Mathf.Abs(this.gene.PostProcessValue(num))).Resolve()
					});
					foreach (Pair<IGeneResourceDrain, float> pair in this.tmpDrainGenes)
					{
						text = string.Concat(new string[]
						{
							text,
							"\n  - ",
							pair.First.DisplayLabel.CapitalizeFirst(),
							": ",
							"PerDay".Translate(this.gene.PostProcessValue(-pair.Second).ToStringWithSign()).Resolve()
						});
					}
				}
			}
			if (!this.gene.def.resourceDescription.NullOrEmpty())
			{
				text = text + "\n\n" + this.gene.def.resourceDescription.Formatted(this.gene.pawn.Named("PAWN")).Resolve();
			}
			return text;
		}

		// Token: 0x0400850E RID: 34062
		private static readonly Texture2D HemogenCostTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.78f, 0.72f, 0.66f));

		// Token: 0x0400850F RID: 34063
		private const float TotalPulsateTime = 0.85f;

		// Token: 0x04008510 RID: 34064
		private static bool draggingBar;

		// Token: 0x04008511 RID: 34065
		private List<Pair<IGeneResourceDrain, float>> tmpDrainGenes = new List<Pair<IGeneResourceDrain, float>>();
	}
}
