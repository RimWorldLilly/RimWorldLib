using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001D86 RID: 7558
	public class ITab_WindTurbineAutoCut : ITab
	{
		// Token: 0x1700245B RID: 9307
		// (get) Token: 0x0600C3E2 RID: 50146 RVA: 0x0047AA47 File Offset: 0x00478C47
		public CompAutoCut AutoCut
		{
			get
			{
				Thing selThing = base.SelThing;
				if (selThing == null)
				{
					return null;
				}
				return selThing.TryGetComp<CompAutoCut>();
			}
		}

		// Token: 0x1700245C RID: 9308
		// (get) Token: 0x0600C3E3 RID: 50147 RVA: 0x0047AA5A File Offset: 0x00478C5A
		public override bool IsVisible
		{
			get
			{
				return this.AutoCut != null;
			}
		}

		// Token: 0x0600C3E4 RID: 50148 RVA: 0x0047AA65 File Offset: 0x00478C65
		public ITab_WindTurbineAutoCut()
		{
			this.size = ITab_WindTurbineAutoCut.WinSize;
			this.labelKey = "TabWindTurbineAutoCut";
		}

		// Token: 0x0600C3E5 RID: 50149 RVA: 0x0047AA8E File Offset: 0x00478C8E
		public override void OnOpen()
		{
			base.OnOpen();
			this.plantFilterState.quickSearch.Reset();
		}

		// Token: 0x0600C3E6 RID: 50150 RVA: 0x0047AAA8 File Offset: 0x00478CA8
		protected override void FillTab()
		{
			CompAutoCut autoCut = this.AutoCut;
			Rect rect = new Rect(0f, 0f, ITab_WindTurbineAutoCut.WinSize.x, ITab_WindTurbineAutoCut.WinSize.y).ContractedBy(10f);
			Widgets.BeginGroup(rect);
			float num = 0f;
			this.DrawAutoCutOptions(ref num, rect.width, autoCut);
			num += 4f;
			this.DrawPlantFilter(ref num, rect.width, rect.height - num, autoCut);
			Widgets.EndGroup();
		}

		// Token: 0x0600C3E7 RID: 50151 RVA: 0x0047AB2C File Offset: 0x00478D2C
		private void DrawPlantFilter(ref float curY, float width, float height, CompAutoCut autoCut)
		{
			Rect rect = new Rect(0f, curY, width, height);
			ThingFilterUI.UIState state = this.plantFilterState;
			ThingFilter autoCutFilter = autoCut.AutoCutFilter;
			ThingFilter fixedAutoCutFilter = autoCut.GetFixedAutoCutFilter();
			int openMask = 1;
			IEnumerable<ThingDef> forceHiddenDefs = null;
			Map map = autoCut.parent.Map;
			ThingFilterUI.DoThingFilterConfigWindow(rect, state, autoCutFilter, fixedAutoCutFilter, openMask, forceHiddenDefs, this.HiddenSpecialThingFilters(), true, false, false, null, map);
		}

		// Token: 0x0600C3E8 RID: 50152 RVA: 0x0047AB7C File Offset: 0x00478D7C
		private void DrawAutoCutOptions(ref float curY, float width, CompAutoCut autoCut)
		{
			Designator_PlantsCut designator_PlantsCut = Find.ReverseDesignatorDatabase.Get<Designator_PlantsCut>();
			Rect position = new Rect(0f, curY, 24f, 24f);
			Rect rect = new Rect(position.xMax + 4f, curY, width, 24f);
			Rect rect2 = new Rect(0f, rect.yMax + 4f, 150f, 27f);
			GUI.DrawTexture(position, designator_PlantsCut.icon);
			Text.Font = GameFont.Tiny;
			Widgets.CheckboxLabeled(rect, "WindTurbineAutoCut_EnabledCheckbox".Translate(), ref autoCut.autoCut, false, null, null, true, false);
			Text.Font = GameFont.Small;
			if (Widgets.ButtonText(rect2, "AutoCutNow".Translate(), true, true, true, null))
			{
				autoCut.DesignatePlantsToCut();
				SoundDef soundSucceeded = designator_PlantsCut.soundSucceeded;
				if (soundSucceeded != null)
				{
					soundSucceeded.PlayOneShotOnCamera(null);
				}
			}
			curY = rect2.yMax;
		}

		// Token: 0x0600C3E9 RID: 50153 RVA: 0x0047AC68 File Offset: 0x00478E68
		private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
		{
			return new ITab_WindTurbineAutoCut.<HiddenSpecialThingFilters>d__14(-2);
		}

		// Token: 0x04006FED RID: 28653
		private static readonly Vector2 WinSize = new Vector2(300f, 480f);

		// Token: 0x04006FEE RID: 28654
		private const int CutNowButtonWidth = 150;

		// Token: 0x04006FEF RID: 28655
		private const int CutNowButtonHeight = 27;

		// Token: 0x04006FF0 RID: 28656
		private const float AutoCutRowHeight = 24f;

		// Token: 0x04006FF1 RID: 28657
		private ThingFilterUI.UIState plantFilterState = new ThingFilterUI.UIState();
	}
}
