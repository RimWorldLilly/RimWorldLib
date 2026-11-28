using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004CD RID: 1229
	public class ImmunityRecord : IExposable
	{
		// Token: 0x06002246 RID: 8774 RVA: 0x000CB814 File Offset: 0x000C9A14
		public void ExposeData()
		{
			Scribe_Defs.Look<HediffDef>(ref this.hediffDef, "hediffDef");
			Scribe_Defs.Look<HediffDef>(ref this.source, "source");
			Scribe_Values.Look<float>(ref this.immunity, "immunity", 0f, false);
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x000CB84C File Offset: 0x000C9A4C
		public void ImmunityTickInterval(Pawn pawn, bool sick, Hediff diseaseInstance, int delta)
		{
			this.immunity += this.ImmunityChangePerTick(pawn, sick, diseaseInstance) * (float)delta;
			this.immunity = Mathf.Clamp01(this.immunity);
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x000CB87C File Offset: 0x000C9A7C
		public float ImmunityChangePerTick(Pawn pawn, bool sick, Hediff diseaseInstance)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return 0f;
			}
			HediffCompProperties_Immunizable hediffCompProperties_Immunizable = this.hediffDef.CompProps<HediffCompProperties_Immunizable>();
			if (sick)
			{
				float num = hediffCompProperties_Immunizable.immunityPerDaySick;
				num *= pawn.GetStatValue(StatDefOf.ImmunityGainSpeed, true, -1);
				if (diseaseInstance != null)
				{
					Rand.PushState();
					Rand.Seed = Gen.HashCombineInt(diseaseInstance.loadID ^ Find.World.info.persistentRandomValue, 156482735);
					num *= Mathf.Lerp(0.8f, 1.2f, Rand.Value);
					Rand.PopState();
				}
				return num / 60000f;
			}
			return hediffCompProperties_Immunizable.immunityPerDayNotSick / 60000f;
		}

		// Token: 0x040019C1 RID: 6593
		public HediffDef hediffDef;

		// Token: 0x040019C2 RID: 6594
		public HediffDef source;

		// Token: 0x040019C3 RID: 6595
		public float immunity;
	}
}
