using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001597 RID: 5527
	public class Pawn_FilthTracker : IExposable
	{
		// Token: 0x1700173A RID: 5946
		// (get) Token: 0x060087D3 RID: 34771 RVA: 0x00311934 File Offset: 0x0030FB34
		public string FilthReport
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("FilthOnFeet".Translate());
				if (this.carriedFilth.Count == 0)
				{
					stringBuilder.Append("(" + "NoneLower".Translate() + ")");
				}
				else
				{
					for (int i = 0; i < this.carriedFilth.Count; i++)
					{
						stringBuilder.AppendLine(this.carriedFilth[i].LabelCap);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700173B RID: 5947
		// (get) Token: 0x060087D4 RID: 34772 RVA: 0x003119CA File Offset: 0x0030FBCA
		public List<Filth> CarriedFilthListForReading
		{
			get
			{
				return this.carriedFilth;
			}
		}

		// Token: 0x1700173C RID: 5948
		// (get) Token: 0x060087D5 RID: 34773 RVA: 0x003119D2 File Offset: 0x0030FBD2
		private FilthSourceFlags AdditionalFilthSourceFlags
		{
			get
			{
				if (this.pawn.Faction != null || !this.pawn.RaceProps.Animal)
				{
					return FilthSourceFlags.Unnatural;
				}
				return FilthSourceFlags.Natural;
			}
		}

		// Token: 0x060087D6 RID: 34774 RVA: 0x003119F6 File Offset: 0x0030FBF6
		public Pawn_FilthTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060087D7 RID: 34775 RVA: 0x00311A10 File Offset: 0x0030FC10
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.lastTerrainFilthDef, "lastTerrainFilthDef");
			Scribe_Collections.Look<Filth>(ref this.carriedFilth, "carriedFilth", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.carriedFilth.RemoveAll((Filth x) => x == null) != 0)
				{
					Log.Error(this.pawn.ToStringSafe<Pawn>() + " had null carried filth after loading.");
				}
				if (this.carriedFilth.RemoveAll((Filth x) => x.def == null) != 0)
				{
					Log.Error(this.pawn.ToStringSafe<Pawn>() + " had carried filth with null def after loading.");
				}
			}
		}

		// Token: 0x060087D8 RID: 34776 RVA: 0x00311ADC File Offset: 0x0030FCDC
		public void Notify_EnteredNewCell()
		{
			if (this.pawn.Flying)
			{
				return;
			}
			if (Rand.Value < 0.05f)
			{
				this.TryDropFilth();
			}
			if (Rand.Value < 0.1f)
			{
				this.TryPickupFilth();
			}
			if (Rand.Value < this.pawn.GetStatValue(StatDefOf.FilthRate, true, -1) * 0.005f)
			{
				if (this.pawn.RaceProps.Humanlike)
				{
					ThingDef filth_Trash;
					if (this.lastTerrainFilthDef != null && Rand.Chance(0.66f))
					{
						filth_Trash = this.lastTerrainFilthDef;
					}
					else
					{
						filth_Trash = ThingDefOf.Filth_Trash;
					}
					if (FilthMaker.TryMakeFilth(this.pawn.Position, this.pawn.Map, filth_Trash, 1, this.AdditionalFilthSourceFlags | FilthSourceFlags.Pawn, true))
					{
						FilthMonitor.Notify_FilthHumanGenerated();
						return;
					}
				}
				else if (this.pawn.RaceProps.Insect)
				{
					if (FilthMaker.TryMakeFilth(this.pawn.Position, this.pawn.Map, ThingDefOf.Filth_Slime, 1, this.AdditionalFilthSourceFlags | FilthSourceFlags.Pawn, true))
					{
						FilthMonitor.Notify_FilthAnimalGenerated();
						return;
					}
				}
				else if (this.pawn.IsAnimal && FilthMaker.TryMakeFilth(this.pawn.Position, this.pawn.Map, ThingDefOf.Filth_AnimalFilth, 1, this.AdditionalFilthSourceFlags | FilthSourceFlags.Pawn, true))
				{
					FilthMonitor.Notify_FilthAnimalGenerated();
				}
			}
		}

		// Token: 0x060087D9 RID: 34777 RVA: 0x00311C28 File Offset: 0x0030FE28
		private void TryPickupFilth()
		{
			TerrainDef terrDef = this.pawn.Map.terrainGrid.TerrainAt(this.pawn.Position);
			if (terrDef.generatedFilth != null)
			{
				for (int i = this.carriedFilth.Count - 1; i >= 0; i--)
				{
					if (this.carriedFilth[i].def.filth.TerrainSourced && this.carriedFilth[i].def != terrDef.generatedFilth)
					{
						this.ThinCarriedFilth(this.carriedFilth[i]);
					}
				}
				Filth filth = this.carriedFilth.FirstOrDefault((Filth f) => f.def == terrDef.generatedFilth);
				if (filth == null || filth.thickness < 1)
				{
					this.GainFilth(terrDef.generatedFilth);
					FilthMonitor.Notify_FilthAccumulated();
				}
			}
			List<Thing> thingList = this.pawn.Position.GetThingList(this.pawn.Map);
			for (int j = thingList.Count - 1; j >= 0; j--)
			{
				Filth filth2 = thingList[j] as Filth;
				if (filth2 != null && filth2.CanFilthAttachNow)
				{
					this.GainFilth(filth2.def, filth2.sources);
					filth2.ThinFilth();
				}
			}
			if (!this.pawn.Position.Roofed(this.pawn.Map))
			{
				foreach (GameCondition gameCondition in this.pawn.Map.GameConditionManager.ActiveConditions)
				{
					if (gameCondition.def.spreadsFilth != null)
					{
						this.GainFilth(gameCondition.def.spreadsFilth, Gen.YieldSingle<string>(gameCondition.Label));
					}
				}
			}
		}

		// Token: 0x060087DA RID: 34778 RVA: 0x00311E14 File Offset: 0x00310014
		private void TryDropFilth()
		{
			if (this.carriedFilth.Count == 0)
			{
				return;
			}
			for (int i = this.carriedFilth.Count - 1; i >= 0; i--)
			{
				if (this.carriedFilth[i].CanDropAt(this.pawn.Position, this.pawn.Map, FilthSourceFlags.None))
				{
					this.DropCarriedFilth(this.carriedFilth[i]);
					FilthMonitor.Notify_FilthDropped();
				}
			}
		}

		// Token: 0x060087DB RID: 34779 RVA: 0x00311E88 File Offset: 0x00310088
		private void DropCarriedFilth(Filth f)
		{
			if (FilthMaker.TryMakeFilth(this.pawn.Position, this.pawn.Map, f.def, f.sources, this.AdditionalFilthSourceFlags))
			{
				this.ThinCarriedFilth(f);
			}
		}

		// Token: 0x060087DC RID: 34780 RVA: 0x00311EC0 File Offset: 0x003100C0
		private void ThinCarriedFilth(Filth f)
		{
			f.ThinFilth();
			if (f.thickness <= 0)
			{
				this.carriedFilth.Remove(f);
			}
		}

		// Token: 0x060087DD RID: 34781 RVA: 0x00311EDE File Offset: 0x003100DE
		public void GainFilth(ThingDef filthDef)
		{
			if (filthDef.filth.TerrainSourced)
			{
				this.lastTerrainFilthDef = filthDef;
			}
			this.GainFilth(filthDef, null);
		}

		// Token: 0x060087DE RID: 34782 RVA: 0x00311EFC File Offset: 0x003100FC
		public void GainFilth(ThingDef filthDef, IEnumerable<string> sources)
		{
			if (filthDef.filth.TerrainSourced)
			{
				this.lastTerrainFilthDef = filthDef;
			}
			Filth filth = null;
			for (int i = 0; i < this.carriedFilth.Count; i++)
			{
				if (this.carriedFilth[i].def == filthDef)
				{
					filth = this.carriedFilth[i];
					break;
				}
			}
			if (filth != null)
			{
				if (filth.CanBeThickened)
				{
					filth.ThickenFilth();
					filth.AddSources(sources);
					return;
				}
			}
			else
			{
				Filth filth2 = (Filth)ThingMaker.MakeThing(filthDef, null);
				filth2.AddSources(sources);
				this.carriedFilth.Add(filth2);
			}
		}

		// Token: 0x040053D9 RID: 21465
		private readonly Pawn pawn;

		// Token: 0x040053DA RID: 21466
		private List<Filth> carriedFilth = new List<Filth>();

		// Token: 0x040053DB RID: 21467
		private ThingDef lastTerrainFilthDef;

		// Token: 0x040053DC RID: 21468
		private const float FilthPickupChance = 0.1f;

		// Token: 0x040053DD RID: 21469
		private const float FilthDropChance = 0.05f;

		// Token: 0x040053DE RID: 21470
		private const int MaxCarriedTerrainFilthThickness = 1;

		// Token: 0x040053DF RID: 21471
		private const float BaseChanceToSpreadPerCell = 0.005f;
	}
}
