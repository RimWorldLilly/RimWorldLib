using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001564 RID: 5476
	[StaticConstructorOnStartup]
	public abstract class Need : IExposable
	{
		// Token: 0x170016BB RID: 5819
		// (get) Token: 0x0600864A RID: 34378 RVA: 0x00308E05 File Offset: 0x00307005
		public string LabelCap
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x170016BC RID: 5820
		// (get) Token: 0x0600864B RID: 34379 RVA: 0x00308E17 File Offset: 0x00307017
		public float CurInstantLevelPercentage
		{
			get
			{
				return this.CurInstantLevel / this.MaxLevel;
			}
		}

		// Token: 0x170016BD RID: 5821
		// (get) Token: 0x0600864C RID: 34380 RVA: 0x00308E26 File Offset: 0x00307026
		public virtual int GUIChangeArrow
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170016BE RID: 5822
		// (get) Token: 0x0600864D RID: 34381 RVA: 0x00308E29 File Offset: 0x00307029
		public virtual float CurInstantLevel
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170016BF RID: 5823
		// (get) Token: 0x0600864E RID: 34382 RVA: 0x00308E30 File Offset: 0x00307030
		public virtual float MaxLevel
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170016C0 RID: 5824
		// (get) Token: 0x0600864F RID: 34383 RVA: 0x00308E37 File Offset: 0x00307037
		// (set) Token: 0x06008650 RID: 34384 RVA: 0x00308E3F File Offset: 0x0030703F
		public virtual float CurLevel
		{
			get
			{
				return this.curLevelInt;
			}
			set
			{
				this.curLevelInt = Mathf.Clamp(value, 0f, this.MaxLevel);
			}
		}

		// Token: 0x170016C1 RID: 5825
		// (get) Token: 0x06008651 RID: 34385 RVA: 0x00308E58 File Offset: 0x00307058
		// (set) Token: 0x06008652 RID: 34386 RVA: 0x00308E67 File Offset: 0x00307067
		public float CurLevelPercentage
		{
			get
			{
				return this.CurLevel / this.MaxLevel;
			}
			set
			{
				this.CurLevel = value * this.MaxLevel;
			}
		}

		// Token: 0x170016C2 RID: 5826
		// (get) Token: 0x06008653 RID: 34387 RVA: 0x00308E78 File Offset: 0x00307078
		protected virtual bool IsFrozen
		{
			get
			{
				return this.pawn.Suspended || (this.def.freezeWhileSleeping && !this.pawn.Awake()) || (this.def.freezeInMentalState && this.pawn.InMentalState) || this.NeedFrozenFromDormanancy() || !this.IsPawnInteractableOrVisible;
			}
		}

		// Token: 0x170016C3 RID: 5827
		// (get) Token: 0x06008654 RID: 34388 RVA: 0x00308EDF File Offset: 0x003070DF
		private bool IsPawnInteractableOrVisible
		{
			get
			{
				return this.pawn.SpawnedOrAnyParentSpawned || this.pawn.IsCaravanMember() || PawnUtility.IsTravelingInTransportPodWorldObject(this.pawn);
			}
		}

		// Token: 0x06008655 RID: 34389 RVA: 0x00308F0F File Offset: 0x0030710F
		private bool NeedFrozenFromDormanancy()
		{
			return this.intDormant != null && !this.intDormant.Awake && this.intDormant.Props.freezeNeeds.Contains(this.def);
		}

		// Token: 0x170016C4 RID: 5828
		// (get) Token: 0x06008656 RID: 34390 RVA: 0x00308F45 File Offset: 0x00307145
		public virtual bool ShowOnNeedList
		{
			get
			{
				return this.def.showOnNeedList;
			}
		}

		// Token: 0x06008657 RID: 34391 RVA: 0x00308F52 File Offset: 0x00307152
		public Need(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.SetInitialLevel();
			this.intDormant = this.pawn.TryGetComp<CompCanBeDormant>();
		}

		// Token: 0x06008658 RID: 34392 RVA: 0x00308F78 File Offset: 0x00307178
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<NeedDef>(ref this.def, "def");
			Scribe_Values.Look<float>(ref this.curLevelInt, "curLevel", 0f, false);
		}

		// Token: 0x06008659 RID: 34393
		public abstract void NeedInterval();

		// Token: 0x0600865A RID: 34394 RVA: 0x00308FA0 File Offset: 0x003071A0
		public virtual string GetTipString()
		{
			string text = (this.LabelCap + ": " + this.CurLevelPercentage.ToStringPercent()).Colorize(ColoredText.TipSectionTitleColor) + "\n" + this.def.description;
			Pawn_StoryTracker story = this.pawn.story;
			Trait trait;
			Gene gene;
			Hediff hediff;
			if (((story != null) ? story.traits : null) != null && this.pawn.story.traits.TryGetNeedEnablingTrait(this.def, out trait))
			{
				string str = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
				defaultInterpolatedStringHandler.AppendLiteral("\n\n");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("ComesFromTrait".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(trait.LabelCap);
				text = str + defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else if (this.pawn.genes != null && this.pawn.genes.TryGetNeedEnablingGene(this.def, out gene))
			{
				string str2 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(4, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("\n\n");
				defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("ComesFromGene".Translate());
				defaultInterpolatedStringHandler2.AppendLiteral(": ");
				defaultInterpolatedStringHandler2.AppendFormatted(gene.LabelCap);
				text = str2 + defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			else if (this.pawn.Ideo != null && this.pawn.Ideo.EnablesNeed(this.def))
			{
				string str3 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(4, 2);
				defaultInterpolatedStringHandler3.AppendLiteral("\n\n");
				defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("ComesFromIdeo".Translate());
				defaultInterpolatedStringHandler3.AppendLiteral(": ");
				defaultInterpolatedStringHandler3.AppendFormatted(this.pawn.Ideo.name.CapitalizeFirst());
				text = str3 + defaultInterpolatedStringHandler3.ToStringAndClear();
			}
			else if (this.pawn.health != null && this.pawn.health.hediffSet.TryGetNeedEnablingHediff(this.def, out hediff))
			{
				string str4 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(4, 2);
				defaultInterpolatedStringHandler4.AppendLiteral("\n\n");
				defaultInterpolatedStringHandler4.AppendFormatted<TaggedString>("ComesFromHediff".Translate());
				defaultInterpolatedStringHandler4.AppendLiteral(": ");
				defaultInterpolatedStringHandler4.AppendFormatted(hediff.LabelCap);
				text = str4 + defaultInterpolatedStringHandler4.ToStringAndClear();
			}
			return text;
		}

		// Token: 0x0600865B RID: 34395 RVA: 0x003091E9 File Offset: 0x003073E9
		public virtual void SetInitialLevel()
		{
			this.CurLevelPercentage = 0.5f;
		}

		// Token: 0x0600865C RID: 34396 RVA: 0x003091F6 File Offset: 0x003073F6
		public virtual void OnNeedRemoved()
		{
		}

		// Token: 0x0600865D RID: 34397 RVA: 0x003091F8 File Offset: 0x003073F8
		protected virtual void OffsetDebugPercent(float offsetPercent)
		{
			this.CurLevelPercentage += offsetPercent;
		}

		// Token: 0x0600865E RID: 34398 RVA: 0x00309208 File Offset: 0x00307408
		public virtual void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true, Rect? rectForTooltip = null, bool drawLabel = true)
		{
			if (rect.height > 70f)
			{
				float num = (rect.height - 70f) / 2f;
				rect.height = 70f;
				rect.y += num;
			}
			Rect valueOrDefault = rectForTooltip.GetValueOrDefault(rect);
			if (Mouse.IsOver(valueOrDefault))
			{
				Widgets.DrawHighlight(valueOrDefault);
			}
			if (doTooltip && Mouse.IsOver(valueOrDefault))
			{
				TooltipHandler.TipRegion(valueOrDefault, new TipSignal(() => this.GetTipString(), valueOrDefault.GetHashCode()));
			}
			float num2 = 14f;
			float num3 = (customMargin >= 0f) ? customMargin : (num2 + 15f);
			if (rect.height < 50f)
			{
				num2 *= Mathf.InverseLerp(0f, 50f, rect.height);
			}
			if (drawLabel)
			{
				Text.Font = ((rect.height > 55f) ? GameFont.Small : GameFont.Tiny);
				Text.Anchor = TextAnchor.LowerLeft;
				Widgets.Label(new Rect(rect.x + num3 + rect.width * 0.1f, rect.y, rect.width - num3 - rect.width * 0.1f, rect.height / 2f), this.LabelCap);
				Text.Anchor = TextAnchor.UpperLeft;
			}
			Rect rect2 = rect;
			if (drawLabel)
			{
				rect2.y += rect.height / 2f;
				rect2.height -= rect.height / 2f;
			}
			rect2 = new Rect(rect2.x + num3, rect2.y, rect2.width - num3 * 2f, rect2.height - num2);
			if (DebugSettings.ShowDevGizmos)
			{
				float lineHeight = Text.LineHeight;
				Rect rect3 = new Rect(rect2.xMax - lineHeight, rect2.y - lineHeight, lineHeight, lineHeight);
				if (Widgets.ButtonImage(rect3.ContractedBy(4f), TexButton.Plus, true, null))
				{
					this.OffsetDebugPercent(0.1f);
				}
				if (Mouse.IsOver(rect3))
				{
					TooltipHandler.TipRegion(rect3, "+ 10%");
				}
				Rect rect4 = new Rect(rect3.xMin - lineHeight, rect2.y - lineHeight, lineHeight, lineHeight);
				if (Widgets.ButtonImage(rect4.ContractedBy(4f), TexButton.Minus, true, null))
				{
					this.OffsetDebugPercent(-0.1f);
				}
				if (Mouse.IsOver(rect4))
				{
					TooltipHandler.TipRegion(rect4, "- 10%");
				}
			}
			Rect rect5 = rect2;
			float num4 = 1f;
			if (this.def.scaleBar && this.MaxLevel < 1f)
			{
				num4 = this.MaxLevel;
			}
			rect5.width *= num4;
			Rect barRect = Widgets.FillableBar(rect5, this.CurLevelPercentage);
			if (drawArrows)
			{
				Widgets.FillableBarChangeArrows(rect5, this.GUIChangeArrow);
			}
			if (this.threshPercents != null)
			{
				for (int i = 0; i < Mathf.Min(this.threshPercents.Count, maxThresholdMarkers); i++)
				{
					this.DrawBarThreshold(barRect, this.threshPercents[i] * num4);
				}
			}
			if (this.def.showUnitTicks)
			{
				int num5 = 1;
				while ((float)num5 < this.MaxLevel)
				{
					this.DrawBarDivision(barRect, (float)num5 / this.MaxLevel * num4);
					num5++;
				}
			}
			float curInstantLevelPercentage = this.CurInstantLevelPercentage;
			if (curInstantLevelPercentage >= 0f)
			{
				this.DrawBarInstantMarkerAt(rect2, curInstantLevelPercentage * num4);
			}
			if (!this.def.tutorHighlightTag.NullOrEmpty())
			{
				UIHighlighter.HighlightOpportunity(rect, this.def.tutorHighlightTag);
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x0600865F RID: 34399 RVA: 0x003095A8 File Offset: 0x003077A8
		protected void DrawBarInstantMarkerAt(Rect barRect, float pct)
		{
			if (pct > 1f)
			{
				NeedDef needDef = this.def;
				Log.ErrorOnce(((needDef != null) ? needDef.ToString() : null) + " drawing bar percent > 1 : " + pct.ToString(), 6932178);
			}
			float num = 12f;
			if (barRect.width < 150f)
			{
				num /= 2f;
			}
			Vector2 vector = new Vector2(barRect.x + barRect.width * pct, barRect.y + barRect.height);
			GUI.DrawTexture(new Rect(vector.x - num / 2f, vector.y, num, num), Need.BarInstantMarkerTex);
		}

		// Token: 0x06008660 RID: 34400 RVA: 0x00309654 File Offset: 0x00307854
		protected void DrawBarThreshold(Rect barRect, float threshPct)
		{
			float num = (float)((barRect.width > 60f) ? 2 : 1);
			Rect position = new Rect(barRect.x + barRect.width * threshPct - (num - 1f), barRect.y + barRect.height / 2f, num, barRect.height / 2f);
			Texture2D image;
			if (threshPct < this.CurLevelPercentage)
			{
				image = BaseContent.BlackTex;
				GUI.color = new Color(1f, 1f, 1f, 0.9f);
			}
			else
			{
				image = BaseContent.GreyTex;
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
			}
			GUI.DrawTexture(position, image);
			GUI.color = Color.white;
		}

		// Token: 0x06008661 RID: 34401 RVA: 0x0030971C File Offset: 0x0030791C
		private void DrawBarDivision(Rect barRect, float threshPct)
		{
			float num = 5f;
			Rect rect = new Rect(barRect.x + barRect.width * threshPct - (num - 1f), barRect.y, num, barRect.height);
			if (threshPct < this.CurLevelPercentage)
			{
				GUI.color = new Color(0f, 0f, 0f, 0.9f);
			}
			else
			{
				GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
			}
			Rect position = rect;
			position.yMax = position.yMin + 4f;
			GUI.DrawTextureWithTexCoords(position, Need.NeedUnitDividerTex, new Rect(0f, 0.5f, 1f, 0.5f));
			Rect position2 = rect;
			position2.yMin = position2.yMax - 4f;
			GUI.DrawTextureWithTexCoords(position2, Need.NeedUnitDividerTex, new Rect(0f, 0f, 1f, 0.5f));
			Rect position3 = rect;
			position3.yMin = position.yMax;
			position3.yMax = position2.yMin;
			if (position3.height > 0f)
			{
				GUI.DrawTextureWithTexCoords(position3, Need.NeedUnitDividerTex, new Rect(0f, 0.4f, 1f, 0.2f));
			}
			GUI.color = Color.white;
		}

		// Token: 0x040052AA RID: 21162
		public NeedDef def;

		// Token: 0x040052AB RID: 21163
		protected readonly Pawn pawn;

		// Token: 0x040052AC RID: 21164
		protected float curLevelInt;

		// Token: 0x040052AD RID: 21165
		protected List<float> threshPercents;

		// Token: 0x040052AE RID: 21166
		private CompCanBeDormant intDormant;

		// Token: 0x040052AF RID: 21167
		public const float MaxDrawHeight = 70f;

		// Token: 0x040052B0 RID: 21168
		private static readonly Texture2D BarInstantMarkerTex = ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarker", true);

		// Token: 0x040052B1 RID: 21169
		private static readonly Texture2D NeedUnitDividerTex = ContentFinder<Texture2D>.Get("UI/Misc/NeedUnitDivider", true);

		// Token: 0x040052B2 RID: 21170
		private const float BarInstantMarkerSize = 12f;
	}
}
