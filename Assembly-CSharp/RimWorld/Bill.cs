using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000997 RID: 2455
	public abstract class Bill : IExposable, ILoadReferenceable
	{
		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x060049F3 RID: 18931 RVA: 0x001CC034 File Offset: 0x001CA234
		public Map Map
		{
			get
			{
				BillStack billStack = this.billStack;
				Map map;
				if (billStack == null)
				{
					map = null;
				}
				else
				{
					IBillGiver billGiver = billStack.billGiver;
					map = ((billGiver != null) ? billGiver.Map : null);
				}
				Map result;
				if ((result = map) == null)
				{
					BillStack billStack2 = this.billStack;
					Thing thing = ((billStack2 != null) ? billStack2.billGiver : null) as Thing;
					if (thing == null)
					{
						return null;
					}
					result = thing.MapHeld;
				}
				return result;
			}
		}

		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x060049F4 RID: 18932 RVA: 0x001CC085 File Offset: 0x001CA285
		public virtual string Label
		{
			get
			{
				if (this.precept == null)
				{
					return this.recipe.label;
				}
				return "RecipeMake".Translate(this.precept.LabelCap).UncapitalizeFirst();
			}
		}

		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x060049F5 RID: 18933 RVA: 0x001CC0BF File Offset: 0x001CA2BF
		public virtual string LabelCap
		{
			get
			{
				if (this.precept == null)
				{
					return this.Label.CapitalizeFirst(this.recipe);
				}
				return "RecipeMake".Translate(this.precept.LabelCap).CapitalizeFirst();
			}
		}

		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x060049F6 RID: 18934 RVA: 0x001CC0FF File Offset: 0x001CA2FF
		public virtual bool CheckIngredientsIfSociallyProper
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x060049F7 RID: 18935 RVA: 0x001CC102 File Offset: 0x001CA302
		public virtual bool CompletableEver
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x060049F8 RID: 18936 RVA: 0x001CC105 File Offset: 0x001CA305
		protected virtual string StatusString
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x060049F9 RID: 18937 RVA: 0x001CC108 File Offset: 0x001CA308
		protected virtual float StatusLineMinHeight
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x060049FA RID: 18938 RVA: 0x001CC10F File Offset: 0x001CA30F
		protected virtual bool CanCopy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x060049FB RID: 18939 RVA: 0x001CC112 File Offset: 0x001CA312
		public virtual bool CanFinishNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x060049FC RID: 18940 RVA: 0x001CC118 File Offset: 0x001CA318
		public bool DeletedOrDereferenced
		{
			get
			{
				if (this.deleted)
				{
					return true;
				}
				Thing thing = this.billStack.billGiver as Thing;
				return thing != null && thing.Destroyed;
			}
		}

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x060049FD RID: 18941 RVA: 0x001CC14E File Offset: 0x001CA34E
		public Pawn PawnRestriction
		{
			get
			{
				return this.pawnRestriction;
			}
		}

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x060049FE RID: 18942 RVA: 0x001CC156 File Offset: 0x001CA356
		public bool SlavesOnly
		{
			get
			{
				return this.slavesOnly;
			}
		}

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x060049FF RID: 18943 RVA: 0x001CC15E File Offset: 0x001CA35E
		public bool MechsOnly
		{
			get
			{
				return this.mechsOnly;
			}
		}

		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x06004A00 RID: 18944 RVA: 0x001CC166 File Offset: 0x001CA366
		public bool NonMechsOnly
		{
			get
			{
				return this.nonMechsOnly;
			}
		}

		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x06004A01 RID: 18945 RVA: 0x001CC16E File Offset: 0x001CA36E
		protected virtual Color BaseColor
		{
			get
			{
				if (this.ShouldDoNow())
				{
					return Color.white;
				}
				return new Color(1f, 0.7f, 0.7f, 0.7f);
			}
		}

		// Token: 0x06004A02 RID: 18946 RVA: 0x001CC197 File Offset: 0x001CA397
		public Bill()
		{
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x001CC1C8 File Offset: 0x001CA3C8
		public Bill(RecipeDef recipe, Precept_ThingStyle precept = null)
		{
			this.recipe = recipe;
			this.precept = precept;
			this.ingredientFilter = new ThingFilter();
			this.ingredientFilter.CopyAllowancesFrom(recipe.defaultIngredientFilter);
			this.InitializeAfterClone();
		}

		// Token: 0x06004A04 RID: 18948 RVA: 0x001CC232 File Offset: 0x001CA432
		public void InitializeAfterClone()
		{
			this.loadID = Find.UniqueIDsManager.GetNextBillID();
		}

		// Token: 0x06004A05 RID: 18949 RVA: 0x001CC244 File Offset: 0x001CA444
		public void SetPawnRestriction(Pawn pawn)
		{
			this.pawnRestriction = pawn;
			this.slavesOnly = false;
			this.mechsOnly = false;
			this.nonMechsOnly = false;
		}

		// Token: 0x06004A06 RID: 18950 RVA: 0x001CC262 File Offset: 0x001CA462
		public void SetAnySlaveRestriction()
		{
			this.pawnRestriction = null;
			this.slavesOnly = true;
			this.mechsOnly = false;
			this.nonMechsOnly = false;
		}

		// Token: 0x06004A07 RID: 18951 RVA: 0x001CC280 File Offset: 0x001CA480
		public void SetAnyPawnRestriction()
		{
			this.slavesOnly = false;
			this.pawnRestriction = null;
			this.mechsOnly = false;
			this.nonMechsOnly = false;
		}

		// Token: 0x06004A08 RID: 18952 RVA: 0x001CC29E File Offset: 0x001CA49E
		public void SetAnyMechRestriction()
		{
			this.slavesOnly = false;
			this.pawnRestriction = null;
			this.mechsOnly = true;
			this.nonMechsOnly = false;
		}

		// Token: 0x06004A09 RID: 18953 RVA: 0x001CC2BC File Offset: 0x001CA4BC
		public void SetAnyNonMechRestriction()
		{
			this.slavesOnly = false;
			this.pawnRestriction = null;
			this.mechsOnly = false;
			this.nonMechsOnly = true;
		}

		// Token: 0x06004A0A RID: 18954 RVA: 0x001CC2DC File Offset: 0x001CA4DC
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Defs.Look<RecipeDef>(ref this.recipe, "recipe");
			Scribe_Values.Look<bool>(ref this.suspended, "suspended", false, false);
			Scribe_Values.Look<float>(ref this.ingredientSearchRadius, "ingredientSearchRadius", 999f, false);
			Scribe_Values.Look<IntRange>(ref this.allowedSkillRange, "allowedSkillRange", default(IntRange), false);
			Scribe_References.Look<Pawn>(ref this.pawnRestriction, "pawnRestriction", false);
			Scribe_References.Look<Precept_ThingStyle>(ref this.precept, "precept", false);
			Scribe_References.Look<Xenogerm>(ref this.xenogerm, "xenogerm", false);
			Scribe_Values.Look<bool>(ref this.slavesOnly, "slavesOnly", false, false);
			Scribe_Values.Look<bool>(ref this.mechsOnly, "mechsOnly", false, false);
			Scribe_Values.Look<bool>(ref this.nonMechsOnly, "nonMechsOnly", false, false);
			Scribe_Defs.Look<ThingStyleDef>(ref this.style, "style");
			Scribe_Values.Look<bool>(ref this.globalStyle, "globalStyle", true, false);
			Scribe_Values.Look<int?>(ref this.graphicIndexOverride, "graphicIndexOverride", null, false);
			if (Scribe.mode == LoadSaveMode.Saving && this.recipe.fixedIngredientFilter != null)
			{
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (!this.recipe.fixedIngredientFilter.Allows(thingDef))
					{
						this.ingredientFilter.SetAllow(thingDef, false);
					}
				}
			}
			Scribe_Deep.Look<ThingFilter>(ref this.ingredientFilter, "ingredientFilter", Array.Empty<object>());
		}

		// Token: 0x06004A0B RID: 18955 RVA: 0x001CC474 File Offset: 0x001CA674
		public virtual bool PawnAllowedToStartAnew(Pawn p)
		{
			if (this.pawnRestriction != null)
			{
				return this.pawnRestriction == p;
			}
			if (this.slavesOnly && !p.IsSlave)
			{
				return false;
			}
			if (this.mechsOnly && !p.IsColonyMechPlayerControlled)
			{
				return false;
			}
			if (this.nonMechsOnly && p.IsColonyMechPlayerControlled)
			{
				return false;
			}
			if (this.recipe.workSkill != null && (p.skills != null || p.IsColonyMech))
			{
				int num = (p.skills != null) ? p.skills.GetSkill(this.recipe.workSkill).Level : p.RaceProps.mechFixedSkillLevel;
				if (num < this.allowedSkillRange.min)
				{
					JobFailReason.Is("UnderAllowedSkill".Translate(this.allowedSkillRange.min), this.Label);
					return false;
				}
				if (num > this.allowedSkillRange.max)
				{
					JobFailReason.Is("AboveAllowedSkill".Translate(this.allowedSkillRange.max), this.Label);
					return false;
				}
			}
			if (ModsConfig.BiotechActive && this.recipe.mechanitorOnlyRecipe && !MechanitorUtility.IsMechanitor(p))
			{
				JobFailReason.Is("NotAMechanitor".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x06004A0C RID: 18956 RVA: 0x001CC5C5 File Offset: 0x001CA7C5
		public virtual void Notify_PawnDidWork(Pawn p)
		{
		}

		// Token: 0x06004A0D RID: 18957 RVA: 0x001CC5C7 File Offset: 0x001CA7C7
		public virtual void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
		}

		// Token: 0x06004A0E RID: 18958
		public abstract bool ShouldDoNow();

		// Token: 0x06004A0F RID: 18959 RVA: 0x001CC5C9 File Offset: 0x001CA7C9
		public virtual void Notify_DoBillStarted(Pawn billDoer)
		{
		}

		// Token: 0x06004A10 RID: 18960 RVA: 0x001CC5CB File Offset: 0x001CA7CB
		public virtual void Notify_BillWorkStarted(Pawn billDoer)
		{
		}

		// Token: 0x06004A11 RID: 18961 RVA: 0x001CC5CD File Offset: 0x001CA7CD
		public virtual void Notify_BillWorkFinished(Pawn billDoer)
		{
		}

		// Token: 0x06004A12 RID: 18962 RVA: 0x001CC5D0 File Offset: 0x001CA7D0
		protected virtual void DoConfigInterface(Rect rect, Color baseColor)
		{
			rect.yMin += 29f;
			float y = rect.center.y;
			Widgets.InfoCardButton(rect.xMax - (rect.yMax - y) - 12f, y - 12f, this.recipe);
		}

		// Token: 0x06004A13 RID: 18963 RVA: 0x001CC627 File Offset: 0x001CA827
		public virtual void DoStatusLineInterface(Rect rect)
		{
		}

		// Token: 0x06004A14 RID: 18964 RVA: 0x001CC62C File Offset: 0x001CA82C
		public Rect DoInterface(float x, float y, float width, int index)
		{
			Rect rect = new Rect(x, y, width, 53f);
			float num = 0f;
			if (!this.StatusString.NullOrEmpty())
			{
				num = Mathf.Max(Text.TinyFontSupported ? 17f : 21f, this.StatusLineMinHeight);
			}
			rect.height += num;
			Color baseColor = this.BaseColor;
			GUI.color = baseColor;
			Text.Font = GameFont.Small;
			if (index % 2 == 0)
			{
				Widgets.DrawAltRect(rect);
			}
			Widgets.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, 24f, 24f);
			if (this.billStack.IndexOf(this) > 0)
			{
				if (Widgets.ButtonImage(rect2, TexButton.ReorderUp, baseColor, true, null))
				{
					this.billStack.Reorder(this, -1);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect2, "ReorderBillUpTip");
			}
			if (this.billStack.IndexOf(this) < this.billStack.Count - 1)
			{
				Rect rect3 = new Rect(0f, 24f, 24f, 24f);
				if (Widgets.ButtonImage(rect3, TexButton.ReorderDown, baseColor, true, null))
				{
					this.billStack.Reorder(this, 1);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect3, "ReorderBillDownTip");
			}
			GUI.color = baseColor;
			Widgets.Label(new Rect(28f, 0f, rect.width - 48f - 20f, rect.height + 5f), this.LabelCap);
			this.DoConfigInterface(rect.AtZero(), baseColor);
			Rect rect4 = new Rect(rect.width - 24f, 0f, 24f, 24f);
			if (Widgets.ButtonImage(rect4, TexButton.Delete, baseColor, baseColor * GenUI.SubtleMouseoverColor, true, null))
			{
				this.billStack.Delete(this);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			TooltipHandler.TipRegionByKey(rect4, "DeleteBillTip");
			Rect rect6;
			if (this.CanCopy)
			{
				Rect rect5 = new Rect(rect4);
				rect5.x -= rect5.width + 4f;
				if (Widgets.ButtonImageFitted(rect5, TexButton.Copy, baseColor))
				{
					BillUtility.Clipboard = this.Clone();
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect5, "CopyBillTip");
				rect6 = new Rect(rect5);
			}
			else
			{
				rect6 = new Rect(rect4);
			}
			rect6.x -= rect6.width;
			if (Widgets.ButtonImage(rect6, TexButton.Suspend, baseColor, true, null))
			{
				this.suspended = !this.suspended;
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			TooltipHandler.TipRegionByKey(rect6, "SuspendBillTip");
			if (!this.StatusString.NullOrEmpty())
			{
				Text.Font = GameFont.Tiny;
				Rect rect7 = new Rect(24f, rect.height - num, rect.width - 24f, num);
				Widgets.Label(rect7, this.StatusString);
				this.DoStatusLineInterface(rect7);
			}
			Widgets.EndGroup();
			if (this.suspended)
			{
				Text.Font = GameFont.Medium;
				Text.Anchor = TextAnchor.MiddleCenter;
				Rect rect8 = new Rect(rect.x + rect.width / 2f - 70f, rect.y + rect.height / 2f - 20f, 140f, 40f);
				GUI.DrawTexture(rect8, TexUI.GrayTextBG);
				Widgets.Label(rect8, "SuspendedCaps".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
			}
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			return rect;
		}

		// Token: 0x06004A15 RID: 18965 RVA: 0x001CC9C0 File Offset: 0x001CABC0
		public virtual bool IsFixedOrAllowedIngredient(Thing thing)
		{
			for (int i = 0; i < this.recipe.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = this.recipe.ingredients[i];
				if (ingredientCount.IsFixedIngredient && ingredientCount.filter.Allows(thing))
				{
					return true;
				}
			}
			return this.recipe.fixedIngredientFilter.Allows(thing) && this.ingredientFilter.Allows(thing);
		}

		// Token: 0x06004A16 RID: 18966 RVA: 0x001CCA34 File Offset: 0x001CAC34
		public bool IsFixedOrAllowedIngredient(ThingDef def)
		{
			for (int i = 0; i < this.recipe.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = this.recipe.ingredients[i];
				if (ingredientCount.IsFixedIngredient && ingredientCount.filter.Allows(def))
				{
					return true;
				}
			}
			return this.recipe.fixedIngredientFilter.Allows(def) && this.ingredientFilter.Allows(def);
		}

		// Token: 0x06004A17 RID: 18967 RVA: 0x001CCAA8 File Offset: 0x001CACA8
		public static void CreateNoPawnsWithSkillDialog(RecipeDef recipe)
		{
			string text = "RecipeRequiresSkills".Translate(recipe.LabelCap);
			text += "\n\n";
			text += recipe.MinSkillString;
			Find.WindowStack.Add(new Dialog_MessageBox(text, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
		}

		// Token: 0x06004A18 RID: 18968 RVA: 0x001CCB07 File Offset: 0x001CAD07
		public virtual BillStoreModeDef GetStoreMode()
		{
			return BillStoreModeDefOf.BestStockpile;
		}

		// Token: 0x06004A19 RID: 18969 RVA: 0x001CCB0E File Offset: 0x001CAD0E
		public virtual ISlotGroup GetSlotGroup()
		{
			return null;
		}

		// Token: 0x06004A1A RID: 18970 RVA: 0x001CCB11 File Offset: 0x001CAD11
		public virtual void SetStoreMode(BillStoreModeDef mode, ISlotGroup group = null)
		{
			Log.ErrorOnce("Tried to set store mode of a non-production bill", 27190980);
		}

		// Token: 0x06004A1B RID: 18971 RVA: 0x001CCB22 File Offset: 0x001CAD22
		public virtual float GetWorkAmount(Thing thing = null)
		{
			return this.recipe.WorkAmountTotal(thing);
		}

		// Token: 0x06004A1C RID: 18972 RVA: 0x001CCB30 File Offset: 0x001CAD30
		public virtual Bill Clone()
		{
			Bill bill = (Bill)Activator.CreateInstance(base.GetType());
			bill.recipe = this.recipe;
			bill.precept = this.precept;
			bill.style = this.style;
			bill.globalStyle = this.globalStyle;
			bill.suspended = this.suspended;
			bill.ingredientFilter = new ThingFilter();
			bill.ingredientFilter.CopyAllowancesFrom(this.ingredientFilter);
			bill.ingredientSearchRadius = this.ingredientSearchRadius;
			bill.allowedSkillRange = this.allowedSkillRange;
			bill.pawnRestriction = this.pawnRestriction;
			bill.slavesOnly = this.slavesOnly;
			bill.xenogerm = this.xenogerm;
			bill.mechsOnly = this.mechsOnly;
			bill.nonMechsOnly = this.nonMechsOnly;
			return bill;
		}

		// Token: 0x06004A1D RID: 18973 RVA: 0x001CCBFC File Offset: 0x001CADFC
		public virtual void ValidateSettings()
		{
			if (this.pawnRestriction != null && (this.pawnRestriction.Dead || this.pawnRestriction.Faction != Faction.OfPlayer || this.pawnRestriction.IsKidnapped()))
			{
				if (this != BillUtility.Clipboard)
				{
					Messages.Message("MessageBillValidationPawnUnavailable".Translate(this.pawnRestriction.LabelShortCap, this.Label, this.billStack.billGiver.LabelShort), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
				}
				this.pawnRestriction = null;
			}
		}

		// Token: 0x06004A1E RID: 18974 RVA: 0x001CCCAE File Offset: 0x001CAEAE
		public string GetUniqueLoadID()
		{
			return "Bill_" + this.recipe.defName + "_" + this.loadID.ToString();
		}

		// Token: 0x06004A1F RID: 18975 RVA: 0x001CCCD5 File Offset: 0x001CAED5
		public override string ToString()
		{
			return this.GetUniqueLoadID();
		}

		// Token: 0x04002F5F RID: 12127
		[Unsaved(false)]
		public BillStack billStack;

		// Token: 0x04002F60 RID: 12128
		private int loadID = -1;

		// Token: 0x04002F61 RID: 12129
		public RecipeDef recipe;

		// Token: 0x04002F62 RID: 12130
		public Precept_ThingStyle precept;

		// Token: 0x04002F63 RID: 12131
		public ThingStyleDef style;

		// Token: 0x04002F64 RID: 12132
		public bool globalStyle = true;

		// Token: 0x04002F65 RID: 12133
		public int? graphicIndexOverride;

		// Token: 0x04002F66 RID: 12134
		public Xenogerm xenogerm;

		// Token: 0x04002F67 RID: 12135
		public bool suspended;

		// Token: 0x04002F68 RID: 12136
		public ThingFilter ingredientFilter;

		// Token: 0x04002F69 RID: 12137
		public float ingredientSearchRadius = 999f;

		// Token: 0x04002F6A RID: 12138
		public IntRange allowedSkillRange = new IntRange(0, 20);

		// Token: 0x04002F6B RID: 12139
		private Pawn pawnRestriction;

		// Token: 0x04002F6C RID: 12140
		private bool slavesOnly;

		// Token: 0x04002F6D RID: 12141
		private bool mechsOnly;

		// Token: 0x04002F6E RID: 12142
		private bool nonMechsOnly;

		// Token: 0x04002F6F RID: 12143
		public bool deleted;

		// Token: 0x04002F70 RID: 12144
		public int nextTickToSearchForIngredients;

		// Token: 0x04002F71 RID: 12145
		public const int MaxIngredientSearchRadius = 999;

		// Token: 0x04002F72 RID: 12146
		public const float ButSize = 24f;

		// Token: 0x04002F73 RID: 12147
		private const float InterfaceBaseHeight = 53f;

		// Token: 0x04002F74 RID: 12148
		private const float InterfaceStatusLineHeight = 17f;
	}
}
