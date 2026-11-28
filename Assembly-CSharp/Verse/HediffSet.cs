using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000409 RID: 1033
	public class HediffSet : IExposable
	{
		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001DE1 RID: 7649 RVA: 0x000B832F File Offset: 0x000B652F
		public float PainTotal
		{
			get
			{
				if (this.cachedPain < 0f)
				{
					this.cachedPain = this.CalculatePain();
				}
				return this.cachedPain;
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001DE2 RID: 7650 RVA: 0x000B8350 File Offset: 0x000B6550
		public float BleedRateTotal
		{
			get
			{
				if (this.cachedBleedRate < 0f)
				{
					this.cachedBleedRate = this.CalculateBleedRate();
				}
				return this.cachedBleedRate;
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001DE3 RID: 7651 RVA: 0x000B8374 File Offset: 0x000B6574
		public bool HasHead
		{
			get
			{
				bool value = this.cachedHasHead.GetValueOrDefault();
				if (this.cachedHasHead == null)
				{
					value = this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord x) => x.def == BodyPartDefOf.Head);
					this.cachedHasHead = new bool?(value);
				}
				return this.cachedHasHead.Value;
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001DE4 RID: 7652 RVA: 0x000B83E0 File Offset: 0x000B65E0
		public bool PreventVacuumBurns
		{
			get
			{
				bool value = this.cachedPreventVacuumBurns.GetValueOrDefault();
				if (this.cachedPreventVacuumBurns == null)
				{
					value = this.hediffs.Any(delegate(Hediff hd)
					{
						HediffStage curStage = hd.CurStage;
						return curStage != null && curStage.preventVacuumBurns;
					});
					this.cachedPreventVacuumBurns = new bool?(value);
				}
				return this.cachedPreventVacuumBurns.Value;
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001DE5 RID: 7653 RVA: 0x000B8448 File Offset: 0x000B6648
		public bool HasRegeneration
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					return false;
				}
				bool value = this.cachedHasRegeneration.GetValueOrDefault();
				if (this.cachedHasRegeneration == null)
				{
					value = this.hediffs.Any(delegate(Hediff hd)
					{
						HediffStage curStage = hd.CurStage;
						return curStage != null && curStage.regeneration > 0f;
					});
					this.cachedHasRegeneration = new bool?(value);
				}
				return this.cachedHasRegeneration.Value;
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001DE6 RID: 7654 RVA: 0x000B84BC File Offset: 0x000B66BC
		public bool RemoveRoamMtb
		{
			get
			{
				bool value = this.cachedRemoveRoamMtb.GetValueOrDefault();
				if (this.cachedRemoveRoamMtb == null)
				{
					value = this.hediffs.Any(delegate(Hediff hd)
					{
						HediffStage curStage = hd.CurStage;
						return curStage != null && curStage.removeRoamMtb;
					});
					this.cachedRemoveRoamMtb = new bool?(value);
				}
				return this.cachedRemoveRoamMtb.Value;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001DE7 RID: 7655 RVA: 0x000B8524 File Offset: 0x000B6724
		public bool HasPreventsDeath
		{
			get
			{
				using (List<Hediff>.Enumerator enumerator = this.hediffs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.preventsDeath)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001DE8 RID: 7656 RVA: 0x000B8584 File Offset: 0x000B6784
		public bool AnyHediffPreventsCrawling
		{
			get
			{
				bool value = this.cachedCanCrawl.GetValueOrDefault();
				if (this.cachedCanCrawl == null)
				{
					value = this.hediffs.Any((Hediff hd) => hd.def.preventsCrawling);
					this.cachedCanCrawl = new bool?(value);
				}
				return this.cachedCanCrawl.Value;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001DE9 RID: 7657 RVA: 0x000B85EC File Offset: 0x000B67EC
		public float HungerRateFactor
		{
			get
			{
				return this.GetHungerRateFactor(null);
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001DEA RID: 7658 RVA: 0x000B85F8 File Offset: 0x000B67F8
		public float RestFallFactor
		{
			get
			{
				float num = 1f;
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					HediffStage curStage = this.hediffs[i].CurStage;
					if (curStage != null)
					{
						num *= curStage.restFallFactor;
					}
				}
				for (int j = 0; j < this.hediffs.Count; j++)
				{
					HediffStage curStage2 = this.hediffs[j].CurStage;
					if (curStage2 != null)
					{
						num += curStage2.restFallFactorOffset;
					}
				}
				return Mathf.Max(num, 0f);
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001DEB RID: 7659 RVA: 0x000B8684 File Offset: 0x000B6884
		public float? OverrideMoodBase
		{
			get
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					if (this.hediffs[i].CurStage != null && this.hediffs[i].CurStage.overrideMoodBase >= 0f)
					{
						return new float?(this.hediffs[i].CurStage.overrideMoodBase);
					}
				}
				return null;
			}
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x000B86FC File Offset: 0x000B68FC
		public HediffSet(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x000B8784 File Offset: 0x000B6984
		public void ExposeData()
		{
			Scribe_Collections.Look<Hediff>(ref this.hediffs, "hediffs", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (this.hediffs.RemoveAll((Hediff x) => x == null) != 0)
				{
					Log.Error(this.pawn.ToStringSafe<Pawn>() + " had some null hediffs.");
				}
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					this.hediffs[i].pawn = this.pawn;
				}
				this.hediffs.RemoveAll(delegate(Hediff hediff)
				{
					if (((hediff != null) ? hediff.def : null) != null)
					{
						return false;
					}
					Log.Error(hediff.ToStringSafe<Hediff>() + " on " + this.pawn.ToStringSafe<Pawn>() + " had a null def.");
					return true;
				});
				this.DirtyCache();
			}
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x000B8850 File Offset: 0x000B6A50
		public float FactorForDamage(DamageInfo dinfo)
		{
			if (dinfo.Def == null || this.hediffs.NullOrEmpty<Hediff>())
			{
				return 1f;
			}
			float result;
			if (this.cachedDamageFactors.TryGetValue(dinfo.Def, out result))
			{
				return result;
			}
			float num = 1f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].CurStage != null && !this.hediffs[i].CurStage.damageFactors.NullOrEmpty<DamageFactor>())
				{
					for (int j = 0; j < this.hediffs[i].CurStage.damageFactors.Count; j++)
					{
						DamageFactor damageFactor = this.hediffs[i].CurStage.damageFactors[j];
						if (damageFactor.damageDef == dinfo.Def)
						{
							num *= damageFactor.factor;
						}
					}
				}
			}
			this.cachedDamageFactors.Add(dinfo.Def, num);
			return num;
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x000B8955 File Offset: 0x000B6B55
		public bool HasBodyPart(BodyPartRecord part)
		{
			return this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(part);
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x000B8968 File Offset: 0x000B6B68
		public void AddDirect(Hediff hediff, DamageInfo? dinfo = null, DamageWorker.DamageResult damageResult = null)
		{
			if (hediff.def == null)
			{
				Log.Error("Tried to add health diff with null def. Canceling.");
				return;
			}
			if (hediff.Part != null && !this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(hediff.Part))
			{
				string str = "Tried to add health diff to missing part ";
				BodyPartRecord part = hediff.Part;
				Log.Error(str + ((part != null) ? part.ToString() : null));
				return;
			}
			hediff.ageTicks = 0;
			hediff.pawn = this.pawn;
			bool flag = false;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].TryMergeWith(hediff))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				this.hediffs.Add(hediff);
				hediff.PostAdd(dinfo);
				if (this.pawn.needs != null && this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
			bool flag2 = hediff is Hediff_MissingPart;
			if (!(hediff is Hediff_MissingPart) && hediff.Part != null && hediff.Part != this.pawn.RaceProps.body.corePart && this.GetPartHealth(hediff.Part) == 0f && hediff.Part != this.pawn.RaceProps.body.corePart)
			{
				bool flag3 = this.HasDirectlyAddedPartFor(hediff.Part);
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, null);
				hediff_MissingPart.IsFresh = !flag3;
				hediff_MissingPart.lastInjury = hediff.def;
				this.pawn.health.AddHediff(hediff_MissingPart, hediff.Part, dinfo, null);
				if (damageResult != null)
				{
					damageResult.AddHediff(hediff_MissingPart);
				}
				if (flag3)
				{
					if (dinfo != null)
					{
						hediff_MissingPart.lastInjury = HealthUtility.GetHediffDefFromDamage(dinfo.Value.Def, this.pawn, hediff.Part);
					}
					else
					{
						hediff_MissingPart.lastInjury = null;
					}
				}
				flag2 = true;
			}
			this.DirtyCache();
			if (flag2 && this.pawn.apparel != null)
			{
				this.pawn.apparel.Notify_LostBodyPart();
			}
			HediffStage curStage = hediff.CurStage;
			if (hediff.def.chemicalNeed != null && !this.pawn.Dead)
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				if (needs != null)
				{
					needs.AddOrRemoveNeedsAsAppropriate();
				}
			}
			else if (curStage != null && (!curStage.enablesNeeds.NullOrEmpty<NeedDef>() || !curStage.disablesNeeds.NullOrEmpty<NeedDef>()) && !this.pawn.Dead)
			{
				Pawn_NeedsTracker needs2 = this.pawn.needs;
				if (needs2 != null)
				{
					needs2.AddOrRemoveNeedsAsAppropriate();
				}
			}
			if (hediff.def.HasDefinedGraphicProperties || hediff.def.forceRenderTreeRecache)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x000B8C48 File Offset: 0x000B6E48
		public void DirtyCache()
		{
			this.CacheMissingPartsCommonAncestors();
			this.CacheThoughtsNullified();
			this.CacheNeeds();
			this.pawn.Drawer.renderer.WoundOverlays.ClearCache();
			PortraitsCache.SetDirty(this.pawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
			this.cachedPain = -1f;
			this.cachedBleedRate = -1f;
			this.cachedHasHead = null;
			this.cachedCanCrawl = null;
			this.cachedHasRegeneration = null;
			this.cachedRemoveRoamMtb = null;
			this.cachedPreventVacuumBurns = null;
			this.cachedDamageFactors.Clear();
			this.pawn.health.capacities.Notify_CapacityLevelsDirty();
			this.pawn.health.summaryHealth.Notify_HealthChanged();
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x000B8D2A File Offset: 0x000B6F2A
		public void DirtyPainCache()
		{
			this.cachedPain = -1f;
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x000B8D38 File Offset: 0x000B6F38
		public void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			for (int i = this.hediffs.Count - 1; i >= 0; i--)
			{
				this.hediffs[i].Notify_PawnDied(dinfo, culprit);
			}
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x000B8D70 File Offset: 0x000B6F70
		public void Notify_Regenerated(float amount)
		{
			foreach (Hediff hediff in this.hediffs)
			{
				hediff.Notify_Regenerated(amount);
			}
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x000B8DC4 File Offset: 0x000B6FC4
		public float GetHungerRateFactor(HediffDef ignore = null)
		{
			float num = 1f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def != ignore)
				{
					HediffStage curStage = this.hediffs[i].CurStage;
					if (curStage != null)
					{
						num *= curStage.hungerRateFactor;
					}
				}
			}
			for (int j = 0; j < this.hediffs.Count; j++)
			{
				if (this.hediffs[j].def != ignore)
				{
					HediffStage curStage2 = this.hediffs[j].CurStage;
					if (curStage2 != null)
					{
						num += curStage2.hungerRateFactorOffset;
					}
				}
			}
			return Mathf.Max(num, 0f);
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x000B8E78 File Offset: 0x000B7078
		public int GetHediffCount(HediffDef def)
		{
			int num = 0;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x000B8EB8 File Offset: 0x000B70B8
		public void GetHediffs<T>(ref List<T> resultHediffs, Predicate<T> filter = null) where T : Hediff
		{
			resultHediffs.Clear();
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				T t = this.hediffs[i] as T;
				if (t != null && (filter == null || filter(t)))
				{
					resultHediffs.Add(t);
				}
			}
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x000B8F18 File Offset: 0x000B7118
		public Hediff GetMostRecentHediff(HediffDef def)
		{
			Hediff hediff = null;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && (hediff == null || this.hediffs[i].ageTicks < hediff.ageTicks))
				{
					hediff = this.hediffs[i];
				}
			}
			return hediff;
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x000B8F7C File Offset: 0x000B717C
		public T GetFirstHediff<T>() where T : Hediff
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				T t = this.hediffs[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x000B8FCC File Offset: 0x000B71CC
		public bool TryGetHediff(HediffDef def, out Hediff hediff)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def)
				{
					hediff = this.hediffs[i];
					return true;
				}
			}
			hediff = null;
			return false;
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x000B9018 File Offset: 0x000B7218
		public bool TryGetHediff<T>(out T hediff) where T : Hediff
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				T t = this.hediffs[i] as T;
				if (t != null)
				{
					hediff = t;
					return true;
				}
			}
			hediff = default(T);
			return false;
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x000B906C File Offset: 0x000B726C
		public Hediff GetFirstHediffOfDef(HediffDef def, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return this.hediffs[i];
				}
			}
			return null;
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x000B90C7 File Offset: 0x000B72C7
		public IEnumerable<T> GetHediffComps<T>() where T : HediffComp
		{
			HediffSet.<GetHediffComps>d__52<T> <GetHediffComps>d__ = new HediffSet.<GetHediffComps>d__52<T>(-2);
			<GetHediffComps>d__.<>4__this = this;
			return <GetHediffComps>d__;
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x000B90D8 File Offset: 0x000B72D8
		public T GetFirstHediffMatchingPart<T>(BodyPartRecord part) where T : Hediff
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				T t = this.hediffs[i] as T;
				if (t != null && t.Part == part)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x000B9134 File Offset: 0x000B7334
		public bool PartIsMissing(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_MissingPart)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x000B9184 File Offset: 0x000B7384
		public float GetPartHealth(BodyPartRecord part)
		{
			if (part == null)
			{
				return 0f;
			}
			float num = part.def.GetMaxHealth(this.pawn);
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i] is Hediff_MissingPart && this.hediffs[i].Part == part)
				{
					return 0f;
				}
				if (this.hediffs[i].Part == part)
				{
					Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
					if (hediff_Injury != null)
					{
						if (hediff_Injury.destroysBodyParts)
						{
							num -= hediff_Injury.Severity;
						}
						else
						{
							num = Mathf.Max(num - hediff_Injury.Severity, 1f);
						}
					}
				}
			}
			num = Mathf.Max(num, 0f);
			if (!part.def.destroyableByDamage)
			{
				num = Mathf.Max(num, 1f);
			}
			return (float)Mathf.RoundToInt(num);
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x000B9270 File Offset: 0x000B7470
		public BodyPartRecord GetBrain()
		{
			foreach (BodyPartRecord bodyPartRecord in this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
			{
				if (bodyPartRecord.def.tags.Contains(BodyPartTagDefOf.ConsciousnessSource))
				{
					return bodyPartRecord;
				}
			}
			return null;
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x000B92D8 File Offset: 0x000B74D8
		public bool HasHediff(HediffDef def, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x000B9328 File Offset: 0x000B7528
		public bool HasHediff<T>(bool mustBeVisible = false) where T : Hediff
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i] is T && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x000B9377 File Offset: 0x000B7577
		public bool TryGetNeedEnablingHediff(NeedDef def, out Hediff hediff)
		{
			if (this.cachedEnabledNeeds == null)
			{
				hediff = null;
				return false;
			}
			return this.cachedEnabledNeeds.TryGetValue(def, out hediff);
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x000B9393 File Offset: 0x000B7593
		public bool TryGetNeedDisablingHediff(NeedDef def, out Hediff hediff)
		{
			if (this.cachedDisabledNeeds == null)
			{
				hediff = null;
				return false;
			}
			return this.cachedDisabledNeeds.TryGetValue(def, out hediff);
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x000B93AF File Offset: 0x000B75AF
		public bool EnablesNeed(NeedDef def)
		{
			Dictionary<NeedDef, Hediff> dictionary = this.cachedEnabledNeeds;
			return dictionary != null && dictionary.ContainsKey(def);
		}

		// Token: 0x06001E07 RID: 7687 RVA: 0x000B93C3 File Offset: 0x000B75C3
		public bool DisablesNeed(NeedDef def)
		{
			Dictionary<NeedDef, Hediff> dictionary = this.cachedDisabledNeeds;
			return dictionary != null && dictionary.ContainsKey(def);
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x000B93D8 File Offset: 0x000B75D8
		public bool HasHediffOrWillBecome(HediffDef def, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (!mustBeVisible || this.hediffs[i].Visible)
				{
					if (this.hediffs[i].def == def)
					{
						return true;
					}
					if (this.CheckHediffDefAndChildren(def, this.hediffs[i].def))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x000B9444 File Offset: 0x000B7644
		private bool CheckHediffDefAndChildren(HediffDef toFind, HediffDef hediff)
		{
			if (hediff == toFind)
			{
				return true;
			}
			if (!hediff.comps.NullOrEmpty<HediffCompProperties>())
			{
				foreach (HediffCompProperties hediffCompProperties in hediff.comps)
				{
					HediffCompProperties_ReplaceHediff hediffCompProperties_ReplaceHediff = hediffCompProperties as HediffCompProperties_ReplaceHediff;
					if (hediffCompProperties_ReplaceHediff != null)
					{
						foreach (HediffCompProperties_ReplaceHediff.TriggeredHediff triggeredHediff in hediffCompProperties_ReplaceHediff.hediffs)
						{
							if (this.CheckHediffDefAndChildren(toFind, triggeredHediff.hediff))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x000B9500 File Offset: 0x000B7700
		public bool HasHediff(HediffDef def, BodyPartRecord bodyPart, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && this.hediffs[i].Part == bodyPart && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x000B9564 File Offset: 0x000B7764
		public bool TryGetBodyPartRecord(BodyPartDef def, out BodyPartRecord record)
		{
			record = this.GetBodyPartRecord(def);
			return record != null;
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x000B9574 File Offset: 0x000B7774
		public BodyPartRecord GetBodyPartRecord(BodyPartDef partDef)
		{
			foreach (BodyPartRecord bodyPartRecord in this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
			{
				if (bodyPartRecord.def == partDef)
				{
					return bodyPartRecord;
				}
			}
			return null;
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x000B95D0 File Offset: 0x000B77D0
		public bool IsBionicOrImplant(BodyPartDef partDef)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff hediff = this.hediffs[i];
				if (hediff.Part != null && hediff.Part.def == partDef && hediff.def.countsAsAddedPartOrImplant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x000B9628 File Offset: 0x000B7828
		public List<Verb> GetHediffsVerbs()
		{
			this.tmpHediffVerbs.Clear();
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				HediffComp_VerbGiver hediffComp_VerbGiver = this.hediffs[i].TryGetComp<HediffComp_VerbGiver>();
				if (hediffComp_VerbGiver != null)
				{
					List<Verb> allVerbs = hediffComp_VerbGiver.VerbTracker.AllVerbs;
					for (int j = 0; j < allVerbs.Count; j++)
					{
						this.tmpHediffVerbs.Add(allVerbs[j]);
					}
				}
			}
			return this.tmpHediffVerbs;
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x000B96A0 File Offset: 0x000B78A0
		public IEnumerable<Hediff> GetHediffsTendable()
		{
			HediffSet.<GetHediffsTendable>d__71 <GetHediffsTendable>d__ = new HediffSet.<GetHediffsTendable>d__71(-2);
			<GetHediffsTendable>d__.<>4__this = this;
			return <GetHediffsTendable>d__;
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x000B96B0 File Offset: 0x000B78B0
		public bool HasTendableHediff(bool forAlert = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((!forAlert || this.hediffs[i].def.makesAlert) && this.hediffs[i].TendableNow(false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x000B9708 File Offset: 0x000B7908
		public bool HasHediffBlocksSleeping()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def.blocksSleeping)
				{
					return true;
				}
				HediffStage curStage = this.hediffs[i].CurStage;
				if (curStage != null && curStage.blocksSleeping)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x000B9768 File Offset: 0x000B7968
		public bool HasHediffPreventsPregnancy()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def.preventsPregnancy)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x000B97A8 File Offset: 0x000B79A8
		public bool HasPregnancyHediff()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def.pregnant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x000B97E6 File Offset: 0x000B79E6
		public IEnumerable<HediffComp> GetAllComps()
		{
			HediffSet.<GetAllComps>d__76 <GetAllComps>d__ = new HediffSet.<GetAllComps>d__76(-2);
			<GetAllComps>d__.<>4__this = this;
			return <GetAllComps>d__;
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x000B97F8 File Offset: 0x000B79F8
		public bool HasHediffShowGizmosOnCorpse()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def.showGizmosOnCorpse)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x000B9838 File Offset: 0x000B7A38
		public bool HasNaturallyHealingInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.CanHealNaturally())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x000B987C File Offset: 0x000B7A7C
		public bool HasTendedAndHealingInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.CanHealFromTending() && hediff_Injury.Severity > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x000B98CC File Offset: 0x000B7ACC
		public bool HasTemperatureInjury(TemperatureInjuryStage minStage)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((this.hediffs[i].def == HediffDefOf.Hypothermia || this.hediffs[i].def == HediffDefOf.Heatstroke) && this.hediffs[i].CurStageIndex >= (int)minStage)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x000B9938 File Offset: 0x000B7B38
		public List<BodyPartRecord> GetInjuredParts()
		{
			HediffSet.tmpInjuredParts.Clear();
			foreach (Hediff hediff in this.hediffs)
			{
				Hediff_Injury hediff_Injury = hediff as Hediff_Injury;
				if (hediff_Injury != null && !HediffSet.tmpInjuredParts.Contains(hediff_Injury.Part))
				{
					HediffSet.tmpInjuredParts.Add(hediff_Injury.Part);
				}
			}
			return HediffSet.tmpInjuredParts;
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x000B99C0 File Offset: 0x000B7BC0
		public List<BodyPartRecord> GetNaturallyHealingInjuredParts()
		{
			HediffSet.tmpNaturallyHealingInjuredParts.Clear();
			foreach (BodyPartRecord bodyPartRecord in this.GetInjuredParts())
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
					if (hediff_Injury != null && this.hediffs[i].Part == bodyPartRecord && hediff_Injury.CanHealNaturally())
					{
						HediffSet.tmpNaturallyHealingInjuredParts.Add(bodyPartRecord);
						break;
					}
				}
			}
			return HediffSet.tmpNaturallyHealingInjuredParts;
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x000B9A70 File Offset: 0x000B7C70
		public List<Hediff_MissingPart> GetMissingPartsCommonAncestors()
		{
			if (this.cachedMissingPartsCommonAncestors == null)
			{
				this.CacheMissingPartsCommonAncestors();
			}
			return this.cachedMissingPartsCommonAncestors;
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x000B9A88 File Offset: 0x000B7C88
		public bool HasMissingPartFor(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_MissingPart)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x000B9AD8 File Offset: 0x000B7CD8
		public Hediff GetMissingPartFor(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_MissingPart)
				{
					return this.hediffs[i];
				}
			}
			return null;
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x000B9B30 File Offset: 0x000B7D30
		public IEnumerable<BodyPartRecord> GetNotMissingParts(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined, BodyPartTagDef tag = null, BodyPartRecord partParent = null)
		{
			HediffSet.<GetNotMissingParts>d__88 <GetNotMissingParts>d__ = new HediffSet.<GetNotMissingParts>d__88(-2);
			<GetNotMissingParts>d__.<>4__this = this;
			<GetNotMissingParts>d__.<>3__height = height;
			<GetNotMissingParts>d__.<>3__depth = depth;
			<GetNotMissingParts>d__.<>3__tag = tag;
			<GetNotMissingParts>d__.<>3__partParent = partParent;
			return <GetNotMissingParts>d__;
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x000B9B60 File Offset: 0x000B7D60
		public BodyPartRecord GetRandomNotMissingPart(DamageDef damDef, BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined, BodyPartRecord partParent = null)
		{
			IEnumerable<BodyPartRecord> notMissingParts;
			if (this.GetNotMissingParts(height, depth, null, partParent).Any((BodyPartRecord p) => p.coverageAbs > 0f))
			{
				notMissingParts = this.GetNotMissingParts(height, depth, null, partParent);
			}
			else
			{
				if (!this.GetNotMissingParts(BodyPartHeight.Undefined, depth, null, partParent).Any((BodyPartRecord p) => p.coverageAbs > 0f))
				{
					return null;
				}
				notMissingParts = this.GetNotMissingParts(BodyPartHeight.Undefined, depth, null, partParent);
			}
			BodyPartRecord result;
			if (notMissingParts.TryRandomElementByWeight((BodyPartRecord x) => x.coverageAbs * x.def.GetHitChanceFactorFor(damDef), out result))
			{
				return result;
			}
			if (notMissingParts.TryRandomElementByWeight((BodyPartRecord x) => x.coverageAbs, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x000B9C40 File Offset: 0x000B7E40
		public float GetCoverageOfNotMissingNaturalParts(BodyPartRecord part)
		{
			if (this.PartIsMissing(part))
			{
				return 0f;
			}
			if (this.PartOrAnyAncestorHasDirectlyAddedParts(part))
			{
				return 0f;
			}
			this.coverageRejectedPartsSet.Clear();
			List<Hediff_MissingPart> missingPartsCommonAncestors = this.GetMissingPartsCommonAncestors();
			for (int i = 0; i < missingPartsCommonAncestors.Count; i++)
			{
				this.coverageRejectedPartsSet.Add(missingPartsCommonAncestors[i].Part);
			}
			for (int j = 0; j < this.hediffs.Count; j++)
			{
				if (this.hediffs[j] is Hediff_AddedPart)
				{
					this.coverageRejectedPartsSet.Add(this.hediffs[j].Part);
				}
			}
			float num = 0f;
			this.coveragePartsStack.Clear();
			this.coveragePartsStack.Push(part);
			while (this.coveragePartsStack.Any<BodyPartRecord>())
			{
				BodyPartRecord bodyPartRecord = this.coveragePartsStack.Pop();
				num += bodyPartRecord.coverageAbs;
				for (int k = 0; k < bodyPartRecord.parts.Count; k++)
				{
					if (!this.coverageRejectedPartsSet.Contains(bodyPartRecord.parts[k]))
					{
						this.coveragePartsStack.Push(bodyPartRecord.parts[k]);
					}
				}
			}
			this.coveragePartsStack.Clear();
			this.coverageRejectedPartsSet.Clear();
			return num;
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x000B9D98 File Offset: 0x000B7F98
		private void CacheMissingPartsCommonAncestors()
		{
			if (this.cachedMissingPartsCommonAncestors == null)
			{
				this.cachedMissingPartsCommonAncestors = new List<Hediff_MissingPart>();
			}
			else
			{
				this.cachedMissingPartsCommonAncestors.Clear();
			}
			this.missingPartsCommonAncestorsQueue.Clear();
			this.missingPartsCommonAncestorsQueue.Enqueue(this.pawn.def.race.body.corePart);
			while (this.missingPartsCommonAncestorsQueue.Count != 0)
			{
				BodyPartRecord bodyPartRecord = this.missingPartsCommonAncestorsQueue.Dequeue();
				if (!this.PartOrAnyAncestorHasDirectlyAddedParts(bodyPartRecord))
				{
					Hediff_MissingPart firstHediffMatchingPart = this.GetFirstHediffMatchingPart<Hediff_MissingPart>(bodyPartRecord);
					if (firstHediffMatchingPart != null)
					{
						this.cachedMissingPartsCommonAncestors.Add(firstHediffMatchingPart);
					}
					else
					{
						for (int i = 0; i < bodyPartRecord.parts.Count; i++)
						{
							this.missingPartsCommonAncestorsQueue.Enqueue(bodyPartRecord.parts[i]);
						}
					}
				}
			}
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x000B9E60 File Offset: 0x000B8060
		private void CacheNeeds()
		{
			Dictionary<NeedDef, Hediff> dictionary = this.cachedDisabledNeeds;
			if (dictionary != null)
			{
				dictionary.Clear();
			}
			Dictionary<NeedDef, Hediff> dictionary2 = this.cachedEnabledNeeds;
			if (dictionary2 != null)
			{
				dictionary2.Clear();
			}
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff hediff = this.hediffs[i];
				if (hediff.def.chemicalNeed != null)
				{
					if (this.cachedEnabledNeeds == null)
					{
						this.cachedEnabledNeeds = new Dictionary<NeedDef, Hediff>();
					}
					this.cachedEnabledNeeds[hediff.def.chemicalNeed] = hediff;
				}
				if (hediff.CurStage != null)
				{
					if (!hediff.CurStage.enablesNeeds.NullOrEmpty<NeedDef>())
					{
						for (int j = 0; j < hediff.CurStage.enablesNeeds.Count; j++)
						{
							NeedDef key = hediff.CurStage.enablesNeeds[j];
							if (this.cachedEnabledNeeds == null)
							{
								this.cachedEnabledNeeds = new Dictionary<NeedDef, Hediff>();
							}
							this.cachedEnabledNeeds[key] = hediff;
						}
					}
					if (!hediff.CurStage.disablesNeeds.NullOrEmpty<NeedDef>())
					{
						for (int k = 0; k < hediff.CurStage.disablesNeeds.Count; k++)
						{
							NeedDef key2 = hediff.CurStage.disablesNeeds[k];
							if (this.cachedDisabledNeeds == null)
							{
								this.cachedDisabledNeeds = new Dictionary<NeedDef, Hediff>();
							}
							this.cachedDisabledNeeds[key2] = hediff;
						}
					}
				}
			}
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x000B9FC0 File Offset: 0x000B81C0
		public void CacheThoughtsNullified()
		{
			this.cachedMaxPctConditionalThoughtsNullified = 0f;
			this.cachedMaxPctAllThoughtsNullified = 0f;
			this.cachedConditionalThoughtNullifyingHediff = null;
			this.cachedAllThoughtNullifyingHediff = null;
			foreach (Hediff hediff in this.hediffs)
			{
				HediffStage curStage = hediff.CurStage;
				if (curStage != null)
				{
					if (curStage.pctConditionalThoughtsNullified > this.cachedMaxPctConditionalThoughtsNullified)
					{
						this.cachedMaxPctConditionalThoughtsNullified = curStage.pctConditionalThoughtsNullified;
						this.cachedConditionalThoughtNullifyingHediff = hediff;
					}
					if (curStage.pctAllThoughtNullification > this.cachedMaxPctAllThoughtsNullified)
					{
						this.cachedMaxPctAllThoughtsNullified = curStage.pctAllThoughtNullification;
						this.cachedAllThoughtNullifyingHediff = hediff;
					}
				}
			}
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x000BA07C File Offset: 0x000B827C
		public Hediff ThoughtNullifyingHediff(ThoughtDef def)
		{
			if (def.nullifyingHediffs != null)
			{
				for (int i = 0; i < def.nullifyingHediffs.Count; i++)
				{
					Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(def.nullifyingHediffs[i], false);
					if (firstHediffOfDef != null)
					{
						return firstHediffOfDef;
					}
				}
			}
			float num = 0f;
			Hediff result = null;
			if (!def.IsMemory)
			{
				num = this.cachedMaxPctConditionalThoughtsNullified;
				result = this.cachedConditionalThoughtNullifyingHediff;
			}
			if (this.cachedMaxPctAllThoughtsNullified > num)
			{
				num = this.cachedMaxPctAllThoughtsNullified;
				result = this.cachedAllThoughtNullifyingHediff;
			}
			if (num == 0f)
			{
				return null;
			}
			Rand.PushState();
			Rand.Seed = this.pawn.thingIDNumber * 31 + (int)(def.index * 139);
			bool flag = Rand.Value < num;
			Rand.PopState();
			if (!flag)
			{
				return null;
			}
			return result;
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x000BA148 File Offset: 0x000B8348
		public bool HasDirectlyAddedPartFor(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_AddedPart)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x000BA198 File Offset: 0x000B8398
		public Hediff GetDirectlyAddedPartFor(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_AddedPart)
				{
					return this.hediffs[i];
				}
			}
			return null;
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x000BA1F0 File Offset: 0x000B83F0
		public bool TryGetDirectlyAddedPartFor(BodyPartRecord part, out Hediff hediff)
		{
			hediff = this.GetDirectlyAddedPartFor(part);
			return hediff != null;
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x000BA200 File Offset: 0x000B8400
		public bool PartOrAnyAncestorHasDirectlyAddedParts(BodyPartRecord part)
		{
			return this.HasDirectlyAddedPartFor(part) || (part.parent != null && this.PartOrAnyAncestorHasDirectlyAddedParts(part.parent));
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x000BA226 File Offset: 0x000B8426
		public bool AncestorHasDirectlyAddedParts(BodyPartRecord part)
		{
			return part.parent != null && this.PartOrAnyAncestorHasDirectlyAddedParts(part.parent);
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x000BA241 File Offset: 0x000B8441
		public IEnumerable<Hediff> GetTendableNonInjuryNonMissingPartHediffs()
		{
			HediffSet.<GetTendableNonInjuryNonMissingPartHediffs>d__107 <GetTendableNonInjuryNonMissingPartHediffs>d__ = new HediffSet.<GetTendableNonInjuryNonMissingPartHediffs>d__107(-2);
			<GetTendableNonInjuryNonMissingPartHediffs>d__.<>4__this = this;
			return <GetTendableNonInjuryNonMissingPartHediffs>d__;
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x000BA254 File Offset: 0x000B8454
		public bool HasTendableNonInjuryNonMissingPartHediff(bool forAlert = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((!forAlert || this.hediffs[i].def.makesAlert) && !(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].TendableNow(false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x000BA2D0 File Offset: 0x000B84D0
		public bool HasImmunizableNotImmuneHediff()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (!(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].Visible && this.hediffs[i].def.PossibleToDevelopImmunityNaturally() && !this.hediffs[i].FullyImmune())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001E2D RID: 7725 RVA: 0x000BA35C File Offset: 0x000B855C
		public bool AnyHediffMakesSickThought
		{
			get
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					if (this.hediffs[i].def.makesSickThought && this.hediffs[i].Visible)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x000BA3AD File Offset: 0x000B85AD
		public bool InLabor(bool includePostpartumExhaustion = true)
		{
			return ModsConfig.BiotechActive && (this.HasHediff(HediffDefOf.PregnancyLabor, false) || this.HasHediff(HediffDefOf.PregnancyLaborPushing, false) || (includePostpartumExhaustion && this.HasHediff(HediffDefOf.PostpartumExhaustion, false)));
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001E2F RID: 7727 RVA: 0x000BA3EA File Offset: 0x000B85EA
		public bool IsPsychicallySuppressed
		{
			get
			{
				return ModsConfig.RoyaltyActive && this.HasHediff(HediffDefOf.PsychicSuppression, false);
			}
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x000BA404 File Offset: 0x000B8604
		private float CalculateBleedRate()
		{
			if (this.pawn.health.Dead)
			{
				return 0f;
			}
			if (this.pawn.Deathresting)
			{
				return 0f;
			}
			if (!this.pawn.health.CanBleed)
			{
				return 0f;
			}
			float num = 1f;
			float num2 = 0f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff hediff = this.hediffs[i];
				HediffStage curStage = hediff.CurStage;
				if (curStage != null)
				{
					num *= curStage.totalBleedFactor;
				}
				num2 += hediff.BleedRate;
			}
			return num2 * num / this.pawn.HealthScale;
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x000BA4B4 File Offset: 0x000B86B4
		private float CalculatePain()
		{
			if (!this.pawn.RaceProps.IsFlesh || this.pawn.Dead)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				num += this.hediffs[i].PainOffset;
			}
			if (this.pawn.genes != null)
			{
				num += this.pawn.genes.PainOffset;
			}
			Pawn_StoryTracker story = this.pawn.story;
			if (((story != null) ? story.traits : null) != null)
			{
				List<Trait> allTraits = this.pawn.story.traits.allTraits;
				for (int j = 0; j < allTraits.Count; j++)
				{
					num += allTraits[j].CurrentData.painOffset;
				}
			}
			for (int k = 0; k < this.hediffs.Count; k++)
			{
				num *= this.hediffs[k].PainFactor;
			}
			if (this.pawn.genes != null)
			{
				num *= this.pawn.genes.PainFactor;
			}
			Pawn_StoryTracker story2 = this.pawn.story;
			if (((story2 != null) ? story2.traits : null) != null)
			{
				List<Trait> allTraits2 = this.pawn.story.traits.allTraits;
				for (int l = 0; l < allTraits2.Count; l++)
				{
					num *= allTraits2[l].CurrentData.painFactor;
				}
			}
			return Mathf.Clamp(num, 0f, 1f);
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x000BA648 File Offset: 0x000B8848
		public Color GetSkinColor(Color baseColor)
		{
			foreach (Hediff hediff in this.pawn.health.hediffSet.hediffs)
			{
				if (hediff.def.skinColorTint != null)
				{
					baseColor = baseColor * (1f - hediff.def.skinColorTintStrength) + hediff.def.skinColorTint.Value * hediff.def.skinColorTintStrength;
				}
				if (hediff.def.skinColorOverride != null)
				{
					return hediff.def.skinColorOverride.Value;
				}
			}
			return baseColor;
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x000BA720 File Offset: 0x000B8920
		public void Clear()
		{
			this.hediffs.Clear();
			this.DirtyCache();
		}

		// Token: 0x040017E4 RID: 6116
		public Pawn pawn;

		// Token: 0x040017E5 RID: 6117
		public List<Hediff> hediffs = new List<Hediff>();

		// Token: 0x040017E6 RID: 6118
		private Dictionary<DamageDef, float> cachedDamageFactors = new Dictionary<DamageDef, float>();

		// Token: 0x040017E7 RID: 6119
		private List<Hediff_MissingPart> cachedMissingPartsCommonAncestors;

		// Token: 0x040017E8 RID: 6120
		private Dictionary<NeedDef, Hediff> cachedEnabledNeeds;

		// Token: 0x040017E9 RID: 6121
		private Dictionary<NeedDef, Hediff> cachedDisabledNeeds;

		// Token: 0x040017EA RID: 6122
		private float cachedPain = -1f;

		// Token: 0x040017EB RID: 6123
		private float cachedBleedRate = -1f;

		// Token: 0x040017EC RID: 6124
		private bool? cachedHasHead;

		// Token: 0x040017ED RID: 6125
		private bool? cachedCanCrawl;

		// Token: 0x040017EE RID: 6126
		private bool? cachedHasRegeneration;

		// Token: 0x040017EF RID: 6127
		private bool? cachedRemoveRoamMtb;

		// Token: 0x040017F0 RID: 6128
		private bool? cachedPreventVacuumBurns;

		// Token: 0x040017F1 RID: 6129
		private List<Verb> tmpHediffVerbs = new List<Verb>();

		// Token: 0x040017F2 RID: 6130
		private static List<BodyPartRecord> tmpInjuredParts = new List<BodyPartRecord>();

		// Token: 0x040017F3 RID: 6131
		private static List<BodyPartRecord> tmpNaturallyHealingInjuredParts = new List<BodyPartRecord>();

		// Token: 0x040017F4 RID: 6132
		private Stack<BodyPartRecord> coveragePartsStack = new Stack<BodyPartRecord>();

		// Token: 0x040017F5 RID: 6133
		private HashSet<BodyPartRecord> coverageRejectedPartsSet = new HashSet<BodyPartRecord>();

		// Token: 0x040017F6 RID: 6134
		private readonly Queue<BodyPartRecord> missingPartsCommonAncestorsQueue = new Queue<BodyPartRecord>();

		// Token: 0x040017F7 RID: 6135
		private float cachedMaxPctConditionalThoughtsNullified = -1f;

		// Token: 0x040017F8 RID: 6136
		private Hediff cachedConditionalThoughtNullifyingHediff;

		// Token: 0x040017F9 RID: 6137
		private float cachedMaxPctAllThoughtsNullified = -1f;

		// Token: 0x040017FA RID: 6138
		private Hediff cachedAllThoughtNullifyingHediff;
	}
}
