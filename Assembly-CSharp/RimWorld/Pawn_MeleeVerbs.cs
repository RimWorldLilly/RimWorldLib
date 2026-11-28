using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200159B RID: 5531
	public class Pawn_MeleeVerbs : IExposable
	{
		// Token: 0x1700175E RID: 5982
		// (get) Token: 0x06008844 RID: 34884 RVA: 0x0031500A File Offset: 0x0031320A
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06008845 RID: 34885 RVA: 0x00315012 File Offset: 0x00313212
		public Pawn_MeleeVerbs(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06008846 RID: 34886 RVA: 0x0031502C File Offset: 0x0031322C
		public static void PawnMeleeVerbsStaticUpdate()
		{
			Pawn_MeleeVerbs.meleeVerbs.Clear();
			Pawn_MeleeVerbs.verbsToAdd.Clear();
		}

		// Token: 0x06008847 RID: 34887 RVA: 0x00315044 File Offset: 0x00313244
		public Verb TryGetMeleeVerb(Thing target)
		{
			if (target == null && this.curMeleeVerb != null && this.curMeleeVerb.IsStillUsableBy(this.pawn))
			{
				return this.curMeleeVerb;
			}
			if (this.curMeleeVerb == null || this.curMeleeVerbTarget != target || Find.TickManager.TicksGame >= this.curMeleeVerbUpdateTick + 60 || !this.curMeleeVerb.IsStillUsableBy(this.pawn) || !this.curMeleeVerb.IsUsableOn(target))
			{
				this.ChooseMeleeVerb(target);
			}
			return this.curMeleeVerb;
		}

		// Token: 0x06008848 RID: 34888 RVA: 0x003150CC File Offset: 0x003132CC
		private void ChooseMeleeVerb(Thing target)
		{
			bool flag = Rand.Chance(0.04f);
			List<VerbEntry> updatedAvailableVerbsList = this.GetUpdatedAvailableVerbsList(flag);
			bool flag2 = false;
			VerbEntry verbEntry;
			if (updatedAvailableVerbsList.TryRandomElementByWeight((VerbEntry ve) => ve.GetSelectionWeight(target), out verbEntry))
			{
				flag2 = true;
			}
			else if (flag)
			{
				updatedAvailableVerbsList = this.GetUpdatedAvailableVerbsList(false);
				flag2 = updatedAvailableVerbsList.TryRandomElementByWeight((VerbEntry ve) => ve.GetSelectionWeight(target), out verbEntry);
			}
			if (flag2)
			{
				this.SetCurMeleeVerb(verbEntry.verb, target);
				return;
			}
			Log.ErrorOnce(string.Concat(new string[]
			{
				this.pawn.ToStringSafe<Pawn>(),
				" has no available melee attack, spawned=",
				this.pawn.Spawned.ToString(),
				" dead=",
				this.pawn.Dead.ToString(),
				" downed=",
				this.pawn.Downed.ToString(),
				" curJob=",
				this.pawn.CurJob.ToStringSafe<Job>(),
				" verbList=",
				updatedAvailableVerbsList.ToStringSafeEnumerable(),
				" bodyVerbs=",
				this.pawn.verbTracker.AllVerbs.ToStringSafeEnumerable()
			}), this.pawn.thingIDNumber ^ 195867354);
			this.SetCurMeleeVerb(null, null);
		}

		// Token: 0x06008849 RID: 34889 RVA: 0x00315234 File Offset: 0x00313434
		public bool TryMeleeAttack(Thing target, Verb verbToUse = null, bool surpriseAttack = false)
		{
			if (this.pawn.stances.FullBodyBusy)
			{
				return false;
			}
			if (!this.pawn.kindDef.canMeleeAttack)
			{
				return false;
			}
			if (verbToUse != null)
			{
				if (!verbToUse.IsStillUsableBy(this.pawn))
				{
					return false;
				}
				if (!verbToUse.IsMeleeAttack)
				{
					string[] array = new string[7];
					array[0] = "Pawn ";
					int num = 1;
					Pawn pawn = this.pawn;
					array[num] = ((pawn != null) ? pawn.ToString() : null);
					array[2] = " tried to melee attack ";
					array[3] = ((target != null) ? target.ToString() : null);
					array[4] = " with non melee-attack verb ";
					array[5] = ((verbToUse != null) ? verbToUse.ToString() : null);
					array[6] = ".";
					Log.Warning(string.Concat(array));
					return false;
				}
			}
			Verb verb = verbToUse ?? this.TryGetMeleeVerb(target);
			if (verb == null)
			{
				return false;
			}
			verb.TryStartCastOn(target, surpriseAttack, true, false, false);
			return true;
		}

		// Token: 0x0600884A RID: 34890 RVA: 0x00315310 File Offset: 0x00313510
		public List<VerbEntry> GetUpdatedAvailableVerbsList(bool terrainTools)
		{
			Pawn_MeleeVerbs.meleeVerbs.Clear();
			Pawn_MeleeVerbs.verbsToAdd.Clear();
			if (!terrainTools)
			{
				List<Verb> allVerbs = this.pawn.verbTracker.AllVerbs;
				for (int i = 0; i < allVerbs.Count; i++)
				{
					if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|20_0(allVerbs[i]))
					{
						Pawn_MeleeVerbs.verbsToAdd.Add(allVerbs[i]);
					}
				}
				if (this.pawn.equipment != null)
				{
					List<ThingWithComps> allEquipmentListForReading = this.pawn.equipment.AllEquipmentListForReading;
					for (int j = 0; j < allEquipmentListForReading.Count; j++)
					{
						CompEquippable comp = allEquipmentListForReading[j].GetComp<CompEquippable>();
						List<Verb> list = (comp != null) ? comp.AllVerbs : null;
						if (list != null)
						{
							for (int k = 0; k < list.Count; k++)
							{
								if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|20_0(list[k]))
								{
									Pawn_MeleeVerbs.verbsToAdd.Add(list[k]);
								}
							}
						}
					}
				}
				if (this.pawn.apparel != null)
				{
					List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
					for (int l = 0; l < wornApparel.Count; l++)
					{
						CompEquippable comp2 = wornApparel[l].GetComp<CompEquippable>();
						List<Verb> list2 = (comp2 != null) ? comp2.AllVerbs : null;
						if (list2 != null)
						{
							for (int m = 0; m < list2.Count; m++)
							{
								if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|20_0(list2[m]))
								{
									Pawn_MeleeVerbs.verbsToAdd.Add(list2[m]);
								}
							}
						}
					}
				}
				foreach (Verb verb in this.pawn.health.hediffSet.GetHediffsVerbs())
				{
					if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|20_0(verb))
					{
						Pawn_MeleeVerbs.verbsToAdd.Add(verb);
					}
				}
				if (!ModsConfig.AnomalyActive || !this.pawn.IsMutant)
				{
					goto IL_2F5;
				}
				using (List<Verb>.Enumerator enumerator = this.pawn.mutant.AllVerbs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Verb verb2 = enumerator.Current;
						if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|20_0(verb2))
						{
							Pawn_MeleeVerbs.verbsToAdd.Add(verb2);
						}
					}
					goto IL_2F5;
				}
			}
			if (this.pawn.Spawned && !this.pawn.IsSubhuman)
			{
				TerrainDef terrain = this.pawn.Position.GetTerrain(this.pawn.Map);
				if (this.terrainVerbs == null || this.terrainVerbs.def != terrain)
				{
					this.terrainVerbs = Pawn_MeleeVerbs_TerrainSource.Create(this, terrain);
				}
				List<Verb> allVerbs2 = this.terrainVerbs.tracker.AllVerbs;
				for (int n = 0; n < allVerbs2.Count; n++)
				{
					Verb verb3 = allVerbs2[n];
					if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|20_0(verb3))
					{
						Pawn_MeleeVerbs.verbsToAdd.Add(verb3);
					}
				}
			}
			IL_2F5:
			float num = 0f;
			Pawn_MeleeVerbs.initialWeights.Clear();
			foreach (Verb v in Pawn_MeleeVerbs.verbsToAdd)
			{
				float num2 = VerbUtility.InitialVerbWeight(v, this.pawn);
				Pawn_MeleeVerbs.initialWeights.Add(num2);
				if (num2 > num)
				{
					num = num2;
				}
			}
			int num3 = 0;
			int num4 = 0;
			Pawn_MeleeVerbs.verbCategories.Clear();
			for (int num5 = 0; num5 < Pawn_MeleeVerbs.verbsToAdd.Count; num5++)
			{
				VerbSelectionCategory verbSelectionCategory = VerbUtility.SelectionCategoryForWeight(Pawn_MeleeVerbs.initialWeights[num5], num);
				Pawn_MeleeVerbs.verbCategories.Add(verbSelectionCategory);
				if (verbSelectionCategory == VerbSelectionCategory.Best)
				{
					num3++;
				}
				else if (verbSelectionCategory == VerbSelectionCategory.Mid)
				{
					num4++;
				}
			}
			for (int num6 = 0; num6 < Pawn_MeleeVerbs.verbsToAdd.Count; num6++)
			{
				VerbSelectionCategory verbSelectionCategory2 = Pawn_MeleeVerbs.verbCategories[num6];
				int numInSameCategory = (verbSelectionCategory2 == VerbSelectionCategory.Best) ? num3 : num4;
				Pawn_MeleeVerbs.meleeVerbs.Add(new VerbEntry(Pawn_MeleeVerbs.verbsToAdd[num6], VerbUtility.FinalSelectionWeightFor(verbSelectionCategory2, numInSameCategory)));
			}
			return Pawn_MeleeVerbs.meleeVerbs;
		}

		// Token: 0x0600884B RID: 34891 RVA: 0x00315750 File Offset: 0x00313950
		public void Notify_PawnKilled()
		{
			this.SetCurMeleeVerb(null, null);
		}

		// Token: 0x0600884C RID: 34892 RVA: 0x0031575A File Offset: 0x0031395A
		public void Notify_PawnDespawned()
		{
			this.SetCurMeleeVerb(null, null);
		}

		// Token: 0x0600884D RID: 34893 RVA: 0x00315764 File Offset: 0x00313964
		public void Notify_UsedTerrainBasedVerb()
		{
			this.lastTerrainBasedVerbUseTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600884E RID: 34894 RVA: 0x00315776 File Offset: 0x00313976
		private void SetCurMeleeVerb(Verb v, Thing target)
		{
			this.curMeleeVerb = v;
			this.curMeleeVerbTarget = target;
			if (Current.ProgramState != ProgramState.Playing)
			{
				this.curMeleeVerbUpdateTick = 0;
				return;
			}
			this.curMeleeVerbUpdateTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600884F RID: 34895 RVA: 0x003157A8 File Offset: 0x003139A8
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.curMeleeVerb != null && !this.curMeleeVerb.IsStillUsableBy(this.pawn))
			{
				this.curMeleeVerb = null;
			}
			Scribe_References.Look<Verb>(ref this.curMeleeVerb, "curMeleeVerb", false);
			Scribe_Values.Look<int>(ref this.curMeleeVerbUpdateTick, "curMeleeVerbUpdateTick", 0, false);
			Scribe_Deep.Look<Pawn_MeleeVerbs_TerrainSource>(ref this.terrainVerbs, "terrainVerbs", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastTerrainBasedVerbUseTick, "lastTerrainBasedVerbUseTick", -99999, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.terrainVerbs != null)
			{
				this.terrainVerbs.parent = this;
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.curMeleeVerb != null && this.curMeleeVerb.BuggedAfterLoading)
			{
				this.curMeleeVerb = null;
				Log.Warning(this.pawn.ToStringSafe<Pawn>() + " had a bugged melee verb after loading.");
			}
		}

		// Token: 0x06008851 RID: 34897 RVA: 0x003158B1 File Offset: 0x00313AB1
		[CompilerGenerated]
		private bool <GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|20_0(Verb v)
		{
			return v.IsMeleeAttack && v.IsStillUsableBy(this.pawn);
		}

		// Token: 0x04005406 RID: 21510
		private Pawn pawn;

		// Token: 0x04005407 RID: 21511
		private Verb curMeleeVerb;

		// Token: 0x04005408 RID: 21512
		private Thing curMeleeVerbTarget;

		// Token: 0x04005409 RID: 21513
		private int curMeleeVerbUpdateTick;

		// Token: 0x0400540A RID: 21514
		private Pawn_MeleeVerbs_TerrainSource terrainVerbs;

		// Token: 0x0400540B RID: 21515
		public int lastTerrainBasedVerbUseTick = -99999;

		// Token: 0x0400540C RID: 21516
		private static List<VerbEntry> meleeVerbs = new List<VerbEntry>();

		// Token: 0x0400540D RID: 21517
		private static List<Verb> verbsToAdd = new List<Verb>();

		// Token: 0x0400540E RID: 21518
		private static List<VerbSelectionCategory> verbCategories = new List<VerbSelectionCategory>();

		// Token: 0x0400540F RID: 21519
		private static List<float> initialWeights = new List<float>();

		// Token: 0x04005410 RID: 21520
		private const int BestMeleeVerbUpdateInterval = 60;

		// Token: 0x04005411 RID: 21521
		public const int TerrainBasedVerbUseDelay = 1200;

		// Token: 0x04005412 RID: 21522
		private const float TerrainBasedVerbChooseChance = 0.04f;
	}
}
