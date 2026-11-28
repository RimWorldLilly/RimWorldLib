using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001992 RID: 6546
	public class CompSpawnSubplant : ThingComp
	{
		// Token: 0x17001C89 RID: 7305
		// (get) Token: 0x0600A448 RID: 42056 RVA: 0x003AB0E8 File Offset: 0x003A92E8
		public CompProperties_SpawnSubplant Props
		{
			get
			{
				return (CompProperties_SpawnSubplant)this.props;
			}
		}

		// Token: 0x17001C8A RID: 7306
		// (get) Token: 0x0600A449 RID: 42057 RVA: 0x003AB0F5 File Offset: 0x003A92F5
		public List<Thing> SubplantsForReading
		{
			get
			{
				this.Cleanup();
				return this.subplants;
			}
		}

		// Token: 0x17001C8B RID: 7307
		// (get) Token: 0x0600A44A RID: 42058 RVA: 0x003AB104 File Offset: 0x003A9304
		private float ProgressMultiplier
		{
			get
			{
				foreach (Pair<int, float> pair in CompSpawnSubplant.TicksToProgressMultipliers)
				{
					if (this.meditationTicksToday < pair.First)
					{
						return pair.Second;
					}
				}
				return CompSpawnSubplant.TicksToProgressMultipliers.Last<Pair<int, float>>().Second;
			}
		}

		// Token: 0x0600A44B RID: 42059 RVA: 0x003AB17C File Offset: 0x003A937C
		public void AddProgress(float progressPerTick, bool ignoreMultiplier = false)
		{
			if (!ModLister.CheckRoyalty("Subplant spawning"))
			{
				return;
			}
			if (!ignoreMultiplier)
			{
				progressPerTick *= this.ProgressMultiplier;
			}
			this.progressToNextSubplant += progressPerTick * (1f + this.parent.GetStatValue(StatDefOf.MeditationPlantGrowthOffset, true, -1));
			this.meditationTicksToday++;
			this.TryGrowSubplants();
		}

		// Token: 0x0600A44C RID: 42060 RVA: 0x003AB1DE File Offset: 0x003A93DE
		public override void CompTickLong()
		{
			if (GenLocalDate.DayTick(this.parent.Map) < 2000)
			{
				this.meditationTicksToday = 0;
			}
		}

		// Token: 0x0600A44D RID: 42061 RVA: 0x003AB1FE File Offset: 0x003A93FE
		public void Cleanup()
		{
			this.subplants.RemoveAll((Thing p) => !p.Spawned);
		}

		// Token: 0x0600A44E RID: 42062 RVA: 0x003AB22C File Offset: 0x003A942C
		public override string CompInspectStringExtra()
		{
			return "TotalMeditationToday".Translate((this.meditationTicksToday / 2500).ToString() + "LetterHour".Translate(), this.ProgressMultiplier.ToStringPercent()) + "\n" + this.Props.subplant.LabelCap + ": " + this.SubplantsForReading.Count.ToString() + " (" + "ProgressToNextSubplant".Translate(this.progressToNextSubplant.ToStringPercent()) + ")";
		}

		// Token: 0x0600A44F RID: 42063 RVA: 0x003AB2F4 File Offset: 0x003A94F4
		private void TryGrowSubplants()
		{
			while (this.progressToNextSubplant >= 1f)
			{
				this.DoGrowSubplant();
				this.progressToNextSubplant -= 1f;
			}
		}

		// Token: 0x0600A450 RID: 42064 RVA: 0x003AB320 File Offset: 0x003A9520
		private void DoGrowSubplant()
		{
			IntVec3 position = this.parent.Position;
			for (int i = 0; i < 1000; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.parent.Map) && WanderUtility.InSameRoom(position, intVec, this.parent.Map))
				{
					bool flag = false;
					List<Thing> thingList = intVec.GetThingList(this.parent.Map);
					foreach (Thing thing in thingList)
					{
						if (thing.def == this.Props.subplant)
						{
							flag = true;
							break;
						}
						if (!this.Props.plantsToNotOverwrite.NullOrEmpty<ThingDef>())
						{
							for (int j = 0; j < this.Props.plantsToNotOverwrite.Count; j++)
							{
								if (thing.def == this.Props.plantsToNotOverwrite[j])
								{
									flag = true;
									break;
								}
							}
						}
					}
					if (!flag && this.Props.subplant.CanEverPlantAt(intVec, this.parent.Map, true, false))
					{
						for (int k = thingList.Count - 1; k >= 0; k--)
						{
							if (thingList[k].def.category == ThingCategory.Plant)
							{
								thingList[k].Destroy(DestroyMode.Vanish);
							}
						}
						Plant plant = (Plant)GenSpawn.Spawn(this.Props.subplant, intVec, this.parent.Map, WipeMode.Vanish);
						this.subplants.Add(plant);
						if (this.Props.initialGrowthRange != null)
						{
							plant.Growth = this.Props.initialGrowthRange.Value.RandomInRange;
						}
						if (this.Props.spawnSound != null)
						{
							this.Props.spawnSound.PlayOneShot(new TargetInfo(this.parent));
						}
						Action action = this.onGrassGrown;
						if (action == null)
						{
							return;
						}
						action();
						return;
					}
				}
			}
		}

		// Token: 0x0600A451 RID: 42065 RVA: 0x003AB550 File Offset: 0x003A9750
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompSpawnSubplant.<CompGetGizmosExtra>d__17 <CompGetGizmosExtra>d__ = new CompSpawnSubplant.<CompGetGizmosExtra>d__17(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A452 RID: 42066 RVA: 0x003AB560 File Offset: 0x003A9760
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.progressToNextSubplant, "progressToNextSubplant", 0f, false);
			Scribe_Collections.Look<Thing>(ref this.subplants, "subplants", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.meditationTicksToday, "meditationTicksToday", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.subplants.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04006127 RID: 24871
		private float progressToNextSubplant;

		// Token: 0x04006128 RID: 24872
		private List<Thing> subplants = new List<Thing>();

		// Token: 0x04006129 RID: 24873
		private int meditationTicksToday;

		// Token: 0x0400612A RID: 24874
		public Action onGrassGrown;

		// Token: 0x0400612B RID: 24875
		private static readonly List<Pair<int, float>> TicksToProgressMultipliers = new List<Pair<int, float>>
		{
			new Pair<int, float>(30000, 1f),
			new Pair<int, float>(60000, 0.5f),
			new Pair<int, float>(120000, 0.25f),
			new Pair<int, float>(240000, 0.15f)
		};
	}
}
