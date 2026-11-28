using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001757 RID: 5975
	public class RoleEffect_GiveThoughtOnTend : RoleEffect
	{
		// Token: 0x060094BC RID: 38076 RVA: 0x003635A4 File Offset: 0x003617A4
		public RoleEffect_GiveThoughtOnTend()
		{
			this.labelKey = "RoleEffectGiveThoughtOnTended";
		}

		// Token: 0x060094BD RID: 38077 RVA: 0x003635B8 File Offset: 0x003617B8
		public override string Label(Pawn pawn, Precept_Role role)
		{
			return this.labelKey.Translate() + ": " + this.thoughtDef.LabelCap.Formatted(role.LabelCap.ResolveTags());
		}

		// Token: 0x060094BE RID: 38078 RVA: 0x00363604 File Offset: 0x00361804
		public override void Notify_Tended(Pawn doctor, Pawn patient)
		{
			base.Notify_Tended(doctor, patient);
			if (doctor != patient && doctor.Ideo == patient.Ideo)
			{
				patient.needs.mood.thoughts.memories.TryGainMemory(this.thoughtDef, doctor, null);
			}
		}

		// Token: 0x040059A5 RID: 22949
		public ThoughtDef thoughtDef;
	}
}
