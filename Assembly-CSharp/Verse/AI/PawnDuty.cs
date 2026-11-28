using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008AE RID: 2222
	public class PawnDuty : IExposable
	{
		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x060044E4 RID: 17636 RVA: 0x001B0620 File Offset: 0x001AE820
		public RandomSocialMode SocialModeMax
		{
			get
			{
				RandomSocialMode? randomSocialMode = this.socialModeMaxOverride;
				if (randomSocialMode == null)
				{
					return this.def.socialModeMax;
				}
				return randomSocialMode.GetValueOrDefault();
			}
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x001B0650 File Offset: 0x001AE850
		public PawnDuty()
		{
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x001B06B8 File Offset: 0x001AE8B8
		public PawnDuty(DutyDef def)
		{
			this.def = def;
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x001B0725 File Offset: 0x001AE925
		public PawnDuty(DutyDef def, LocalTargetInfo focus, float radius = -1f) : this(def)
		{
			this.focus = focus;
			this.radius = radius;
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x001B073C File Offset: 0x001AE93C
		public PawnDuty(DutyDef def, LocalTargetInfo focus, LocalTargetInfo focusSecond, float radius = -1f) : this(def, focus, radius)
		{
			this.focusSecond = focusSecond;
		}

		// Token: 0x060044E9 RID: 17641 RVA: 0x001B074F File Offset: 0x001AE94F
		public PawnDuty(DutyDef def, LocalTargetInfo focus, LocalTargetInfo focusSecond, LocalTargetInfo focusThird, float radius = -1f) : this(def, focus, radius)
		{
			this.focusSecond = focusSecond;
			this.focusThird = focusThird;
		}

		// Token: 0x060044EA RID: 17642 RVA: 0x001B076C File Offset: 0x001AE96C
		public void ExposeData()
		{
			Scribe_Defs.Look<DutyDef>(ref this.def, "def");
			Scribe_TargetInfo.Look(ref this.focus, true, "focus", LocalTargetInfo.Invalid, false);
			Scribe_TargetInfo.Look(ref this.focusSecond, true, "focusSecond", LocalTargetInfo.Invalid, false);
			Scribe_TargetInfo.Look(ref this.focusThird, true, "focusThird", LocalTargetInfo.Invalid, false);
			Scribe_Values.Look<float>(ref this.radius, "radius", -1f, false);
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.None, false);
			Scribe_Values.Look<Danger>(ref this.maxDanger, "maxDanger", Danger.Unspecified, false);
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.All, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<int>(ref this.transportersGroup, "transportersGroup", -1, false);
			Scribe_Values.Look<bool>(ref this.attackDownedIfStarving, "attackDownedIfStarving", false, false);
			Scribe_Values.Look<float?>(ref this.wanderRadius, "wanderRadius", null, false);
			Scribe_Values.Look<IntRange>(ref this.spectateDistance, "spectateDistance", default(IntRange), false);
			Scribe_Values.Look<int?>(ref this.ropeeLimit, "ropeeLimit", null, false);
			Scribe_Values.Look<bool>(ref this.pickupOpportunisticWeapon, "pickupOpportunisticWeapon", false, false);
			Scribe_Values.Look<Rot4>(ref this.overrideFacing, "overrideFacing", Rot4.Invalid, false);
			Scribe_References.Look<ILoadReferenceable>(ref this.source, "source", false);
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
			Scribe_Values.Look<RandomSocialMode?>(ref this.socialModeMaxOverride, "socialModeMaxOverride", null, false);
		}

		// Token: 0x060044EB RID: 17643 RVA: 0x001B0920 File Offset: 0x001AEB20
		public override string ToString()
		{
			string text = this.focus.IsValid ? this.focus.ToString() : "";
			string text2 = this.focusSecond.IsValid ? (", second=" + this.focusSecond.ToString()) : "";
			string text3 = this.focusThird.IsValid ? (", third=" + this.focusThird.ToString()) : "";
			string text4 = (this.radius > 0f) ? (", rad=" + this.radius.ToString("F2")) : "";
			string[] array = new string[8];
			array[0] = "(";
			int num = 1;
			DutyDef dutyDef = this.def;
			array[num] = ((dutyDef != null) ? dutyDef.ToString() : null);
			array[2] = " ";
			array[3] = text;
			array[4] = text2;
			array[5] = text3;
			array[6] = text4;
			array[7] = ")";
			return string.Concat(array);
		}

		// Token: 0x060044EC RID: 17644 RVA: 0x001B0A2C File Offset: 0x001AEC2C
		internal void DrawDebug(Pawn pawn)
		{
			if (this.focus.IsValid)
			{
				GenDraw.DrawLineBetween(pawn.DrawPos, this.focus.Cell.ToVector3Shifted());
				if (this.radius > 0f)
				{
					GenDraw.DrawRadiusRing(this.focus.Cell, this.radius);
				}
			}
		}

		// Token: 0x04002CAF RID: 11439
		public DutyDef def;

		// Token: 0x04002CB0 RID: 11440
		public LocalTargetInfo focus = LocalTargetInfo.Invalid;

		// Token: 0x04002CB1 RID: 11441
		public LocalTargetInfo focusSecond = LocalTargetInfo.Invalid;

		// Token: 0x04002CB2 RID: 11442
		public LocalTargetInfo focusThird = LocalTargetInfo.Invalid;

		// Token: 0x04002CB3 RID: 11443
		public float radius = -1f;

		// Token: 0x04002CB4 RID: 11444
		public LocomotionUrgency locomotion;

		// Token: 0x04002CB5 RID: 11445
		public Danger maxDanger;

		// Token: 0x04002CB6 RID: 11446
		public CellRect spectateRect;

		// Token: 0x04002CB7 RID: 11447
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;

		// Token: 0x04002CB8 RID: 11448
		public SpectateRectSide spectateRectPreferredSide;

		// Token: 0x04002CB9 RID: 11449
		public IntRange spectateDistance = new IntRange(2, 5);

		// Token: 0x04002CBA RID: 11450
		public bool canDig;

		// Token: 0x04002CBB RID: 11451
		public int transportersGroup = -1;

		// Token: 0x04002CBC RID: 11452
		public bool attackDownedIfStarving;

		// Token: 0x04002CBD RID: 11453
		public float? wanderRadius;

		// Token: 0x04002CBE RID: 11454
		public int? ropeeLimit;

		// Token: 0x04002CBF RID: 11455
		public bool pickupOpportunisticWeapon;

		// Token: 0x04002CC0 RID: 11456
		public Rot4 overrideFacing = Rot4.Invalid;

		// Token: 0x04002CC1 RID: 11457
		public ILoadReferenceable source;

		// Token: 0x04002CC2 RID: 11458
		public string tag;

		// Token: 0x04002CC3 RID: 11459
		public RandomSocialMode? socialModeMaxOverride;
	}
}
