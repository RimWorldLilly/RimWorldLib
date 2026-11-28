using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015AC RID: 5548
	[StaticConstructorOnStartup]
	public class Pawn_TrainingTracker : IExposable
	{
		// Token: 0x06008967 RID: 35175 RVA: 0x0031C1CE File Offset: 0x0031A3CE
		public Pawn_TrainingTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.countDecayFrom = Find.TickManager.TicksGame;
		}

		// Token: 0x06008968 RID: 35176 RVA: 0x0031C210 File Offset: 0x0031A410
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<TrainableDef, bool>>(ref this.wantedTrainables, "wantedTrainables", Array.Empty<object>());
			Scribe_Deep.Look<DefMap<TrainableDef, int>>(ref this.steps, "steps", Array.Empty<object>());
			Scribe_Deep.Look<DefMap<TrainableDef, bool>>(ref this.learned, "learned", Array.Empty<object>());
			Scribe_References.Look<Thing>(ref this.attackTarget, "attackTarget", false);
			Scribe_Values.Look<int>(ref this.countDecayFrom, "countDecayFrom", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.PawnTrainingTrackerPostLoadInit(this, ref this.wantedTrainables, ref this.steps, ref this.learned);
			}
		}

		// Token: 0x06008969 RID: 35177 RVA: 0x0031C29F File Offset: 0x0031A49F
		public bool GetWanted(TrainableDef td)
		{
			return this.wantedTrainables[td];
		}

		// Token: 0x0600896A RID: 35178 RVA: 0x0031C2AD File Offset: 0x0031A4AD
		private void SetWanted(TrainableDef td, bool wanted)
		{
			this.wantedTrainables[td] = wanted;
		}

		// Token: 0x0600896B RID: 35179 RVA: 0x0031C2BC File Offset: 0x0031A4BC
		internal int GetSteps(TrainableDef td)
		{
			return this.steps[td];
		}

		// Token: 0x0600896C RID: 35180 RVA: 0x0031C2CC File Offset: 0x0031A4CC
		public bool CanBeTrained(TrainableDef td)
		{
			if (this.steps[td] >= td.steps)
			{
				return false;
			}
			List<TrainableDef> prerequisites = td.prerequisites;
			if (!prerequisites.NullOrEmpty<TrainableDef>())
			{
				for (int i = 0; i < prerequisites.Count; i++)
				{
					if (!this.HasLearned(prerequisites[i]) || this.CanBeTrained(prerequisites[i]))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600896D RID: 35181 RVA: 0x0031C330 File Offset: 0x0031A530
		public bool HasLearned(TrainableDef td)
		{
			TrainabilityDef trainability = TrainableUtility.GetTrainability(this.pawn);
			if (td.requiredTrainability != null && trainability != null)
			{
				int intelligenceOrder = trainability.intelligenceOrder;
				TrainabilityDef requiredTrainability = td.requiredTrainability;
				int? num = (requiredTrainability != null) ? new int?(requiredTrainability.intelligenceOrder) : null;
				if (intelligenceOrder < num.GetValueOrDefault() & num != null)
				{
					return false;
				}
			}
			return this.learned[td];
		}

		// Token: 0x0600896E RID: 35182 RVA: 0x0031C39C File Offset: 0x0031A59C
		public AcceptanceReport CanAssignToTrain(TrainableDef td)
		{
			bool flag;
			return this.CanAssignToTrain(td, out flag);
		}

		// Token: 0x0600896F RID: 35183 RVA: 0x0031C3B2 File Offset: 0x0031A5B2
		public AcceptanceReport CanAssignToTrain(TrainableDef td, out bool visible)
		{
			return Pawn_TrainingTracker.CanAssignToTrain(td, this.pawn.def, out visible, this.pawn);
		}

		// Token: 0x06008970 RID: 35184 RVA: 0x0031C3CC File Offset: 0x0031A5CC
		public static AcceptanceReport CanAssignToTrain(TrainableDef td, ThingDef pawnDef, out bool visible, Pawn pawn = null)
		{
			if (pawnDef.race.untrainableTags != null)
			{
				for (int i = 0; i < pawnDef.race.untrainableTags.Count; i++)
				{
					if (td.MatchesTag(pawnDef.race.untrainableTags[i]))
					{
						visible = false;
						return false;
					}
				}
			}
			if (ModsConfig.OdysseyActive && td.specialTrainable)
			{
				List<TrainableDef> specialTrainables = pawnDef.race.specialTrainables;
				if (specialTrainables == null || !specialTrainables.Contains(td))
				{
					visible = false;
					return false;
				}
			}
			float num = (pawn != null) ? pawn.BodySize : pawnDef.race.baseBodySize;
			string value = ((pawn != null) ? pawn.LabelShort : null) ?? pawnDef.label;
			TrainabilityDef trainabilityDef = TrainableUtility.GetTrainability(pawn) ?? pawnDef.race.trainability;
			if (pawnDef.race.trainableTags != null)
			{
				int j = 0;
				while (j < pawnDef.race.trainableTags.Count)
				{
					if (td.MatchesTag(pawnDef.race.trainableTags[j]))
					{
						if (num < td.minBodySize)
						{
							visible = true;
							return new AcceptanceReport("CannotTrainTooSmall".Translate(value).Resolve());
						}
						visible = true;
						return true;
					}
					else
					{
						j++;
					}
				}
			}
			if (!td.defaultTrainable && !td.specialTrainable)
			{
				visible = false;
				return false;
			}
			if (num < td.minBodySize)
			{
				visible = true;
				return new AcceptanceReport("CannotTrainTooSmall".Translate(value).Resolve());
			}
			if (td.requiredTrainability != null && trainabilityDef.intelligenceOrder < td.requiredTrainability.intelligenceOrder)
			{
				visible = true;
				return new AcceptanceReport("CannotTrainNotSmartEnough".Translate(td.requiredTrainability.label).Resolve());
			}
			visible = true;
			return true;
		}

		// Token: 0x06008971 RID: 35185 RVA: 0x0031C5B0 File Offset: 0x0031A7B0
		public TrainableDef NextTrainableToTrain()
		{
			List<TrainableDef> trainableDefsInListOrder = TrainableUtility.TrainableDefsInListOrder;
			for (int i = 0; i < trainableDefsInListOrder.Count; i++)
			{
				if (this.GetWanted(trainableDefsInListOrder[i]) && this.CanBeTrained(trainableDefsInListOrder[i]))
				{
					return trainableDefsInListOrder[i];
				}
			}
			return null;
		}

		// Token: 0x06008972 RID: 35186 RVA: 0x0031C5FC File Offset: 0x0031A7FC
		public void Train(TrainableDef td, Pawn trainer, bool complete = false)
		{
			if (complete)
			{
				this.steps[td] = td.steps;
			}
			else
			{
				DefMap<TrainableDef, int> defMap = this.steps;
				int num = defMap[td];
				defMap[td] = num + 1;
			}
			if (this.steps[td] >= td.steps)
			{
				this.learned[td] = true;
				if (td == TrainableDefOf.Obedience && trainer != null && this.pawn.playerSettings != null && this.pawn.playerSettings.Master == null)
				{
					this.pawn.playerSettings.Master = trainer;
				}
			}
		}

		// Token: 0x06008973 RID: 35187 RVA: 0x0031C698 File Offset: 0x0031A898
		public void SetWantedRecursive(TrainableDef td, bool checkOn)
		{
			this.SetWanted(td, checkOn);
			if (checkOn)
			{
				if (td.prerequisites != null)
				{
					for (int i = 0; i < td.prerequisites.Count; i++)
					{
						this.SetWantedRecursive(td.prerequisites[i], true);
					}
					return;
				}
			}
			else
			{
				foreach (TrainableDef td2 in from t in DefDatabase<TrainableDef>.AllDefsListForReading
				where t.prerequisites != null && t.prerequisites.Contains(td)
				select t)
				{
					this.SetWantedRecursive(td2, false);
				}
			}
		}

		// Token: 0x06008974 RID: 35188 RVA: 0x0031C754 File Offset: 0x0031A954
		public void TrainingTrackerTickRare()
		{
			if (this.pawn.Suspended)
			{
				this.countDecayFrom += 250;
				return;
			}
			if (!this.pawn.Spawned)
			{
				this.countDecayFrom += 250;
				return;
			}
			if (this.steps[TrainableDefOf.Tameness] == 0)
			{
				this.countDecayFrom = Find.TickManager.TicksGame;
				return;
			}
			if (Find.TickManager.TicksGame < this.countDecayFrom + TrainableUtility.DegradationPeriodTicks(this.pawn))
			{
				return;
			}
			if (this.pawn.RaceProps.animalType == AnimalType.Dryad)
			{
				return;
			}
			TrainableDef trainableDef = (from kvp in this.steps
			where kvp.Value > 0
			select kvp.Key).Except((from kvp in this.steps
			where kvp.Value > 0 && kvp.Key.prerequisites != null
			select kvp).SelectMany((KeyValuePair<TrainableDef, int> kvp) => kvp.Key.prerequisites)).RandomElement<TrainableDef>();
			if (trainableDef == TrainableDefOf.Tameness && !TrainableUtility.TamenessCanDecay(this.pawn))
			{
				this.countDecayFrom = Find.TickManager.TicksGame;
				return;
			}
			this.countDecayFrom = Find.TickManager.TicksGame;
			DefMap<TrainableDef, int> defMap = this.steps;
			TrainableDef def = trainableDef;
			int value = defMap[def] - 1;
			defMap[def] = value;
			if (this.steps[trainableDef] <= 0 && this.learned[trainableDef])
			{
				this.learned[trainableDef] = false;
				if (this.pawn.Faction == Faction.OfPlayer)
				{
					if (trainableDef == TrainableDefOf.Tameness)
					{
						this.pawn.SetFaction(null, null);
						Messages.Message("MessageAnimalReturnedWild".Translate(this.pawn.LabelShort, this.pawn), this.pawn, MessageTypeDefOf.NegativeEvent, true);
						return;
					}
					Messages.Message("MessageAnimalLostSkill".Translate(this.pawn.LabelShort, trainableDef.LabelCap, this.pawn.Named("ANIMAL")), this.pawn, MessageTypeDefOf.NegativeEvent, true);
				}
			}
		}

		// Token: 0x06008975 RID: 35189 RVA: 0x0031C9D8 File Offset: 0x0031ABD8
		private static bool CanEverDoAttackTarget(Pawn pawn)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			Pawn_TrainingTracker training = pawn.training;
			return training != null && training.HasLearned(TrainableDefOf.AttackTarget);
		}

		// Token: 0x06008976 RID: 35190 RVA: 0x0031C9FC File Offset: 0x0031ABFC
		private static AcceptanceReport CanDoAttackTarget(Pawn pawn)
		{
			if (!Pawn_TrainingTracker.CanEverDoAttackTarget(pawn))
			{
				return false;
			}
			if (pawn.playerSettings.Master == null)
			{
				return "NoMaster".Translate();
			}
			if (!pawn.playerSettings.Master.Spawned || pawn.playerSettings.Master.Map != pawn.Map)
			{
				return "MasterNotSpawned".Translate();
			}
			return true;
		}

		// Token: 0x06008977 RID: 35191 RVA: 0x0031CA75 File Offset: 0x0031AC75
		public IEnumerable<Gizmo> GetGizmos()
		{
			Pawn_TrainingTracker.<GetGizmos>d__25 <GetGizmos>d__ = new Pawn_TrainingTracker.<GetGizmos>d__25(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06008978 RID: 35192 RVA: 0x0031CA85 File Offset: 0x0031AC85
		public void Debug_MakeDegradeHappenSoon()
		{
			this.countDecayFrom = Find.TickManager.TicksGame - TrainableUtility.DegradationPeriodTicks(this.pawn) - 500;
		}

		// Token: 0x0400549A RID: 21658
		public Pawn pawn;

		// Token: 0x0400549B RID: 21659
		private DefMap<TrainableDef, bool> wantedTrainables = new DefMap<TrainableDef, bool>();

		// Token: 0x0400549C RID: 21660
		private DefMap<TrainableDef, int> steps = new DefMap<TrainableDef, int>();

		// Token: 0x0400549D RID: 21661
		private DefMap<TrainableDef, bool> learned = new DefMap<TrainableDef, bool>();

		// Token: 0x0400549E RID: 21662
		public Thing attackTarget;

		// Token: 0x0400549F RID: 21663
		private int countDecayFrom;

		// Token: 0x040054A0 RID: 21664
		public const float AttackTargetRange = 25.9f;

		// Token: 0x040054A1 RID: 21665
		public static readonly Texture2D AttackTargetTexture = ContentFinder<Texture2D>.Get("UI/Commands/AnimalAttack", true);

		// Token: 0x040054A2 RID: 21666
		private static readonly Texture2D CancelIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);
	}
}
