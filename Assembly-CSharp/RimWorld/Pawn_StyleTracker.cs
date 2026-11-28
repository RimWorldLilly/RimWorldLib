using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015A8 RID: 5544
	public class Pawn_StyleTracker : IExposable
	{
		// Token: 0x170017A0 RID: 6048
		// (get) Token: 0x06008939 RID: 35129 RVA: 0x0031B48B File Offset: 0x0031968B
		// (set) Token: 0x0600893A RID: 35130 RVA: 0x0031B493 File Offset: 0x00319693
		public TattooDef FaceTattoo
		{
			get
			{
				return this.faceTattoo;
			}
			set
			{
				if (!ModLister.CheckIdeology("Tattoos"))
				{
					return;
				}
				this.faceTattoo = value;
			}
		}

		// Token: 0x170017A1 RID: 6049
		// (get) Token: 0x0600893B RID: 35131 RVA: 0x0031B4A9 File Offset: 0x003196A9
		// (set) Token: 0x0600893C RID: 35132 RVA: 0x0031B4B1 File Offset: 0x003196B1
		public TattooDef BodyTattoo
		{
			get
			{
				return this.bodyTattoo;
			}
			set
			{
				if (!ModLister.CheckIdeology("Tattoos"))
				{
					return;
				}
				this.bodyTattoo = value;
			}
		}

		// Token: 0x170017A2 RID: 6050
		// (get) Token: 0x0600893D RID: 35133 RVA: 0x0031B4C7 File Offset: 0x003196C7
		public bool ShouldSpawnHairFilth
		{
			get
			{
				return (this.nextHairDef != null || this.nextBeardDef != null) && (this.pawn.story.hairDef != this.nextHairDef || this.beardDef != this.nextBeardDef);
			}
		}

		// Token: 0x170017A3 RID: 6051
		// (get) Token: 0x0600893E RID: 35134 RVA: 0x0031B508 File Offset: 0x00319708
		public bool CanWantBeard
		{
			get
			{
				return (this.pawn.gender != Gender.Female || (this.pawn.genes != null && this.pawn.genes.CanHaveBeard)) && this.pawn.DevelopmentalStage.Adult();
			}
		}

		// Token: 0x170017A4 RID: 6052
		// (get) Token: 0x0600893F RID: 35135 RVA: 0x0031B559 File Offset: 0x00319759
		public bool LookChangeDesired
		{
			get
			{
				return this.lookChangeDesired && this.CanDesireLookChange;
			}
		}

		// Token: 0x170017A5 RID: 6053
		// (get) Token: 0x06008940 RID: 35136 RVA: 0x0031B56C File Offset: 0x0031976C
		public bool CanDesireLookChange
		{
			get
			{
				return ModsConfig.IdeologyActive && this.pawn.IsColonistPlayerControlled && !this.pawn.guest.IsSlave && !this.pawn.IsQuestLodger() && !this.pawn.IsCreepJoiner;
			}
		}

		// Token: 0x170017A6 RID: 6054
		// (get) Token: 0x06008941 RID: 35137 RVA: 0x0031B5BC File Offset: 0x003197BC
		public bool HasAnyUnwantedStyleItem
		{
			get
			{
				return this.HasUnwantedHairStyle || this.HasUnwantedBeard || this.HasUnwantedFaceTattoo || this.HasUnwantedBodyTattoo;
			}
		}

		// Token: 0x170017A7 RID: 6055
		// (get) Token: 0x06008942 RID: 35138 RVA: 0x0031B5E0 File Offset: 0x003197E0
		public bool HasUnwantedHairStyle
		{
			get
			{
				return !PawnStyleItemChooser.WantsToUseStyle(this.pawn, this.pawn.story.hairDef, null);
			}
		}

		// Token: 0x170017A8 RID: 6056
		// (get) Token: 0x06008943 RID: 35139 RVA: 0x0031B614 File Offset: 0x00319814
		public bool HasUnwantedBeard
		{
			get
			{
				return !PawnStyleItemChooser.WantsToUseStyle(this.pawn, this.beardDef, null);
			}
		}

		// Token: 0x170017A9 RID: 6057
		// (get) Token: 0x06008944 RID: 35140 RVA: 0x0031B63E File Offset: 0x0031983E
		public bool HasUnwantedFaceTattoo
		{
			get
			{
				return !PawnStyleItemChooser.WantsToUseStyle(this.pawn, this.faceTattoo, new TattooType?(TattooType.Face));
			}
		}

		// Token: 0x170017AA RID: 6058
		// (get) Token: 0x06008945 RID: 35141 RVA: 0x0031B65A File Offset: 0x0031985A
		public bool HasUnwantedBodyTattoo
		{
			get
			{
				return !PawnStyleItemChooser.WantsToUseStyle(this.pawn, this.bodyTattoo, new TattooType?(TattooType.Body));
			}
		}

		// Token: 0x06008946 RID: 35142 RVA: 0x0031B676 File Offset: 0x00319876
		public Pawn_StyleTracker()
		{
		}

		// Token: 0x06008947 RID: 35143 RVA: 0x0031B689 File Offset: 0x00319889
		public Pawn_StyleTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06008948 RID: 35144 RVA: 0x0031B6A4 File Offset: 0x003198A4
		public void StyleTrackerTickInterval(int delta)
		{
			if (!this.lookChangeDesired && this.pawn.IsHashIntervalTick(2500, delta) && this.CanDesireLookChange && this.HasAnyUnwantedStyleItem && Rand.MTBEventOccurs(20f, 60000f, 2500f))
			{
				this.RequestLookChange();
			}
		}

		// Token: 0x06008949 RID: 35145 RVA: 0x0031B6F8 File Offset: 0x003198F8
		public void RequestLookChange()
		{
			Find.LetterStack.ReceiveLetter("LetterWantLookChange".Translate() + ": " + Find.ActiveLanguageWorker.WithDefiniteArticle(this.pawn.Name.ToStringShort, this.pawn.gender, false, true), "LetterWantLookChangeDesc".Translate(this.pawn.Named("PAWN")), LetterDefOf.NeutralEvent, new LookTargets(this.pawn), null, null, new List<ThingDef>
			{
				ThingDefOf.StylingStation
			}, null, 0, true);
			this.lookChangeDesired = true;
			this.ResetNextStyleChangeAttemptTick();
		}

		// Token: 0x0600894A RID: 35146 RVA: 0x0031B79C File Offset: 0x0031999C
		public void ResetNextStyleChangeAttemptTick()
		{
			this.nextStyleChangeAttemptTick = Find.TickManager.TicksGame + Pawn_StyleTracker.AutoStyleChangeTicksOffsetRange.RandomInRange;
		}

		// Token: 0x0600894B RID: 35147 RVA: 0x0031B7C8 File Offset: 0x003199C8
		public void Notify_StyleItemChanged()
		{
			if (!this.HasAnyUnwantedStyleItem)
			{
				this.lookChangeDesired = false;
			}
			this.nextHairDef = null;
			this.nextBeardDef = null;
			this.nextFaceTattooDef = null;
			this.nextBodyTatooDef = null;
			this.pawn.Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x0600894C RID: 35148 RVA: 0x0031B815 File Offset: 0x00319A15
		public void FinalizeHairColor()
		{
			if (this.nextHairColor != null)
			{
				this.pawn.story.HairColor = this.nextHairColor.Value;
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
		}

		// Token: 0x0600894D RID: 35149 RVA: 0x0031B854 File Offset: 0x00319A54
		public void MakeHairFilth()
		{
			foreach (IntVec3 c in GenRadial.RadialCellsAround(this.pawn.Position, 1f, true))
			{
				if (c.InBounds(this.pawn.Map) && Rand.Value < 0.5f)
				{
					FilthMaker.TryMakeFilth(c, this.pawn.Map, ThingDefOf.Filth_Hair, this.pawn.LabelIndefinite(), Rand.Range(1, 3), FilthSourceFlags.None);
				}
			}
		}

		// Token: 0x0600894E RID: 35150 RVA: 0x0031B8F4 File Offset: 0x00319AF4
		public void SetupNextLookChangeData(HairDef hair = null, BeardDef beard = null, TattooDef faceTatoo = null, TattooDef bodyTattoo = null, Color? hairColor = null)
		{
			this.nextHairDef = hair;
			this.nextBeardDef = beard;
			this.nextFaceTattooDef = faceTatoo;
			this.nextBodyTatooDef = bodyTattoo;
			if (hairColor != null)
			{
				if (hairColor.Value == this.pawn.story.HairColor)
				{
					this.nextHairColor = null;
					return;
				}
				this.nextHairColor = new Color?(hairColor.Value);
			}
		}

		// Token: 0x0600894F RID: 35151 RVA: 0x0031B964 File Offset: 0x00319B64
		public void SetupTattoos_NoIdeology()
		{
			this.faceTattoo = TattooDefOf.NoTattoo_Face;
			this.bodyTattoo = TattooDefOf.NoTattoo_Body;
		}

		// Token: 0x06008950 RID: 35152 RVA: 0x0031B97C File Offset: 0x00319B7C
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.lookChangeDesired, "lookChangeDesired", false, false);
			Scribe_Values.Look<int>(ref this.nextStyleChangeAttemptTick, "nextStyleChangeAttemptTick", -99999, false);
			Scribe_Values.Look<Color?>(ref this.nextHairColor, "nextHairColor", null, false);
			Scribe_Defs.Look<BeardDef>(ref this.beardDef, "beardDef");
			Scribe_Defs.Look<TattooDef>(ref this.faceTattoo, "faceTattoo");
			Scribe_Defs.Look<TattooDef>(ref this.bodyTattoo, "bodyTattoo");
			Scribe_Defs.Look<HairDef>(ref this.nextHairDef, "nextHairDef");
			Scribe_Defs.Look<BeardDef>(ref this.nextBeardDef, "nextBeardDef");
			Scribe_Defs.Look<TattooDef>(ref this.nextFaceTattooDef, "nextFaceTattooDef");
			Scribe_Defs.Look<TattooDef>(ref this.nextBodyTatooDef, "nextBodyTattooDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.beardDef == null)
				{
					this.beardDef = BeardDefOf.NoBeard;
				}
				if (!this.CanWantBeard && this.beardDef != BeardDefOf.NoBeard)
				{
					this.beardDef = BeardDefOf.NoBeard;
					Log.Error(this.pawn.LabelShort + " had a beard. Removed.");
				}
				if (this.faceTattoo == null)
				{
					this.faceTattoo = TattooDefOf.NoTattoo_Face;
				}
				if (this.bodyTattoo == null)
				{
					this.bodyTattoo = TattooDefOf.NoTattoo_Body;
				}
			}
		}

		// Token: 0x04005487 RID: 21639
		public Pawn pawn;

		// Token: 0x04005488 RID: 21640
		public BeardDef beardDef;

		// Token: 0x04005489 RID: 21641
		public int nextStyleChangeAttemptTick = -99999;

		// Token: 0x0400548A RID: 21642
		private TattooDef faceTattoo;

		// Token: 0x0400548B RID: 21643
		private TattooDef bodyTattoo;

		// Token: 0x0400548C RID: 21644
		private bool lookChangeDesired;

		// Token: 0x0400548D RID: 21645
		public HairDef nextHairDef;

		// Token: 0x0400548E RID: 21646
		public BeardDef nextBeardDef;

		// Token: 0x0400548F RID: 21647
		public TattooDef nextFaceTattooDef;

		// Token: 0x04005490 RID: 21648
		public TattooDef nextBodyTatooDef;

		// Token: 0x04005491 RID: 21649
		public Color? nextHairColor;

		// Token: 0x04005492 RID: 21650
		private static readonly IntRange AutoStyleChangeTicksOffsetRange = new IntRange(15000, 30000);

		// Token: 0x04005493 RID: 21651
		private const float StyleChangeMTBDays = 20f;

		// Token: 0x04005494 RID: 21652
		private const int LookChangeCheckInterval = 2500;
	}
}
