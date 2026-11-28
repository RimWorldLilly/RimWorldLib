using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001626 RID: 5670
	public static class IdeoGenerator
	{
		// Token: 0x06008DCB RID: 36299 RVA: 0x0033C116 File Offset: 0x0033A316
		public static Ideo GenerateIdeo(IdeoGenerationParms parms)
		{
			Ideo ideo = IdeoGenerator.MakeIdeo(DefDatabase<IdeoFoundationDef>.AllDefs.RandomElement<IdeoFoundationDef>());
			ideo.foundation.Init(parms);
			return ideo;
		}

		// Token: 0x06008DCC RID: 36300 RVA: 0x0033C134 File Offset: 0x0033A334
		public static Ideo MakeIdeo(IdeoFoundationDef foundationDef)
		{
			Ideo ideo = new Ideo();
			ideo.id = Find.UniqueIDsManager.GetNextIdeoID();
			ideo.foundation = IdeoGenerator.MakeFoundation(foundationDef);
			ideo.foundation.ideo = ideo;
			return ideo;
		}

		// Token: 0x06008DCD RID: 36301 RVA: 0x0033C170 File Offset: 0x0033A370
		public static Ideo MakeFixedIdeo(IdeoGenerationParms parms)
		{
			Ideo ideo = IdeoGenerator.MakeIdeo(DefDatabase<IdeoFoundationDef>.AllDefs.RandomElement<IdeoFoundationDef>());
			ideo.foundation.Init(parms);
			if (parms.deities != null)
			{
				IdeoFoundation_Deity ideoFoundation_Deity = ideo.foundation as IdeoFoundation_Deity;
				if (ideoFoundation_Deity != null)
				{
					List<IdeoFoundation_Deity.Deity> list = new List<IdeoFoundation_Deity.Deity>();
					foreach (DeityPreset deityPreset in parms.deities)
					{
						IdeoFoundation_Deity.Deity item = new IdeoFoundation_Deity.Deity
						{
							name = deityPreset.nameType.name,
							type = deityPreset.nameType.type,
							gender = deityPreset.gender,
							iconPath = deityPreset.iconPath
						};
						list.Add(item);
					}
					ideoFoundation_Deity.SetDeities(list);
				}
			}
			if (parms.name != "")
			{
				ideo.name = parms.name;
			}
			if (parms.styles != null)
			{
				ideo.thingStyleCategories.Clear();
				for (int i = 0; i < parms.styles.Count; i++)
				{
					StyleCategoryDef category = parms.styles[i];
					ideo.thingStyleCategories.Add(new ThingStyleCategoryWithPriority(category, (float)(3 - i)));
				}
				ideo.SortStyleCategories();
			}
			if (parms.description != "")
			{
				ideo.description = parms.description;
			}
			ideo.hidden = parms.hidden;
			ideo.solid = true;
			return ideo;
		}

		// Token: 0x06008DCE RID: 36302 RVA: 0x0033C2FC File Offset: 0x0033A4FC
		public static IdeoFoundation MakeFoundation(IdeoFoundationDef foundationDef)
		{
			IdeoFoundation ideoFoundation = (IdeoFoundation)Activator.CreateInstance(foundationDef.foundationClass);
			ideoFoundation.def = foundationDef;
			return ideoFoundation;
		}

		// Token: 0x06008DCF RID: 36303 RVA: 0x0033C315 File Offset: 0x0033A515
		public static Ideo InitLoadedIdeo(Ideo ideo)
		{
			ideo.id = Find.UniqueIDsManager.GetNextIdeoID();
			ideo.style.ideo = ideo;
			return ideo;
		}

		// Token: 0x06008DD0 RID: 36304 RVA: 0x0033C334 File Offset: 0x0033A534
		public static Ideo GenerateClassicIdeo(CultureDef culture, IdeoGenerationParms genParms, bool noExpansionIdeo)
		{
			IdeoGenerator.<>c__DisplayClass5_0 CS$<>8__locals1;
			CS$<>8__locals1.culture = culture;
			Ideo ideo = new Ideo();
			ideo.id = Find.UniqueIDsManager.GetNextIdeoID();
			ideo.culture = CS$<>8__locals1.culture;
			ideo.createdFromNoExpansionGame = noExpansionIdeo;
			ideo.name = IdeoGenerator.<GenerateClassicIdeo>g__ComputeIdeoName|5_0(ref CS$<>8__locals1);
			ideo.memberName = FactionDefOf.PlayerColony.basicMemberKind.label;
			ideo.classicMode = !noExpansionIdeo;
			if (ModsConfig.IdeologyActive)
			{
				IdeoFoundationDef ideoFoundationDef = DefDatabase<IdeoFoundationDef>.AllDefs.RandomElement<IdeoFoundationDef>();
				ideo.foundation = (IdeoFoundation)Activator.CreateInstance(ideoFoundationDef.foundationClass);
				ideo.foundation.def = ideoFoundationDef;
				ideo.foundation.ideo = ideo;
				ideo.SetIcon(IdeoFoundation.GetRandomIconDef(ideo), IdeoFoundation.GetRandomColorDef(ideo), false);
			}
			List<PreceptDef> allDefsListForReading = DefDatabase<PreceptDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				PreceptDef preceptDef = allDefsListForReading[i];
				if (preceptDef.classic && (genParms.disallowedPrecepts == null || !genParms.disallowedPrecepts.Contains(preceptDef)))
				{
					int num = 1;
					if (preceptDef.preceptInstanceCountCurve != null)
					{
						num = Mathf.CeilToInt(preceptDef.preceptInstanceCountCurve.Evaluate(Rand.Value));
					}
					for (int j = 0; j < num; j++)
					{
						ideo.AddPrecept(PreceptMaker.MakePrecept(preceptDef), true, null, preceptDef.ritualPatternBase);
					}
				}
			}
			return ideo;
		}

		// Token: 0x06008DD1 RID: 36305 RVA: 0x0033C48A File Offset: 0x0033A68A
		public static Ideo GenerateNoExpansionIdeo(CultureDef culture, IdeoGenerationParms genParms)
		{
			return IdeoGenerator.GenerateClassicIdeo(culture, genParms, true);
		}

		// Token: 0x06008DD2 RID: 36306 RVA: 0x0033C494 File Offset: 0x0033A694
		public static Ideo GenerateTutorialIdeo()
		{
			IdeoGenerationParms parms = new IdeoGenerationParms(Find.Scenario.playerFaction.factionDef, false, null, null, null, false, false, false, false, "", null, null, false, "", false);
			parms.disallowedPrecepts = (from x in DefDatabase<PreceptDef>.AllDefs
			where x.impact == PreceptImpact.High
			select x).ToList<PreceptDef>();
			parms.disallowedMemes = (from x in DefDatabase<MemeDef>.AllDefs
			where !x.allowDuringTutorial
			select x).ToList<MemeDef>();
			return IdeoGenerator.GenerateIdeo(parms);
		}

		// Token: 0x06008DD3 RID: 36307 RVA: 0x0033C540 File Offset: 0x0033A740
		[CompilerGenerated]
		internal static TaggedString <GenerateClassicIdeo>g__ComputeIdeoName|5_0(ref IdeoGenerator.<>c__DisplayClass5_0 A_0)
		{
			int num = 1;
			string text;
			for (;;)
			{
				text = A_0.culture.LabelCap;
				if (num > 1)
				{
					text = text + " " + num.ToString();
				}
				if (!IdeoGenerator.<GenerateClassicIdeo>g__NameUsed|5_1(text))
				{
					break;
				}
				num++;
			}
			return text;
		}

		// Token: 0x06008DD4 RID: 36308 RVA: 0x0033C58C File Offset: 0x0033A78C
		[CompilerGenerated]
		internal static bool <GenerateClassicIdeo>g__NameUsed|5_1(string name)
		{
			using (List<Ideo>.Enumerator enumerator = Find.IdeoManager.IdeosListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.name == name)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
