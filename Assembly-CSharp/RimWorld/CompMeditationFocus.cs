using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F8 RID: 6392
	public class CompMeditationFocus : CompStatOffsetBase
	{
		// Token: 0x17001B9E RID: 7070
		// (get) Token: 0x0600A039 RID: 41017 RVA: 0x0039615B File Offset: 0x0039435B
		public new CompProperties_MeditationFocus Props
		{
			get
			{
				return (CompProperties_MeditationFocus)this.props;
			}
		}

		// Token: 0x17001B9F RID: 7071
		// (get) Token: 0x0600A03A RID: 41018 RVA: 0x00396168 File Offset: 0x00394368
		public override Pawn LastUser
		{
			get
			{
				if (this.lastUsedTick < 0 || Find.TickManager.TicksGame - this.lastUsedTick > 5)
				{
					return null;
				}
				return base.LastUser;
			}
		}

		// Token: 0x0600A03B RID: 41019 RVA: 0x00396190 File Offset: 0x00394390
		public override float GetStatOffset(Pawn pawn = null)
		{
			if (!ModLister.CheckRoyalty("Meditation focus"))
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.Props.offsets.Count; i++)
			{
				if (this.Props.offsets[i].CanApply(this.parent, pawn))
				{
					num += this.Props.offsets[i].GetOffset(this.parent, pawn);
				}
			}
			return num;
		}

		// Token: 0x0600A03C RID: 41020 RVA: 0x00396210 File Offset: 0x00394410
		public override IEnumerable<string> GetExplanation()
		{
			CompMeditationFocus.<GetExplanation>d__6 <GetExplanation>d__ = new CompMeditationFocus.<GetExplanation>d__6(-2);
			<GetExplanation>d__.<>4__this = this;
			return <GetExplanation>d__;
		}

		// Token: 0x0600A03D RID: 41021 RVA: 0x00396220 File Offset: 0x00394420
		public bool CanPawnUse(Pawn pawn)
		{
			for (int i = 0; i < this.Props.focusTypes.Count; i++)
			{
				if (this.Props.focusTypes[i].CanPawnUse(pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A03E RID: 41022 RVA: 0x00396264 File Offset: 0x00394464
		public bool WillBeAffectedBy(ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			CellRect cellRect = GenAdj.OccupiedRect(pos, rotation, def.size);
			foreach (FocusStrengthOffset focusStrengthOffset in this.Props.offsets)
			{
				FocusStrengthOffset_ArtificialBuildings focusStrengthOffset_ArtificialBuildings = focusStrengthOffset as FocusStrengthOffset_ArtificialBuildings;
				if (focusStrengthOffset_ArtificialBuildings != null && MeditationUtility.CountsAsArtificialBuilding(def, faction) && cellRect.ClosestCellTo(this.parent.Position).DistanceTo(this.parent.Position) <= focusStrengthOffset_ArtificialBuildings.radius)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A03F RID: 41023 RVA: 0x00396308 File Offset: 0x00394508
		public override void Used(Pawn pawn)
		{
			base.Used(pawn);
			this.lastUsedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600A040 RID: 41024 RVA: 0x00396324 File Offset: 0x00394524
		public override string CompInspectStringExtra()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (this.LastUser != null)
			{
				stringBuilder.Append("UserMeditationFocusStrength".Translate(this.LastUser.Named("LASTUSER")) + ": " + this.Props.statDef.ValueToString(this.parent.GetStatValueForPawn(this.Props.statDef, this.LastUser, true), ToStringNumberSense.Absolute, true));
			}
			for (int i = 0; i < this.Props.offsets.Count; i++)
			{
				string text = this.Props.offsets[i].InspectStringExtra(this.parent, null);
				if (!text.NullOrEmpty())
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600A041 RID: 41025 RVA: 0x0039640B File Offset: 0x0039460B
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompMeditationFocus.<SpecialDisplayStats>d__11 <SpecialDisplayStats>d__ = new CompMeditationFocus.<SpecialDisplayStats>d__11(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600A042 RID: 41026 RVA: 0x0039641C File Offset: 0x0039461C
		public override void PostDrawExtraSelectionOverlays()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			base.PostDrawExtraSelectionOverlays();
			if (ModsConfig.RoyaltyActive)
			{
				for (int i = 0; i < this.Props.offsets.Count; i++)
				{
					this.Props.offsets[i].PostDrawExtraSelectionOverlays(this.parent, this.LastUser);
				}
			}
		}

		// Token: 0x0600A043 RID: 41027 RVA: 0x00396481 File Offset: 0x00394681
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.lastUsedTick, "lastUsedTick", -1, false);
		}

		// Token: 0x04005EC5 RID: 24261
		private int lastUsedTick = -1;
	}
}
