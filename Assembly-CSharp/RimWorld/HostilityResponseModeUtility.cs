using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015AE RID: 5550
	[StaticConstructorOnStartup]
	public static class HostilityResponseModeUtility
	{
		// Token: 0x0600897A RID: 35194 RVA: 0x0031CACB File Offset: 0x0031ACCB
		public static Texture2D GetIcon(this HostilityResponseMode response)
		{
			switch (response)
			{
			case HostilityResponseMode.Ignore:
				return HostilityResponseModeUtility.IgnoreIcon;
			case HostilityResponseMode.Attack:
				return HostilityResponseModeUtility.AttackIcon;
			case HostilityResponseMode.Flee:
				return HostilityResponseModeUtility.FleeIcon;
			default:
				return BaseContent.BadTex;
			}
		}

		// Token: 0x0600897B RID: 35195 RVA: 0x0031CAF8 File Offset: 0x0031ACF8
		public static HostilityResponseMode GetNextResponse(Pawn pawn)
		{
			switch (pawn.playerSettings.hostilityResponse)
			{
			case HostilityResponseMode.Ignore:
				if (pawn.WorkTagIsDisabled(WorkTags.Violent))
				{
					return HostilityResponseMode.Flee;
				}
				return HostilityResponseMode.Attack;
			case HostilityResponseMode.Attack:
				return HostilityResponseMode.Flee;
			case HostilityResponseMode.Flee:
				return HostilityResponseMode.Ignore;
			default:
				return HostilityResponseMode.Ignore;
			}
		}

		// Token: 0x0600897C RID: 35196 RVA: 0x0031CB37 File Offset: 0x0031AD37
		public static string GetLabel(this HostilityResponseMode response)
		{
			return ("HostilityResponseMode_" + response.ToString()).Translate();
		}

		// Token: 0x0600897D RID: 35197 RVA: 0x0031CB5C File Offset: 0x0031AD5C
		public static void DrawResponseButton(Rect rect, Pawn pawn, bool paintable)
		{
			Widgets.Dropdown<Pawn, HostilityResponseMode>(rect, pawn, HostilityResponseModeUtility.IconColor, new Func<Pawn, HostilityResponseMode>(HostilityResponseModeUtility.DrawResponseButton_GetResponse), new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>>(HostilityResponseModeUtility.DrawResponseButton_GenerateMenu), null, pawn.playerSettings.hostilityResponse.GetIcon(), null, null, delegate()
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HostilityResponse, KnowledgeAmount.SpecificInteraction);
			}, paintable, new float?(4f));
			UIHighlighter.HighlightOpportunity(rect, "HostilityResponse");
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, "HostilityReponseTip".Translate() + "\n\n" + "HostilityResponseCurrentMode".Translate() + ": " + pawn.playerSettings.hostilityResponse.GetLabel());
			}
		}

		// Token: 0x0600897E RID: 35198 RVA: 0x0031CC2A File Offset: 0x0031AE2A
		private static HostilityResponseMode DrawResponseButton_GetResponse(Pawn pawn)
		{
			return pawn.playerSettings.hostilityResponse;
		}

		// Token: 0x0600897F RID: 35199 RVA: 0x0031CC37 File Offset: 0x0031AE37
		private static IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>> DrawResponseButton_GenerateMenu(Pawn p)
		{
			HostilityResponseModeUtility.<DrawResponseButton_GenerateMenu>d__9 <DrawResponseButton_GenerateMenu>d__ = new HostilityResponseModeUtility.<DrawResponseButton_GenerateMenu>d__9(-2);
			<DrawResponseButton_GenerateMenu>d__.<>3__p = p;
			return <DrawResponseButton_GenerateMenu>d__;
		}

		// Token: 0x040054A7 RID: 21671
		private static readonly Texture2D IgnoreIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Ignore", true);

		// Token: 0x040054A8 RID: 21672
		private static readonly Texture2D AttackIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Attack", true);

		// Token: 0x040054A9 RID: 21673
		private static readonly Texture2D FleeIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Flee", true);

		// Token: 0x040054AA RID: 21674
		private static readonly Color IconColor = new Color(0.84f, 0.84f, 0.84f);
	}
}
