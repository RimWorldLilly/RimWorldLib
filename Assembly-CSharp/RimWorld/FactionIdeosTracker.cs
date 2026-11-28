using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001602 RID: 5634
	public class FactionIdeosTracker : IExposable
	{
		// Token: 0x1700184E RID: 6222
		// (get) Token: 0x06008C3C RID: 35900 RVA: 0x0032F834 File Offset: 0x0032DA34
		public List<Ideo> IdeosMinorListForReading
		{
			get
			{
				return this.ideosMinor;
			}
		}

		// Token: 0x1700184F RID: 6223
		// (get) Token: 0x06008C3D RID: 35901 RVA: 0x0032F83C File Offset: 0x0032DA3C
		public Ideo PrimaryIdeo
		{
			get
			{
				return this.primaryIdeo;
			}
		}

		// Token: 0x17001850 RID: 6224
		// (get) Token: 0x06008C3E RID: 35902 RVA: 0x0032F844 File Offset: 0x0032DA44
		public IEnumerable<Ideo> AllIdeos
		{
			get
			{
				FactionIdeosTracker.<get_AllIdeos>d__12 <get_AllIdeos>d__ = new FactionIdeosTracker.<get_AllIdeos>d__12(-2);
				<get_AllIdeos>d__.<>4__this = this;
				return <get_AllIdeos>d__;
			}
		}

		// Token: 0x17001851 RID: 6225
		// (get) Token: 0x06008C3F RID: 35903 RVA: 0x0032F854 File Offset: 0x0032DA54
		public CultureDef PrimaryCulture
		{
			get
			{
				if (this.PrimaryIdeo == null)
				{
					return null;
				}
				return this.PrimaryIdeo.culture;
			}
		}

		// Token: 0x17001852 RID: 6226
		// (get) Token: 0x06008C40 RID: 35904 RVA: 0x0032F86C File Offset: 0x0032DA6C
		public Ideo FluidIdeo
		{
			get
			{
				if (this.primaryIdeo != null && this.primaryIdeo.Fluid)
				{
					return this.primaryIdeo;
				}
				for (int i = 0; i < this.ideosMinor.Count; i++)
				{
					if (this.ideosMinor[i].Fluid)
					{
						return this.ideosMinor[i];
					}
				}
				return null;
			}
		}

		// Token: 0x06008C41 RID: 35905 RVA: 0x0032F8CC File Offset: 0x0032DACC
		public FactionIdeosTracker(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x06008C42 RID: 35906 RVA: 0x0032F8E6 File Offset: 0x0032DAE6
		public FactionIdeosTracker()
		{
		}

		// Token: 0x06008C43 RID: 35907 RVA: 0x0032F8F9 File Offset: 0x0032DAF9
		public bool Has(Ideo ideo)
		{
			return ideo == this.primaryIdeo || this.ideosMinor.Contains(ideo);
		}

		// Token: 0x06008C44 RID: 35908 RVA: 0x0032F914 File Offset: 0x0032DB14
		public bool HasAnyIdeoWithMeme(MemeDef meme)
		{
			if (this.primaryIdeo.memes.Contains(meme))
			{
				return true;
			}
			using (List<Ideo>.Enumerator enumerator = this.ideosMinor.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.memes.Contains(meme))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06008C45 RID: 35909 RVA: 0x0032F988 File Offset: 0x0032DB88
		public bool IsPrimary(Ideo ideo)
		{
			return ideo == this.primaryIdeo;
		}

		// Token: 0x06008C46 RID: 35910 RVA: 0x0032F993 File Offset: 0x0032DB93
		public bool IsMinor(Ideo ideo)
		{
			return this.ideosMinor.Contains(ideo);
		}

		// Token: 0x06008C47 RID: 35911 RVA: 0x0032F9A1 File Offset: 0x0032DBA1
		public void Notify_MemberGainedOrLost()
		{
			if (this.faction.IsPlayer)
			{
				this.RecalculateIdeosBasedOnPlayerPawns();
			}
		}

		// Token: 0x06008C48 RID: 35912 RVA: 0x0032F9B6 File Offset: 0x0032DBB6
		public void Notify_ColonistChangedIdeo()
		{
			this.RecalculateIdeosBasedOnPlayerPawns();
		}

		// Token: 0x06008C49 RID: 35913 RVA: 0x0032F9C0 File Offset: 0x0032DBC0
		public void RecalculateIdeosBasedOnPlayerPawns()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			if (Current.ProgramState != ProgramState.Playing || Find.WindowStack.IsOpen<Dialog_ConfigureIdeo>())
			{
				return;
			}
			this.ideosMinor.Clear();
			List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonists = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists;
			FactionIdeosTracker.tmpPlayerIdeos.Clear();
			for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Count; i++)
			{
				if (allMapsCaravansAndTravellingTransporters_Alive_FreeColonists[i].HomeFaction == Faction.OfPlayer)
				{
					Ideo ideo = allMapsCaravansAndTravellingTransporters_Alive_FreeColonists[i].Ideo;
					if (ideo != null)
					{
						FactionIdeosTracker.tmpPlayerIdeos.Increment(ideo);
					}
				}
			}
			int num = 0;
			Ideo ideo2 = null;
			foreach (KeyValuePair<Ideo, int> keyValuePair in FactionIdeosTracker.tmpPlayerIdeos)
			{
				if (keyValuePair.Value > num)
				{
					num = keyValuePair.Value;
					ideo2 = keyValuePair.Key;
				}
			}
			if (ideo2 != null && num > FactionIdeosTracker.tmpPlayerIdeos.TryGetValue(this.primaryIdeo, 0))
			{
				if (this.primaryIdeo != null)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelNewPrimaryIdeo".Translate(this.primaryIdeo.Named("OLDIDEO"), ideo2.Named("NEWIDEO")), "LetterNewPrimaryIdeo".Translate(this.primaryIdeo.Named("OLDIDEO"), ideo2.Named("NEWIDEO"), Faction.OfPlayer.Named("FACTION")), LetterDefOf.NeutralEvent, null, 0, true);
					this.primaryIdeo.Notify_NotPrimaryAnymore(ideo2);
				}
				if (this.primaryIdeo != null && ideo2.ColonistBelieverCountCached < Ideo.MinBelieversToEnableObligations)
				{
					Find.LetterStack.ReceiveLetter("LetterTitleObligationsActivated".Translate(ideo2), "LetterTitleObligationsActivatedIdeoBecameMajor".Translate(ideo2.Named("IDEO")), LetterDefOf.NeutralEvent, null, 0, true);
				}
				this.primaryIdeo = ideo2;
			}
			foreach (KeyValuePair<Ideo, int> keyValuePair2 in FactionIdeosTracker.tmpPlayerIdeos)
			{
				if (keyValuePair2.Key != this.primaryIdeo && !this.ideosMinor.Contains(keyValuePair2.Key))
				{
					this.ideosMinor.Add(keyValuePair2.Key);
				}
			}
			FactionIdeosTracker.tmpPlayerIdeos.Clear();
		}

		// Token: 0x06008C4A RID: 35914 RVA: 0x0032FC10 File Offset: 0x0032DE10
		public void SetPrimary(Ideo ideo)
		{
			this.primaryIdeo = ideo;
		}

		// Token: 0x06008C4B RID: 35915 RVA: 0x0032FC1C File Offset: 0x0032DE1C
		public void ChooseOrGenerateIdeo(IdeoGenerationParms parms)
		{
			this.primaryIdeo = null;
			this.ideosMinor.Clear();
			if (!ModsConfig.IdeologyActive || parms.forceNoExpansionIdeo)
			{
				Ideo ideo = IdeoGenerator.GenerateNoExpansionIdeo((!this.faction.def.allowedCultures.NullOrEmpty<CultureDef>()) ? this.faction.def.allowedCultures[0] : DefDatabase<CultureDef>.AllDefs.RandomElement<CultureDef>(), parms);
				this.primaryIdeo = ideo;
				Find.IdeoManager.Add(ideo);
				return;
			}
			if (parms.fixedIdeo)
			{
				Ideo ideo2 = IdeoGenerator.MakeFixedIdeo(parms);
				ideo2.primaryFactionColor = new Color?(this.faction.Color);
				this.primaryIdeo = ideo2;
				Find.IdeoManager.Add(ideo2);
				return;
			}
			if (Find.IdeoManager.classicMode && Faction.OfPlayer != null && Faction.OfPlayer.ideos.PrimaryIdeo != null)
			{
				this.primaryIdeo = Faction.OfPlayer.ideos.PrimaryIdeo;
				return;
			}
			if (!Rand.Chance(0.2f))
			{
				if (Find.IdeoManager.IdeosListForReading.Count((Ideo i) => !i.solid) < 10 && !this.faction.def.hidden)
				{
					goto IL_18C;
				}
			}
			Ideo ideo3;
			if ((from x in Find.IdeoManager.IdeosListForReading
			where IdeoUtility.CanUseIdeo(this.faction.def, x, parms) && !x.solid
			select x).TryRandomElement(out ideo3))
			{
				this.primaryIdeo = ideo3;
				return;
			}
			IL_18C:
			Ideo ideo4 = IdeoGenerator.GenerateIdeo(parms);
			ideo4.primaryFactionColor = new Color?(this.faction.Color);
			this.primaryIdeo = ideo4;
			Find.IdeoManager.Add(ideo4);
		}

		// Token: 0x06008C4C RID: 35916 RVA: 0x0032FDEE File Offset: 0x0032DFEE
		public Ideo GetRandomIdeoForNewPawn()
		{
			return this.AllIdeos.RandomElementByWeightWithFallback(delegate(Ideo x)
			{
				if (!this.IsPrimary(x))
				{
					return 1f;
				}
				return 4f;
			}, null);
		}

		// Token: 0x06008C4D RID: 35917 RVA: 0x0032FE08 File Offset: 0x0032E008
		public Precept GetPrecept(PreceptDef precept)
		{
			foreach (Ideo ideo in this.AllIdeos)
			{
				foreach (Precept precept2 in ideo.PreceptsListForReading)
				{
					if (precept2.def == precept)
					{
						return precept2;
					}
				}
			}
			return null;
		}

		// Token: 0x06008C4E RID: 35918 RVA: 0x0032FE98 File Offset: 0x0032E098
		public bool AnyPreceptWithRequiredScars()
		{
			using (IEnumerator<Ideo> enumerator = this.AllIdeos.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.RequiredScars > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06008C4F RID: 35919 RVA: 0x0032FEEC File Offset: 0x0032E0EC
		public static IdeoGenerationParms IdeoGenerationParmsForFaction_BackCompatibility(FactionDef factionDef, bool forceNoExpansion = false)
		{
			IdeoGenerationParms result = new IdeoGenerationParms(factionDef, forceNoExpansion, null, null, null, false, false, false, false, "", null, null, false, "", false);
			if (factionDef.isPlayer)
			{
				result.disallowedPrecepts = (from x in DefDatabase<PreceptDef>.AllDefs
				where x.impact == PreceptImpact.High
				select x).ToList<PreceptDef>();
				result.disallowedMemes = (from x in DefDatabase<MemeDef>.AllDefs
				where !x.allowDuringTutorial
				select x).ToList<MemeDef>();
			}
			return result;
		}

		// Token: 0x06008C50 RID: 35920 RVA: 0x0032FF8A File Offset: 0x0032E18A
		public void RemoveAll()
		{
			this.primaryIdeo = null;
			this.ideosMinor.Clear();
		}

		// Token: 0x06008C51 RID: 35921 RVA: 0x0032FFA0 File Offset: 0x0032E1A0
		public void ExposeData()
		{
			Scribe_References.Look<Ideo>(ref this.primaryIdeo, "primaryIdeo", false);
			Scribe_Collections.Look<Ideo>(ref this.ideosMinor, "ideosMinor", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.ideosMinor.RemoveAll((Ideo x) => x == null) != 0)
				{
					Log.Error("Some ideoligion references were null after loading.");
				}
				if (this.primaryIdeo == null)
				{
					Log.Error("Faction had no ideoligions after loading. Adding random one.");
					Ideo ideo;
					if (Find.IdeoManager.IdeosListForReading.TryRandomElement(out ideo))
					{
						this.primaryIdeo = ideo;
					}
				}
				if (this.primaryIdeo != null && this.primaryIdeo.createdFromNoExpansionGame && ModsConfig.IdeologyActive)
				{
					IdeoFoundationDef ideoFoundationDef = DefDatabase<IdeoFoundationDef>.AllDefs.RandomElement<IdeoFoundationDef>();
					this.primaryIdeo.foundation = (IdeoFoundation)Activator.CreateInstance(ideoFoundationDef.foundationClass);
					this.primaryIdeo.foundation.def = ideoFoundationDef;
					this.primaryIdeo.foundation.ideo = this.primaryIdeo;
					this.primaryIdeo.foundation.Init(FactionIdeosTracker.IdeoGenerationParmsForFaction_BackCompatibility(this.faction.def, false));
					this.primaryIdeo.createdFromNoExpansionGame = false;
				}
			}
		}

		// Token: 0x0400561C RID: 22044
		private Faction faction;

		// Token: 0x0400561D RID: 22045
		private Ideo primaryIdeo;

		// Token: 0x0400561E RID: 22046
		private List<Ideo> ideosMinor = new List<Ideo>();

		// Token: 0x0400561F RID: 22047
		private const float MajorIdeoSelectionWeight = 4f;

		// Token: 0x04005620 RID: 22048
		private const float MinorIdeoSelectionWeight = 1f;

		// Token: 0x04005621 RID: 22049
		private const float ChanceToReuseExistingIdeo = 0.2f;

		// Token: 0x04005622 RID: 22050
		private const int MaxIdeos = 10;

		// Token: 0x04005623 RID: 22051
		private static Dictionary<Ideo, int> tmpPlayerIdeos = new Dictionary<Ideo, int>();
	}
}
