using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200158F RID: 5519
	public class Pawn_ApparelTracker : IThingHolder, IExposable
	{
		// Token: 0x1700171F RID: 5919
		// (get) Token: 0x06008745 RID: 34629 RVA: 0x0030E4EA File Offset: 0x0030C6EA
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17001720 RID: 5920
		// (get) Token: 0x06008746 RID: 34630 RVA: 0x0030E4F2 File Offset: 0x0030C6F2
		public List<Apparel> WornApparel
		{
			get
			{
				return this.wornApparel.InnerListForReading;
			}
		}

		// Token: 0x17001721 RID: 5921
		// (get) Token: 0x06008747 RID: 34631 RVA: 0x0030E500 File Offset: 0x0030C700
		public List<ApparelRequirement> ActiveRequirementsForReading
		{
			get
			{
				if (this.activeRequirementsDirty)
				{
					this.activeRequirements.Clear();
					foreach (ApparelRequirementWithSource apparelRequirementWithSource in this.AllRequirements)
					{
						string text;
						if (ApparelUtility.IsRequirementActive(apparelRequirementWithSource.requirement, apparelRequirementWithSource.Source, this.pawn, out text))
						{
							this.activeRequirements.Add(apparelRequirementWithSource.requirement);
						}
					}
					this.activeRequirementsDirty = false;
				}
				return this.activeRequirements;
			}
		}

		// Token: 0x17001722 RID: 5922
		// (get) Token: 0x06008748 RID: 34632 RVA: 0x0030E59C File Offset: 0x0030C79C
		public List<ApparelRequirementWithSource> AllRequirements
		{
			get
			{
				if (this.allRequirementsDirty)
				{
					this.allRequirements.Clear();
					if (ModsConfig.RoyaltyActive && this.pawn.royalty != null)
					{
						foreach (RoyalTitle royalTitle in this.pawn.royalty.AllTitlesForReading)
						{
							if (!royalTitle.def.requiredApparel.NullOrEmpty<ApparelRequirement>())
							{
								foreach (ApparelRequirement apparelRequirement in royalTitle.def.requiredApparel)
								{
									if (apparelRequirement.IsValid)
									{
										this.allRequirements.Add(new ApparelRequirementWithSource(apparelRequirement, royalTitle));
									}
								}
							}
						}
					}
					if (ModsConfig.IdeologyActive && this.pawn.Ideo != null)
					{
						Precept_Role role = this.pawn.Ideo.GetRole(this.pawn);
						if (role != null && !role.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
						{
							foreach (PreceptApparelRequirement preceptApparelRequirement in role.apparelRequirements)
							{
								if (preceptApparelRequirement.requirement.IsValid)
								{
									this.allRequirements.Add(new ApparelRequirementWithSource(preceptApparelRequirement.requirement, role));
								}
							}
						}
					}
					this.allRequirementsDirty = false;
				}
				return this.allRequirements;
			}
		}

		// Token: 0x17001723 RID: 5923
		// (get) Token: 0x06008749 RID: 34633 RVA: 0x0030E744 File Offset: 0x0030C944
		public int WornApparelCount
		{
			get
			{
				return this.wornApparel.Count;
			}
		}

		// Token: 0x17001724 RID: 5924
		// (get) Token: 0x0600874A RID: 34634 RVA: 0x0030E751 File Offset: 0x0030C951
		public bool AnyApparel
		{
			get
			{
				return this.wornApparel.Count != 0;
			}
		}

		// Token: 0x17001725 RID: 5925
		// (get) Token: 0x0600874B RID: 34635 RVA: 0x0030E764 File Offset: 0x0030C964
		public bool AnyClothing
		{
			get
			{
				using (List<Apparel>.Enumerator enumerator = this.wornApparel.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.apparel.countsAsClothingForNudity)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17001726 RID: 5926
		// (get) Token: 0x0600874C RID: 34636 RVA: 0x0030E7C8 File Offset: 0x0030C9C8
		public bool AnyApparelLocked
		{
			get
			{
				return !this.lockedApparel.NullOrEmpty<Apparel>();
			}
		}

		// Token: 0x17001727 RID: 5927
		// (get) Token: 0x0600874D RID: 34637 RVA: 0x0030E7D8 File Offset: 0x0030C9D8
		public bool AnyApparelUnlocked
		{
			get
			{
				if (!this.AnyApparelLocked)
				{
					return this.AnyApparel;
				}
				for (int i = 0; i < this.wornApparel.Count; i++)
				{
					if (!this.IsLocked(this.wornApparel[i]))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001728 RID: 5928
		// (get) Token: 0x0600874E RID: 34638 RVA: 0x0030E824 File Offset: 0x0030CA24
		public bool AllApparelLocked
		{
			get
			{
				for (int i = 0; i < this.wornApparel.Count; i++)
				{
					if (!this.IsLocked(this.wornApparel[i]))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001729 RID: 5929
		// (get) Token: 0x0600874F RID: 34639 RVA: 0x0030E85E File Offset: 0x0030CA5E
		public List<Apparel> LockedApparel
		{
			get
			{
				if (this.lockedApparel == null)
				{
					return Pawn_ApparelTracker.EmptyApparel;
				}
				return this.lockedApparel;
			}
		}

		// Token: 0x1700172A RID: 5930
		// (get) Token: 0x06008750 RID: 34640 RVA: 0x0030E874 File Offset: 0x0030CA74
		public IEnumerable<Apparel> UnlockedApparel
		{
			get
			{
				if (!this.AnyApparelLocked)
				{
					return this.WornApparel;
				}
				return from x in this.WornApparel
				where !this.IsLocked(x)
				select x;
			}
		}

		// Token: 0x1700172B RID: 5931
		// (get) Token: 0x06008751 RID: 34641 RVA: 0x0030E8AC File Offset: 0x0030CAAC
		public bool PsychologicallyNude
		{
			get
			{
				if (this.pawn.gender == Gender.None)
				{
					return false;
				}
				if (this.pawn.IsWildMan())
				{
					return false;
				}
				bool flag;
				bool flag2;
				this.HasBasicApparel(out flag, out flag2);
				if (!flag)
				{
					bool flag3 = false;
					using (IEnumerator<BodyPartRecord> enumerator = this.pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.IsInGroup(BodyPartGroupDefOf.Legs))
							{
								flag3 = true;
								break;
							}
						}
					}
					if (!flag3)
					{
						flag = true;
					}
				}
				if (this.pawn.gender == Gender.Male)
				{
					return !flag;
				}
				return this.pawn.gender == Gender.Female && (!flag || !flag2);
			}
		}

		// Token: 0x1700172C RID: 5932
		// (get) Token: 0x06008752 RID: 34642 RVA: 0x0030E974 File Offset: 0x0030CB74
		public bool AnyApparelNeedsRecoloring
		{
			get
			{
				using (List<Apparel>.Enumerator enumerator = this.WornApparel.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.DesiredColor != null)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x1700172D RID: 5933
		// (get) Token: 0x06008753 RID: 34643 RVA: 0x0030E9D8 File Offset: 0x0030CBD8
		public IEnumerable<Verb> AllApparelVerbs
		{
			get
			{
				Pawn_ApparelTracker.<get_AllApparelVerbs>d__40 <get_AllApparelVerbs>d__ = new Pawn_ApparelTracker.<get_AllApparelVerbs>d__40(-2);
				<get_AllApparelVerbs>d__.<>4__this = this;
				return <get_AllApparelVerbs>d__;
			}
		}

		// Token: 0x1700172E RID: 5934
		// (get) Token: 0x06008754 RID: 34644 RVA: 0x0030E9E8 File Offset: 0x0030CBE8
		public Verb FirstApparelVerb
		{
			get
			{
				List<Apparel> list = this.WornApparel;
				for (int i = 0; i < list.Count; i++)
				{
					CompApparelVerbOwner comp = list[i].GetComp<CompApparelVerbOwner>();
					List<Verb> list2 = (comp != null) ? comp.AllVerbs : null;
					if (list2 != null && list2.Count != 0)
					{
						return list2[0];
					}
				}
				return null;
			}
		}

		// Token: 0x06008755 RID: 34645 RVA: 0x0030EA3C File Offset: 0x0030CC3C
		public Pawn_ApparelTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.wornApparel = new ThingOwner<Apparel>(this);
		}

		// Token: 0x06008756 RID: 34646 RVA: 0x0030EA90 File Offset: 0x0030CC90
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<Apparel>>(ref this.wornApparel, "wornApparel", new object[]
			{
				this
			});
			Scribe_Collections.Look<Apparel>(ref this.lockedApparel, "lockedApparel", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastApparelWearoutTick, "lastApparelWearoutTick", 0, false);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.SortWornApparelIntoDrawOrder();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.lockedApparel != null)
				{
					this.lockedApparel.RemoveAll((Apparel x) => x == null);
				}
				foreach (Apparel apparel in this.WornApparel)
				{
					CompApparelVerbOwner comp = apparel.GetComp<CompApparelVerbOwner>();
					if (comp != null)
					{
						foreach (Verb verb in comp.AllVerbs)
						{
							verb.caster = this.pawn;
						}
					}
				}
			}
		}

		// Token: 0x06008757 RID: 34647 RVA: 0x0030EBB8 File Offset: 0x0030CDB8
		public void ApparelTrackerTickRare()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (this.lastApparelWearoutTick < 0)
			{
				this.lastApparelWearoutTick = ticksGame;
			}
			if (ticksGame - this.lastApparelWearoutTick >= 60000)
			{
				if (!this.pawn.IsWorldPawn())
				{
					for (int i = this.wornApparel.Count - 1; i >= 0; i--)
					{
						this.TakeWearoutDamageForDay(this.wornApparel[i]);
					}
				}
				this.lastApparelWearoutTick = ticksGame;
			}
		}

		// Token: 0x06008758 RID: 34648 RVA: 0x0030EC30 File Offset: 0x0030CE30
		public void ApparelTrackerTickInterval(int delta)
		{
			if (this.pawn.IsColonist && this.pawn.Spawned && !this.pawn.Dead && this.PsychologicallyNude && this.pawn.IsHashIntervalTick(60000, delta))
			{
				TaleRecorder.RecordTale(TaleDefOf.WalkedNaked, new object[]
				{
					this.pawn
				});
			}
			if (this.lockedApparel == null)
			{
				return;
			}
			for (int i = this.lockedApparel.Count - 1; i >= 0; i--)
			{
				if (this.lockedApparel[i].def.useHitPoints && (float)this.lockedApparel[i].HitPoints / (float)this.lockedApparel[i].MaxHitPoints < 0.5f)
				{
					this.Unlock(this.lockedApparel[i]);
				}
			}
		}

		// Token: 0x06008759 RID: 34649 RVA: 0x0030ED10 File Offset: 0x0030CF10
		public bool IsLocked(Apparel apparel)
		{
			return this.lockedApparel != null && this.lockedApparel.Contains(apparel);
		}

		// Token: 0x0600875A RID: 34650 RVA: 0x0030ED28 File Offset: 0x0030CF28
		public void Lock(Apparel apparel)
		{
			if (this.IsLocked(apparel))
			{
				return;
			}
			if (this.lockedApparel == null)
			{
				this.lockedApparel = new List<Apparel>();
			}
			this.lockedApparel.Add(apparel);
		}

		// Token: 0x0600875B RID: 34651 RVA: 0x0030ED53 File Offset: 0x0030CF53
		public void Unlock(Apparel apparel)
		{
			if (!this.IsLocked(apparel))
			{
				return;
			}
			this.lockedApparel.Remove(apparel);
		}

		// Token: 0x0600875C RID: 34652 RVA: 0x0030ED6C File Offset: 0x0030CF6C
		public void LockAll()
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				this.Lock(this.wornApparel[i]);
			}
		}

		// Token: 0x0600875D RID: 34653 RVA: 0x0030EDA4 File Offset: 0x0030CFA4
		public void UnlockAll()
		{
			if (!this.lockedApparel.NullOrEmpty<Apparel>())
			{
				for (int i = this.lockedApparel.Count - 1; i >= 0; i--)
				{
					this.Unlock(this.lockedApparel[i]);
				}
			}
		}

		// Token: 0x0600875E RID: 34654 RVA: 0x0030EDE8 File Offset: 0x0030CFE8
		private void TakeWearoutDamageForDay(Thing ap)
		{
			int num = GenMath.RoundRandom(ap.def.apparel.wearPerDay);
			if (num > 0)
			{
				ap.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)num, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
			if (ap.Destroyed && PawnUtility.ShouldSendNotificationAbout(this.pawn) && !this.pawn.Dead)
			{
				Messages.Message("MessageWornApparelDeterioratedAway".Translate(GenLabel.ThingLabel(ap.def, ap.Stuff, 1), this.pawn).CapitalizeFirst(), this.pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x0600875F RID: 34655 RVA: 0x0030EEA4 File Offset: 0x0030D0A4
		public bool CanWearWithoutDroppingAnything(ThingDef apDef)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				if (!ApparelUtility.CanWearTogether(apDef, this.wornApparel[i].def, this.pawn.RaceProps.body))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008760 RID: 34656 RVA: 0x0030EEF4 File Offset: 0x0030D0F4
		public void Wear(Apparel newApparel, bool dropReplacedApparel = true, bool locked = false)
		{
			newApparel.DeSpawnOrDeselect(DestroyMode.Vanish);
			if (!ApparelUtility.HasPartsToWear(this.pawn, newApparel.def))
			{
				Pawn pawn = this.pawn;
				Log.Warning(((pawn != null) ? pawn.ToString() : null) + " tried to wear " + ((newApparel != null) ? newApparel.ToString() : null) + " but he has no body parts required to wear it.");
				return;
			}
			if (CompBiocodable.IsBiocoded(newApparel) && !CompBiocodable.IsBiocodedFor(newApparel, this.pawn))
			{
				CompBiocodable compBiocodable = newApparel.TryGetComp<CompBiocodable>();
				string[] array = new string[6];
				int num = 0;
				Pawn pawn2 = this.pawn;
				array[num] = ((pawn2 != null) ? pawn2.ToString() : null);
				array[1] = " tried to wear ";
				array[2] = ((newApparel != null) ? newApparel.ToString() : null);
				array[3] = " but it is biocoded for ";
				array[4] = compBiocodable.CodedPawnLabel;
				array[5] = " .";
				Log.Warning(string.Concat(array));
				return;
			}
			if (!newApparel.PawnCanWear(this.pawn, true))
			{
				Pawn pawn3 = this.pawn;
				Log.Warning(((pawn3 != null) ? pawn3.ToString() : null) + " tried to wear " + ((newApparel != null) ? newApparel.ToString() : null) + " but is not allowed to.");
				return;
			}
			for (int i = this.wornApparel.Count - 1; i >= 0; i--)
			{
				Apparel apparel = this.wornApparel[i];
				if (!ApparelUtility.CanWearTogether(newApparel.def, apparel.def, this.pawn.RaceProps.body))
				{
					if (dropReplacedApparel)
					{
						bool forbid = this.pawn.Faction != null && this.pawn.Faction.HostileTo(Faction.OfPlayer);
						Apparel apparel2;
						if (this.pawn.Map == null)
						{
							this.wornApparel.Remove(apparel);
						}
						else if (!this.TryDrop(apparel, out apparel2, this.pawn.PositionHeld, forbid))
						{
							Pawn pawn4 = this.pawn;
							string str = (pawn4 != null) ? pawn4.ToString() : null;
							string str2 = " could not drop ";
							Apparel apparel3 = apparel;
							Log.Error(str + str2 + ((apparel3 != null) ? apparel3.ToString() : null));
							return;
						}
					}
					else
					{
						this.Remove(apparel);
					}
				}
			}
			if (newApparel.Wearer != null)
			{
				string[] array2 = new string[6];
				int num2 = 0;
				Pawn pawn5 = this.pawn;
				array2[num2] = ((pawn5 != null) ? pawn5.ToString() : null);
				array2[1] = " is trying to wear ";
				array2[2] = ((newApparel != null) ? newApparel.ToString() : null);
				array2[3] = " but this apparel already has a wearer (";
				int num3 = 4;
				Pawn wearer = newApparel.Wearer;
				array2[num3] = ((wearer != null) ? wearer.ToString() : null);
				array2[5] = "). This may or may not cause bugs.";
				Log.Warning(string.Concat(array2));
			}
			this.wornApparel.TryAdd(newApparel, false);
			if (locked)
			{
				this.Lock(newApparel);
			}
		}

		// Token: 0x06008761 RID: 34657 RVA: 0x0030F16F File Offset: 0x0030D36F
		public void Remove(Apparel ap)
		{
			this.wornApparel.Remove(ap);
		}

		// Token: 0x06008762 RID: 34658 RVA: 0x0030F180 File Offset: 0x0030D380
		public bool TryDrop(Apparel ap)
		{
			Apparel apparel;
			return this.TryDrop(ap, out apparel);
		}

		// Token: 0x06008763 RID: 34659 RVA: 0x0030F196 File Offset: 0x0030D396
		public bool TryDrop(Apparel ap, out Apparel resultingAp)
		{
			return this.TryDrop(ap, out resultingAp, this.pawn.PositionHeld, true);
		}

		// Token: 0x06008764 RID: 34660 RVA: 0x0030F1AC File Offset: 0x0030D3AC
		public bool TryDrop(Apparel ap, out Apparel resultingAp, IntVec3 pos, bool forbid = true)
		{
			if (this.wornApparel.TryDrop(ap, pos, this.pawn.MapHeld, ThingPlaceMode.Near, out resultingAp, null, null))
			{
				if (resultingAp != null)
				{
					resultingAp.SetForbidden(forbid, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06008765 RID: 34661 RVA: 0x0030F1DD File Offset: 0x0030D3DD
		public bool TryMoveToInventory(Apparel apparel)
		{
			if (!this.wornApparel.Contains(apparel))
			{
				return false;
			}
			this.Remove(apparel);
			return this.pawn.inventory.innerContainer.TryAdd(apparel, true);
		}

		// Token: 0x06008766 RID: 34662 RVA: 0x0030F210 File Offset: 0x0030D410
		public void DropAll(IntVec3 pos, bool forbid = true, bool dropLocked = true, Predicate<Apparel> selector = null)
		{
			Pawn_ApparelTracker.tmpApparelList.Clear();
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				if ((dropLocked || !this.IsLocked(this.wornApparel[i])) && (selector == null || selector(this.wornApparel[i])))
				{
					Pawn_ApparelTracker.tmpApparelList.Add(this.wornApparel[i]);
				}
			}
			for (int j = 0; j < Pawn_ApparelTracker.tmpApparelList.Count; j++)
			{
				Apparel apparel;
				this.TryDrop(Pawn_ApparelTracker.tmpApparelList[j], out apparel, pos, forbid);
			}
		}

		// Token: 0x06008767 RID: 34663 RVA: 0x0030F2B0 File Offset: 0x0030D4B0
		public void MoveAllToInventory(bool moveLocked = true, Predicate<Apparel> selector = null)
		{
			Pawn_ApparelTracker.tmpApparelList.Clear();
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				if ((moveLocked || !this.IsLocked(this.wornApparel[i])) && (selector == null || selector(this.wornApparel[i])))
				{
					Pawn_ApparelTracker.tmpApparelList.Add(this.wornApparel[i]);
				}
			}
			for (int j = 0; j < Pawn_ApparelTracker.tmpApparelList.Count; j++)
			{
				this.TryMoveToInventory(Pawn_ApparelTracker.tmpApparelList[j]);
			}
		}

		// Token: 0x06008768 RID: 34664 RVA: 0x0030F348 File Offset: 0x0030D548
		public void DropAllOrMoveAllToInventory(Predicate<Apparel> selector = null)
		{
			if (this.pawn.SpawnedOrAnyParentSpawned)
			{
				this.DropAll(this.pawn.PositionHeld, false, true, selector);
				return;
			}
			this.MoveAllToInventory(true, selector);
		}

		// Token: 0x06008769 RID: 34665 RVA: 0x0030F374 File Offset: 0x0030D574
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.wornApparel.ClearAndDestroyContents(mode);
		}

		// Token: 0x0600876A RID: 34666 RVA: 0x0030F382 File Offset: 0x0030D582
		public bool Contains(Thing apparel)
		{
			return this.wornApparel.Contains(apparel);
		}

		// Token: 0x0600876B RID: 34667 RVA: 0x0030F390 File Offset: 0x0030D590
		public bool Wearing(Thing apparel)
		{
			using (List<Apparel>.Enumerator enumerator = this.wornApparel.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == apparel)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600876C RID: 34668 RVA: 0x0030F3E8 File Offset: 0x0030D5E8
		public bool WouldReplaceLockedApparel(Apparel newApparel)
		{
			if (!this.AnyApparelLocked)
			{
				return false;
			}
			for (int i = 0; i < this.lockedApparel.Count; i++)
			{
				if (!ApparelUtility.CanWearTogether(newApparel.def, this.lockedApparel[i].def, this.pawn.RaceProps.body))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600876D RID: 34669 RVA: 0x0030F448 File Offset: 0x0030D648
		public void Notify_PawnKilled(DamageInfo? dinfo)
		{
			if (dinfo != null && dinfo.Value.Def.ExternalViolenceFor(this.pawn))
			{
				for (int i = 0; i < this.wornApparel.Count; i++)
				{
					if (this.wornApparel[i].def.useHitPoints)
					{
						int num = Mathf.RoundToInt((float)this.wornApparel[i].HitPoints * Rand.Range(0.15f, 0.4f));
						this.wornApparel[i].TakeDamage(new DamageInfo(dinfo.Value.Def, (float)num, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
					}
				}
			}
			for (int j = 0; j < this.wornApparel.Count; j++)
			{
				this.wornApparel[j].Notify_PawnKilled();
			}
		}

		// Token: 0x0600876E RID: 34670 RVA: 0x0030F540 File Offset: 0x0030D740
		public void Notify_LostBodyPart()
		{
			Pawn_ApparelTracker.tmpApparel.Clear();
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Pawn_ApparelTracker.tmpApparel.Add(this.wornApparel[i]);
			}
			for (int j = 0; j < Pawn_ApparelTracker.tmpApparel.Count; j++)
			{
				Apparel apparel = Pawn_ApparelTracker.tmpApparel[j];
				if (!ApparelUtility.HasPartsToWear(this.pawn, apparel.def))
				{
					this.Remove(apparel);
				}
			}
		}

		// Token: 0x0600876F RID: 34671 RVA: 0x0030F5BE File Offset: 0x0030D7BE
		public void Notify_TitleChanged()
		{
			this.activeRequirementsDirty = true;
			this.allRequirementsDirty = true;
		}

		// Token: 0x06008770 RID: 34672 RVA: 0x0030F5CE File Offset: 0x0030D7CE
		public void Notify_RoleChanged()
		{
			this.activeRequirementsDirty = true;
			this.allRequirementsDirty = true;
		}

		// Token: 0x06008771 RID: 34673 RVA: 0x0030F5DE File Offset: 0x0030D7DE
		public void Notify_IdeoChanged()
		{
			this.activeRequirementsDirty = true;
			this.allRequirementsDirty = true;
		}

		// Token: 0x06008772 RID: 34674 RVA: 0x0030F5EE File Offset: 0x0030D7EE
		private void SortWornApparelIntoDrawOrder()
		{
			this.wornApparel.InnerListForReading.Sort((Apparel a, Apparel b) => a.def.apparel.LastLayer.drawOrder.CompareTo(b.def.apparel.LastLayer.drawOrder));
		}

		// Token: 0x06008773 RID: 34675 RVA: 0x0030F620 File Offset: 0x0030D820
		public void HasBasicApparel(out bool hasPants, out bool hasShirt)
		{
			hasShirt = false;
			hasPants = false;
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Apparel apparel = this.wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
					{
						hasShirt = true;
					}
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Legs)
					{
						hasPants = true;
					}
					if (hasShirt & hasPants)
					{
						return;
					}
				}
			}
		}

		// Token: 0x06008774 RID: 34676 RVA: 0x0030F6C4 File Offset: 0x0030D8C4
		public Apparel FirstApparelOnBodyPartGroup(BodyPartGroupDef g)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Apparel apparel = this.wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == g)
					{
						return apparel;
					}
				}
			}
			return null;
		}

		// Token: 0x06008775 RID: 34677 RVA: 0x0030F730 File Offset: 0x0030D930
		public bool TryGetFirstApparelOnBodyPart(BodyPartRecord record, out Apparel apparel, Predicate<Apparel> validator = null)
		{
			apparel = this.FirstApparelOnBodyPart(record, validator);
			return apparel != null;
		}

		// Token: 0x06008776 RID: 34678 RVA: 0x0030F744 File Offset: 0x0030D944
		public Apparel FirstApparelOnBodyPart(BodyPartRecord record, Predicate<Apparel> validator = null)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Apparel apparel = this.wornApparel[i];
				if (apparel.def.apparel.CoversBodyPart(record) && (validator == null || validator(apparel)))
				{
					return apparel;
				}
			}
			return null;
		}

		// Token: 0x06008777 RID: 34679 RVA: 0x0030F798 File Offset: 0x0030D998
		public bool BodyPartGroupIsCovered(BodyPartGroupDef bp, Predicate<Apparel> validator = null)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Apparel apparel = this.wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == bp && (validator == null || validator(apparel)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06008778 RID: 34680 RVA: 0x0030F810 File Offset: 0x0030DA10
		public IEnumerable<Gizmo> GetGizmos()
		{
			Pawn_ApparelTracker.<GetGizmos>d__80 <GetGizmos>d__ = new Pawn_ApparelTracker.<GetGizmos>d__80(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06008779 RID: 34681 RVA: 0x0030F820 File Offset: 0x0030DA20
		public void Notify_ApparelChanged()
		{
			this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			Pawn_AbilityTracker abilities = this.pawn.abilities;
			if (abilities != null)
			{
				abilities.Notify_TemporaryAbilitiesChanged();
			}
			if (ModsConfig.BiotechActive && this.pawn.mechanitor != null)
			{
				this.pawn.mechanitor.Notify_ApparelChanged();
			}
		}

		// Token: 0x0600877A RID: 34682 RVA: 0x0030F87C File Offset: 0x0030DA7C
		public void Notify_ApparelAdded(Apparel apparel)
		{
			this.SortWornApparelIntoDrawOrder();
			this.Notify_ApparelChanged();
			CompApparelVerbOwner_Charged comp = apparel.GetComp<CompApparelVerbOwner_Charged>();
			List<Verb> list = (comp != null) ? comp.AllVerbs : null;
			if (list != null)
			{
				foreach (Verb verb in list)
				{
					verb.caster = this.pawn;
					verb.Notify_PickedUp();
				}
			}
			if (!apparel.def.equippedStatOffsets.NullOrEmpty<StatModifier>())
			{
				this.pawn.health.capacities.Notify_CapacityLevelsDirty();
			}
			apparel.Notify_Equipped(this.pawn);
		}

		// Token: 0x0600877B RID: 34683 RVA: 0x0030F928 File Offset: 0x0030DB28
		public void Notify_ApparelRemoved(Apparel apparel)
		{
			this.Notify_ApparelChanged();
			if (this.pawn.outfits != null && this.pawn.outfits.forcedHandler != null)
			{
				this.pawn.outfits.forcedHandler.SetForced(apparel, false);
			}
			if (this.IsLocked(apparel))
			{
				this.Unlock(apparel);
			}
			if (!apparel.def.equippedStatOffsets.NullOrEmpty<StatModifier>())
			{
				this.pawn.health.capacities.Notify_CapacityLevelsDirty();
			}
			apparel.Notify_Unequipped(this.pawn);
		}

		// Token: 0x0600877C RID: 34684 RVA: 0x0030F9B4 File Offset: 0x0030DBB4
		public void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				this.wornApparel[i].Notify_BulletImpactNearby(impactData);
			}
		}

		// Token: 0x0600877D RID: 34685 RVA: 0x0030F9E9 File Offset: 0x0030DBE9
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.wornApparel;
		}

		// Token: 0x0600877E RID: 34686 RVA: 0x0030F9F1 File Offset: 0x0030DBF1
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x040053A1 RID: 21409
		public Pawn pawn;

		// Token: 0x040053A2 RID: 21410
		private ThingOwner<Apparel> wornApparel;

		// Token: 0x040053A3 RID: 21411
		private List<Apparel> lockedApparel;

		// Token: 0x040053A4 RID: 21412
		private int lastApparelWearoutTick = -1;

		// Token: 0x040053A5 RID: 21413
		private List<ApparelRequirementWithSource> allRequirements = new List<ApparelRequirementWithSource>();

		// Token: 0x040053A6 RID: 21414
		private List<ApparelRequirement> activeRequirements = new List<ApparelRequirement>();

		// Token: 0x040053A7 RID: 21415
		private bool activeRequirementsDirty = true;

		// Token: 0x040053A8 RID: 21416
		private bool allRequirementsDirty = true;

		// Token: 0x040053A9 RID: 21417
		private const int RecordWalkedNakedTaleIntervalTicks = 60000;

		// Token: 0x040053AA RID: 21418
		private const float AutoUnlockHealthPctThreshold = 0.5f;

		// Token: 0x040053AB RID: 21419
		private static readonly List<Apparel> EmptyApparel = new List<Apparel>();

		// Token: 0x040053AC RID: 21420
		private static List<Apparel> tmpApparelList = new List<Apparel>();

		// Token: 0x040053AD RID: 21421
		private static List<Apparel> tmpApparel = new List<Apparel>();
	}
}
