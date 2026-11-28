using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020015B0 RID: 5552
	[StaticConstructorOnStartup]
	public static class MedicalCareUtility
	{
		// Token: 0x06008981 RID: 35201 RVA: 0x0031CC9E File Offset: 0x0031AE9E
		public static void Reset()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				MedicalCareUtility.careTextures = new Texture2D[5];
				MedicalCareUtility.careTextures[0] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoCare", true);
				MedicalCareUtility.careTextures[1] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoMeds", true);
				MedicalCareUtility.careTextures[2] = ThingDefOf.MedicineHerbal.uiIcon;
				MedicalCareUtility.careTextures[3] = ThingDefOf.MedicineIndustrial.uiIcon;
				MedicalCareUtility.careTextures[4] = ThingDefOf.MedicineUltratech.uiIcon;
			});
		}

		// Token: 0x06008982 RID: 35202 RVA: 0x0031CCC4 File Offset: 0x0031AEC4
		public static void MedicalCareSetter(Rect rect, ref MedicalCareCategory medCare)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width / 5f, rect.height);
			for (int i = 0; i < 5; i++)
			{
				MedicalCareCategory mc = (MedicalCareCategory)i;
				Widgets.DrawHighlightIfMouseover(rect2);
				MouseoverSounds.DoRegion(rect2);
				GUI.DrawTexture(rect2, MedicalCareUtility.careTextures[i]);
				Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect2, false);
				if (draggableResult == Widgets.DraggableResult.Dragged)
				{
					MedicalCareUtility.medicalCarePainting = true;
				}
				if ((MedicalCareUtility.medicalCarePainting && Mouse.IsOver(rect2) && medCare != mc) || draggableResult.AnyPressed())
				{
					medCare = mc;
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				if (medCare == mc)
				{
					Widgets.DrawBox(rect2, 2, null);
				}
				if (Mouse.IsOver(rect2))
				{
					TooltipHandler.TipRegion(rect2, () => mc.GetLabel().CapitalizeFirst(), 632165 + i * 17);
				}
				rect2.x += rect2.width;
			}
			if (!Input.GetMouseButton(0))
			{
				MedicalCareUtility.medicalCarePainting = false;
			}
		}

		// Token: 0x06008983 RID: 35203 RVA: 0x0031CDD2 File Offset: 0x0031AFD2
		public static string GetLabel(this MedicalCareCategory cat)
		{
			return ("MedicalCareCategory_" + cat.ToString()).Translate();
		}

		// Token: 0x06008984 RID: 35204 RVA: 0x0031CDF8 File Offset: 0x0031AFF8
		public static bool AllowsMedicine(this MedicalCareCategory cat, ThingDef meds)
		{
			switch (cat)
			{
			case MedicalCareCategory.NoCare:
				return false;
			case MedicalCareCategory.NoMeds:
				return false;
			case MedicalCareCategory.HerbalOrWorse:
				return meds.GetStatValueAbstract(StatDefOf.MedicalPotency, null) <= ThingDefOf.MedicineHerbal.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
			case MedicalCareCategory.NormalOrWorse:
				return meds.GetStatValueAbstract(StatDefOf.MedicalPotency, null) <= ThingDefOf.MedicineIndustrial.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
			case MedicalCareCategory.Best:
				return true;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06008985 RID: 35205 RVA: 0x0031CE70 File Offset: 0x0031B070
		public static void MedicalCareSelectButton(Rect rect, Pawn pawn)
		{
			Widgets.Dropdown<Pawn, MedicalCareCategory>(rect, pawn, new Func<Pawn, MedicalCareCategory>(MedicalCareUtility.MedicalCareSelectButton_GetMedicalCare), new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>>(MedicalCareUtility.MedicalCareSelectButton_GenerateMenu), null, MedicalCareUtility.careTextures[(int)pawn.playerSettings.medCare], null, null, null, true);
		}

		// Token: 0x06008986 RID: 35206 RVA: 0x0031CEB2 File Offset: 0x0031B0B2
		private static MedicalCareCategory MedicalCareSelectButton_GetMedicalCare(Pawn pawn)
		{
			return pawn.playerSettings.medCare;
		}

		// Token: 0x06008987 RID: 35207 RVA: 0x0031CEBF File Offset: 0x0031B0BF
		private static IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>> MedicalCareSelectButton_GenerateMenu(Pawn p)
		{
			MedicalCareUtility.<MedicalCareSelectButton_GenerateMenu>d__10 <MedicalCareSelectButton_GenerateMenu>d__ = new MedicalCareUtility.<MedicalCareSelectButton_GenerateMenu>d__10(-2);
			<MedicalCareSelectButton_GenerateMenu>d__.<>3__p = p;
			return <MedicalCareSelectButton_GenerateMenu>d__;
		}

		// Token: 0x06008988 RID: 35208 RVA: 0x0031CED0 File Offset: 0x0031B0D0
		private static Texture2D MedicalCareIcon(MedicalCareCategory category)
		{
			switch (category)
			{
			case MedicalCareCategory.NoCare:
				return MedicalCareUtility.careTextures[0];
			case MedicalCareCategory.NoMeds:
				return MedicalCareUtility.careTextures[1];
			case MedicalCareCategory.HerbalOrWorse:
				return MedicalCareUtility.careTextures[2];
			case MedicalCareCategory.NormalOrWorse:
				return MedicalCareUtility.careTextures[3];
			case MedicalCareCategory.Best:
				return MedicalCareUtility.careTextures[4];
			default:
				return null;
			}
		}

		// Token: 0x040054B1 RID: 21681
		private static Texture2D[] careTextures;

		// Token: 0x040054B2 RID: 21682
		public const float CareSetterHeight = 28f;

		// Token: 0x040054B3 RID: 21683
		public const float CareSetterWidth = 140f;

		// Token: 0x040054B4 RID: 21684
		private static bool medicalCarePainting;
	}
}
