using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B20 RID: 6944
	public class Apparel : ThingWithComps
	{
		// Token: 0x1700200C RID: 8204
		// (get) Token: 0x0600B1D7 RID: 45527 RVA: 0x003EA63C File Offset: 0x003E883C
		public Pawn Wearer
		{
			get
			{
				Pawn_ApparelTracker pawn_ApparelTracker = base.ParentHolder as Pawn_ApparelTracker;
				if (pawn_ApparelTracker == null)
				{
					return null;
				}
				return pawn_ApparelTracker.pawn;
			}
		}

		// Token: 0x1700200D RID: 8205
		// (get) Token: 0x0600B1D8 RID: 45528 RVA: 0x003EA660 File Offset: 0x003E8860
		// (set) Token: 0x0600B1D9 RID: 45529 RVA: 0x003EA668 File Offset: 0x003E8868
		public bool WornByCorpse
		{
			get
			{
				return this.wornByCorpseInt;
			}
			set
			{
				this.wornByCorpseInt = value;
			}
		}

		// Token: 0x1700200E RID: 8206
		// (get) Token: 0x0600B1DA RID: 45530 RVA: 0x003EA674 File Offset: 0x003E8874
		public string WornGraphicPath
		{
			get
			{
				if (this.StyleDef != null && !this.StyleDef.wornGraphicPath.NullOrEmpty())
				{
					return this.StyleDef.wornGraphicPath;
				}
				if (!this.def.apparel.wornGraphicPaths.NullOrEmpty<string>())
				{
					return this.def.apparel.wornGraphicPaths[this.thingIDNumber % this.def.apparel.wornGraphicPaths.Count];
				}
				return this.def.apparel.wornGraphicPath;
			}
		}

		// Token: 0x1700200F RID: 8207
		// (get) Token: 0x0600B1DB RID: 45531 RVA: 0x003EA700 File Offset: 0x003E8900
		public override string DescriptionDetailed
		{
			get
			{
				string text = base.DescriptionDetailed;
				if (this.WornByCorpse)
				{
					text += "\n" + "WasWornByCorpse".Translate();
				}
				return text;
			}
		}

		// Token: 0x17002010 RID: 8208
		// (get) Token: 0x0600B1DC RID: 45532 RVA: 0x003EA740 File Offset: 0x003E8940
		public override Color DrawColor
		{
			get
			{
				Color color;
				if (this.StyleDef != null && this.StyleDef.color != default(Color))
				{
					color = this.StyleDef.color;
				}
				else
				{
					color = base.DrawColor;
				}
				if (this.WornByCorpse)
				{
					color = PawnRenderUtility.GetRottenColor(color);
				}
				Pawn wearer = this.Wearer;
				if (wearer != null)
				{
					Pawn_DrawTracker drawer = wearer.Drawer;
					if (drawer != null)
					{
						PawnRenderer renderer = drawer.renderer;
						if (renderer != null)
						{
							Color? statueColor = renderer.StatueColor;
							if (statueColor != null)
							{
								Color valueOrDefault = statueColor.GetValueOrDefault();
								color = valueOrDefault;
							}
						}
					}
				}
				return color;
			}
		}

		// Token: 0x17002011 RID: 8209
		// (get) Token: 0x0600B1DD RID: 45533 RVA: 0x003EA7D4 File Offset: 0x003E89D4
		// (set) Token: 0x0600B1DE RID: 45534 RVA: 0x003EA7FC File Offset: 0x003E89FC
		public Color? DesiredColor
		{
			get
			{
				CompColorable comp = base.GetComp<CompColorable>();
				if (comp == null)
				{
					return null;
				}
				return comp.DesiredColor;
			}
			set
			{
				CompColorable comp = base.GetComp<CompColorable>();
				if (comp != null)
				{
					comp.DesiredColor = value;
					return;
				}
				Log.Error("Tried setting Apparel.DesiredColor without having CompColorable comp!");
			}
		}

		// Token: 0x17002012 RID: 8210
		// (get) Token: 0x0600B1DF RID: 45535 RVA: 0x003EA825 File Offset: 0x003E8A25
		public IEnumerable<Ability> AllAbilitiesForReading
		{
			get
			{
				Apparel.<get_AllAbilitiesForReading>d__17 <get_AllAbilitiesForReading>d__ = new Apparel.<get_AllAbilitiesForReading>d__17(-2);
				<get_AllAbilitiesForReading>d__.<>4__this = this;
				return <get_AllAbilitiesForReading>d__;
			}
		}

		// Token: 0x0600B1E0 RID: 45536 RVA: 0x003EA838 File Offset: 0x003E8A38
		public override string GetInspectStringLowPriority()
		{
			string text = base.GetInspectStringLowPriority();
			if (this.StyleDef != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "VariantOf".Translate().CapitalizeFirst() + ": " + this.def.LabelCap;
			}
			if (ModsConfig.BiotechActive)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "WearableBy".Translate() + ": " + this.def.apparel.developmentalStageFilter.ToCommaList(false).CapitalizeFirst();
			}
			return text;
		}

		// Token: 0x0600B1E1 RID: 45537 RVA: 0x003EA8FC File Offset: 0x003E8AFC
		public bool PawnCanWear(Pawn pawn, bool ignoreGender = false)
		{
			return this.def.IsApparel && this.def.apparel.PawnCanWear(pawn, ignoreGender);
		}

		// Token: 0x0600B1E2 RID: 45538 RVA: 0x003EA924 File Offset: 0x003E8B24
		public Color GetColorIgnoringTainted()
		{
			if (this.StyleDef != null && this.StyleDef.color != default(Color))
			{
				return this.StyleDef.color;
			}
			return base.DrawColor;
		}

		// Token: 0x0600B1E3 RID: 45539 RVA: 0x003EA968 File Offset: 0x003E8B68
		public void Notify_PawnKilled()
		{
			if (this.def.apparel.careIfWornByCorpse)
			{
				this.wornByCorpseInt = true;
			}
			foreach (ThingComp thingComp in base.AllComps)
			{
				thingComp.Notify_WearerDied();
			}
		}

		// Token: 0x0600B1E4 RID: 45540 RVA: 0x003EA9D4 File Offset: 0x003E8BD4
		public void Notify_PawnResurrected(Pawn pawn)
		{
			if (pawn.IsMutant && pawn.mutant.Def.isConsideredCorpse)
			{
				return;
			}
			this.wornByCorpseInt = false;
		}

		// Token: 0x0600B1E5 RID: 45541 RVA: 0x003EA9F8 File Offset: 0x003E8BF8
		public override void Notify_ColorChanged()
		{
			if (this.Wearer != null)
			{
				this.Wearer.apparel.Notify_ApparelChanged();
			}
			base.Notify_ColorChanged();
		}

		// Token: 0x0600B1E6 RID: 45542 RVA: 0x003EAA18 File Offset: 0x003E8C18
		public override void Notify_Equipped(Pawn pawn)
		{
			base.Notify_Equipped(pawn);
			foreach (Ability ability in this.AllAbilitiesForReading)
			{
				ability.pawn = pawn;
				ability.verb.caster = pawn;
			}
		}

		// Token: 0x0600B1E7 RID: 45543 RVA: 0x003EAA78 File Offset: 0x003E8C78
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wornByCorpseInt, "wornByCorpse", false, false);
			Scribe_Collections.Look<Ability>(ref this.abilities, "abilities", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.FillOrUpdateAbilities();
			}
		}

		// Token: 0x0600B1E8 RID: 45544 RVA: 0x003EAAB8 File Offset: 0x003E8CB8
		public void FillOrUpdateAbilities()
		{
			if (this.def.apparel.abilities == null)
			{
				return;
			}
			if (this.abilities == null)
			{
				this.abilities = new List<Ability>();
				foreach (AbilityDef def in this.def.apparel.abilities)
				{
					this.abilities.Add(AbilityUtility.MakeAbility(def, this.Wearer));
				}
			}
			foreach (Ability ability in this.abilities)
			{
				ability.pawn = this.Wearer;
				ability.verb.caster = this.Wearer;
			}
		}

		// Token: 0x0600B1E9 RID: 45545 RVA: 0x003EABA4 File Offset: 0x003E8DA4
		public virtual void DrawWornExtras()
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].CompDrawWornExtras();
			}
		}

		// Token: 0x0600B1EA RID: 45546 RVA: 0x003EABD8 File Offset: 0x003E8DD8
		public virtual bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				bool flag;
				allComps[i].PostPreApplyDamage(ref dinfo, out flag);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B1EB RID: 45547 RVA: 0x003EAC14 File Offset: 0x003E8E14
		public virtual bool AllowVerbCast(Verb verb)
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				if (!allComps[i].CompAllowVerbCast(verb))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B1EC RID: 45548 RVA: 0x003EAC4B File Offset: 0x003E8E4B
		public virtual IEnumerable<Gizmo> GetWornGizmos()
		{
			Apparel.<GetWornGizmos>d__30 <GetWornGizmos>d__ = new Apparel.<GetWornGizmos>d__30(-2);
			<GetWornGizmos>d__.<>4__this = this;
			return <GetWornGizmos>d__;
		}

		// Token: 0x0600B1ED RID: 45549 RVA: 0x003EAC5B File Offset: 0x003E8E5B
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			Apparel.<SpecialDisplayStats>d__31 <SpecialDisplayStats>d__ = new Apparel.<SpecialDisplayStats>d__31(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600B1EE RID: 45550 RVA: 0x003EAC6C File Offset: 0x003E8E6C
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.WornByCorpse)
			{
				if (text.Length > 0)
				{
					text += "\n";
				}
				text += "WasWornByCorpse".Translate();
			}
			return text;
		}

		// Token: 0x0600B1EF RID: 45551 RVA: 0x003EACB4 File Offset: 0x003E8EB4
		public virtual float GetSpecialApparelScoreOffset()
		{
			float num = 0f;
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				num += allComps[i].CompGetSpecialApparelScoreOffset();
			}
			return num;
		}

		// Token: 0x0600B1F0 RID: 45552 RVA: 0x003EACF0 File Offset: 0x003E8EF0
		[DebugOutput]
		private static void ApparelValidLifeStages()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("name", (ThingDef t) => t.LabelCap));
			list.Add(new TableDataGetter<ThingDef>("valid life stage", (ThingDef t) => t.apparel.developmentalStageFilter.ToCommaList(false)));
			DebugTables.MakeTablesDialog<ThingDef>(from t in DefDatabase<ThingDef>.AllDefs
			where t.IsApparel
			select t, list.ToArray());
		}

		// Token: 0x040066AE RID: 26286
		private bool wornByCorpseInt;

		// Token: 0x040066AF RID: 26287
		private List<Ability> abilities;
	}
}
