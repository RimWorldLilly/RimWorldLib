using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FDD RID: 8157
	public class ThreatsGeneratorParams : IExposable
	{
		// Token: 0x0600CEF7 RID: 52983 RVA: 0x004C3D04 File Offset: 0x004C1F04
		public void ExposeData()
		{
			Scribe_Values.Look<AllowedThreatsGeneratorThreats>(ref this.allowedThreats, "allowedThreats", AllowedThreatsGeneratorThreats.None, false);
			Scribe_Values.Look<int>(ref this.randSeed, "randSeed", 0, false);
			Scribe_Values.Look<float>(ref this.onDays, "onDays", 0f, false);
			Scribe_Values.Look<float>(ref this.offDays, "offDays", 0f, false);
			Scribe_Values.Look<float>(ref this.minSpacingDays, "minSpacingDays", 0f, false);
			Scribe_Values.Look<FloatRange>(ref this.numIncidentsRange, "numIncidentsRange", default(FloatRange), false);
			Scribe_Values.Look<float?>(ref this.threatPoints, "threatPoints", null, false);
			Scribe_Values.Look<float?>(ref this.minThreatPoints, "minThreatPoints", null, false);
			Scribe_Values.Look<float>(ref this.currentThreatPointsFactor, "currentThreatPointsFactor", 1f, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x0600CEF8 RID: 52984 RVA: 0x004C3DEC File Offset: 0x004C1FEC
		public override string ToString()
		{
			string text = "(";
			text = text + "onDays=" + this.onDays.ToString("0.##");
			text = text + " offDays=" + this.offDays.ToString("0.##");
			text = text + " minSpacingDays=" + this.minSpacingDays.ToString("0.##");
			string str = text;
			string str2 = " numIncidentsRange=";
			FloatRange floatRange = this.numIncidentsRange;
			text = str + str2 + floatRange.ToString();
			if (this.threatPoints != null)
			{
				text = text + " threatPoints=" + this.threatPoints.Value.ToString();
			}
			if (this.minThreatPoints != null)
			{
				text = text + " minThreatPoints=" + this.minThreatPoints.Value.ToString();
			}
			if (this.faction != null)
			{
				string str3 = text;
				string str4 = " faction=";
				Faction faction = this.faction;
				text = str3 + str4 + ((faction != null) ? faction.ToString() : null);
			}
			return text + ")";
		}

		// Token: 0x040083B1 RID: 33713
		public AllowedThreatsGeneratorThreats allowedThreats;

		// Token: 0x040083B2 RID: 33714
		public int randSeed;

		// Token: 0x040083B3 RID: 33715
		public float onDays;

		// Token: 0x040083B4 RID: 33716
		public float offDays;

		// Token: 0x040083B5 RID: 33717
		public float minSpacingDays;

		// Token: 0x040083B6 RID: 33718
		public FloatRange numIncidentsRange;

		// Token: 0x040083B7 RID: 33719
		public float? threatPoints;

		// Token: 0x040083B8 RID: 33720
		public float? minThreatPoints;

		// Token: 0x040083B9 RID: 33721
		public float currentThreatPointsFactor = 1f;

		// Token: 0x040083BA RID: 33722
		public Faction faction;
	}
}
